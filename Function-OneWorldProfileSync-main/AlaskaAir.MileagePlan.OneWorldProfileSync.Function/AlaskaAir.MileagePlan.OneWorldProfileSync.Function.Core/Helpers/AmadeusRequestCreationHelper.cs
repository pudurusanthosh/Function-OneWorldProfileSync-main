using System;
using AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Models;
using Microsoft.Extensions.Configuration;

namespace AlaskaAir.MileagePlan.OneWorldProfileSync.Function.Core.Helpers
{
    public class AmadeusRequestCreationHelper : IAmadeusRequestCreationHelper
    {
        private readonly string _password;
        private readonly string _wsaTo;
        private readonly string _sourceOffice;
        private readonly string _originator;

        public AmadeusRequestCreationHelper(IConfigurationRoot config)
        {
            _password = config["amadeusPassword"];
            _wsaTo = config["wsaTo"];
            _sourceOffice = config["sourceOffice"];
            _originator = config["originator"];
        }

        public string Base64Encode(string stringToEncode)
        {
            byte[] encData_byte;
            encData_byte = System.Text.Encoding.UTF8.GetBytes(stringToEncode);
            return Convert.ToBase64String(encData_byte);
        }

        public string GenerateAuthenticationRequest()
        {
            return (@$"<SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ns1=""http://xml.amadeus.com/VLSSLQ_06_1_1A"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:awss=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
				        <SOAP-ENV:Header>
					        <wsa:To>{_wsaTo}</wsa:To>
					        <wsa:Action>http://webservices.amadeus.com/VLSSLQ_06_1_1A</wsa:Action>
					        <wsa:MessageID>{Guid.NewGuid().ToString("N")}</wsa:MessageID>
				        </SOAP-ENV:Header>
				        <SOAP-ENV:Body>
					        <ns1:Security_Authenticate>
						        <ns1:userIdentifier>
							        <ns1:originIdentification>
								        <ns1:sourceOffice>{_sourceOffice}</ns1:sourceOffice>
							        </ns1:originIdentification>
							        <ns1:originatorTypeCode>U</ns1:originatorTypeCode>
							        <ns1:originator>{_originator}</ns1:originator>
						        </ns1:userIdentifier>
						        <ns1:dutyCode>
							        <ns1:dutyCodeDetails>
								        <ns1:referenceQualifier>DUT</ns1:referenceQualifier>
								        <ns1:referenceIdentifier>SU</ns1:referenceIdentifier>
							        </ns1:dutyCodeDetails>
						        </ns1:dutyCode>
						        <ns1:passwordInfo>
							        <ns1:dataLength>{_password.Length}</ns1:dataLength>
							        <ns1:dataType>E</ns1:dataType>
							        <ns1:binaryData>{Base64Encode(_password)}</ns1:binaryData>
						        </ns1:passwordInfo>
					        </ns1:Security_Authenticate>
				        </SOAP-ENV:Body>
			        </SOAP-ENV:Envelope>");
        }

        public string GenerateProfileAirlineUpsert(Session _session, ProfilePublisherRequest _profilePublisherRequest)
        {
            var oneWorldMappedTier = MapToOneWorldTier(_profilePublisherRequest.OneWorldTier);

            return ($@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:sec=""http://xml.amadeus.com/2010/06/Security_v1"" xmlns:typ=""http://xml.amadeus.com/2010/06/Types_v1"" xmlns:iat=""http://www.iata.org/IATA/2007/00/IATA2010.1"" xmlns:app=""http://xml.amadeus.com/2010/06/AppMdw_CommonTypes_v3"" xmlns:link=""http://wsdl.amadeus.com/2010/06/ws/Link_v1"" xmlns:ses=""http://xml.amadeus.com/2010/06/Session_v3"" xmlns:prof=""http://xml.amadeus.com/2008/10/AMA/Profile"" xmlns:typ1=""http://xml.amadeus.com/2010/06/Types_v2"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:awss=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
                <soapenv:Header>
                    <h:Session xmlns=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:h=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
                        <SessionId>{_session.SessionId}</SessionId>
                        <SequenceNumber>{_session.SequenceNumber}</SequenceNumber>
                        <SecurityToken>{_session.SecurityToken}</SecurityToken>
                    </h:Session>
                    <wsa:To>{_wsaTo}</wsa:To>
                    <wsa:Action>http://webservices.amadeus.com/Profile_AirlineUpdate_12.2</wsa:Action>
                    <wsa:MessageID>{Guid.NewGuid().ToString("N")}</wsa:MessageID>
                </soapenv:Header>
                <soapenv:Body>
                    <prof:AMA_UpdateRQ Version=""12.3"" xmlns=""http://xml.amadeus.com/2008/10/AMA/Profile"">
                        <prof:Position XPath=""/AMA_UpdateRQ/Position"">
                            <prof:Root Operation=""createupdate"">
                                <UniqueID Type=""21"" ID_Context=""LOYALTYNUMBER"" ID=""{_profilePublisherRequest.MileagePlanNumber}""/>
                                <UniqueID Type=""9"" ID_Context=""AIRCODE"" ID=""AS""/>
                                <ExternalID Type=""1"" Instance=""{_profilePublisherRequest.ModificationNumber}"" ID_Context=""RemoteID"" ID=""{_profilePublisherRequest.MileagePlanNumber}""/>
                                <Profile ProfileType=""21"">
                                    <Customer>
                                        <PersonName>
                                            <GivenName>{_profilePublisherRequest.FirstName}</GivenName>
                                            <Surname>{_profilePublisherRequest.LastName}</Surname>
                                        </PersonName>
                                        <CustLoyalty CustomerType=""FREQ"" LoyalLevel=""{oneWorldMappedTier.LoyaltyLevel}"" LoyalLevelDescription=""{oneWorldMappedTier.Name}""/>
                                    </Customer>
                                </Profile>
                            </prof:Root>
                        </prof:Position>
                    </prof:AMA_UpdateRQ>
                </soapenv:Body>
            </soapenv:Envelope>");
        }

        public string GenerateProfileAirlineRefresh(Session _session, ProfilePublisherRequest _profilePublisherRequest)
        {
            var oneWorldMappedTier = MapToOneWorldTier(_profilePublisherRequest.OneWorldTier);

            return ($@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:sec=""http://xml.amadeus.com/2010/06/Security_v1"" xmlns:typ=""http://xml.amadeus.com/2010/06/Types_v1"" xmlns:iat=""http://www.iata.org/IATA/2007/00/IATA2010.1"" xmlns:app=""http://xml.amadeus.com/2010/06/AppMdw_CommonTypes_v3"" xmlns:link=""http://wsdl.amadeus.com/2010/06/ws/Link_v1"" xmlns:ses=""http://xml.amadeus.com/2010/06/Session_v3"" xmlns:prof=""http://xml.amadeus.com/2008/10/AMA/Profile"" xmlns:typ1=""http://xml.amadeus.com/2010/06/Types_v2"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:awss=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
                <soapenv:Header>
                    <h:Session xmlns=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:h=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
                        <SessionId>{_session.SessionId}</SessionId>
                        <SequenceNumber>{_session.SequenceNumber}</SequenceNumber>
                        <SecurityToken>{_session.SecurityToken}</SecurityToken>
                    </h:Session>
                    <wsa:To>{_wsaTo}</wsa:To>
                    <wsa:Action>http://webservices.amadeus.com/Profile_AirlineUpdate_12.2</wsa:Action>
                    <wsa:MessageID>{Guid.NewGuid().ToString("N")}</wsa:MessageID>
                </soapenv:Header>
                <soapenv:Body>
                    <prof:AMA_UpdateRQ Version=""12.3"" xmlns=""http://xml.amadeus.com/2008/10/AMA/Profile"">
                        <prof:Position XPath=""/AMA_UpdateRQ/Position"">
                            <prof:Root Operation=""refresh"">
                                <UniqueID Type=""21"" ID_Context=""LOYALTYNUMBER"" ID=""{_profilePublisherRequest.MileagePlanNumber}""/>
                                <UniqueID Type=""9"" ID_Context=""AIRCODE"" ID=""AS""/>
                                <ExternalID Type=""1"" Instance=""{_profilePublisherRequest.ModificationNumber}"" ID_Context=""ProfileRemoteID"" ID=""{_profilePublisherRequest.MileagePlanNumber}""/>
                                <Profile ProfileType=""21"">
                                    <Customer>
                                        <PersonName>
                                            <GivenName>{_profilePublisherRequest.FirstName}</GivenName>
                                            <Surname>{_profilePublisherRequest.LastName}</Surname>
                                        </PersonName>
                                        <CustLoyalty CustomerType=""FREQ"" LoyalLevel=""{oneWorldMappedTier.LoyaltyLevel}"" LoyalLevelDescription=""{oneWorldMappedTier.Name}""/>
                                    </Customer>
                                </Profile>
                            </prof:Root>
                        </prof:Position>
                    </prof:AMA_UpdateRQ>
                </soapenv:Body>
            </soapenv:Envelope>");
        }

        public string GenerateProfileDelete(Session session, ProfilePublisherRequest profilePublisherRequest)
        {
            return ($@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:sec=""http://xml.amadeus.com/2010/06/Security_v1"" xmlns:typ=""http://xml.amadeus.com/2010/06/Types_v1"" xmlns:iat=""http://www.iata.org/IATA/2007/00/IATA2010.1"" xmlns:app=""http://xml.amadeus.com/2010/06/AppMdw_CommonTypes_v3"" xmlns:link=""http://wsdl.amadeus.com/2010/06/ws/Link_v1"" xmlns:ses=""http://xml.amadeus.com/2010/06/Session_v3"" xmlns:prof=""http://xml.amadeus.com/2008/10/AMA/Profile"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:awss=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
                        <soapenv:Header>
                            <h:Session xmlns=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:h=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
                                <SessionId>{session.SessionId}</SessionId>
                                <SequenceNumber>{session.SequenceNumber}</SequenceNumber>
                                <SecurityToken>{session.SecurityToken}</SecurityToken>
                            </h:Session>
                            <wsa:To>{_wsaTo}</wsa:To>
                            <wsa:Action>http://webservices.amadeus.com/Profile_AirlineDelete_12.2</wsa:Action>
                            <wsa:MessageID>{Guid.NewGuid().ToString("N")}</wsa:MessageID>
                        </soapenv:Header>
                        <soapenv:Body>
                            <prof:AMA_DeleteRQ Version=""12.3"" xmlns=""http://xml.amadeus.com/2008/10/AMA/Profile"">
                                <UniqueID Type=""21"" ID_Context=""LOYALTYNUMBER"" ID=""{profilePublisherRequest.MileagePlanNumber}""/>
                                <UniqueID Type=""9"" ID_Context=""AIRCODE"" ID=""AS""/>
                                <ExternalID Type=""1"" ID_Context=""RemoteID"" ID=""{profilePublisherRequest.MileagePlanNumber}""/>
                                <prof:DeleteRequests>
                                    <prof:ProfileDeleteRequest ProfileType=""21""/>
                                </prof:DeleteRequests>
                            </prof:AMA_DeleteRQ>
                        </soapenv:Body>
                    </soapenv:Envelope>");
        }

        public string GenerateSignoutRequest(Session session)
        {
            return ($@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:sec=""http://xml.amadeus.com/2010/06/Security_v1"" xmlns:typ=""http://xml.amadeus.com/2010/06/Types_v1"" xmlns:iat=""http://www.iata.org/IATA/2007/00/IATA2010.1"" xmlns:app=""http://xml.amadeus.com/2010/06/AppMdw_CommonTypes_v3"" xmlns:link=""http://wsdl.amadeus.com/2010/06/ws/Link_v1"" xmlns:ses=""http://xml.amadeus.com/2010/06/Session_v3"" xmlns:vls=""http://xml.amadeus.com/VLSSOQ_04_1_1A"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:awss=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
                        <soapenv:Header>
                            <ses:Session>
                                <SessionId>{session.SessionId}</SessionId>
                                <SequenceNumber>{session.SequenceNumber}</SequenceNumber>
                                <SecurityToken>{session.SecurityToken}</SecurityToken>
                            </ses:Session>
                            <wsa:To>{_wsaTo}</wsa:To>
                            <wsa:Action>http://webservices.amadeus.com/VLSSOQ_04_1_1A</wsa:Action>
                            <wsa:MessageID>{Guid.NewGuid().ToString("N")}</wsa:MessageID>
                        </soapenv:Header>
                        <soapenv:Body>
                            <vls:Security_SignOut>
                            </vls:Security_SignOut>
                        </soapenv:Body>
                    </soapenv:Envelope>");
        }

        public OneWorldTierMapping MapToOneWorldTier(string oneWorldTier)
        {
            oneWorldTier = oneWorldTier.ToUpper();

            if (oneWorldTier == "RUBY")
            {
                return new OneWorldTierMapping
                {
                    Name = "RUBY",                    
                    LoyaltyLevel = 1
                };
            }            
            else if (oneWorldTier == "SAPPHIRE")
            {
                return new OneWorldTierMapping
                {
                    Name = "SAPH",
                    LoyaltyLevel = 2
                };
            }
            else if (oneWorldTier == "EMERALD") 
            {
                return new OneWorldTierMapping
                {
                    Name = "EMER",
                    LoyaltyLevel = 3
                };
            }
            else
            {
                throw new InvalidOperationException($"OneWorldTier is not a valid:{oneWorldTier}");
            }
        }

        public string GenerateProfileRetrieveRequest(Session session, ProfilePublisherRequest profilePublisherRequest)
        {
            return ($@"<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:sec=""http://xml.amadeus.com/2010/06/Security_v1"" xmlns:typ=""http://xml.amadeus.com/2010/06/Types_v1"" xmlns:iat=""http://www.iata.org/IATA/2007/00/IATA2010.1"" xmlns:app=""http://xml.amadeus.com/2010/06/AppMdw_CommonTypes_v3"" xmlns:link=""http://wsdl.amadeus.com/2010/06/ws/Link_v1"" xmlns:ses=""http://xml.amadeus.com/2010/06/Session_v3"" xmlns:prof=""http://xml.amadeus.com/2008/10/AMA/Profile"" xmlns:typ1=""http://xml.amadeus.com/2010/06/Types_v2"" xmlns:wsa=""http://www.w3.org/2005/08/addressing"" xmlns:awss=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
                       <soapenv:Header>
                          <h:Session xmlns=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:h=""http://xml.amadeus.com/ws/2009/01/WBS_Session-2.0.xsd"">
                             <SessionId>{session.SessionId}</SessionId>
                             <SequenceNumber>{session.SequenceNumber}</SequenceNumber>
                             <SecurityToken>{session.SecurityToken}</SecurityToken>
                          </h:Session>
                          <wsa:To>{_wsaTo}</wsa:To>
                          <wsa:Action>http://webservices.amadeus.com/Profile_AirlineRetrieve_12.2</wsa:Action>
                          <wsa:MessageID>{Guid.NewGuid().ToString("N")}</wsa:MessageID>
                       </soapenv:Header>
                       <soapenv:Body>
		                    <prof:AMA_ProfileReadRQ Version=""12.3"" xmlns=""http://xml.amadeus.com/2008/10/AMA/Profile"">
			                    <prof:UniqueID Type=""9"" ID_Context=""AIRCODE"" ID=""AS""/>
			                    <prof:UniqueID Type=""21"" ID_Context=""LOYALTYNUMBER"" ID=""{profilePublisherRequest.MileagePlanNumber}""/>
			                    <prof:ExternalID Type=""1"" ID_Context=""RemoteID"" ID=""{profilePublisherRequest.MileagePlanNumber}""/>
			                    <prof:ReadRequests>
				                    <prof:ProfileReadRequest ProfileType=""21"">
				                    </prof:ProfileReadRequest>
			                    </prof:ReadRequests>
		                    </prof:AMA_ProfileReadRQ>		
	                    </soapenv:Body>
                    </soapenv:Envelope>");
        }
    }
}
