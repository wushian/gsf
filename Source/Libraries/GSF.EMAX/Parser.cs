﻿//******************************************************************************************************
//  Parser.cs - Gbtc
//
//  Copyright © 2012, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  06/17/2012 - J. Ritchie Carroll
//       Generated original version of source code.
//  12/13/2012 - Starlynn Danyelle Gilliam
//       Modified Header.
//
//******************************************************************************************************

using System;
using System.IO;
using System.Linq;
using GSF.IO;

namespace GSF.EMAX
{
    /// <summary>
    /// EMAX data file(s) parser.
    /// </summary>
    public class Parser : IDisposable
    {
        #region [ Members ]

        // Fields
        private ControlFile m_controlFile;
        private string m_fileName;
        private bool m_disposed;
        private FileStream[] m_fileStreams;
        private DateTime m_timestamp;
        private double[] m_values;
        private double[] m_nextValues;
        private ushort[] m_eventGroups;
        private DateTime m_baseTime;
        private TimeZoneInfo m_sourceTimeZone;
        private bool m_timeError;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Releases the unmanaged resources before the <see cref="Parser"/> object is reclaimed by <see cref="GC"/>.
        /// </summary>
        ~Parser()
        {
            Dispose(false);
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets or sets associated EMAX control file for this <see cref="Parser"/>.
        /// </summary>
        /// <remarks>
        /// This is similar in function to a COMTRADE schema file.
        /// </remarks>
        public ControlFile ControlFile
        {
            get
            {
                return m_controlFile;
            }
            set
            {
                m_controlFile = value;

                if ((object)m_controlFile != null)
                {
                    if (m_controlFile.AnalogChannelCount > 0)
                        m_values = new double[m_controlFile.AnalogChannelCount];
                    else
                        throw new InvalidOperationException("Invalid control file: total analog channels defined in control file is zero.");

                    if (m_controlFile.EventGroupCount > 0)
                        m_eventGroups = new ushort[m_controlFile.EventGroupCount];
                    else
                        throw new InvalidOperationException("Invalid control file: total event groups defined in control file is zero.");

                    SYSTEM_PARAMETERS systemParameters = m_controlFile.SystemParameters;

                    m_baseTime = systemParameters.FaultTime.BaselinedTimestamp(BaselineTimeInterval.Year);
                    m_sourceTimeZone = systemParameters.GetTimeZoneInfo();
                }
                else
                {
                    m_values = null;
                }
            }
        }

        /// <summary>
        /// Gets or sets EMAX data filename.
        /// </summary>
        /// <remarks>
        /// If there are more than one data files in a set (e.g., RCL/RCU), this should be set to first file name in the set, e.g., DATA123.RCL.
        /// </remarks>
        public string FileName
        {
            get
            {
                return m_fileName;
            }
            set
            {
                m_fileName = value;
            }
        }

        /// <summary>
        /// Gets timestamp of current record in the timezone of provided IRIG signal.
        /// </summary>
        public DateTime Timestamp
        {
            get
            {
                return m_timestamp;
            }
        }

        /// <summary>
        /// Gets a flag that indicates whether the parser encountered an error
        /// while parsing the timestamp in the last call to <see cref="ReadNext"/>.
        /// </summary>
        public bool TimeError
        {
            get
            {
                return m_timeError;
            }
        }

        /// <summary>
        /// Attempts to get current timestamp converted to UTC.
        /// </summary>
        /// <remarks>
        /// This will only be accurate if timezone configured in device matches IRIG clock.
        /// </remarks>
        public DateTime TimestampAsUtc
        {
            get
            {
                return TimeZoneInfo.ConvertTimeToUtc(m_timestamp, m_sourceTimeZone);
            }
        }

        /// <summary>
        /// Gets values of current record.
        /// </summary>
        public double[] Values
        {
            get
            {
                return m_values;
            }
        }

        /// <summary>
        /// Gets event groups for current record.
        /// </summary>
        public ushort[] EventGroups
        {
            get
            {
                return m_eventGroups;
            }
        }

        // Gets total number of offset bytes in the file
        private int OffsetBytes
        {
            get
            {
                return m_controlFile.SystemParameters.channel_offset * 2;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Releases all the resources used by the <see cref="Parser"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Parser"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                try
                {
                    if (disposing)
                    {
                        CloseFiles();
                    }
                }
                finally
                {
                    m_disposed = true;  // Prevent duplicate dispose.
                }
            }
        }

        /// <summary>
        /// Opens all EMAX data file streams.
        /// </summary>
        public void OpenFiles()
        {
            if (string.IsNullOrWhiteSpace(m_fileName))
                throw new InvalidOperationException("Initial EMAX data file name was not specified, cannot open file(s).");

            if (!File.Exists(m_fileName))
                throw new FileNotFoundException(string.Format("Specified EMAX data file \"{0}\" was not found, cannot open file(s).", m_fileName));

            string extension = FilePath.GetExtension(m_fileName);
            string[] fileNames;

            if (extension.Equals(".RCU", StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Please specify the .RCL file instead of the .RCU as the initial file - the .RCU will be automatically loaded: " + m_fileName);

            if (extension.Equals(".RCD", StringComparison.OrdinalIgnoreCase))
            {
                // RCD files mean there is only one card in the system
                fileNames = new string[1];
                fileNames[0] = m_fileName;
            }
            else if (extension.Equals(".RCL", StringComparison.OrdinalIgnoreCase))
            {
                // RCL files mean there are two cards in the system - one low (RCL) and one high (RCU)
                fileNames = new string[2];
                fileNames[0] = m_fileName;
                fileNames[1] = Path.Combine(FilePath.GetDirectoryName(m_fileName), FilePath.GetFileNameWithoutExtension(m_fileName) + ".RCU");
            }
            else
            {
                throw new InvalidOperationException("Specified file name does not have a valid EMAX extension: " + m_fileName);
            }

            // Create a new file stream for each file
            m_fileStreams = new FileStream[fileNames.Length];

            for (int i = 0; i < fileNames.Length; i++)
                m_fileStreams[i] = new FileStream(fileNames[i], FileMode.Open, FileAccess.Read, FileShare.Read);
        }

        /// <summary>
        /// Closes all EMAX data file streams.
        /// </summary>
        public void CloseFiles()
        {
            if ((object)m_fileStreams != null)
            {
                foreach (FileStream fileStream in m_fileStreams)
                {
                    if ((object)fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                }
            }

            m_fileStreams = null;
        }

        /// <summary>
        /// Reads next EMAX record.
        /// </summary>
        /// <returns><c>true</c> if read succeeded; otherwise <c>false</c> if end of data set was reached.</returns>
        public bool ReadNext()
        {
            const int HighSampleRateThreshold = 5760;
            const int LargeFrameValueCount = 64;
            const int SmallFrameValueCount = 32;

            // 64 channel values + 4 event groups + 4 clock words + 4 more event groups + 4 more clock words (unused)
            const int LargeFrameSize = (LargeFrameValueCount + 4 + 4 + 4 + 4) * sizeof(ushort);

            // 32 channel values + 4 event groups + 4 clock words
            const int SmallFrameSize = (SmallFrameValueCount + 4 + 4) * sizeof(ushort);

            if ((object)m_fileStreams == null)
                throw new InvalidOperationException("EMAX data files are not open, cannot read next record.");

            if ((object)m_controlFile == null)
                throw new InvalidOperationException("No EMAX schema has been defined, cannot read records.");

            // Records with high sample rates define two samples per frame so we need to
            // check whether there are values left over from the last frame that was parsed
            if ((object)m_nextValues != null)
            {
                m_values = m_nextValues;
                m_nextValues = null;
                return true;
            }

            byte[] buffer = null;

            FileStream currentFile;
            int recordLength;
            ushort[] clockWords;
            ushort[] eventGroups;
            int eventGroupIndex = 0;
            int eventGroupCopyLength = 0;
            ushort value;
            int index;
            double scalingFactor;

            int sampleRate = m_controlFile.SystemParameters.samples_per_second;
            int largeFrameChannelCount = LargeFrameValueCount;
            int smallFrameChannelCount = SmallFrameValueCount;
            int remainingChannelCount = m_controlFile.AnalogChannelCount;

            // Records with high sample rates store two samples per frame,
            // so the number of channels in each frame is halved
            if (sampleRate > HighSampleRateThreshold)
            {
                largeFrameChannelCount /= 2;
                smallFrameChannelCount /= 2;
            }

            for (int streamIndex = 0; streamIndex < m_fileStreams.Length; streamIndex++)
            {
                // Pick the smallest frame size in which the channels will fit
                int frameChannelCount = (remainingChannelCount <= smallFrameChannelCount) ? smallFrameChannelCount : largeFrameChannelCount;
                int frameValueCount = (frameChannelCount == smallFrameChannelCount) ? SmallFrameValueCount : LargeFrameValueCount;

                // Determine the actual number of channels
                int channelCount = Math.Min(remainingChannelCount, frameChannelCount);

                // Remaining channels are defined in the next file
                remainingChannelCount -= channelCount;

                currentFile = m_fileStreams[streamIndex];
                recordLength = (frameChannelCount == smallFrameChannelCount) ? SmallFrameSize : LargeFrameSize;
                index = 0;

                if ((object)buffer == null || buffer.Length < recordLength)
                    buffer = new byte[recordLength];

                // Skip to the beginning of the first set of channels
                if (currentFile.Position == 0 && m_controlFile.SystemParameters.channel_offset > 0)
                    currentFile.Read(buffer, 0, m_controlFile.SystemParameters.channel_offset * sizeof(ushort));

                // Read next record from file
                int bytesRead = currentFile.Read(buffer, 0, recordLength);

                // Determine whether we've read enough bytes to get data for every analog channel
                if (bytesRead < frameValueCount * sizeof(ushort))
                    return false;

                for (int i = bytesRead; i < buffer.Length; i++)
                    buffer[i] = 0xFF;

                // Calculate the index of the channel at the start of this frame
                int startIndex = streamIndex * largeFrameChannelCount;

                // Parse all analog record values
                for (int i = startIndex; i < startIndex + channelCount; i++)
                {
                    // Read next value
                    value = LittleEndian.ToUInt16(buffer, index);

                    if (m_controlFile.ScalingFactors.TryGetValue(i, out scalingFactor))
                    {
                        if (value >= 32768)
                            m_values[i] = (65535 - value) / 32768.0D * scalingFactor;
                        else
                            m_values[i] = -value / 32768.0D * scalingFactor;
                    }
                    else if (m_controlFile.DataSize == DataSize.Bits12)
                    {
                        m_values[i] = value >> 4;
                    }
                    else
                    {
                        m_values[i] = value;
                    }

                    index += 2;
                }

                // If this record has a high sample rate, parse all the analog record values again
                if (sampleRate > HighSampleRateThreshold)
                {
                    // Skip past the first set of frame values
                    index = frameChannelCount * sizeof(ushort);

                    // Parse all analog record values
                    for (int i = startIndex; i < startIndex + channelCount; i++)
                    {
                        // Read next value
                        value = LittleEndian.ToUInt16(buffer, index);

                        if (m_controlFile.ScalingFactors.TryGetValue(i, out scalingFactor))
                        {
                            if (value >= 32768)
                                m_nextValues[i] = (65535 - value) / 32768.0D * scalingFactor;
                            else
                                m_nextValues[i] = -value / 32768.0D * scalingFactor;
                        }
                        else if (m_controlFile.DataSize == DataSize.Bits12)
                        {
                            m_nextValues[i] = value >> 4;
                        }
                        else
                        {
                            m_nextValues[i] = value;
                        }

                        index += 2;
                    }
                }

                // Place the index in the location just
                // after the last analog channel so we
                // can start parsing event groups
                index = frameValueCount * sizeof(ushort);

                eventGroups = Enumerable.Range(0, 4)
                    .Select(i => index + 2 * i)
                    .Select(i => (i + 1 < buffer.Length) ? LittleEndian.ToUInt16(buffer, i) : ushort.MinValue)
                    .ToArray();

                eventGroupCopyLength = Math.Max(0, Math.Min(4, m_eventGroups.Length - eventGroupIndex));
                Buffer.BlockCopy(eventGroups, 0, m_eventGroups, eventGroupIndex, eventGroupCopyLength);
                eventGroupIndex += 4;
                index += 4 * sizeof(ushort);

                // Clock words always come right after event words,
                // but we need to ignore the clock words in the RCU
                // file because they are not used
                if (streamIndex == 0)
                {
                    clockWords = Enumerable.Range(0, 4)
                        .Select(i => index + 2 * i)
                        .Select(i => (i + 1 < buffer.Length) ? LittleEndian.ToUInt16(buffer, i) : ushort.MaxValue)
                        .ToArray();

                    m_timestamp = ParseTimestamp(clockWords);
                }

                index += 4 * sizeof(ushort);

                if (frameChannelCount == largeFrameChannelCount)
                {
                    // 64-channel frames have an extra set of event words
                    eventGroups = Enumerable.Range(0, 4)
                        .Select(i => index + 2 * i)
                        .Select(i => (i + 1 < buffer.Length) ? LittleEndian.ToUInt16(buffer, i) : ushort.MinValue)
                        .ToArray();

                    eventGroupCopyLength = Math.Max(0, Math.Min(4, m_eventGroups.Length - eventGroupIndex));
                    Buffer.BlockCopy(eventGroups, 0, m_eventGroups, eventGroupIndex, eventGroupCopyLength);
                    eventGroupIndex += 4;

                    // There is also an extra set of clock words,
                    // but they are not used so we ignore them
                }
            }

            return true;
        }

        private DateTime ParseTimestamp(ushort[] clockWords)
        {
            if ((object)clockWords == null)
                throw new NullReferenceException("Clock words array was null - cannot parse timestamp");

            if (clockWords.Length != 4)
                throw new InvalidOperationException("Clock words array must have four values - cannot parse timestamp");

            int days, hours, minutes, seconds, milliseconds, microseconds;
            byte highByte, lowByte;

            m_timeError = clockWords
                .SelectMany(clockWord => new byte[]
                {
                    clockWord.HighByte().HighNibble(),
                    clockWord.HighByte().LowNibble(),
                    clockWord.LowByte().HighNibble(),
                    clockWord.LowByte().LowNibble()
                })
                .Take(12)
                .Any(b => b > 9);

            if (m_timeError)
                return DateTime.MaxValue;

            highByte = clockWords[0].HighByte();
            lowByte = clockWords[0].LowByte();

            days = highByte.HighNibble() * 100 + highByte.LowNibble() * 10 + lowByte.HighNibble();
            hours = lowByte.LowNibble() * 10;

            highByte = clockWords[1].HighByte();
            lowByte = clockWords[1].LowByte();

            hours += highByte.HighNibble();
            minutes = highByte.LowNibble() * 10 + lowByte.HighNibble();
            seconds = lowByte.LowNibble() * 10;

            highByte = clockWords[2].HighByte();
            lowByte = clockWords[2].LowByte();

            seconds += highByte.HighNibble();

            milliseconds = highByte.LowNibble() * 100 + lowByte.HighNibble() * 10 + lowByte.LowNibble();

            highByte = clockWords[3].HighByte();
            lowByte = clockWords[3].LowByte();

            microseconds = 0;

            if (highByte.HighNibble() <= 9)
                microseconds += highByte.HighNibble() * 100;

            if (highByte.LowNibble() <= 9)
                microseconds += highByte.LowNibble() * 10;

            if (lowByte.HighNibble() <= 9)
                microseconds += lowByte.HighNibble();

            return m_baseTime
                .AddDays(days - 1) // Base time starts at day one, so we subtract one for target day
                .AddHours(hours)
                .AddMinutes(minutes)
                .AddSeconds(seconds)
                .AddMilliseconds(milliseconds)
                .AddTicks(Ticks.FromMicroseconds(microseconds));
        }

        #endregion
    }
}
