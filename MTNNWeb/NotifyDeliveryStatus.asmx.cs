using System;
using System.Collections.Generic;
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
    [WebService(Namespace = "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class NotifyDeliveryStatus : System.Web.Services.WebService
    {
        SyncOrderEntities db = new SyncOrderEntities(); string slastStatus = "";

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
                mmSender mm = new mmSender();//msgInx.PadLeft(10, '0');

                mm = db.mmSenders.AsEnumerable().FirstOrDefault(f => f.mIndex.GetValueOrDefault().ToString().PadLeft(10, '0') == notifySmsDeliveryReceipt1.correlator);
                if (mm != null && mm.ID > 0)
                {
                    slastStatus =Enum.GetName(typeof(SVC.DeliveryStatus),notifySmsDeliveryReceipt1.deliveryStatus.deliveryStatus).ToString();
                    if (slastStatus != "DeliveredToTerminal")
                    {
                        mm.sErrorDesc = slastStatus;
                        mm.mState = 0;
                        mm.mStatus = -1;
                        mm.sLastStatus = slastStatus;
                        mm.sLastTimeStamp = DateTime.Now.ToString();
                    }
                    else
                        mm.sErrorDesc = slastStatus;

                    db.SaveChanges();
                }
                
            }

            return val;
        }
    }

}
