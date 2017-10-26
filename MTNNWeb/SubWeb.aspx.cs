using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MTNNWeb
{
    public partial class SubWeb : System.Web.UI.Page
    {
        SyncOrderEntities db = new SyncOrderEntities();
        string result = ""; int subType = 1;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["sTy"] != null)
                    int.TryParse(Request.QueryString["sTy"].ToString(), out subType);
                else subType = 1;
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
                            AppUtil.TestMode = db.SyncOrderProducts.FirstOrDefault(f => f.ServiceID == svID).TestMode.GetValueOrDefault();

                            phone = Request.QueryString["phone"].ToString();

                            RequestSOAPHeader authentication = new RequestSOAPHeader();
                            string timestamp = AppUtil.GetTimeStamp();
                            authentication.spId = AppUtil.SPID;
                            authentication.spPassword = AppUtil.GetMD5Hash(AppUtil.SPID + AppUtil.Password + timestamp);
                            authentication.serviceId = svID;
                            authentication.timeStamp = timestamp;


                            //temp
                            //authentication.spId = "601425";
                            //authentication.spPassword = GetMD5Hash("601425" + "Bvp@5560" + timestamp);
                            //authentication.serviceId = "6014252000001754";
                            //temp

                            SUBVC.SubscribeManageService svc = new SUBVC.SubscribeManageService();
                            svc.authCred = authentication;

                            SUBVC.UserID userInfo = new SUBVC.UserID(); SUBVC.SubInfo subInfo = new SUBVC.SubInfo();

                            userInfo.ID = phone;
                            userInfo.type = 0; //User type. 0: MSISDN/Other values: reserved.

                            subInfo.channelID = 2; //Operation channel. 1: WEB/ 2: SMS/ 3: USSD/ 4: IVR
                            subInfo.isAutoExtend = 1; //Indicates whether to automatically renew the subscription. 0: No/ 1: Yes
                            subInfo.isAutoExtendSpecified = true;
                            subInfo.operCode = "en";
                            subInfo.productID = pID;


                            SUBVC.subscribeProductRequest subsg = new SUBVC.subscribeProductRequest();
                            subsg.subscribeProductReq = new SUBVC.SubscribeProductReq();


                            SUBVC.unSubscribeProductRequest unsubsg = new SUBVC.unSubscribeProductRequest();
                            unsubsg.unSubscribeProductReq = new SUBVC.UnSubscribeProductReq();

                            if (subType == 1)
                            {
                                subsg.subscribeProductReq.subInfo = subInfo;
                                subsg.subscribeProductReq.userID = userInfo;

                                SUBVC.subscribeProductResponse res = new SUBVC.subscribeProductResponse();
                                res = svc.subscribeProduct(subsg);

                                result = (res.subscribeProductRsp.result.ToLower() == "0") | res.subscribeProductRsp.result.ToLower() == "00000000" ?
                                    "OK"
                                    : res.subscribeProductRsp.result + "| |" + res.subscribeProductRsp.resultDescription;
                            }
                            else
                            {
                                unsubsg.unSubscribeProductReq.subInfo = subInfo;
                                unsubsg.unSubscribeProductReq.userID = userInfo;

                                SUBVC.unSubscribeProductResponse unres = new SUBVC.unSubscribeProductResponse();
                                unres = svc.unSubscribeProduct(unsubsg);

                                result = (unres.unSubscribeProductRsp.result.ToLower() == "0") | unres.unSubscribeProductRsp.result.ToLower() == "00000000" ?
                                    "OK"
                                    : unres.unSubscribeProductRsp.result + "| |" + unres.unSubscribeProductRsp.resultDescription;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.ToString());
                result = "Error";
            }
            finally
            {

                if (db != null)
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Open)
                        db.Database.Connection.Close();

                    db.Dispose();
                }
            }

            Response.Write(result);
        }




    }
}