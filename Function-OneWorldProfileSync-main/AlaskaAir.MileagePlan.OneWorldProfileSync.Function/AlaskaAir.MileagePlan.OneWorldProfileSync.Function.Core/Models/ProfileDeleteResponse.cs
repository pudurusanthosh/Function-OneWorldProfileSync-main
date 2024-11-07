
namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models
{
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    [System.Xml.Serialization.XmlRootAttribute(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/", IsNullable = false)]
    public partial class ProfileDeleteResponse
    {

        private EnvelopeHeader headerField;

        private ProfileDeleteEnvelopeBody bodyField;

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
        public ProfileDeleteEnvelopeBody Body
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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName = "EnvelopeBody", AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class ProfileDeleteEnvelopeBody
    {

        private AMA_DeleteRS aMA_DeleteRSField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
        public AMA_DeleteRS AMA_DeleteRS
        {
            get
            {
                return this.aMA_DeleteRSField;
            }
            set
            {
                this.aMA_DeleteRSField = value;
            }
        }
    }

    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile", IsNullable = false)]
    public partial class AMA_DeleteRS
    {

        private object successField;

        private AMA_DeleteRSUniqueID[] uniqueIDField;

        private AMA_UpdateRSErrors errorsField;

        private decimal versionField;

        [System.Xml.Serialization.XmlElement(ElementName = "Errors")]
        public AMA_UpdateRSErrors Errors
        {
            get
            {
                return this.errorsField;
            }
            set
            {
                this.errorsField = value;
            }
        }

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
        [System.Xml.Serialization.XmlElementAttribute("UniqueID")]
        public AMA_DeleteRSUniqueID[] UniqueID
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/2008/10/AMA/Profile")]
    public partial class AMA_DeleteRSUniqueID
    {

        private int typeField;

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
}
