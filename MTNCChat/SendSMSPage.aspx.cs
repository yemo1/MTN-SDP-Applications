﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MTNCChat
{
    public partial class SendSMSPage : System.Web.UI.Page
    {
        mmSender outgoingMsg = new mmSender();
        SyncOrderEntities db = new SyncOrderEntities();
        string result = "";
        int mindex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string svcID = "",//(Request.QueryString["svcID"] != null) ? Request.QueryString["svcID"].ToString() : 
                    txtMsg = "", //(Request.QueryString["txtMsg"] != null) ? Request.QueryString["txtMsg"].ToString() : 
                    phone = "", //(Request.QueryString["phone"] != "") ? Request.QueryString["phone"].ToString() :
                    msgInx = string.IsNullOrEmpty(Request.QueryString["msgInx"]) == false ? Request.QueryString["msgInx"].ToString() : "";
                if (string.IsNullOrEmpty(msgInx))
                {
                    result = "Unknown";
                }
                else
                {
                    //string.IsNullOrEmpty(svcID) | string.IsNullOrEmpty(txtMsg) | string.IsNullOrEmpty(phone)
                    if (int.TryParse(msgInx, out mindex) == false)
                    {
                        result = "Wrong Index";
                    }
                    else
                    {
                        SyncOrderEntities db = new SyncOrderEntities();

                        ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 180;

                        if (db.Database.Connection.State != System.Data.ConnectionState.Open)
                        {
                            db.Database.Connection.Open();
                        }

                        outgoingMsg = db.mmSenders.AsNoTracking().AsEnumerable().FirstOrDefault(f => f.mIndex.GetValueOrDefault() == mindex);

                        if (outgoingMsg != null && (string.IsNullOrEmpty(outgoingMsg.ServiceTo) == false & string.IsNullOrEmpty(outgoingMsg.TextMessage) == false &
                            string.IsNullOrEmpty(outgoingMsg.Destination) == false))
                        {
                            svcID = outgoingMsg.ServiceTo;
                            AppUtil.TestMode = db.SyncOrderProducts.FirstOrDefault(f => f.ServiceID == svcID).TestMode.GetValueOrDefault();
                            txtMsg = outgoingMsg.TextMessage; phone = outgoingMsg.Destination; AppUtil.Accesscode = outgoingMsg.Originator;
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

                            SVC.SendSmsService svc = new SVC.SendSmsService();
                            svc.authCred = authentication;

                            SVC.SimpleReference simple = new SVC.SimpleReference();
                            simple.endpoint = AppUtil.NotificationStatus;
                            simple.interfaceName = "SmsNotification";
                            simple.correlator = msgInx.PadLeft(10, '0');
                            SVC.sendSms msg = new SVC.sendSms();
                            msg.addresses = new string[] { string.Format("tel:{0}", phone) };
                            msg.message = txtMsg; msg.senderName = AppUtil.Accesscode;
                            msg.receiptRequest = simple;


                            //SVC.sendSmsResponse res = new SVC.sendSmsResponse();
                            //res = svc.sendSms(msg);

                            svc.sendSmsCompleted += svc_sendSmsCompleted;
                            svc.sendSmsAsync(msg, mindex);

                            //result = res.result;
                            //double retIndex = 0;
                            //if (double.TryParse(result, out retIndex))
                            //{
                            //    outgoingMsg.mState = 1; outgoingMsg.mStatus = 0; outgoingMsg.sLastStatus = "transmitted";
                            //}
                            //else
                            //{
                            //    outgoingMsg.mState = 0; outgoingMsg.mStatus = 1; outgoingMsg.sLastStatus = "none";
                            //}
                            //outgoingMsg.mErrorDesc = result;
                            //outgoingMsg.sLastTimeStamp = DateTime.Now.ToString();
                            //db.SaveChanges();
                        }
                        else
                        {
                            result = "Invalid Message Parameters";
                            if (outgoingMsg != null && outgoingMsg.ID > 0) outgoingMsg.mErrorDesc = result; outgoingMsg.mState = 0; outgoingMsg.mStatus = -1;
                        }

                    }
                }
            }
            catch (SoapException sex)
            {
                // The variable 'sex' can access the exception's information.
                //AppUtil.LogFileWrite(sex.ToString());
                result = "Soap Error";
                if (outgoingMsg != null && outgoingMsg.ID > 0)
                {
                    outgoingMsg.mErrorDesc = result;
                    outgoingMsg.mStatus = -1;
                    outgoingMsg.mState = 2;
                    outgoingMsg.sErrorText = sex.Detail.InnerXml.ToString();
                    outgoingMsg.sErrorDesc = sex.Code.Name;
                    outgoingMsg.sLastTimeStamp = DateTime.Now.ToString();
                }
                AppUtil.LogDBWrite(sex.Detail.InnerXml.ToString(), "Response");
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.ToString());
                result = "Error";
                if (outgoingMsg != null && outgoingMsg.ID > 0)
                {
                    outgoingMsg.mErrorDesc = result;
                    outgoingMsg.mStatus = -1;
                    outgoingMsg.mState = 0;
                    outgoingMsg.sLastTimeStamp = DateTime.Now.ToString();
                }
            }
            finally
            {
                if (outgoingMsg != null && outgoingMsg.ID > 0)
                {
                    db.SaveChanges();
                }

                if (db != null)
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Open)
                        db.Database.Connection.Close();

                    db.Dispose();
                }
            }


            Response.Write(result);
        }

        void svc_sendSmsCompleted(object sender, SVC.sendSmsCompletedEventArgs e)
        {
            SyncOrderEntities db = new SyncOrderEntities();
            if (db.Database.Connection.State != System.Data.ConnectionState.Open)
            {
                db.Database.Connection.Open();
            }
            
            SVC.sendSmsResponse res = new SVC.sendSmsResponse();
            mmSender outgoinMsg = new mmSender();
            string asresult = "";
            try
            {
                mindex = (int)e.UserState;

                outgoingMsg = db.mmSenders.AsEnumerable().FirstOrDefault(f => f.mIndex.GetValueOrDefault() == mindex);

                res = e.Result;//AsyncCompletedEventArgs.UserState
                if (e.Cancelled == false)
                {
                    if (e.Error == null)
                    {
                        asresult = res.result;
                        double retIndex = 0;
                        if (double.TryParse(asresult, out retIndex))
                        {
                            outgoingMsg.mState = 1; outgoingMsg.mStatus = 0; outgoingMsg.sLastStatus = "transmitted";
                        }
                        else
                        {
                            outgoingMsg.mState = 0; outgoingMsg.mStatus = 1; outgoingMsg.sLastStatus = "none";
                        }
                        outgoingMsg.mErrorDesc = asresult;
                        outgoingMsg.sLastTimeStamp = DateTime.Now.ToString();
                        db.SaveChanges();
                    }
                    else
                    {
                        if (e.Error is SoapException)
                        {
                            SoapException sex = (SoapException)e.Error;

                            // The variable 'sex' can access the exception's information.
                            //AppUtil.LogFileWrite(sex.ToString());
                            asresult = "Soap Error";
                            if (outgoingMsg != null && outgoingMsg.ID > 0)
                            {
                                outgoingMsg.mErrorDesc = asresult;
                                outgoingMsg.mStatus = -1;
                                outgoingMsg.mState = 2;
                                outgoingMsg.sErrorText = sex.Detail.InnerXml.ToString();
                                outgoingMsg.sErrorDesc = sex.Code.Name;
                                outgoingMsg.sLastTimeStamp = DateTime.Now.ToString();
                            }
                            AppUtil.LogDBWrite(sex.Detail.InnerXml.ToString(), "Response");

                        }
                        else
                        {
                            Exception ex = e.Error;

                            AppUtil.LogFileWrite(ex.ToString());
                            asresult = "Error";
                            if (outgoingMsg != null && outgoingMsg.ID > 0)
                            {
                                outgoingMsg.mErrorDesc = asresult;
                                outgoingMsg.mStatus = -1;
                                outgoingMsg.mState = 0;
                                outgoingMsg.sLastTimeStamp = DateTime.Now.ToString();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (outgoingMsg != null && outgoingMsg.ID > 0)
                {
                    db.SaveChanges();
                }

                if (db != null)
                {
                    if (db.Database.Connection.State == System.Data.ConnectionState.Open)
                        db.Database.Connection.Close();

                    db.Dispose();
                }
            }


            Response.Write(asresult);


        }

        #region Comment

        //  protected void Page_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string svcID = "",//(Request.QueryString["svcID"] != null) ? Request.QueryString["svcID"].ToString() : 
        //            txtMsg = "", //(Request.QueryString["txtMsg"] != null) ? Request.QueryString["txtMsg"].ToString() : 
        //            phone = "", //(Request.QueryString["phone"] != "") ? Request.QueryString["phone"].ToString() :
        //            msgInx = string.IsNullOrEmpty(Request.QueryString["msgInx"]) == false ? Request.QueryString["msgInx"].ToString() : "";
        //        if (string.IsNullOrEmpty(msgInx))
        //        {
        //            result = "Unknown";
        //        }
        //        else
        //        {
        //            //string.IsNullOrEmpty(svcID) | string.IsNullOrEmpty(txtMsg) | string.IsNullOrEmpty(phone)
        //            if (int.TryParse(msgInx, out mindex) == false)
        //            {
        //                result = "Wrong Index";
        //            }
        //            else
        //            {
        //                SyncOrderEntities db = new SyncOrderEntities();

        //                ((IObjectContextAdapter)db).ObjectContext.CommandTimeout = 180;

        //                outgoingMsg = db.mmSenders.AsEnumerable().FirstOrDefault(f => f.mIndex.GetValueOrDefault() == mindex);

        //                if (outgoingMsg != null && (string.IsNullOrEmpty(outgoingMsg.ServiceTo) == false & string.IsNullOrEmpty(outgoingMsg.TextMessage) == false &
        //                    string.IsNullOrEmpty(outgoingMsg.Destination) == false))
        //                {
        //                    svcID = outgoingMsg.ServiceTo;
        //                    AppUtil.TestMode = db.SyncOrderProducts.FirstOrDefault(f => f.ServiceID == svcID).TestMode.GetValueOrDefault();
        //                    txtMsg = outgoingMsg.TextMessage; phone = outgoingMsg.Destination; AppUtil.Accesscode = outgoingMsg.Originator;
        //                    RequestSOAPHeader authentication = new RequestSOAPHeader();
        //                    string timestamp = AppUtil.GetTimeStamp();
        //                    authentication.spId = AppUtil.SPID;
        //                    authentication.spPassword = AppUtil.GetMD5Hash(AppUtil.SPID + AppUtil.Password + timestamp);
        //                    authentication.serviceId = svcID;
        //                    authentication.timeStamp = timestamp;


        //                    //temp
        //                    //authentication.spId = "601425";
        //                    //authentication.spPassword = GetMD5Hash("601425" + "Bvp@5560" + timestamp);
        //                    //authentication.serviceId = "6014252000001754";
        //                    //temp

        //                    SVC.SendSmsService svc = new SVC.SendSmsService();
        //                    svc.authCred = authentication;

        //                    SVC.SimpleReference simple = new SVC.SimpleReference();
        //                    simple.endpoint = AppUtil.NotificationStatus;
        //                    simple.interfaceName = "SmsNotification";
        //                    simple.correlator = msgInx.PadLeft(10, '0');
        //                    SVC.sendSms msg = new SVC.sendSms();
        //                    msg.addresses = new string[] { string.Format("tel:{0}", phone) };
        //                    msg.message = txtMsg; msg.senderName = AppUtil.Accesscode;
        //                    msg.receiptRequest = simple;

        //                    SVC.sendSmsResponse res = new SVC.sendSmsResponse();
        //                    res = svc.sendSms(msg);

        //                    result = res.result;
        //                    double retIndex = 0;
        //                    if (double.TryParse(result, out retIndex))
        //                    {
        //                        outgoingMsg.mState = 1; outgoingMsg.mStatus = 0; outgoingMsg.sLastStatus = "transmitted";
        //                    }
        //                    else
        //                    {
        //                        outgoingMsg.mState = 0; outgoingMsg.mStatus = 1; outgoingMsg.sLastStatus = "none";
        //                    }
        //                    outgoingMsg.mErrorDesc = result;
        //                    outgoingMsg.sLastTimeStamp = DateTime.Now.ToString();
        //                    db.SaveChanges();
        //                }
        //                else
        //                {
        //                    result = "Invalid Message Parameters";
        //                    if (outgoingMsg != null && outgoingMsg.ID > 0) outgoingMsg.mErrorDesc = result; outgoingMsg.mState = 0; outgoingMsg.mStatus = -1;
        //                }

        //            }
        //        }
        //    }
        //    catch (SoapException sex)
        //    {
        //        // The variable 'sex' can access the exception's information.
        //        //AppUtil.LogFileWrite(sex.ToString());
        //        result = "Soap Error";
        //        if (outgoingMsg != null && outgoingMsg.ID > 0)
        //        {
        //            outgoingMsg.mErrorDesc = result;
        //            outgoingMsg.mStatus = -1;
        //            outgoingMsg.mState = 2;
        //            outgoingMsg.sErrorText = sex.Detail.InnerXml.ToString();
        //            outgoingMsg.sErrorDesc = sex.Code.Name;
        //            outgoingMsg.sLastTimeStamp = DateTime.Now.ToString();
        //        }
        //        AppUtil.LogDBWrite(sex.Detail.InnerXml.ToString(), "Response");
        //    }
        //    catch (Exception ex)
        //    {
        //        AppUtil.LogFileWrite(ex.ToString());
        //        result = "Error";
        //        if (outgoingMsg != null && outgoingMsg.ID > 0)
        //        {
        //            outgoingMsg.mErrorDesc = result;
        //            outgoingMsg.mStatus = -1;
        //            outgoingMsg.mState = 0;
        //            outgoingMsg.sLastTimeStamp = DateTime.Now.ToString();
        //        }
        //    }
        //    finally
        //    {
        //        if (outgoingMsg != null && outgoingMsg.ID > 0)
        //        {
        //            db.SaveChanges();
        //        }

        //        if (db != null)
        //        {
        //            if (db.Database.Connection.State == System.Data.ConnectionState.Open)
        //                db.Database.Connection.Close();

        //           //db.Dispose();
        //        }
        //    }


        //    Response.Write(result);
        //}

        #endregion
    }
}