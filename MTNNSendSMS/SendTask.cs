using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;
using System.Configuration;
using System.Net;
using System.Threading;
using System.Data.Entity.Infrastructure;
using System.Web.Services.Protocols;

namespace MTNNSendSMS
{
    public partial class SendTask : ServiceBase
    {
        int wWinSize = Convert.ToInt32(ConfigurationManager.AppSettings["wWinSize"]);
        int wAsync = Convert.ToInt32(ConfigurationManager.AppSettings["wAsync"]);
        int wWinInterval = Convert.ToInt32(ConfigurationManager.AppSettings["wWinInterval"]);
        string strUsername = ConfigurationManager.AppSettings["wUsername"];
        //"Administrator"
        string strPassword = ConfigurationManager.AppSettings["wPassword"];
        //"admin1"
        string wWebAPIURL = ConfigurationManager.AppSettings["wWebAPI"];
        string strReference = "SDPNGChat";
        string strSessionID = "";

        private Task _proccessDoLatestQueueTask;
        private CancellationTokenSource _cancellationDoLatestTokenSource;

        private Task _proccessDoOldestQueueTask;
        private CancellationTokenSource _cancellationDoOldestTokenSource;

        private Task _proccessDoMiddleDownQueueTask;
        private CancellationTokenSource _cancellationDoMiddleDownTokenSource;

        private Task _proccessDoMiddleUpQueueTask;
        private CancellationTokenSource _cancellationDoMiddleUpTokenSource;


        private Task _proccessDoCheckingQueueTask;
        private CancellationTokenSource _cancellationDoCheckingTokenSource; 

