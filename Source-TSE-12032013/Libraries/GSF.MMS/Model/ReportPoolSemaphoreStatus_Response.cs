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
    [ASN1Sequence(Name = "ReportPoolSemaphoreStatus_Response", IsSet = false)]
    public class ReportPoolSemaphoreStatus_Response : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(ReportPoolSemaphoreStatus_Response));
        private ICollection<ListOfNamedTokensChoiceType> listOfNamedTokens_;


        private bool moreFollows_;

        [ASN1SequenceOf(Name = "listOfNamedTokens", IsSetOf = false)]
        [ASN1Element(Name = "listOfNamedTokens", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public ICollection<ListOfNamedTokensChoiceType> ListOfNamedTokens
        {
            get
            {
                return listOfNamedTokens_;
            }
            set
            {
                listOfNamedTokens_ = value;
            }
        }

        [ASN1Boolean(Name = "")]
        [ASN1Element(Name = "moreFollows", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = true)]
        public bool MoreFollows
        {
            get
            {
                return moreFollows_;
            }
            set
            {
                moreFollows_ = value;
            }
        }


        public void initWithDefaults()
        {
            bool param_MoreFollows =
                false;
            MoreFollows = param_MoreFollows;
        }


        public IASN1PreparedElementData PreparedData
        {
            get
            {
                return preparedData;
            }
        }

        [ASN1PreparedElement]
        [ASN1Choice(Name = "listOfNamedTokens")]
        public class ListOfNamedTokensChoiceType : IASN1PreparedElement
        {
            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(ListOfNamedTokensChoiceType));
            private Identifier freeNamedToken_;
            private bool freeNamedToken_selected;
            private Identifier hungNamedToken_;
            private bool hungNamedToken_selected;


            private Identifier ownedNamedToken_;
            private bool ownedNamedToken_selected;

            [ASN1Element(Name = "freeNamedToken", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
            public Identifier FreeNamedToken
            {
                get
                {
                    return freeNamedToken_;
                }
                set
                {
                    selectFreeNamedToken(value);
                }
            }


            [ASN1Element(Name = "ownedNamedToken", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
            public Identifier OwnedNamedToken
            {
                get
                {
                    return ownedNamedToken_;
                }
                set
                {
                    selectOwnedNamedToken(value);
                }
            }


            [ASN1Element(Name = "hungNamedToken", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
            public Identifier HungNamedToken
            {
                get
                {
                    return hungNamedToken_;
                }
                set
                {
                    selectHungNamedToken(value);
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


            public bool isFreeNamedTokenSelected()
            {
                return freeNamedToken_selected;
            }


            public void selectFreeNamedToken(Identifier val)
            {
                freeNamedToken_ = val;
                freeNamedToken_selected = true;


                ownedNamedToken_selected = false;

                hungNamedToken_selected = false;
            }


            public bool isOwnedNamedTokenSelected()
            {
                return ownedNamedToken_selected;
            }


            public void selectOwnedNamedToken(Identifier val)
            {
                ownedNamedToken_ = val;
                ownedNamedToken_selected = true;


                freeNamedToken_selected = false;

                hungNamedToken_selected = false;
            }


            public bool isHungNamedTokenSelected()
            {
                return hungNamedToken_selected;
            }


            public void selectHungNamedToken(Identifier val)
            {
                hungNamedToken_ = val;
                hungNamedToken_selected = true;


                freeNamedToken_selected = false;

                ownedNamedToken_selected = false;
            }
        }
    }
}