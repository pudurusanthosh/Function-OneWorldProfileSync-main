namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models
{

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public partial class AmadeusProfileRetrieveResponse
    {

        private EnvelopeHeader headerField;

        private AmedeusProfileEnvelopeBody bodyField;

        /// <remarks/>
        public EnvelopeHeader Header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public AmedeusProfileEnvelopeBody Body
        {
            get
            {
                return this.bodyField;
            }
            set
            {
                this.bodyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "EnvelopeBod", AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class AmedeusProfileEnvelopeBody
    {

        private AMA_ProfileReadRS aMA_ProfileReadRSField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
        public AMA_ProfileReadRS AMA_ProfileReadRS
        {
            get
            {
                return this.aMA_ProfileReadRSField;
            }
            set
            {
                this.aMA_ProfileReadRSField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile", IsNullable = false)]
    public partial class AMA_ProfileReadRS
    {

        private object successField;

        private AMA_ProfileReadRSProfiles profilesField;

        private decimal versionField;

        private AMA_ProfileReadRSErrors errorsField;

        /// <remarks/>
        public object Success
        {
            get
            {
                return this.successField;
            }
            set
            {
                this.successField = value;
            }
        }

        /// <remarks/>
        public AMA_ProfileReadRSProfiles Profiles
        {
            get
            {
                return this.profilesField;
            }
            set
            {
                this.profilesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal Version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        [System.Xml.Serialization.XmlElement(ElementName = "Errors")]
        public AMA_ProfileReadRSErrors Errors
        {
            get { 
                return errorsField; 
            }
            set {
                errorsField = value; 
            }
        }
    }


    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile", IsNullable = false)]
    public partial class AMA_ProfileReadRSErrors
    {

        private AMA_ProfileReadRSErrorsError errorField;

        /// <remarks/>
        public AMA_ProfileReadRSErrorsError Error
        {
            get
            {
                return this.errorField;
            }
            set
            {
                this.errorField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    public partial class AMA_ProfileReadRSErrorsError
    {

        private int typeField;

        private string shortTextField;

        private string codeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ShortText
        {
            get
            {
                return this.shortTextField;
            }
            set
            {
                this.shortTextField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }



    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    public partial class AMA_ProfileReadRSProfiles
    {

        private AMA_ProfileReadRSProfilesProfileInfo profileInfoField;

        /// <remarks/>
        public AMA_ProfileReadRSProfilesProfileInfo ProfileInfo
        {
            get
            {
                return this.profileInfoField;
            }
            set
            {
                this.profileInfoField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    public partial class AMA_ProfileReadRSProfilesProfileInfo
    {

        private AMA_ProfileReadRSProfilesProfileInfoUniqueID[] uniqueIDField;

        private AMA_ProfileReadRSProfilesProfileInfoProfile profileField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("UniqueID")]
        public AMA_ProfileReadRSProfilesProfileInfoUniqueID[] UniqueID
        {
            get
            {
                return this.uniqueIDField;
            }
            set
            {
                this.uniqueIDField = value;
            }
        }

        /// <remarks/>
        public AMA_ProfileReadRSProfilesProfileInfoProfile Profile
        {
            get
            {
                return this.profileField;
            }
            set
            {
                this.profileField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    public partial class AMA_ProfileReadRSProfilesProfileInfoUniqueID
    {

        private int typeField;

        private int instanceField;

        private bool instanceFieldSpecified;

        private string idField;

        private string iD_ContextField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int Instance
        {
            get
            {
                return this.instanceField;
            }
            set
            {
                this.instanceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InstanceSpecified
        {
            get
            {
                return this.instanceFieldSpecified;
            }
            set
            {
                this.instanceFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID_Context
        {
            get
            {
                return this.iD_ContextField;
            }
            set
            {
                this.iD_ContextField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    public partial class AMA_ProfileReadRSProfilesProfileInfoProfile
    {

        private AMA_ProfileReadRSProfilesProfileInfoProfileCustomer customerField;

        private int profileTypeField;

        /// <remarks/>
        public AMA_ProfileReadRSProfilesProfileInfoProfileCustomer Customer
        {
            get
            {
                return this.customerField;
            }
            set
            {
                this.customerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int ProfileType
        {
            get
            {
                return this.profileTypeField;
            }
            set
            {
                this.profileTypeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    public partial class AMA_ProfileReadRSProfilesProfileInfoProfileCustomer
    {

        private AMA_ProfileReadRSProfilesProfileInfoProfileCustomerPersonName personNameField;

        private AMA_ProfileReadRSProfilesProfileInfoProfileCustomerCustLoyalty custLoyaltyField;

        /// <remarks/>
        public AMA_ProfileReadRSProfilesProfileInfoProfileCustomerPersonName PersonName
        {
            get
            {
                return this.personNameField;
            }
            set
            {
                this.personNameField = value;
            }
        }

        /// <remarks/>
        public AMA_ProfileReadRSProfilesProfileInfoProfileCustomerCustLoyalty CustLoyalty
        {
            get
            {
                return this.custLoyaltyField;
            }
            set
            {
                this.custLoyaltyField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    public partial class AMA_ProfileReadRSProfilesProfileInfoProfileCustomerPersonName
    {

        private string givenNameField;

        private string surnameField;

        private string contextField;

        /// <remarks/>
        public string GivenName
        {
            get
            {
                return this.givenNameField;
            }
            set
            {
                this.givenNameField = value;
            }
        }

        /// <remarks/>
        public string Surname
        {
            get
            {
                return this.surnameField;
            }
            set
            {
                this.surnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string Context
        {
            get
            {
                return this.contextField;
            }
            set
            {
                this.contextField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    public partial class AMA_ProfileReadRSProfilesProfileInfoProfileCustomerCustLoyalty
    {

        private int loyalLevelField;

        private string loyalLevelDescriptionField;

        private string customerTypeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int LoyalLevel
        {
            get
            {
                return this.loyalLevelField;
            }
            set
            {
                this.loyalLevelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string LoyalLevelDescription
        {
            get
            {
                return this.loyalLevelDescriptionField;
            }
            set
            {
                this.loyalLevelDescriptionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string CustomerType
        {
            get
            {
                return this.customerTypeField;
            }
            set
            {
                this.customerTypeField = value;
            }
        }
    }


}
