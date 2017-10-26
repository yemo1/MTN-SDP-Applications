using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MTNNSub
{
    public partial class AuthWeb : System.Web.UI.Page
    {
        SyncOrderEntities db = new SyncOrderEntities();
        string result = ""; int amt = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //http://368795-web2/MTNNSub/AuthWeb.aspx?sID='+@svcID+'&pID='+@pID+'&phone='+@MSISDN+'&amt='+CAST(@ServiceCharge as varchar)
                if (Request.QueryString["amt"] != null)
                    if (int.TryParse(Request.QueryString["amt"].ToString(), out amt))
                        amt = amt * 1;
                    else
                        amt = -1;
                else amt = -1;
                string pID = (Request.QueryString["pID"] != null) ? Request.QueryString["pID"].ToString() : "",
                    phone = "", //(Request.QueryString["phone"] != "") ? Request.QueryString["phone"].ToString() :
                    svID = (Request.QueryString["sID"] != null) ? Request.QueryString["sID"].ToString() : "";
                if (string.IsNullOrEmpty(svID))
                {
                    result = "Empty Service ID";
                }
                else
                {
                    if (string.IsNullOrEmpty(pID))
                    {
                        result = "Empty Product ID";
                    }
                    else
                    {
                        //string.IsNullOrEmpty(svcID) | string.IsNullOrEmpty(txtMsg) | string.IsNullOrEmpty(phone)
                        if (string.IsNullOrEmpty(Request.QueryString["phone"]))
                        {
                            result = "Empty Phone.";
                        }
                        else
                        {
                            if (amt < 0)
                            {
                                result = "Invalid/Empty amount";
                            }
                            else
                            {
                                AppUtil.TestMode = db.SyncOrderProducts.FirstOrDefault(f => f.ServiceID == svID).TestMode.GetValueOrDefault();
                                //amt = Request.QueryString["amt"].ToString().ToLower();
                                phone = Request.QueryString["phone"].ToString();

                                AUTHSVC.RequestSOAPHeader authentication = new AUTHSVC.RequestSOAPHeader();
                                string timestamp = AppUtil.GetTimeStamp();
                                authentication.spId = AppUtil.SPID;
                                authentication.spPassword = GetMD5Hash(AppUtil.SPID + AppUtil.Password + timestamp);
                                authentication.serviceId = svID;
                                authentication.timeStamp = timestamp;



                                //temp
                                //authentication.spId = "601425";
                                //authentication.spPassword = GetMD5Hash("601425" + "Bvp@5560" + timestamp);
                                //authentication.serviceId = "6014252000001754";
                                //temp
                                AUTHSVC.authorizationService avc = new AUTHSVC.authorizationService();
                                avc.authCred = authentication;

                                AUTHSVC.Authorization appsg = new AUTHSVC.Authorization();
                                appsg.amount = amt;
                                appsg.amountSpecified = true;
                                //appsg.contentId = pID;
                                appsg.currency = "NGN";
                                appsg.description = "Payment";
                                appsg.endUserIdentifier = phone;
                                appsg.frequency = "1";
                                appsg.notificationURL = AppUtil.NotificationCONSURL;
                                appsg.scope = "17"; //The scope list supported by the server are listed here: 17:Payment||99:Subscription
                                appsg.serviceId = svID;
                                appsg.tokenValidity = 1;
                                appsg.tokenValiditySpecified = true;

                                appsg.extensionInfo = extInfList(pID, amt);


                                appsg.transactionId = GUIDN(GenerateId(), RandomNumber()); //.Substring(0, 30)



                                AUTHSVC.AuthorizationResponse res = new AUTHSVC.AuthorizationResponse();
                                res = avc.authorization(appsg);

                                result = (res.result.resultCode.ToLower() == "0") | res.result.resultCode.ToLower() == "00000000" ?
                                    ("OK" + (string.IsNullOrEmpty(res.token) ? "" : ("|" + res.token)))
                                    : res.result.resultCode + "| |" + res.result.resultDescription;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.ToString() + Environment.NewLine + ex.InnerException.ToString());
                result = "Error";
            }

            Response.Write(result);
        }

        private AUTHSVC.NamedParameter[] extInfList(string pID, int amount)
        {
            AUTHSVC.NamedParameter[] extList = new AUTHSVC.NamedParameter[20];
            AUTHSVC.NamedParameter ext = new AUTHSVC.NamedParameter();

            ext.key = "productName";
            ext.value = "FunChat On-demand";

            extList[0] = ext;

            ext = new AUTHSVC.NamedParameter();
            ext.key = "productId";
            ext.value = pID;
            extList[1] = ext;
            ext = new AUTHSVC.NamedParameter();
            ext.key = "totalAmount";
            ext.value = amount.ToString();
            extList[2] = ext;
            ext = new AUTHSVC.NamedParameter();
            ext.key = "channel";
            ext.value = "1";
            extList[3] = ext;
            ext = new AUTHSVC.NamedParameter();
            ext.key = "serviceInterval";
            ext.value = "1";
            extList[4] = ext;
            ext = new AUTHSVC.NamedParameter();
            ext.key = "serviceIntervalUnit";
            ext.value = "2";
            extList[5] = ext;
            ext = new AUTHSVC.NamedParameter();
            ext.key = "tokenType";
            ext.value = "1";

            extList[6] = ext;
            return extList;
        }
        private string GetMD5Hash(string sInputString)
        {
            // Returns a hash (message digest) of the input string.
            // See internet RFC 1321, "The MD5 Message-Digest Algorithm"
            ASCIIEncoding enc = new ASCIIEncoding();


            //Convert the string into an array of bytes.
            byte[] buffer = enc.GetBytes(sInputString);


            //Create the md5 hash provider.
            MD5 md = new MD5CryptoServiceProvider();


            //Compute the hash value from the array of bytes.
            byte[] hash = md.ComputeHash(buffer);


            string sResult = "";
            foreach (byte b in hash)
            {
                sResult += b.ToString("x2");
            }
            return sResult;
        }


        private List<int> NewNumber()
        {
            Random a = new Random(DateTime.Now.Ticks.GetHashCode()); // replace from new Random(DateTime.Now.Ticks.GetHashCode());
            // Since similar code is done in default constructor internally
            int MyNumber = 0;

            List<int> randomList = new List<int>();
            MyNumber = a.Next();
            if (!randomList.Contains(MyNumber))
                randomList.Add(MyNumber);

            return randomList;
        }

        private int RandomNumber()
        {
            Random a = new Random(DateTime.Now.Ticks.GetHashCode());
            return a.Next();
        }
        private long GenerateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        private string GUIDN(long id, int num)
        {
            var r = new Random();
            string ordered = id.ToString() + num.ToString();
            string shuffled = new string(ordered.ToCharArray().OrderBy(x => r.Next()).ToArray());
            return shuffled;
        }
    }
}