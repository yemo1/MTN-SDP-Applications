using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MTNNWeb
{
    public partial class StartSMS : System.Web.UI.Page
    {
        SyncOrderEntities db = new SyncOrderEntities();
        string result = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string svcID = (Request.QueryString["svcID"] != null) ? Request.QueryString["svcID"].ToString() : "",
                    op = (Request.QueryString["op"] != null) ? Request.QueryString["op"].ToString() : "";

                if (string.IsNullOrEmpty(op) && (new List<string> { "0", "1" }).Contains(op.ToLower()) == false)
                {
                    result = "Empty/Invalid Operation";
                }
                else
                {
                    if (string.IsNullOrEmpty(svcID))
                    {
                        result = "Empty Service ID";
                    }
                    else
                    {
                        SyncOrderProduct sncProduct = new SyncOrderProduct();
                        sncProduct = db.SyncOrderProducts.FirstOrDefault(f => f.ServiceID == svcID);
                        AppUtil.TestMode = sncProduct.TestMode.GetValueOrDefault();
                        AppUtil.Accesscode = sncProduct.AccessCode;

                        RequestSOAPHeader authentication = new RequestSOAPHeader();
                        string timestamp = AppUtil.GetTimeStamp();
                        authentication.spId = AppUtil.SPID;
                        authentication.spPassword = AppUtil.GetMD5Hash(AppUtil.SPID + AppUtil.Password + timestamp);
                        authentication.serviceId = svcID;
                        authentication.timeStamp = timestamp;


                        //temp
                        //authentication.spId = "601425";
                        //authentication.spPassword = GetMD5Hash("601425" + "Bvp@5560" + timestamp);
                        //authentication.serviceId = "6014252000001754";
                        //temp

                        SNSVC.SmsNotificationManagerService svc = new SNSVC.SmsNotificationManagerService();
                        svc.authCred = authentication;

                        SNSVC.SimpleReference simple = new SNSVC.SimpleReference();
                        simple.endpoint = AppUtil.SMSReceptionURL;
                        simple.interfaceName = "notifySmsReception";
                        simple.correlator = svcID.PadLeft(15, '0');

                        if (op.ToLower() == "1")
                        {
                            SNSVC.startSmsNotification msg = new SNSVC.startSmsNotification();
                            msg.criteria = "";
                            msg.smsServiceActivationNumber = AppUtil.Accesscode;
                            msg.reference = simple;


                            SNSVC.startSmsNotificationResponse res = new SNSVC.startSmsNotificationResponse();
                            res = svc.startSmsNotification(msg);
                            result = res.ToString();

                        }
                        else
                        {
                            SNSVC.stopSmsNotification msg = new SNSVC.stopSmsNotification();
                            msg.correlator = svcID;


                            SNSVC.stopSmsNotificationResponse res = new SNSVC.stopSmsNotificationResponse();
                            res = svc.stopSmsNotification(msg);
                            result = res.ToString();
                        }

                        result = "OK";
                    }
                }
            }
            catch (SoapException sex)
            {
                // The variable 'sex' can access the exception's information.
                //AppUtil.LogFileWrite(sex.ToString());
                result = "Soap Error";
                AppUtil.LogDBWrite(sex.Detail.InnerXml.ToString(), "Response");
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.ToString());
                result = "Error";
            }

            Response.Write(result);
        }

    }
}