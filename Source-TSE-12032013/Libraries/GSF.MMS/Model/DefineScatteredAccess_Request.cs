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
    [ASN1Sequence(Name = "DefineScatteredAccess_Request", IsSet = false)]
    public class DefineScatteredAccess_Request : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(DefineScatteredAccess_Request));
        private ScatteredAccessDescription scatteredAccessDescription_;
        private ObjectName scatteredAccessName_;

        [ASN1Element(Name = "scatteredAccessName", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public ObjectName ScatteredAccessName
        {
            get
            {
                return scatteredAccessName_;
            }
            set
            {
                scatteredAccessName_ = value;
            }
        }


        [ASN1Element(Name = "scatteredAccessDescription", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public ScatteredAccessDescription ScatteredAccessDescription
        {
            get
            {
                return scatteredAccessDescription_;
            }
            set
            {
                scatteredAccessDescription_ = value;
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