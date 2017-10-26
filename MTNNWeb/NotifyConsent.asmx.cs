using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace MTNNWeb
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
        public OTPSVC.notifySubscriberConsentResultResponse getnotifySmsDeliveryReceiptResponse([System.Xml.Serialization.XmlElementAttribute("notifySubscriberConsentResult", Namespace = "http://www.csapi.org/schema/subscriberconsnet/data/v1_0/local")] OTPSVC.notifySubscriberConsentResult notifySmsDeliveryReceipt1)
        {
            OTPSVC.notifySubscriberConsentResultResponse val = new OTPSVC.notifySubscriberConsentResultResponse();
            string msisdn = "", token = "", serviceId = "", tokenExpiryTime = "";
            int conResul = 0, tokenType = 0, capType = 0; DateTime dtokenExpiryTime;
            val.result = 3;
            val.resultDescription = "Send request to Partner failed";

            SyncOrderEntities db = new SyncOrderEntities();

            try
            {
                if (notifySmsDeliveryReceipt1 != null)
                {

                    token = notifySmsDeliveryReceipt1.accessToken;
                    msisdn = notifySmsDeliveryReceipt1.subscriberID.ID;
                    serviceId = notifySmsDeliveryReceipt1.serviceID;
                    AppUtil.TestMode = db.SyncOrderProducts.FirstOrDefault(f => f.ServiceID == serviceId).TestMode.GetValueOrDefault();
                    conResul = notifySmsDeliveryReceipt1.consentResult; // 0:Approved 1:Reject
                    //0=One-off Token, namely it can only be used for one time. 1=Token with limited life cycle. Once the life time expired, the token will be revoked.
                    tokenType = Convert.ToInt32(notifySmsDeliveryReceipt1.tokenType);
                    capType = notifySmsDeliveryReceipt1.capabilityType; //17=Payment //1 =SMS
                    tokenExpiryTime = notifySmsDeliveryReceipt1.tokenExpiryTime; //yyyy-MM-dd hh:mm:ss
                    if (capType == 17 && string.IsNullOrEmpty(tokenExpiryTime)==false && tokenType ==1)
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
                        val.resultDescription = val.resultDescription + ((capType != 17) ? "Wrong Capability Type" : (tokenType != 1) ? "Wrong Token Type" : "Token Expiry Not specified");
                    }


                }
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.ToString());
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
    }

}
