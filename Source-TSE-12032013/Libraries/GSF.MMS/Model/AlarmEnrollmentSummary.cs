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
    [ASN1Sequence(Name = "AlarmEnrollmentSummary", IsSet = false)]
    public class AlarmEnrollmentSummary : IASN1PreparedElement
    {
        private static readonly IASN1PreparedElementData preparedData = CoderFactory.getInstance().newPreparedElementData(typeof(AlarmEnrollmentSummary));
        private AlarmAckRule alarmAcknowledgmentRule_;
        private ApplicationReference clientApplication_;

        private bool clientApplication_present;


        private EC_State currentState_;


        private EN_Additional_Detail displayEnhancement_;

        private bool displayEnhancement_present;


        private EE_State enrollmentState_;

        private bool enrollmentState_present;
        private ObjectName eventEnrollmentName_;
        private bool notificationLost_;
        private Unsigned8 severity_;


        private EventTime timeActiveAcknowledged_;

        private bool timeActiveAcknowledged_present;
        private EventTime timeIdleAcknowledged_;

        private bool timeIdleAcknowledged_present;
        private EventTime timeOfLastTransitionToActive_;

        private bool timeOfLastTransitionToActive_present;


        private EventTime timeOfLastTransitionToIdle_;

        private bool timeOfLastTransitionToIdle_present;

        [ASN1Element(Name = "eventEnrollmentName", IsOptional = false, HasTag = true, Tag = 0, HasDefaultValue = false)]
        public ObjectName EventEnrollmentName
        {
            get
            {
                return eventEnrollmentName_;
            }
            set
            {
                eventEnrollmentName_ = value;
            }
        }

        [ASN1Element(Name = "clientApplication", IsOptional = true, HasTag = true, Tag = 2, HasDefaultValue = false)]
        public ApplicationReference ClientApplication
        {
            get
            {
                return clientApplication_;
            }
            set
            {
                clientApplication_ = value;
                clientApplication_present = true;
            }
        }

        [ASN1Element(Name = "severity", IsOptional = false, HasTag = true, Tag = 3, HasDefaultValue = false)]
        public Unsigned8 Severity
        {
            get
            {
                return severity_;
            }
            set
            {
                severity_ = value;
            }
        }

        [ASN1Element(Name = "currentState", IsOptional = false, HasTag = true, Tag = 4, HasDefaultValue = false)]
        public EC_State CurrentState
        {
            get
            {
                return currentState_;
            }
            set
            {
                currentState_ = value;
            }
        }

        [ASN1Element(Name = "displayEnhancement", IsOptional = true, HasTag = true, Tag = 5, HasDefaultValue = false)]
        public EN_Additional_Detail DisplayEnhancement
        {
            get
            {
                return displayEnhancement_;
            }
            set
            {
                displayEnhancement_ = value;
                displayEnhancement_present = true;
            }
        }

        [ASN1Boolean(Name = "")]
        [ASN1Element(Name = "notificationLost", IsOptional = false, HasTag = true, Tag = 6, HasDefaultValue = true)]
        public bool NotificationLost
        {
            get
            {
                return notificationLost_;
            }
            set
            {
                notificationLost_ = value;
            }
        }

        [ASN1Element(Name = "alarmAcknowledgmentRule", IsOptional = false, HasTag = true, Tag = 7, HasDefaultValue = false)]
        public AlarmAckRule AlarmAcknowledgmentRule
        {
            get
            {
                return alarmAcknowledgmentRule_;
            }
            set
            {
                alarmAcknowledgmentRule_ = value;
            }
        }

        [ASN1Element(Name = "enrollmentState", IsOptional = true, HasTag = true, Tag = 8, HasDefaultValue = false)]
        public EE_State EnrollmentState
        {
            get
            {
                return enrollmentState_;
            }
            set
            {
                enrollmentState_ = value;
                enrollmentState_present = true;
            }
        }

        [ASN1Element(Name = "timeOfLastTransitionToActive", IsOptional = true, HasTag = true, Tag = 9, HasDefaultValue = false)]
        public EventTime TimeOfLastTransitionToActive
        {
            get
            {
                return timeOfLastTransitionToActive_;
            }
            set
            {
                timeOfLastTransitionToActive_ = value;
                timeOfLastTransitionToActive_present = true;
            }
        }

        [ASN1Element(Name = "timeActiveAcknowledged", IsOptional = true, HasTag = true, Tag = 10, HasDefaultValue = false)]
        public EventTime TimeActiveAcknowledged
        {
            get
            {
                return timeActiveAcknowledged_;
            }
            set
            {
                timeActiveAcknowledged_ = value;
                timeActiveAcknowledged_present = true;
            }
        }

        [ASN1Element(Name = "timeOfLastTransitionToIdle", IsOptional = true, HasTag = true, Tag = 11, HasDefaultValue = false)]
        public EventTime TimeOfLastTransitionToIdle
        {
            get
            {
                return timeOfLastTransitionToIdle_;
            }
            set
            {
                timeOfLastTransitionToIdle_ = value;
                timeOfLastTransitionToIdle_present = true;
            }
        }


        [ASN1Element(Name = "timeIdleAcknowledged", IsOptional = true, HasTag = true, Tag = 12, HasDefaultValue = false)]
        public EventTime TimeIdleAcknowledged
        {
            get
            {
                return timeIdleAcknowledged_;
            }
            set
            {
                timeIdleAcknowledged_ = value;
                timeIdleAcknowledged_present = true;
            }
        }

        public void initWithDefaults()
        {
            bool param_NotificationLost =
                false;
            NotificationLost = param_NotificationLost;
        }

        public IASN1PreparedElementData PreparedData
        {
            get
            {
                return preparedData;
            }
        }


        public bool isClientApplicationPresent()
        {
            return clientApplication_present;
        }

        public bool isDisplayEnhancementPresent()
        {
            return displayEnhancement_present;
        }

        public bool isEnrollmentStatePresent()
        {
            return enrollmentState_present;
        }

        public bool isTimeOfLastTransitionToActivePresent()
        {
            return timeOfLastTransitionToActive_present;
        }

        public bool isTimeActiveAcknowledgedPresent()
        {
            return timeActiveAcknowledged_present;
        }

        public bool isTimeOfLastTransitionToIdlePresent()
        {
            return timeOfLastTransitionToIdle_present;
        }

        public bool isTimeIdleAcknowledgedPresent()
        {
            return timeIdleAcknowledged_present;
        }
    }
}