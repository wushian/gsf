﻿//******************************************************************************************************
//  ControlFile.cs - Gbtc
//
//  Copyright © 2010, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  08/01/2014 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GSF.EMAX
{
    /// <summary>
    /// Represents an EMAX control file (i.e., a .CTL file).
    /// </summary>
    public class ControlFile
    {
        #region [ Members ]

        // Fields
        public string FileName;
        public DataSize DataSize;
        public int ConfiguredAnalogChannels;
        public CTL_HEADER Header;
        public CTL_FILE_STRUCT[] FileStructures;
        public SYSTEM_PARAMETERS SystemParameters;
        public SYS_SETTINGS SystemSettings;
        public ANALOG_GROUP AnalogGroup;
        public EVENT_GROUP EventGroup;
        public A_E_RSLTS AnalogEventResults;
        public IDENTSTRING IdentityString;
        public ANLG_CHNL_NEW AnalogChannelSettings;
        public EVNT_CHNL_NEW EventChannelSettings;
        public EVENT_DISPLAY EventDisplay;
        public SENS_RSLTS SensorResults;
        public TPwrRcd PowerRecord;
        public BoardAnalogEventChannels BoardAnalogEventChannels;
        public A_SELECTION AnalogSelection;
        public E_GRP_SELECT EventGroupSelection;
        public PHASOR_GROUPS PhasorGroups;
        public LINE_CONSTANTS LineConstants;
        public LINE_NAMES LineNames;
        public FAULT_LOCATIONS FaultLocations;
        public SEQUENCE_CHANNELS SequenceChannels;
        public BREAKER_TRIP_TIMES BreakerTripTimes;

        #endregion

        #region [ Constructors ]

        public ControlFile()
        {
        }

        public ControlFile(string fileName)
        {
            FileName = fileName;
            Parse();
        }

        #endregion

        #region [ Properties ]

        public int AnalogChannelCount
        {
            get
            {
                ushort samplesPerSecond = SystemParameters.samples_per_second;
                return ConfiguredAnalogChannels * (samplesPerSecond > 5760 ? samplesPerSecond / 5760 : 1);
            }
        }

        public int EventGroupCount
        {
            get
            {
                if (SystemParameters.samples_per_second > 5760)
                    return 4;

                return 4 * (ConfiguredAnalogChannels > 32 ? 2 : 1);
            }
        }

        public int FrameLength
        {
            get
            {
                return 2 * (2 + AnalogChannelCount + 2 * EventGroupCount);
            }
        }

        #endregion

        #region [ Methods ]

        // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
        public void Parse()
        {
            if (string.IsNullOrEmpty(FileName))
                throw new InvalidOperationException("No EMAX control file name was specified.");

            if (!File.Exists(FileName))
                throw new FileNotFoundException(string.Format("EMAX control file {0} not found.", FileName));

            byte byteValue;

            using (FileStream stream = File.OpenRead(FileName))
            {
                // Read in header and file structure definitions
                using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, true))
                {
                    // Read byte that defines number of analog channels
                    ConfiguredAnalogChannels = BinaryCodedDecimal.Decode((ushort)reader.ReadByte());

                    // Read byte that defines data size (i.e., 12 or 16 bits)
                    byteValue = reader.ReadByte();

                    if (!Enum.IsDefined(typeof(DataSize), byteValue))
                        throw new InvalidOperationException("Invalid EMAX data size code encountered: 0x" + byteValue.ToString("X").PadLeft(2, '0'));

                    DataSize = (DataSize)byteValue;

                    // Read control header
                    Header = new CTL_HEADER(reader);

                    // Create array of file structures
                    List<CTL_FILE_STRUCT> fileStructures = new List<CTL_FILE_STRUCT>();
                    CTL_FILE_STRUCT fileStructure = new CTL_FILE_STRUCT
                    {
                        type = StructureType.SYSTEM_PARAMETERS,
                        offset = Header.sp_offset
                    };

                    do
                    {
                        if (fileStructure.type != StructureType.Unknown)
                            fileStructures.Add(fileStructure);

                        fileStructure = new CTL_FILE_STRUCT(reader);
                    }
                    while (fileStructure.type != StructureType.EndOfStructures);

                    FileStructures = fileStructures.ToArray();
                }

                // Read in actual file structures
                foreach (CTL_FILE_STRUCT fileStructure in FileStructures)
                {
                    if (fileStructure.type == StructureType.Unknown)
                        continue;

                    // Locate structure in the file
                    stream.Position = fileStructure.offset;

                    // Parse the structure type
                    using (BinaryReader reader = new BinaryReader(stream, Encoding.ASCII, true))
                    {
                        switch (fileStructure.type)
                        {
                            case StructureType.SYSTEM_PARAMETERS:
                                SystemParameters = reader.ReadStructure<SYSTEM_PARAMETERS>();
                                break;
                            case StructureType.SYS_SETTINGS:
                                SystemSettings = reader.ReadStructure<SYS_SETTINGS>();
                                break;
                            case StructureType.A_E_RSLTS:
                                AnalogEventResults = new A_E_RSLTS(reader, ConfiguredAnalogChannels, SystemParameters.analog_groups);
                                break;
                            case StructureType.ANALOG_GROUP:
                                AnalogGroup = new ANALOG_GROUP(reader, ConfiguredAnalogChannels);
                                break;
                            case StructureType.EVENT_GROUP:
                                EventGroup = reader.ReadStructure<EVENT_GROUP>();
                                break;
                            case StructureType.ANLG_CHNL_NEW:
                                AnalogChannelSettings = reader.ReadStructure<ANLG_CHNL_NEW>();
                                break;
                            case StructureType.EVNT_CHNL_NEW:
                                EventChannelSettings = reader.ReadStructure<EVNT_CHNL_NEW>();
                                break;
                            case StructureType.ANLG_CHNLS:
                                break;
                            case StructureType.SHORT_HEADER:
                                // TODO: Add decoder once structure definition is known...
                                break;
                            case StructureType.FAULT_HEADER:
                                // TODO: Add decoder once structure definition is known...
                                break;
                            case StructureType.EVENT_DISPLAY:
                                EventDisplay = new EVENT_DISPLAY(reader, SystemParameters.event_groups);
                                break;
                            case StructureType.IDENTSTRING:
                                IdentityString = reader.ReadStructure<IDENTSTRING>();
                                IdentityString.value.TruncateRight(IdentityString.length);
                                break;
                            case StructureType.A_SELECTION:
                                AnalogSelection = new A_SELECTION(reader);
                                break;
                            case StructureType.E_GROUP_SELECT:
                                EventGroupSelection = new E_GRP_SELECT(reader);
                                break;
                            case StructureType.PHASOR_GROUP:
                                PhasorGroups = new PHASOR_GROUPS(reader);
                                break;
                            case StructureType.LINE_CONSTANTS:
                                LineConstants = new LINE_CONSTANTS(reader);
                                break;
                            case StructureType.LINE_NAMES:
                                LineNames = new LINE_NAMES(reader);
                                break;
                            case StructureType.FAULT_LOCATION:
                                FaultLocations = new FAULT_LOCATIONS(reader);
                                break;
                            case StructureType.SENS_RSLTS:
                                SensorResults = reader.ReadStructure<SENS_RSLTS>();
                                break;
                            case StructureType.SEQUENCE_CHANNELS:
                                SequenceChannels = new SEQUENCE_CHANNELS(reader);
                                break;
                            case StructureType.TPwrRcd:
                                PowerRecord = reader.ReadStructure<TPwrRcd>();
                                break;
                            case StructureType.BoardAnalogEventChannels:
                                BoardAnalogEventChannels = reader.ReadStructure<BoardAnalogEventChannels>();
                                break;
                            case StructureType.BREAKER_TRIP_TIMES:
                                BreakerTripTimes = new BREAKER_TRIP_TIMES(reader, ConfiguredAnalogChannels, SystemParameters.analog_groups);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
        }

        #endregion

        #region [ Old Code ]

        //private readonly StructureType[] StructureTypeProcessingOrder = 
        //{
        //    StructureType.SYSTEM_PARAMETERS,
        //    StructureType.SYS_SETTINGS,
        //    StructureType.A_E_RSLTS,
        //    StructureType.ANALOG_GROUP,
        //    StructureType.EVENT_GROUP,
        //    StructureType.ANLG_CHNL_NEW,
        //    StructureType.EVNT_CHNL_NEW,
        //    StructureType.ANLG_CHNLS,
        //    StructureType.SHORT_HEADER,
        //    StructureType.FAULT_HEADER,
        //    StructureType.EVENT_DISPLAY,
        //    StructureType.IDENTSTRING,
        //    StructureType.A_SELECTION,
        //    StructureType.E_GROUP_SELECT,
        //    StructureType.PHASOR_GROUP,
        //    StructureType.LINE_CONSTANTS,
        //    StructureType.LINE_NAMES,
        //    StructureType.FAULT_LOCATION,
        //    StructureType.SENS_RSLTS,
        //    StructureType.SEQUENCE_CHANNELS,
        //    StructureType.TPwrRcd,
        //    StructureType.BoardAnalogEventChannels,
        //    StructureType.Unknown,
        //    StructureType.EndOfStructures
        //};

        //Debug.Assert(StructureTypeProcessingOrder.Length == Enum.GetValues(typeof(StructureType)).Length, "StructureTypeProcessOrder is not the same length as StructureType enumeration!");

        //// Sort file structures in desired processing order
        //Func<StructureType, int> getProcessingOrder = structureType => Array.IndexOf(StructureTypeProcessingOrder, structureType);

        //fileStructures.Sort((x, y) =>
        //{
        //    int indexX = getProcessingOrder(x.type);
        //    int indexY = getProcessingOrder(y.type);
        //    return indexX < indexY ? -1 : indexX > indexY ? 1 : 0;
        //});

        #endregion
    }
}