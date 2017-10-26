using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace MTNNSub
{
    /// <summary>
    /// Summary description for DeliveryStatus
    /// </summary>
    [WebService(Namespace = "http://www.csapi.org/wsdl/subscriberconsnet/data/v1_0/service")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotifyReceiptStatus : System.Web.Services.WebService
    {
        [XmlElement(Namespace = "http://www.huawei.com.cn/schema/common/v2_1")]
        public NotifySOAPHeader authCred { get; set; }

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapHeader("authCred", Direction = SoapHeaderDirection.InOut)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        //[XmlInclude (typeof(UserID))]
        //[XmlInclude(typeof(NotifySOAPHeader))]
        [return: System.Xml.Serialization.XmlElementAttribute("notifySmsReceptionResponse", Namespace = "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local")]
        public notifySmsReceptionResponse getnotifySmsDeliveryReceiptResponse([System.Xml.Serialization.XmlElementAttribute("notifySmsReception", Namespace = "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local")] notifySmsReception notifySmsReception1)
        {
            notifySmsReceptionResponse val = new notifySmsReceptionResponse();
            string msisdn = "", message = "", serviceId = "", shortcode = "", exMessage = "";
            int conErr = 0;
            
            SyncOrderEntities db = new SyncOrderEntities();

            try
            {
                if (notifySmsReception1 != null)
                {
                    //if (authCred == null)
                    //    authCred = getAuth();
                    message = notifySmsReception1.message.message;
                    msisdn = notifySmsReception1.message.senderAddress;
                    if (string.IsNullOrEmpty(msisdn)==false)
                        msisdn = (msisdn.ToLower().StartsWith("tel:") | msisdn.Contains(":")) ? msisdn.Split(':')[1].Trim() : msisdn;

                    serviceId = (authCred == null) ? notifySmsReception1.correlator : authCred.serviceId;
                    AppUtil.TestMode = db.SyncOrderProducts.FirstOrDefault(f => f.ServiceID == serviceId).TestMode.GetValueOrDefault();

                    shortcode = notifySmsReception1.message.smsServiceActivationNumber;
                    if (string.IsNullOrEmpty(shortcode) == false)
                        shortcode = (shortcode.ToLower().StartsWith("tel:") | shortcode.Contains(":")) ? shortcode.Split(':')[1].Trim() : shortcode;

                    if (string.IsNullOrEmpty(message) == false && string.IsNullOrEmpty(msisdn) == false && string.IsNullOrEmpty(shortcode) == false)
                    {
                        ObjectParameter rspOut = new ObjectParameter("result", typeof(string));
                        var it = db.receive(message, msisdn, shortcode, serviceId, rspOut);
                        if (rspOut.Value.ToString() == "0000" | rspOut.Value.ToString() == "0")
                        {
                            //success
                        }
                        else
                        {
                            conErr = 1;

                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        conErr = 2;

                    }


                }
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.ToString());
                exMessage = ex.Message;
                conErr = 3;
            }

            if (conErr == 1)
            {
                SoapException soa = new SoapException("Internal Error", new System.Xml.XmlQualifiedName("ServiceEror"));
                throw soa;

            }
            if (conErr == 2)
            {
                SoapException soa = new SoapException("Invalid parameters", new System.Xml.XmlQualifiedName("PolicyException"));
                throw soa;

            }
            if (conErr == 3)
            {
                SoapException soa = new SoapException("Exception: " + exMessage, new System.Xml.XmlQualifiedName("ServiceException"));
                throw soa;
            }


            return val;

            //0==Success.
            //1=Auth failed.
            //2=Invalid Parameter: %1.(%1 indicates parameter name.)
            //3=Send request to Partner failed.
            //4=System Error. Caused by %1.(%1 indicates detail information.)
        }

        private NotifySOAPHeader getAuth()
        {
            NotifySOAPHeader apiHeader = new NotifySOAPHeader();
            for (int i = 0; i < System.ServiceModel.OperationContext.Current.IncomingMessageHeaders.Count; i++)
            {
                System.ServiceModel.Channels.MessageHeaderInfo h = System.ServiceModel.OperationContext.Current.IncomingMessageHeaders[i];

                // For any reference parameters with the correct name.
                if (h.Name == "spId")
                {
                    // Read the value of that header.
                    XmlReader xr = OperationContext.Current.IncomingMessageHeaders.GetReaderAtHeader(i);
                    xr.ReadToFollowing("spId");
                    apiHeader.spId = xr.ReadElementContentAsString();
                }
                if (h.Name == "serviceId")
                {
                    // Read the value of that header.
                    XmlReader xr = OperationContext.Current.IncomingMessageHeaders.GetReaderAtHeader(i);
                    xr.ReadToFollowing("serviceId");
                    apiHeader.serviceId = xr.ReadElementContentAsString();
                }
                if (h.Name == "linkid")
                {
                    // Read the value of that header.
                    XmlReader xr = OperationContext.Current.IncomingMessageHeaders.GetReaderAtHeader(i);
                    xr.ReadToFollowing("linkid");
                    apiHeader.linkid = xr.ReadElementContentAsString();
                }
                if (h.Name == "traceUniqueID")
                {
                    // Read the value of that header.
                    XmlReader xr = OperationContext.Current.IncomingMessageHeaders.GetReaderAtHeader(i);
                    xr.ReadToFollowing("traceUniqueID");
                    apiHeader.traceUniqueID = xr.ReadElementContentAsString();
                }
                if (h.Name == "OperatorID")
                {
                    // Read the value of that header.
                    XmlReader xr = OperationContext.Current.IncomingMessageHeaders.GetReaderAtHeader(i);
                    xr.ReadToFollowing("OperatorID");
                    apiHeader.OperatorID = xr.ReadElementContentAsString();
                }
            }
            return apiHeader;

        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        //[System.Web.Services.Protocols.SoapHeader("authCred", Direction = SoapHeaderDirection.InOut)]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local")]
        public partial class notifySmsReception
        {

            private string correlatorField;

            private SmsMessage messageField;

            /// <remarks/>
            public string correlator
            {
                get
                {
                    return this.correlatorField;
                }
                set
                {
                    this.correlatorField = value;
                }
            }

            /// <remarks/>
            public SmsMessage message
            {
                get
                {
                    return this.messageField;
                }
                set
                {
                    this.messageField = value;
                }
            }
        }


        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/sms/v2_2")]
        public partial class SmsMessage
        {

            private string messageField;

            private string senderAddressField;

            private string smsServiceActivationNumberField;

            private System.DateTime dateTimeField;

            private bool dateTimeFieldSpecified;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
            public string message
            {
                get
                {
                    return this.messageField;
                }
                set
                {
                    this.messageField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "anyURI")]
            public string senderAddress
            {
                get
                {
                    return this.senderAddressField;
                }
                set
                {
                    this.senderAddressField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "anyURI")]
            public string smsServiceActivationNumber
            {
                get
                {
                    return this.smsServiceActivationNumberField;
                }
                set
                {
                    this.smsServiceActivationNumberField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
            public System.DateTime dateTime
            {
                get
                {
                    return this.dateTimeField;
                }
                set
                {
                    this.dateTimeField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlIgnoreAttribute()]
            public bool dateTimeSpecified
            {
                get
                {
                    return this.dateTimeFieldSpecified;
                }
                set
                {
                    this.dateTimeFieldSpecified = value;
                }
            }
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
        [System.SerializableAttribute()]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local")]
        public partial class notifySmsReceptionResponse
        {
        }

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
        public delegate void notifySmsReceptionCompletedEventHandler(object sender, notifySmsReceptionCompletedEventArgs e);

        /// <remarks/>
        [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.34209")]
        [System.Diagnostics.DebuggerStepThroughAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        public partial class notifySmsReceptionCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
        {

            private object[] results;

            internal notifySmsReceptionCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
            {
                this.results = results;
            }

            /// <remarks/>
            public notifySmsReceptionResponse Result
            {
                get
                {
                    this.RaiseExceptionIfNecessary();
                    return ((notifySmsReceptionResponse)(this.results[0]));
                }
            }
        }
    }

}
