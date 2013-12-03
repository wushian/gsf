//
// This file was generated by the BinaryNotes compiler.
// See http://bnotes.sourceforge.net 
// Any modifications to this file will be lost upon recompilation of the source ASN.1. 
//

using System.Runtime.CompilerServices;
using GSF.ASN1;
using GSF.ASN1.Attributes;
using GSF.ASN1.Coders;

namespace GSF.MMS.Model
{
    [CompilerGenerated]
    [ASN1PreparedElement]
    [ASN1Sequence(Name = "JournalEntry", IsSet = false)]
    public class JournalEntry : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(JournalEntry));
        private EntryContent entryContent_;
        private byte[] entryIdentifier_;


        private ApplicationReference originatingApplication_;

        [ASN1OctetString(Name = "")]
        [ASN1Element(Name = "entryIdentifier", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public byte[] EntryIdentifier
        {
            get
            {
                return entryIdentifier_;
            }
            set
            {
                entryIdentifier_ = value;
            }
        }

        [ASN1Element(Name = "originatingApplication", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public ApplicationReference OriginatingApplication
        {
            get
            {
                return originatingApplication_;
            }
            set
            {
                originatingApplication_ = value;
            }
        }


        [ASN1Element(Name = "entryContent", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
        public EntryContent EntryContent
        {
            get
            {
                return entryContent_;
            }
            set
            {
                entryContent_ = value;
            }
        }


        public void initWithDefaults()
        {
        }


        public IASN1PreparedElementData PreparedData
        {
            get
            {
                return preparedData;
            }
        }
    }
}