using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MTNSAWeb
{
    [XmlTypeAttribute(Namespace = "http://www.huawei.com.cn/schema/common/v2_1", TypeName = "v2")]
    public class RequestSOAPHeader : SoapHeader
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string spId { get; set; }
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string spPassword { get; set; }
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string serviceId { get; set; }
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string timeStamp { get; set; }
    }

    [XmlTypeAttribute(Namespace = "http://www.huawei.com.cn/schema/common/v2_1", TypeName = "ns1")]
    public class NotifySOAPHeader : SoapHeader
    {
        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string spRevId { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string traceUniqueID { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string spId { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string spRevpassword { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string serviceId { get; set; }

        [XmlElement(Form = XmlSchemaForm.Unqualified)]
        public string timeStamp { get; set; }
    }

    [XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local")]
    public class notifySmsDeliveryReceipt
    {
        private string correlatorField;

        private MTNSAWeb.SVC.DeliveryInformation deliveryStatusField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
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
        public MTNSAWeb.SVC.DeliveryInformation deliveryStatus
        {
            get
            {
                return this.deliveryStatusField;
            }
            set
            {
                this.deliveryStatusField = value;
            }
        }
    }

    [XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/sms/notification/v2_2/local")]
    public class notifySmsDeliveryReceiptResponse
    {
        private string resultField;

        /// <remarks/>
        [XmlElement("result", Form = XmlSchemaForm.Unqualified, IsNullable = false, Order = 0)]
        public string result
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
    }
    public class AppUtil
    {
        public static void LogFileWrite(string message)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                string text = AppDomain.CurrentDomain.BaseDirectory + FileLog;
                text = text + "AppLog-" + DateTime.Today.ToString("yyyyMMdd") + ".txt";
                string str = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                if (!text.Equals(""))
                {
                    FileInfo fileInfo = new FileInfo(text);
                    DirectoryInfo directoryInfo = new DirectoryInfo(fileInfo.DirectoryName);
                    if (!directoryInfo.Exists)
                    {
                        directoryInfo.Create();
                    }
                    if (!fileInfo.Exists)
                    {
                        fileStream = fileInfo.Create();
                    }
                    else
                    {
                        fileStream = new FileStream(text, FileMode.Append);
                    }
                    streamWriter = new StreamWriter(fileStream);
                    streamWriter.WriteLine(str + message);
                }
            }
            finally
            {
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }
        public static void LogDBWrite(string message, string Mode)
        {
            SyncOrderEntities db = new SyncOrderEntities();
            AppLog appLog = new AppLog();
            try
            {
                appLog.Details = message; appLog.Header = Mode; appLog.Timestamped = DateTime.Now;
                db.AppLogs.Add(appLog);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                LogFileWrite(ex.ToString());
                LogFileWrite(message);
            }
        }
        public static string TimeFormat = ConfigurationManager.AppSettings["TimeFormat"];
        public static string LiveIP = ConfigurationManager.AppSettings["LiveIP"]; //(TestMode == 0) ? 
        public static string TestIP = ConfigurationManager.AppSettings["TestIP"];
        private static string FileLog = ConfigurationManager.AppSettings["FileLog"] + "\\";

        static string _spid = (TestMode == 0) ? ConfigurationManager.AppSettings["SPID"] : ConfigurationManager.AppSettings["tSPID"];
        public static string SPID
        {
            get
            {
                return _spid;
            }
            set
            {
                _spid = value;
            }
        }

        static string _password = (TestMode == 0) ? ConfigurationManager.AppSettings["Password"] : ConfigurationManager.AppSettings["tPassword"];
        public static string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }

        static string _AccessCode = (TestMode == 0) ? ConfigurationManager.AppSettings["Accesscode"] : ConfigurationManager.AppSettings["TestAccesscode"];
        public static string Accesscode
        {
            get
            {
                return _AccessCode;
            }
            set
            {
                _AccessCode = value;
            }
        }
        static string _serviceID = (TestMode == 0) ? ConfigurationManager.AppSettings["serviceId"] : ConfigurationManager.AppSettings["serviceId"];
        public static string serviceId
        {
            get
            {
                return _serviceID;
            }
            set
            {
                _serviceID = value;
            }
        }

        static string _sendsmsapiurl = string.Format(ConfigurationManager.AppSettings["SendSMSAPIURL"], ((TestMode == 0) ? LiveIP : TestIP));
        public static string SendSMSAPIURL
        {
            get
            {
                return _sendsmsapiurl;
            }
            set
            {
                _sendsmsapiurl = value;
            }
        }
        static string _notificationapiurl = string.Format(ConfigurationManager.AppSettings["NotificationAPIURL"], ((TestMode == 0) ? LiveIP : TestIP));
        public static string NotificationAPIURL
        {
            get
            {
                return _notificationapiurl;
            }
            set
            {
                _notificationapiurl = value;
            }
        }
        static string _chargingurl = string.Format(ConfigurationManager.AppSettings["ChargingURL"], ((TestMode == 0) ? LiveIP : TestIP));

        public static string chargingURL
        {
            get
            {
                return _chargingurl;
            }
            set
            {
                _chargingurl = value;
            }
        }

        static string _notificationstatus = (TestMode == 0) ? ConfigurationManager.AppSettings["NotificationStatus"] : ConfigurationManager.AppSettings["NotificationStatus"];
        public static string NotificationStatus
        {
            get
            {
                return _notificationstatus;
            }
            set
            {
                _notificationstatus = value;
            }
        }
        static string _notificationconsurl = (TestMode == 0) ? ConfigurationManager.AppSettings["NotificationCONSURL"] : ConfigurationManager.AppSettings["NotificationCONSURL"];
        public static string NotificationCONSURL
        {
            get
            {
                return _notificationconsurl;
            }
            set
            {
                _notificationconsurl = value;
            }
        }

        public static int TestMode
        {
            get
            {
                return _testMode;
            }
            set
            {
                _testMode = value;
            }

        }

        static int _testMode = Convert.ToInt32(ConfigurationManager.AppSettings["TestMode"]);




        public static string GetTimeStamp()
        {
            return DateTime.Now.ToString(TimeFormat);
        }
        public static DateTime UTC(string time)
        {
            DateTime? val = null;
            try
            { val = DateTime.ParseExact(time, TimeFormat, System.Globalization.CultureInfo.InvariantCulture); }
            catch
            { }

            //DateTime.TryParseExact(dateValue, pattern, null, DateTimeStyles.None, out parsedDate)
            return val.GetValueOrDefault();
        }

        public static int DayDiff(DateTime start, DateTime end)
        {
            int val = 0;
            try
            {
                TimeSpan timespan = (end - start);
                val = Convert.ToInt32(timespan.TotalDays);
            }
            catch { }
            return val;
        }
        public static string GetMD5Hash(string sInputString)
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

        public static string MD5Hashed(string input)
        {
            MD5 mD = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(input);
            byte[] array = mD.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                stringBuilder.Append(array[i].ToString("x2"));
            }
            return stringBuilder.ToString();
        }

    }
    /// <summary>
    /// SOAP Extension that traces the SOAP request and
    /// SOAP response for the PayPal SOAP API Web service. 
    /// </summary>

    public class TraceExtension : SoapExtension
    {
        private Stream oldStream;
        private Stream newStream;

        private static XmlDocument xmlRequest;
        /// <summary>
        /// Gets the outgoing XML request sent to PayPal
        /// </summary>
        public static XmlDocument XmlRequest
        {
            get { return xmlRequest; }
        }

        private static XmlDocument xmlResponse;
        /// <summary>
        /// Gets the incoming XML response sent from PayPal
        /// </summary>
        public static XmlDocument XmlResponse
        {
            get { return xmlResponse; }
        }

        /// <summary>
        /// Save the Stream representing the SOAP request
        /// or SOAP response into a local memory buffer. 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        /// <summary>
        /// If the SoapMessageStage is such that the SoapRequest or
        /// SoapResponse is still in the SOAP format to be sent or received,
        /// save it to the xmlRequest or xmlResponse property.
        /// </summary>
        /// <param name="message"></param>
        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    break;
                case SoapMessageStage.AfterSerialize:
                    xmlRequest = GetSoapEnvelope(newStream, "Request");
                    CopyStream(newStream, oldStream);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    CopyStream(oldStream, newStream);
                    xmlResponse = GetSoapEnvelope(newStream, "Response");
                    break;
                case SoapMessageStage.AfterDeserialize:
                    break;
            }
        }

        /// <summary>
        /// Returns the XML representation of the Soap Envelope in the supplied stream.
        /// Resets the position of stream to zero.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private XmlDocument GetSoapEnvelope(Stream stream)
        {
            XmlDocument xml = new XmlDocument();
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            xml.LoadXml(reader.ReadToEnd());
            stream.Position = 0;

            AppUtil.LogFileWrite(xml.InnerXml.ToString());

            return xml;
        }

        /// <summary>
        /// Returns the XML representation of the Soap Envelope in the supplied stream.
        /// Resets the position of stream to zero.
        /// </summary>
        /// <param name="stream"></param>
        /// /// <param name="Request Or Response"></param>
        /// <returns></returns>
        private XmlDocument GetSoapEnvelope(Stream stream, string Mode)
        {
            XmlDocument xml = new XmlDocument();
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);
            xml.LoadXml(reader.ReadToEnd());
            stream.Position = 0;

            AppUtil.LogDBWrite(xml.InnerXml.ToString(), Mode);

            return xml;
        }
        /// <summary>
        /// Copies a stream.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private void CopyStream(Stream from, Stream to)
        {
            TextReader reader = new StreamReader(from);
            TextWriter writer = new StreamWriter(to);
            writer.WriteLine(reader.ReadToEnd());
            writer.Flush();
        }
        #region NoOp
        /// <summary>
        /// Included only because it must be implemented.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public override object GetInitializer(LogicalMethodInfo methodInfo,
            SoapExtensionAttribute attribute)
        {
            return null;
        }

        /// <summary>
        /// Included only because it must be implemented.
        /// </summary>
        /// <param name="WebServiceType"></param>
        /// <returns></returns>
        public override object GetInitializer(Type WebServiceType)
        {
            return null;
        }

        /// <summary>
        /// Included only because it must be implemented.
        /// </summary>
        /// <param name="initializer"></param>
        public override void Initialize(object initializer)
        {
        }
        #endregion NoOp
    }

    public static class Extension
    {
        public static bool IsNumeric(this string value)
        {
            return value.All(Char.IsDigit);
        }
    }
}