//
// This file was generated by the BinaryNotes compiler.
// See http://bnotes.sourceforge.net 
// Any modifications to this file will be lost upon recompilation of the source ASN.1. 
//

using System.Runtime.CompilerServices;
using System.Collections.Generic;
using GSF.ASN1;
using GSF.ASN1.Attributes;
using GSF.ASN1.Coders;

namespace GSF.MMS.Model
{
    [CompilerGenerated]
    [ASN1PreparedElement]
    [ASN1Sequence(Name = "RequestDomainDownload_Request", IsSet = false)]
    public class RequestDomainDownload_Request : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(RequestDomainDownload_Request));
        private Identifier domainName_;
        private FileName fileName_;


        private ICollection<MMSString> listOfCapabilities_;

        private bool listOfCapabilities_present;


        private bool sharable_;

        [ASN1Element(Name = "domainName", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public Identifier DomainName
        {
            get
            {
                return domainName_;
            }
            set
            {
                domainName_ = value;
            }
        }

        [ASN1SequenceOf(Name = "listOfCapabilities", IsSetOf = false)]
        [ASN1Element(Name = "listOfCapabilities", IsOptional = true, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public ICollection<MMSString> ListOfCapabilities
        {
            get
            {
                return listOfCapabilities_;
            }
            set
            {
                listOfCapabilities_ = value;
                listOfCapabilities_present = true;
            }
        }

        [ASN1Boolean(Name = "")]
        [ASN1Element(Name = "sharable", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
        public bool Sharable
        {
            get
            {
                return sharable_;
            }
            set
            {
                sharable_ = value;
            }
        }


        [ASN1Element(Name = "fileName", IsOptional = false, HasTag = true, Tag = 4, HasDefaultValue = false)]
        public FileName FileName
        {
            get
            {
                return fileName_;
            }
            set
            {
                fileName_ = value;
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

        public bool isListOfCapabilitiesPresent()
        {
            return listOfCapabilities_present;
        }
    }
}