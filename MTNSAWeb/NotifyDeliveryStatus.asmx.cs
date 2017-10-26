using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace MTNSAWeb
{
    /// <summary>
    /// Summary description for DeliveryStatus
    /// </summary>
    [WebService(Namespace = "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotifyDeliveryStatus : System.Web.Services.WebService
    {
        [XmlElement(Namespace = "http://www.huawei.com.cn/schema/common/v2_1")]
        public NotifySOAPHeader authCred;

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapHeader("authCred", Direction = SoapHeaderDirection.InOut)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        //[XmlInclude (typeof(UserID))]
        //[XmlInclude(typeof(NotifySOAPHeader))]
        [return: System.Xml.Serialization.XmlElementAttribute("notifySmsDeliveryReceiptResponse", Namespace = "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local")]
        public notifySmsDeliveryReceiptResponse getnotifySmsDeliveryReceiptResponse([System.Xml.Serialization.XmlElementAttribute("notifySmsDeliveryReceipt", Namespace = "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local")] notifySmsDeliveryReceipt notifySmsDeliveryReceipt1)
        {
            notifySmsDeliveryReceiptResponse val = new notifySmsDeliveryReceiptResponse();
            val.result = "Fail";
            if (notifySmsDeliveryReceipt1 != null)
            {
                val.result = "Good";
            }

            return val;
        }
    }

}
