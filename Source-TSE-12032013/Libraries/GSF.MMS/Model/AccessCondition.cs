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
using GSF.ASN1.Types;

namespace GSF.MMS.Model
{
    [CompilerGenerated]
    [ASN1PreparedElement]
    [ASN1Choice(Name = "AccessCondition")]
    public class AccessCondition : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(AccessCondition));
        private ICollection<AccessCondition> alternate_;
        private bool alternate_selected;
        private ICollection<AccessCondition> joint_;
        private bool joint_selected;
        private NullObject never_;
        private bool never_selected;
        private Authentication_value password_;
        private bool password_selected;


        private Identifier semaphore_;
        private bool semaphore_selected;


        private UserChoiceType user_;
        private bool user_selected;

        [ASN1Null(Name = "never")]
        [ASN1Element(Name = "never", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public NullObject Never
        {
            get
            {
                return never_;
            }
            set
            {
                selectNever(value);
            }
        }

        [ASN1Element(Name = "semaphore", IsOptional = false, HasTag = true, Tag = 1, HasDefaultValue = false)]
        public Identifier Semaphore
        {
            get
            {
                return semaphore_;
            }
            set
            {
                selectSemaphore(value);
            }
        }


        [ASN1Element(Name = "user", IsOptional = false, HasTag = true, Tag = 2, HasDefaultValue = false)]
        public UserChoiceType User
        {
            get
            {
                return user_;
            }
            set
            {
                selectUser(value);
            }
        }


        [ASN1Element(Name = "password", IsOptional = false, HasTag = true, Tag = 3, HasDefaultValue = false)]
        public Authentication_value Password
        {
            get
            {
                return password_;
            }
            set
            {
                selectPassword(value);
            }
        }


        [ASN1SequenceOf(Name = "joint", IsSetOf = false)]
        [ASN1Element(Name = "joint", IsOptional = false, HasTag = true, Tag = 4, HasDefaultValue = false)]
        public ICollection<AccessCondition> Joint
        {
            get
            {
                return joint_;
            }
            set
            {
                selectJoint(value);
            }
        }


        [ASN1SequenceOf(Name = "alternate", IsSetOf = false)]
        [ASN1Element(Name = "alternate", IsOptional = false, HasTag = true, Tag = 5, HasDefaultValue = false)]
        public ICollection<AccessCondition> Alternate
        {
            get
            {
                return alternate_;
            }
            set
            {
                selectAlternate(value);
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


        public bool isNeverSelected()
        {
            return never_selected;
        }


        public void selectNever()
        {
            selectNever(new NullObject());
        }


        public void selectNever(NullObject val)
        {
            never_ = val;
            never_selected = true;


            semaphore_selected = false;

            user_selected = false;

            password_selected = false;

            joint_selected = false;

            alternate_selected = false;
        }


        public bool isSemaphoreSelected()
        {
            return semaphore_selected;
        }


        public void selectSemaphore(Identifier val)
        {
            semaphore_ = val;
            semaphore_selected = true;


            never_selected = false;

            user_selected = false;

            password_selected = false;

            joint_selected = false;

            alternate_selected = false;
        }


        public bool isUserSelected()
        {
            return user_selected;
        }


        public void selectUser(UserChoiceType val)
        {
            user_ = val;
            user_selected = true;


            never_selected = false;

            semaphore_selected = false;

            password_selected = false;

            joint_selected = false;

            alternate_selected = false;
        }


        public bool isPasswordSelected()
        {
            return password_selected;
        }


        public void selectPassword(Authentication_value val)
        {
            password_ = val;
            password_selected = true;


            never_selected = false;

            semaphore_selected = false;

            user_selected = false;

            joint_selected = false;

            alternate_selected = false;
        }


        public bool isJointSelected()
        {
            return joint_selected;
        }


        public void selectJoint(ICollection<AccessCondition> val)
        {
            joint_ = val;
            joint_selected = true;


            never_selected = false;

            semaphore_selected = false;

            user_selected = false;

            password_selected = false;

            alternate_selected = false;
        }


        public bool isAlternateSelected()
        {
            return alternate_selected;
        }


        public void selectAlternate(ICollection<AccessCondition> val)
        {
            alternate_ = val;
            alternate_selected = true;


            never_selected = false;

            semaphore_selected = false;

            user_selected = false;

            password_selected = false;

            joint_selected = false;
        }

        [ASN1PreparedElement]
        [ASN1Choice(Name = "user")]
        public class UserChoiceType : IASN1PreparedElement
        {
            private static IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(UserChoiceType));
            private ApplicationReference association_;
            private bool association_selected;


            private NullObject none_;
            private bool none_selected;

            [ASN1Element(Name = "association", IsOptional = false, HasTag = false, HasDefaultValue = false)]
            public ApplicationReference Association
            {
                get
                {
                    return association_;
                }
                set
                {
                    selectAssociation(value);
                }
            }


            [ASN1Null(Name = "none")]
            [ASN1Element(Name = "none", IsOptional = false, HasTag = false, HasDefaultValue = false)]
            public NullObject None
            {
                get
                {
                    return none_;
                }
                set
                {
                    selectNone(value);
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


            public bool isAssociationSelected()
            {
                return association_selected;
            }


            public void selectAssociation(ApplicationReference val)
            {
                association_ = val;
                association_selected = true;


                none_selected = false;
            }


            public bool isNoneSelected()
            {
                return none_selected;
            }


            public void selectNone()
            {
                selectNone(new NullObject());
            }


            public void selectNone(NullObject val)
            {
                none_ = val;
                none_selected = true;


                association_selected = false;
            }
        }
    }
}