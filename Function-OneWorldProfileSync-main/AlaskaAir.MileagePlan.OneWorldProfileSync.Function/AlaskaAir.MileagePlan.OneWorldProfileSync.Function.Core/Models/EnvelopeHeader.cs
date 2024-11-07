namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models
{
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategory("code")]
    [System.Xml.Serialization.XmlType(AnonymousType = true, Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public partial class EnvelopeHeader
    {

        private Session sessionField;

        private string toField;

        private From fromField;

        private string actionField;

        private string messageIDField;

        private RelatesTo relatesToField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd")]
        public Session Session
        {
            get
            {
                return sessionField;
            }
            set
            {
                sessionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
        public string To
        {
            get
            {
                return toField;
            }
            set
            {
                toField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
        public From From
        {
            get
            {
                return fromField;
            }
            set
            {
                fromField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
        public string Action
        {
            get
            {
                return actionField;
            }
            set
            {
                actionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
        public string MessageID
        {
            get
            {
                return messageIDField;
            }
            set
            {
                messageIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement(Namespace = "http://www.w3.org/2005/08/addressing")]
        public RelatesTo RelatesTo
        {
            get
            {
                return relatesToField;
            }
            set
            {
                relatesToField = value;
            }
        }
    }
}
