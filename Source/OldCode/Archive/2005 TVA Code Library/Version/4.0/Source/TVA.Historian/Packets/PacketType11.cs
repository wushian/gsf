//*******************************************************************************************************
//  PacketType11.cs
//  Copyright � 2009 - TVA, all rights reserved - Gbtc
//
//  Build Environment: C#, Visual Studio 2008
//  Primary Developer: Pinal C. Patel
//      Office: INFO SVCS APP DEV, CHATTANOOGA - MR BK-C
//       Phone: 423/751-3024
//       Email: pcpatel@tva.gov
//
//  Code Modification History:
//  -----------------------------------------------------------------------------------------------------
//  06/18/2007 - Pinal C. Patel
//       Generated original version of source code.
//  04/21/2009 - Pinal C. Patel
//       Converted to C#.
//
//*******************************************************************************************************

using System.Collections.Generic;
using TVA.Historian.Files;

namespace TVA.Historian.Packets
{
    /// <summary>
    /// Represents a packet to be used for requesting <see cref="StateRecord.Summary"/> for the <see cref="QueryPacketBase.RequestIDs"/>.
    /// </summary>
    public class PacketType11 : QueryPacketBase
    {
        #region [ Constructors ]

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketType11"/> class.
        /// </summary>
        public PacketType11()
            : base(11)
        {
            ProcessHandler = Process;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketType11"/> class.
        /// </summary>
        /// <param name="binaryImage">Binary image to be used for initializing <see cref="PacketType11"/>.</param>
        /// <param name="startIndex">0-based starting index of initialization data in the <paramref name="binaryImage"/>.</param>
        /// <param name="length">Valid number of bytes in <paramref name="binaryImage"/> from <paramref name="startIndex"/>.</param>
        public PacketType11(byte[] binaryImage, int startIndex, int length)
            : this()
        {
            Initialize(binaryImage, startIndex, length);
        }

        #endregion

        #region [ Methods ]

        /// <summary>
        /// Processes <see cref="PacketType11"/>.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> object containing the binary images of <see cref="StateRecord.Summary"/> for the <see cref="QueryPacketBase.RequestIDs"/>.</returns>
        protected virtual IEnumerable<byte[]> Process()
        {
            if (Archive == null)
                return null;

            List<byte[]> reply = new List<byte[]>();
            if (RequestIDs.Count == 0 || (RequestIDs.Count == 1 && RequestIDs[0] == -1))
            {
                // Information for all defined records is requested.
                int id = 0;
                while (true)
                {
                    try
                    {
                        reply.Add(Archive.ReadStateDataSummary(++id));
                    }
                    catch
                    {
                        break;
                    }
                }
            }
            else
            {
                // Information for specific records is requested.
                foreach (int id in RequestIDs)
                {
                    try
                    {
                        reply.Add(Archive.ReadStateDataSummary(id));
                    }
                    catch
                    {
                    }
                }
            }
            reply.Add(new StateRecord(-1).Summary.BinaryImage);

            return reply;
        }

        #endregion
    }
}