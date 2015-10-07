//*******************************************************************************************************
//  DataFrame.cs
//  Copyright © 2009 - TVA, all rights reserved - Gbtc
//
//  Build Environment: C#, Visual Studio 2008
//  Primary Developer: James R. Carroll
//      Office: PSO TRAN & REL, CHATTANOOGA - MR BK-C
//       Phone: 423/751-4165
//       Email: jrcarrol@tva.gov
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  04/27/2009 - James R. Carroll
//       Generated original version of source code.
//
//*******************************************************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Security.Permissions;
using TVA.IO.Checksums;
using TVA.Parsing;

namespace TVA.PhasorProtocols.SelFastMessage
{
    /// <summary>
    /// Represents the SEL Fast Message implementation of a <see cref="IDataFrame"/> that can be sent or received.
    /// </summary>
    [Serializable()]
    public class DataFrame : DataFrameBase, ISupportFrameImage<int>
    {
        #region [ Members ]

        // Fields
        private CommonFrameHeader m_frameHeader;

        #endregion

        #region [ Constructors ]

        /// <summary>
        /// Creates a new <see cref="DataFrame"/>.
        /// </summary>
        /// <remarks>
        /// This constructor is used by <see cref="FrameImageParserBase{TTypeIdentifier,TOutputType}"/> to parse a SEL Fast Message data frame.
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DataFrame()
            : base(new DataCellCollection(), 0, null)
        {
        }

        /// <summary>
        /// Creates a new <see cref="DataFrame"/> from specified parameters.
        /// </summary>
        /// <param name="timestamp">The exact timestamp, in <see cref="Ticks"/>, of the data represented by this <see cref="DataFrame"/>.</param>
        /// <param name="frameSize">The SEL Fast Message frame size of this <see cref="DataFrame"/>.</param>
        /// <param name="idCode">The SEL Fast Message destination address (PMADDR setting) for this <see cref="DataFrame"/></param>
        /// <remarks>
        /// This constructor is used by a consumer to generate a SEL Fast Message data frame.
        /// </remarks>
        public DataFrame(Ticks timestamp, FrameSize frameSize, uint idCode)
            : base(new DataCellCollection(), timestamp, null)
        {
            IDCode = idCode;
            FrameSize = frameSize;
        }

        /// <summary>
        /// Creates a new <see cref="DataFrame"/> from serialization parameters.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> with populated with data.</param>
        /// <param name="context">The source <see cref="StreamingContext"/> for this deserialization.</param>
        protected DataFrame(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Deserialize data frame
            m_frameHeader = (CommonFrameHeader)info.GetValue("frameHeader", typeof(CommonFrameHeader));
        }

        #endregion

        #region [ Properties ]

        /// <summary>
        /// Gets reference to the <see cref="DataCellCollection"/> for this <see cref="DataFrame"/>.
        /// </summary>
        public new DataCellCollection Cells
        {
            get
            {
                return base.Cells as DataCellCollection;
            }
        }

        /// <summary>
        /// Gets or sets <see cref="ConfigurationFrame"/> associated with this <see cref="DataFrame"/>.
        /// </summary>
        public new ConfigurationFrame ConfigurationFrame
        {
            get
            {
                return base.ConfigurationFrame as ConfigurationFrame;
            }
            set
            {
                base.ConfigurationFrame = value;
            }
        }

        /// <summary>
        /// Gets or sets the SEL Fast Message destination address (PMADDR setting) for this <see cref="DataFrame"/>.
        /// </summary>
        public new uint IDCode
        {
            get
            {
                return CommonHeader.IDCode;
            }
            set
            {
                CommonHeader.IDCode = value;
            }
        }

        /// <summary>
        /// Gets or sets the SEL Fast Message frame size of this <see cref="DataFrame"/>.
        /// </summary>
        public FrameSize FrameSize
        {
            get
            {
                return CommonHeader.FrameSize;
            }
            set
            {
                CommonHeader.FrameSize = value;

                // We dynamically define the configuration based on frame size, SEL Fast Message
                // defines three distinct configurations (one per frame size)
                if (ConfigurationFrame == null || ConfigurationFrame.FrameSize != value)
                    ConfigurationFrame = new ConfigurationFrame(value, MessagePeriod.DefaultRate, CommonHeader.IDCode);
            }
        }

