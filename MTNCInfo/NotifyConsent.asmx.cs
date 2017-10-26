using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace MTNCInfo
{
    /// <summary>
    /// Summary description for DeliveryStatus
    /// </summary>
    [WebService(Namespace = "http://www.csapi.org/wsdl/subscriberconsnet/data/v1_0/service")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotifyConsent : System.Web.Services.WebService
    {
        [XmlElement(Namespace = "http://www.huawei.com.cn/schema/common/v2_1")]
        public NotifySOAPHeader authCred;

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapHeader("authCred", Direction = SoapHeaderDirection.InOut)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        //[XmlInclude (typeof(UserID))]
        //[XmlInclude(typeof(NotifySOAPHeader))]
        [return: System.Xml.Serialization.XmlElementAttribute("notifySubscriberConsentResultResponse", Namespace = "http://www.csapi.org/schema/subscriberconsnet/data/v1_0/local")]
        public notifySubscriberConsentResultResponse getnotifySmsDeliveryReceiptResponse([System.Xml.Serialization.XmlElementAttribute("notifySubscriberConsentResult", Namespace = "http://www.csapi.org/schema/subscriberconsnet/data/v1_0/local")] notifySubscriberConsentResult notifySmsDeliveryReceipt1)
        {
            notifySubscriberConsentResultResponse val = new notifySubscriberConsentResultResponse();
            string msisdn = "", token = "",accesstoken="", serviceId = "", tokenExpiryTime = "";
            int conResul = 0, tokenType = 0, capType = 0; DateTime dtokenExpiryTime;
            val.result = 3;
            val.resultDescription = "Send request to Partner failed";

            SyncOrderEntities db = new SyncOrderEntities();

            try
            {
                if (notifySmsDeliveryReceipt1 != null)
                {

                    token = notifySmsDeliveryReceipt1.accessToken;   //11/01/2016 token = notifySmsDeliveryReceipt1.accessToken; 
                    //accesstoken = notifySmsDeliveryReceipt1.accessToken;
                    msisdn = notifySmsDeliveryReceipt1.subscriberID.ID;
                    serviceId = string.IsNullOrEmpty(notifySmsDeliveryReceipt1.serviceID) ? AppUtil.serviceId : notifySmsDeliveryReceipt1.serviceID;
                    AppUtil.TestMode = db.SyncOrderProducts.FirstOrDefault(f => f.ServiceID == serviceId).TestMode.GetValueOrDefault();
                    conResul = notifySmsDeliveryReceipt1.consentResult; // 0:Approved 1:Reject
                    //0=One-off Token, namely it can only be used for one time. 1=Token with limited life cycle. Once the life time expired, the token will be revoked.
                    tokenType = Convert.ToInt32(notifySmsDeliveryReceipt1.tokenType);
                    capType = notifySmsDeliveryReceipt1.capabilityType; //17=Payment //1 =SMS
                    tokenExpiryTime = notifySmsDeliveryReceipt1.tokenExpiryTime; //yyyy-MM-dd hh:mm:ss
                    if (capType == 17 && string.IsNullOrEmpty(tokenExpiryTime)==false && tokenType ==0) //tokenType changed to 0 04/01/2016
                    {
                        //DateTime.TryParse(tokenExpiryTime, out dtokenExpiryTime)
                        if (DateTime.TryParseExact(tokenExpiryTime,AppUtil.TimeFormat,CultureInfo.InvariantCulture,System.Globalization.DateTimeStyles.None, out dtokenExpiryTime))//yyyy-MM-dd hh:mm:ss 
                        {
                            ObjectParameter rspOut = new ObjectParameter("refKey", typeof(string));
                            var it = db.otpsub(token, msisdn, serviceId, serviceId, dtokenExpiryTime, rspOut);
                            
                            if (rspOut.Value.ToString() == "0")
                            {
                                val.result = 0;
                                val.resultDescription = "Sucess";
                            }
                            else
                            {
                                val.result = 2;
                                val.resultDescription = rspOut.Value.ToString();
                            }
                        }
                        else
                        {
                            val.result = 2;
                            val.resultDescription = "Invalid Parameter: Wrong Token Expiry Time";
                        }
                    }
                    else
                    {
                        val.result = 2;
                        val.resultDescription = "Invalid Parameter: ";
                        val.resultDescription = val.resultDescription + ((capType != 17) ? "Wrong Capability Type" : (tokenType != 0) ? "Wrong Token Type" : "Token Expiry Not specified");
                    }


                }
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.ToString() + ((ex.InnerException != null) ? ex.InnerException.ToString() : ""));
                val.result = 4;
                val.resultDescription = "Server Error: " + ex.Message;
            }

            return val;

            //0==Success.
            //1=Auth failed.
            //2=Invalid Parameter: %1.(%1 indicates parameter name.)
            //3=Send request to Partner failed.
            //4=System Error. Caused by %1.(%1 indicates detail information.)
        }


        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/subscriberconsnet/data/v1_0/local")]
        public partial class notifySubscriberConsentResultResponse
        {

            private int resultField;

            private string resultDescriptionField;

            private NamedParameter[] extensionInfoField;

            /// <remarks/>
            public int result
            {
                get
                {
                    return this.resultField;
                }
                set
                {
                    this.resultField = value;
                }
            }

            /// <remarks/>
            public string resultDescription
            {
                get
                {
                    return this.resultDescriptionField;
                }
                set
                {
                    this.resultDescriptionField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("item", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
            public NamedParameter[] extensionInfo
            {
                get
                {
                    return this.extensionInfoField;
                }
                set
                {
                    this.extensionInfoField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/subscriberconsnet/data/v1_0/local")]
        public partial class notifySubscriberConsentResult
        {

            private subscriberID subscriberIDField;

            private string partnerIDField;

            private string serviceIDField;

            private int capabilityTypeField;

            private int consentResultField;

            private string accessTokenField;

            private string oauth_tokenField;

            private TokenType tokenTypeField;

            private bool tokenTypeFieldSpecified;

            private string tokenExpiryTimeField;

            private NamedParameter[] extensionInfoField;

            /// <remarks/>
            public subscriberID subscriberID
            {
                get
                {
                    return this.subscriberIDField;
                }
                set
                {
                    this.subscriberIDField = value;
                }
            }

            /// <remarks/>
            public string partnerID
            {
                get
                {
                    return this.partnerIDField;
                }
                set
                {
                    this.partnerIDField = value;
                }
            }

            /// <remarks/>
            public string serviceID
            {
                get
                {
                    return this.serviceIDField;
                }
                set
                {
                    this.serviceIDField = value;
                }
            }

            /// <remarks/>
            public int capabilityType
            {
                get
                {
                    return this.capabilityTypeField;
                }
                set
                {
                    this.capabilityTypeField = value;
                }
            }

            /// <remarks/>
            public int consentResult
            {
                get
                {
                    return this.consentResultField;
                }
                set
                {
                    this.consentResultField = value;
                }
            }

            /// <remarks/>
            public string accessToken
            {
                get
                {
                    return this.accessTokenField;
                }
                set
                {
                    this.accessTokenField = value;
                }
            }


            public string oauth_token  // 11/01/2016
            {
                get
                {
                    return this.oauth_tokenField;
                }
                set
                {
                    this.oauth_tokenField = value;
                }
            }

            /// <remarks/>
            public TokenType tokenType
            {
                get
                {
                    return this.tokenTypeField;
                }
                set
                {
                    this.tokenTypeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool tokenTypeSpecified
            {
                get
                {
                    return this.tokenTypeFieldSpecified;
                }
                set
                {
                    this.tokenTypeFieldSpecified = value;
                }
            }

            /// <remarks/>
            public string tokenExpiryTime
            {
                get
                {
                    return this.tokenExpiryTimeField;
                }
                set
                {
                    this.tokenExpiryTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("item", Form = System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable = false)]
            public NamedParameter[] extensionInfo
            {
                get
                {
                    return this.extensionInfoField;
                }
                set
                {
                    this.extensionInfoField = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/subscriberconsnet/data/v1_0")]
        public partial class subscriberID
        {

            private string idField;

            private int typeField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
            [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
            public int type
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
        }



        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
        [System.SerializableAttribute()]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/subscriberconsnet/data/v1_0")]
        public enum TokenType
        {

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("0")]
            Item0,

            /// <remarks/>
            [System.Xml.Serialization.XmlEnumAttribute("1")]
            Item1,
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
        public delegate void notifySubscriberConsentResultCompletedEventHandler(object sender, notifySubscriberConsentResultCompletedEventArgs e);

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        public partial class notifySubscriberConsentResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
        {

            private object[] results;

            internal notifySubscriberConsentResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
            {
                this.results = results;
            }

            /// <remarks/>
            public notifySubscriberConsentResultResponse Result
            {
                get
                {
                    this.RaiseExceptionIfNecessary();
                    return ((notifySubscriberConsentResultResponse)(this.results[0]));
                }
            }
        }

    }

}