        public SendTask()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            try
            {
                AppUtil.LogFileWrite("MTNNSendSMS starting.....");
                Thread.Sleep(10000);
                _cancellationDoLatestTokenSource = new CancellationTokenSource();
                _proccessDoLatestQueueTask = Task.Run(() => DoLatestWorkAsync(_cancellationDoLatestTokenSource.Token)); //, _cancellationLatestTokenSource.Token

                _cancellationDoOldestTokenSource = new CancellationTokenSource();
                _proccessDoOldestQueueTask = Task.Run(() => DoOldestWorkAsync(_cancellationDoOldestTokenSource.Token)); //, _cancellationLatestTokenSource.Token

                _cancellationDoMiddleDownTokenSource = new CancellationTokenSource();
                _proccessDoMiddleDownQueueTask = Task.Run(() => DoMiddleDownWorkAsync(_cancellationDoMiddleDownTokenSource.Token)); //, _cancellationLatestTokenSource.Token

                _cancellationDoMiddleUpTokenSource = new CancellationTokenSource();
                _proccessDoMiddleUpQueueTask = Task.Run(() => DoMiddleUpWorkAsync(_cancellationDoMiddleUpTokenSource.Token)); //, _cancellationLatestTokenSource.Token


                _cancellationDoCheckingTokenSource = new CancellationTokenSource();
                _proccessDoCheckingQueueTask = Task.Run(() => DoCheckingWorkAsync(_cancellationDoCheckingTokenSource.Token)); //, _cancellationLatestTokenSource.Token


                AppUtil.LogFileWrite("MTNNSendSMS started.....");
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.ToString());
            }
        }

        public async Task DoLatestWorkAsync(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    DoLatest();
                    // Handle exception
                }
                catch (Exception ex)
                { AppUtil.LogFileWrite(ex.ToString()); }
                await Task.Delay(wWinInterval, token);
            }
        }

        public async Task DoOldestWorkAsync(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    DoOldest();
                    // Handle exception
                }
                catch (Exception ex)
                { AppUtil.LogFileWrite(ex.ToString()); }
                await Task.Delay(wWinInterval, token);
            }
        }

        public async Task DoMiddleDownWorkAsync(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    DoMiddleDown();
                    // Handle exception
                }
                catch (Exception ex)
                { AppUtil.LogFileWrite(ex.ToString()); }
                await Task.Delay(wWinInterval, token);
            }
        }

        public async Task DoMiddleUpWorkAsync(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    DoMiddleUp();
                    // Handle exception
                }
                catch (Exception ex)
                { AppUtil.LogFileWrite(ex.ToString()); }
                await Task.Delay(wWinInterval, token);
            }
        }


        public async Task DoCheckingWorkAsync(CancellationToken token)
        {
            while (true)
            {
                try
                {
                    DoChecking();
                    // Handle exception
                }
                catch (Exception ex)
                { AppUtil.LogFileWrite(ex.ToString()); }
                await Task.Delay(wWinInterval * 10, token);
            }
        }

        public void DoLatest()
        {
            string mm = wWebAPIURL;
            SyncOrderEntities db = new SyncOrderEntities();

            string strErrorDescription = string.Empty;
            try
            {
                if (db.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    db.Database.Connection.Open();
                }

                var textInfo = db.mmSenders.AsNoTracking().AsEnumerable().Where(w => w.mStatus == 1 & w.mState == 0 &
                    (w.SendDate.GetValueOrDefault() == null | w.SendDate.GetValueOrDefault() <= DateTime.Now)).OrderByDescending(w => w.ID).ThenByDescending(w => w.Priority).Take(wWinSize);
                if (textInfo != null && textInfo.Any())
                {
                    foreach (mmSender item in textInfo)
                    {
                        if (wAsync == 0)
                            SendMessage(item.ID); //String.Format(mm, item.ID)
                        else
                            SendMessageAsync(item.ID);
                    }
                    //myTimer.Interval = wWinInterval;
                }
                //else
                    //myTimer.Interval = wWinInterval;
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.Message);
            }
            finally
            {
                if (db.Database.Connection.State == System.Data.ConnectionState.Open)
                    db.Database.Connection.Close();

                db.Dispose();

            }

        }

        public void DoOldest()
        {
            string mm = wWebAPIURL;
            SyncOrderEntities db = new SyncOrderEntities();

            string strErrorDescription = string.Empty;

            int minBuffer = wWinSize * 10;
            try
            {
                if (db.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    db.Database.Connection.Open();
                }

                if (db.mmSenders.AsNoTracking().AsEnumerable().Where(w => w.mStatus == 1 & w.mState == 0 &
                        (w.SendDate.GetValueOrDefault() == null | w.SendDate.GetValueOrDefault() <= DateTime.Now)).Count() > minBuffer)
                {
                    var textInfo = db.mmSenders.AsNoTracking().AsEnumerable().Where(w => w.mStatus == 1 & w.mState == 0 &
                        (w.SendDate.GetValueOrDefault() == null | w.SendDate.GetValueOrDefault() <= DateTime.Now)).OrderBy(w => w.ID).ThenByDescending(w => w.Priority).Take(wWinSize);
                    if (textInfo != null && textInfo.Any())
                    {
                        foreach (mmSender item in textInfo)
                        {
                            if (wAsync == 0)
                                SendMessage(item.ID); //String.Format(mm, item.ID)
                            else
                                SendMessageAsync(item.ID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.Message);
            }
            finally
            {
                if (db.Database.Connection.State == System.Data.ConnectionState.Open)
                    db.Database.Connection.Close();

                db.Dispose();

            }

        }

        public void DoMiddleDown()
        {
            string mm = wWebAPIURL;
            SyncOrderEntities db = new SyncOrderEntities();

            string strErrorDescription = string.Empty;

            int minBuffer = wWinSize * 10 * 10, count = 0, rowid = 0;
            try
            {
                if (db.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    db.Database.Connection.Open();
                }

                count = db.mmSenders.AsNoTracking().AsEnumerable().Where(w => w.mStatus == 1 & w.mState == 0 &
                        (w.SendDate.GetValueOrDefault() == null | w.SendDate.GetValueOrDefault() <= DateTime.Now)).Count();

                if (count > minBuffer)
                {
                    //rowid = db.IncomingMOs.Where(Function(w) w.IsRead = 0).OrderBy(Function(w) w.ID).Skip(count / 2).FirstOrDefault().ID
                    rowid = db.mmSenders.AsNoTracking().AsEnumerable().Where(w => w.mStatus == 1 & w.mState == 0 &
                        (w.SendDate.GetValueOrDefault() == null | w.SendDate.GetValueOrDefault() <= DateTime.Now)).OrderBy(w => w.ID).Skip(count / 2).FirstOrDefault().ID;

                    var textInfo = db.mmSenders.AsNoTracking().AsEnumerable().Where(w => w.mStatus == 1 & w.mState == 0 & w.ID < rowid &
                        (w.SendDate.GetValueOrDefault() == null | w.SendDate.GetValueOrDefault() <= DateTime.Now)).OrderBy(w => w.ID).ThenByDescending(w => w.Priority).Take(wWinSize);
                    if (textInfo != null && textInfo.Any())
                    {
                        foreach (mmSender item in textInfo)
                        {
                            if (wAsync == 0)
                                SendMessage(item.ID); //String.Format(mm, item.ID)
                            else
                                SendMessageAsync(item.ID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.Message);
            }
            finally
            {
                if (db.Database.Connection.State == System.Data.ConnectionState.Open)
                    db.Database.Connection.Close();

                db.Dispose();

            }

        }

        public void DoMiddleUp()
        {
            string mm = wWebAPIURL;
            SyncOrderEntities db = new SyncOrderEntities();

            string strErrorDescription = string.Empty;

            int minBuffer = wWinSize * 10 * 10, count = 0, rowid = 0;
            try
            {
                if (db.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    db.Database.Connection.Open();
                }

                count = db.mmSenders.AsNoTracking().AsEnumerable().Where(w => w.mStatus == 1 & w.mState == 0 &
                        (w.SendDate.GetValueOrDefault() == null | w.SendDate.GetValueOrDefault() <= DateTime.Now)).Count();

                if (count > minBuffer)
                {
                    //rowid = db.IncomingMOs.Where(Function(w) w.IsRead = 0).OrderBy(Function(w) w.ID).Skip(count / 2).FirstOrDefault().ID
                    rowid = db.mmSenders.AsNoTracking().AsEnumerable().Where(w => w.mStatus == 1 & w.mState == 0 &
                        (w.SendDate.GetValueOrDefault() == null | w.SendDate.GetValueOrDefault() <= DateTime.Now)).OrderBy(w => w.ID).Skip(count / 2).FirstOrDefault().ID;

                    var textInfo = db.mmSenders.AsNoTracking().AsEnumerable().Where(w => w.mStatus == 1 & w.mState == 0 & w.ID >= rowid &
                        (w.SendDate.GetValueOrDefault() == null | w.SendDate.GetValueOrDefault() <= DateTime.Now)).OrderBy(w => w.ID).ThenByDescending(w => w.Priority).Take(wWinSize);
                    if (textInfo != null && textInfo.Any())
                    {
                        foreach (mmSender item in textInfo)
                        {
                            if (wAsync == 0)
                                SendMessage(item.ID); //String.Format(mm, item.ID)
                            else
                                SendMessageAsync(item.ID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.Message);
            }
            finally
            {
                if (db.Database.Connection.State == System.Data.ConnectionState.Open)
                    db.Database.Connection.Close();

                db.Dispose();

            }

        }

        public void DoChecking()
        {
            SyncOrderEntities db = new SyncOrderEntities();

            string mm = wWebAPIURL, strErrorDescription = string.Empty,
                strLastStatus = string.Empty, strErrorText = string.Empty, strLastTimestamp = string.Empty;
            int nErrorCode = 0, itemid = 0;
            try
            {
                if (db.Database.Connection.State != System.Data.ConnectionState.Open)
                {
                    db.Database.Connection.Open();
                }

                var textInfo = db.mmSenders.Where(w => w.mState == 0 & w.mStatus == -1).Take(wWinSize);
                if (textInfo != null && textInfo.Any())
                {
                    foreach (mmSender item in textInfo)
                    {
                        itemid = item.ID;
                        db.Database.ExecuteSqlCommand("Update mmSender SET mState=1,sLastStatus={0},sErrorCode={1},sErrorText={2},sLastTimeStamp={3},sErrorDesc={4} WHERE ID ={5} ",
                                          strLastStatus, nErrorCode, strErrorText, strLastTimestamp, strErrorDescription, item.ID);
                    }
                }
                //else
            }
            catch (Exception ex)
            {
                if (itemid != 0) db.Database.ExecuteSqlCommand("Update mmSender SET mState=1 WHERE ID ={0}", itemid);
                AppUtil.LogFileWrite(ex.Message);
            }
            finally
            {
                if (db.Database.Connection.State == System.Data.ConnectionState.Open)
                    db.Database.Connection.Close();

                db.Dispose();
            }

        }

        void SendMessage(int mIndex)
        {
            mmSender outgoingMsg = new mmSender();
            SyncOrderEntities db = new SyncOrderEntities();

            string result = "";
            int mindex = 0;

            try
            {
                string svcID = "",//(Request.QueryString["svcID"] != null) ? Request.QueryString["svcID"].ToString() : 
                    txtMsg = "", //(Request.QueryString["txtMsg"] != null) ? Request.QueryString["txtMsg"].ToString() : 
                    phone = "", //(Request.QueryString["phone"] != "") ? Request.QueryString["phone"].ToString() :
                    msgInx = (mIndex != null) ? mIndex.ToString() : "";

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


                            SVC.sendSmsResponse res = new SVC.sendSmsResponse();
                            res = svc.sendSms(msg);

                            result = res.result;
                            double retIndex = 0;
                            if (double.TryParse(result, out retIndex))
                            {
                                outgoingMsg.mState = 1; outgoingMsg.mStatus = 0; outgoingMsg.sLastStatus = "transmitted";
                            }
                            else
                            {
                                outgoingMsg.mState = 0; outgoingMsg.mStatus = 1; outgoingMsg.sLastStatus = "none";
                            }
                            outgoingMsg.mErrorDesc = result;
                            outgoingMsg.sLastTimeStamp = DateTime.Now.ToString();
                            db.SaveChanges();
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


        }

        void SendMessageAsync(int mIndex)
        {
            mmSender outgoingMsg = new mmSender();
            SyncOrderEntities db = new SyncOrderEntities();

            string result = "";
            int mindex = 0;

            try
            {
                string svcID = "",//(Request.QueryString["svcID"] != null) ? Request.QueryString["svcID"].ToString() : 
                    txtMsg = "", //(Request.QueryString["txtMsg"] != null) ? Request.QueryString["txtMsg"].ToString() : 
                    phone = "", //(Request.QueryString["phone"] != "") ? Request.QueryString["phone"].ToString() :
                    msgInx = (mIndex != null) ? mIndex.ToString() : "";

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


                            SVC.sendSmsResponse res = new SVC.sendSmsResponse();
                            //res = svc.sendSms(msg);

                            svc.sendSmsCompleted += svc_sendSmsCompleted;
                            svc.sendSmsAsync(msg, mindex);

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


        }


        void svc_sendSmsCompleted(object sender, SVC.sendSmsCompletedEventArgs e)
        {
            SyncOrderEntities db = new SyncOrderEntities();
            mmSender outgoingMsg = new mmSender();

            int mindex = 0;

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

        }

        protected override void OnStop()
        {
            _cancellationDoLatestTokenSource.Cancel();
            _cancellationDoOldestTokenSource.Cancel();
            _cancellationDoMiddleDownTokenSource.Cancel();
            _cancellationDoMiddleUpTokenSource.Cancel();

            _cancellationDoCheckingTokenSource.Cancel();
            try
            {
                AppUtil.LogFileWrite("MTNNSendSMS stopping.....");

                _proccessDoLatestQueueTask.Wait();
                _proccessDoOldestQueueTask.Wait();
                _proccessDoMiddleDownQueueTask.Wait();
                _proccessDoMiddleUpQueueTask.Wait();

                AppUtil.LogFileWrite("MTNNSendSMS stopped.....");
            }
            catch (Exception e)
            {
                AppUtil.LogFileWrite("On Stop Error: " + e.Message);
            }
        }

    }
}