        /// <summary>
        /// Gets or sets exact timestamp, in ticks, of the data represented by this <see cref="DataFrame"/>.
        /// </summary>
        /// <remarks>
        /// The value of this property represents the number of 100-nanosecond intervals that have elapsed since 12:00:00 midnight, January 1, 0001.
        /// </remarks>
        public override Ticks Timestamp
        {
            get
            {
                return CommonHeader.Timestamp;
            }
            set
            {
                // Keep timestamp updates synchrnonized...
                CommonHeader.Timestamp = value;
                base.Timestamp = value;
            }
        }

        /// <summary>
        /// Gets the timestamp of this frame in NTP format.
        /// </summary>
        public new NtpTimeTag TimeTag
        {
            get
            {
                return CommonHeader.TimeTag;
            }
        }

        /// <summary>
        /// Gets the identifier that is used to identify the SEL Fast Message frame.
        /// </summary>
        public int TypeID
        {
            get
            {
                // SEL Fast Message only defines a single frame type...
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets current <see cref="CommonFrameHeader"/>.
        /// </summary>
        public CommonFrameHeader CommonHeader
        {
            get
            {
                // Make sure frame header exists - using base class timestamp to
                // prevent recursion (m_frameHeader doesn't exist yet)
                if (m_frameHeader == null)
                    m_frameHeader = new CommonFrameHeader(FrameSize.A, base.Timestamp);

                return m_frameHeader;
            }
            set
            {
                m_frameHeader = value;

                if (m_frameHeader != null)
                {
                    State = m_frameHeader.State as IDataFrameParsingState;
                    base.Timestamp = m_frameHeader.Timestamp;
                }
            }
        }

        // This interface implementation satisfies ISupportFrameImage<int>.CommonHeader
        ICommonHeader<int> ISupportFrameImage<int>.CommonHeader
        {
            get
            {
                return CommonHeader;
            }
            set
            {
                CommonHeader = value as CommonFrameHeader;
            }
        }

        /// <summary>
        /// Gets the length of the <see cref="HeaderImage"/>.
        /// </summary>
        protected override int HeaderLength
        {
            get
            {
                return CommonFrameHeader.FixedLength;
            }
        }

        /// <summary>
        /// Gets the binary header image of the <see cref="DataFrame"/> object.
        /// </summary>
        protected override byte[] HeaderImage
        {
            get
            {
                return CommonHeader.BinaryImage;
            }
        }

        /// <summary>
        /// <see cref="Dictionary{TKey,TValue}"/> of string based property names and values for the <see cref="DataFrame"/> object.
        /// </summary>
        public override Dictionary<string, string> Attributes
        {
            get
            {
                Dictionary<string, string> baseAttributes = base.Attributes;

                baseAttributes.Add("Frame Size", (byte)FrameSize + ": " + FrameSize);
                baseAttributes.Add("32-Bit ID Code", IDCode.ToString());
                baseAttributes.Add("Register Count", CommonHeader.RegisterCount.ToString());
                baseAttributes.Add("Sample Number", CommonHeader.SampleNumber.ToString());
                baseAttributes.Add("Second of Century", CommonHeader.SecondOfCentury.ToString());
                
                return baseAttributes;
            }
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Parses the binary header image.
        /// </summary>
        /// <param name="binaryImage">Binary image to parse.</param>
        /// <param name="startIndex">Start index into <paramref name="binaryImage"/> to begin parsing.</param>
        /// <param name="length">Length of valid data within <paramref name="binaryImage"/>.</param>
        /// <returns>The length of the data that was parsed.</returns>
        protected override int ParseHeaderImage(byte[] binaryImage, int startIndex, int length)
        {
            // We already parsed the frame header, so we just skip past it...
            return CommonFrameHeader.FixedLength;
        }

        /// <summary>
        /// Calculates checksum of given <paramref name="buffer"/>.
        /// </summary>
        /// <param name="buffer">Buffer image over which to calculate checksum.</param>
        /// <param name="offset">Start index into <paramref name="buffer"/> to calculate checksum.</param>
        /// <param name="length">Length of data within <paramref name="buffer"/> to calculate checksum.</param>
        /// <returns>Checksum over specified portion of <paramref name="buffer"/>.</returns>
        protected override ushort CalculateChecksum(byte[] buffer, int offset, int length)
        {
            // SEL Fast Message uses CRC Modbus to calculate checksum for frames
            return buffer.ModBusCrcChecksum(offset, length);
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination <see cref="StreamingContext"/> for this serialization.</param>
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            // Serialize data frame
            info.AddValue("frameHeader", m_frameHeader, typeof(CommonFrameHeader));
        }

        #endregion
    }
}