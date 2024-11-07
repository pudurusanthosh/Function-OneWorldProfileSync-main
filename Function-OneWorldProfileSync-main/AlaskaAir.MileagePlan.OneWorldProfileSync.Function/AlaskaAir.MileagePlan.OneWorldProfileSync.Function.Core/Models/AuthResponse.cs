// NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models
{
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public partial class AuthResponse
    {

        private EnvelopeHeader headerField;

        private AuthEnvelopeBody bodyField;

        /// <remarks/>
        public EnvelopeHeader Header
        {
            get
            {
                return headerField;
            }
            set
            {
                headerField = value;
            }
        }

        /// <remarks/>
        public AuthEnvelopeBody Body
        {
            get
            {
                return bodyField;
            }
            set
            {
                bodyField = value;
            }
        }
    }
       

    

    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class AuthEnvelopeBody
    {

        private Security_AuthenticateReply security_AuthenticateReplyField;

        private Security_SignOutReply securitySignOutReply;

        [System.Xml.Serialization.XmlElement(Namespace = "http://xml.amadeus.com/VLSSOR_04_1_1A")]
        public Security_SignOutReply Security_SignOutReply
        {
            get
            {
                return securitySignOutReply;
            }
            set
            {
                securitySignOutReply = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "http://xml.amadeus.com/VLSSLR_06_1_1A")]
        public Security_AuthenticateReply Security_AuthenticateReply
        {
            get
            {
                return security_AuthenticateReplyField;
            }
            set
            {
                security_AuthenticateReplyField = value;
            }
        }
    }

    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/VLSSLR_06_1_1A")]
    [System.Xml.Serialization.XmlRoot(Namespace = "http://xml.amadeus.com/VLSSLR_06_1_1A", IsNullable = false)]
    public partial class Security_AuthenticateReply
    {

        private Security_AuthenticateReplyProcessStatus processStatusField;

        private Security_AuthenticateReplyErrorSection errorSectionField;

        /// <remarks/>
        public Security_AuthenticateReplyErrorSection errorSection
        {
            get
            {
                return this.errorSectionField;
            }
            set
            {
                this.errorSectionField = value;
            }
        }

        /// <remarks/>
        public Security_AuthenticateReplyProcessStatus processStatus
        {
            get
            {
                return processStatusField;
            }
            set
            {
                processStatusField = value;
            }
        }
    }


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/VLSSOR_04_1_1A")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xml.amadeus.com/VLSSOR_04_1_1A", IsNullable = false)]
    public partial class Security_SignOutReply
    {

        private Security_SignOutReplyProcessStatus processStatusField;

        /// <remarks/>
        public Security_SignOutReplyProcessStatus processStatus
        {
            get
            {
                return this.processStatusField;
            }
            set
            {
                this.processStatusField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/VLSSOR_04_1_1A")]
    public partial class Security_SignOutReplyProcessStatus
    {

        private string statusCodeField;

        /// <remarks/>
        public string statusCode
        {
            get
            {
                return this.statusCodeField;
            }
            set
            {
                this.statusCodeField = value;
            }
        }
    }



    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/VLSSLR_06_1_1A")]
    public partial class Security_AuthenticateReplyErrorSection
    {

        private Security_AuthenticateReplyErrorSectionApplicationError applicationErrorField;

        private Security_AuthenticateReplyErrorSectionInteractiveFreeText interactiveFreeTextField;

        /// <remarks/>
        public Security_AuthenticateReplyErrorSectionApplicationError applicationError
        {
            get
            {
                return this.applicationErrorField;
            }
            set
            {
                this.applicationErrorField = value;
            }
        }

        /// <remarks/>
        public Security_AuthenticateReplyErrorSectionInteractiveFreeText interactiveFreeText
        {
            get
            {
                return this.interactiveFreeTextField;
            }
            set
            {
                this.interactiveFreeTextField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/VLSSLR_06_1_1A")]
    public partial class Security_AuthenticateReplyErrorSectionApplicationError
    {

        private Security_AuthenticateReplyErrorSectionApplicationErrorErrorDetails errorDetailsField;

        /// <remarks/>
        public Security_AuthenticateReplyErrorSectionApplicationErrorErrorDetails errorDetails
        {
            get
            {
                return this.errorDetailsField;
            }
            set
            {
                this.errorDetailsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/VLSSLR_06_1_1A")]
    public partial class Security_AuthenticateReplyErrorSectionApplicationErrorErrorDetails
    {

        private int errorCodeField;

        private string errorCategoryField;

        private string errorCodeOwnerField;

        /// <remarks/>
        public int errorCode
        {
            get
            {
                return this.errorCodeField;
            }
            set
            {
                this.errorCodeField = value;
            }
        }

        /// <remarks/>
        public string errorCategory
        {
            get
            {
                return this.errorCategoryField;
            }
            set
            {
                this.errorCategoryField = value;
            }
        }

        /// <remarks/>
        public string errorCodeOwner
        {
            get
            {
                return this.errorCodeOwnerField;
            }
            set
            {
                this.errorCodeOwnerField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/VLSSLR_06_1_1A")]
    public partial class Security_AuthenticateReplyErrorSectionInteractiveFreeText
    {

        private Security_AuthenticateReplyErrorSectionInteractiveFreeTextFreeTextQualif freeTextQualifField;

        private string[] freeTextField;

        /// <remarks/>
        public Security_AuthenticateReplyErrorSectionInteractiveFreeTextFreeTextQualif freeTextQualif
        {
            get
            {
                return this.freeTextQualifField;
            }
            set
            {
                this.freeTextQualifField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("freeText")]
        public string[] freeText
        {
            get
            {
                return this.freeTextField;
            }
            set
            {
                this.freeTextField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/VLSSLR_06_1_1A")]
    public partial class Security_AuthenticateReplyErrorSectionInteractiveFreeTextFreeTextQualif
    {

        private int subjectField;

        private string infoTypeField;

        private string languageField;

        /// <remarks/>
        public int subject
        {
            get
            {
                return this.subjectField;
            }
            set
            {
                this.subjectField = value;
            }
        }

        /// <remarks/>
        public string infoType
        {
            get
            {
                return this.infoTypeField;
            }
            set
            {
                this.infoTypeField = value;
            }
        }

        /// <remarks/>
        public string language
        {
            get
            {
                return this.languageField;
            }
            set
            {
                this.languageField = value;
            }
        }
    }

    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://xml.amadeus.com/VLSSLR_06_1_1A")]
    public partial class Security_AuthenticateReplyProcessStatus
    {

        private string statusCodeField;

        /// <remarks/>
        public string statusCode
        {
            get
            {
                return statusCodeField;
            }
            set
            {
                statusCodeField = value;
            }
        }
    }
}