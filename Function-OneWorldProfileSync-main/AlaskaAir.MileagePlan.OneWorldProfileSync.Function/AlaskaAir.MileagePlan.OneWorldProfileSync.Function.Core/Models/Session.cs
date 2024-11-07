namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models
{
    /// <remarks/>
    [System.Serializable()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd", IsNullable = false)]
    public partial class Session
    {

        private string sessionIdField;

        private int sequenceNumberField;

        private string securityTokenField;

        /// <remarks/>
        public string SessionId
        {
            get
            {
                return this.sessionIdField;
            }
            set
            {
                this.sessionIdField = value;
            }
        }

        /// <remarks/>
        public int SequenceNumber
        {
            get
            {
                return this.sequenceNumberField;
            }
            set
            {
                this.sequenceNumberField = value;
            }
        }

        /// <remarks/>
        public string SecurityToken
        {
            get
            {
                return this.securityTokenField;
            }
            set
            {
                this.securityTokenField = value;
            }
        }
    }
}
