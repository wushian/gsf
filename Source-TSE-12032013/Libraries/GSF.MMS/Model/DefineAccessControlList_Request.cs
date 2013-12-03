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
    [ASN1Sequence(Name = "DefineAccessControlList_Request", IsSet = false)]
    public class DefineAccessControlList_Request : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(DefineAccessControlList_Request));
        private AccessControlListElementsSequenceType accessControlListElements_;
        private Identifier accessControlListName_;

        [ASN1Element(Name = "accessControlListName", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public Identifier AccessControlListName
        {
            get
            {
                return accessControlListName_;
            }
            set
            {
                accessControlListName_ = value;
            }
        }


        [ASN1Element(Name = "accessControlListElements", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public AccessControlListElementsSequenceType AccessControlListElements
        {
            get
            {
                return accessControlListElements_;
            }
            set
            {
                accessControlListElements_ = value;
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

        [ASN1PreparedElement]
        [ASN1Sequence(Name = "accessControlListElements", IsSet = false)]
        public class AccessControlListElementsSequenceType : IASN1PreparedElement
        {
            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(AccessControlListElementsSequenceType));
            private AccessCondition deleteAccessCondition_;

            private bool deleteAccessCondition_present;
            private AccessCondition editAccessCondition_;

            private bool editAccessCondition_present;
            private AccessCondition executeAccessCondition_;

            private bool executeAccessCondition_present;
            private AccessCondition loadAccessCondition_;

            private bool loadAccessCondition_present;
            private AccessCondition readAccessCondition_;

            private bool readAccessCondition_present;


            private AccessCondition storeAccessCondition_;

            private bool storeAccessCondition_present;


            private AccessCondition writeAccessCondition_;

            private bool writeAccessCondition_present;

            [ASN1Element(Name = "readAccessCondition", IsOptional = true, HasTag = true, Tag = 0, HasDefaultValue = false)]
            public AccessCondition ReadAccessCondition
            {
                get
                {
                    return readAccessCondition_;
                }
                set
                {
                    readAccessCondition_ = value;
                    readAccessCondition_present = true;
                }
            }

            [ASN1Element(Name = "storeAccessCondition", IsOptional = true, HasTag = true, Tag = 1, HasDefaultValue = false)]
            public AccessCondition StoreAccessCondition
            {
                get
                {
                    return storeAccessCondition_;
                }
                set
                {
                    storeAccessCondition_ = value;
                    storeAccessCondition_present = true;
                }
            }

            [ASN1Element(Name = "writeAccessCondition", IsOptional = true, HasTag = true, Tag = 2, HasDefaultValue = false)]
            public AccessCondition WriteAccessCondition
            {
                get
                {
                    return writeAccessCondition_;
                }
                set
                {
                    writeAccessCondition_ = value;
                    writeAccessCondition_present = true;
                }
            }


            [ASN1Element(Name = "loadAccessCondition", IsOptional = true, HasTag = true, Tag = 3, HasDefaultValue = false)]
            public AccessCondition LoadAccessCondition
            {
                get
                {
                    return loadAccessCondition_;
                }
                set
                {
                    loadAccessCondition_ = value;
                    loadAccessCondition_present = true;
                }
            }


            [ASN1Element(Name = "executeAccessCondition", IsOptional = true, HasTag = true, Tag = 4, HasDefaultValue = false)]
            public AccessCondition ExecuteAccessCondition
            {
                get
                {
                    return executeAccessCondition_;
                }
                set
                {
                    executeAccessCondition_ = value;
                    executeAccessCondition_present = true;
                }
            }


            [ASN1Element(Name = "deleteAccessCondition", IsOptional = true, HasTag = true, Tag = 5, HasDefaultValue = false)]
            public AccessCondition DeleteAccessCondition
            {
                get
                {
                    return deleteAccessCondition_;
                }
                set
                {
                    deleteAccessCondition_ = value;
                    deleteAccessCondition_present = true;
                }
            }


            [ASN1Element(Name = "editAccessCondition", IsOptional = true, HasTag = true, Tag = 6, HasDefaultValue = false)]
            public AccessCondition EditAccessCondition
            {
                get
                {
                    return editAccessCondition_;
                }
                set
                {
                    editAccessCondition_ = value;
                    editAccessCondition_present = true;
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


            public bool isReadAccessConditionPresent()
            {
                return readAccessCondition_present;
            }

            public bool isStoreAccessConditionPresent()
            {
                return storeAccessCondition_present;
            }

            public bool isWriteAccessConditionPresent()
            {
                return writeAccessCondition_present;
            }

            public bool isLoadAccessConditionPresent()
            {
                return loadAccessCondition_present;
            }

            public bool isExecuteAccessConditionPresent()
            {
                return executeAccessCondition_present;
            }

            public bool isDeleteAccessConditionPresent()
            {
                return deleteAccessCondition_present;
            }

            public bool isEditAccessConditionPresent()
            {
                return editAccessCondition_present;
            }
        }
    }
}