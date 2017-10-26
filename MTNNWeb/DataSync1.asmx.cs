using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Collections;
using System.Collections.Specialized;
using Microsoft.VisualBasic;
using System.IO;
using System.Xml.Serialization;
using System.Web.Services.Protocols;
using System.Globalization;
using System.ComponentModel;
using System.Xml;


namespace MTNSAWebOld
{
    /// <summary>
    /// Data Sync For MTN SA
    /// </summary>
    /// 
    //[System.SerializableAttribute()]

    [WebService(Namespace = "http://www.csapi.org/schema/parlayx/data/v1_0")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DataSync1 : System.Web.Services.WebService
    {
        string TimeFormat = ConfigurationManager.AppSettings["TimeFormat"];
        SyncOrderEntities db = new SyncOrderEntities();

        public NamedParameterList extenInfo;

        [SoapDocumentMethod(Action = "")]
        [WebMethod]
        //[XmlElement("userID", typeof(UserID))]
        //[XmlElement("extensionInfo", typeof(NamedParameterList))] 
        public SyncResponse syncOrderRelation(UserID userID, string spID, string productID, string serviceID, string serviceList, int updateType, string updateTime, string updateDesc,
            string effectiveTime, string expiryTime, NamedParameterList extensionInfo)
        {
            //MimeHeaders mimeHeader = message.getMimeHeaders(); 
            ////change header’s attribute mimeHeader.setHeader(“Content-Type”, “text/xml”); 
            //mimeHeader.setHeader(“SOAPACTION”, “http://www.abc.com/ProcedureName”);

            SyncOrderError syncResponse = new SyncOrderError();
            SyncOrderProduct product = new SyncOrderProduct();
            SyncOrderRelation orderRequest = new MTNSAWeb.SyncOrderRelation();
            SyncOrderRelationExtended orderRequestE = new SyncOrderRelationExtended();
            SyncOrderRelationResponse orderResponse = new SyncOrderRelationResponse();
            SyncOrderRequest oReq = new SyncOrderRequest();

            SyncResponse ret = new SyncResponse();
            ExtendedParameter extInfo = new ExtendedParameter();
            NamedParameterList exIn = new NamedParameterList();


            //UpdateTime yyyyMMddHHmmss -UTC time
            ret.result = "0";
            ret.resultDescription = "OK";
            try
            {
                extInfo = GetOrderExtension(extensionInfo);
                //log datat
                //sync relate
                orderRequest.EffectiveTime = effectiveTime;
                orderRequest.ExpiryTime = expiryTime;
                orderRequest.ProductID = productID;
                orderRequest.ServiceID = serviceID;
                orderRequest.ServiceList = serviceList;
                orderRequest.ServiceProviderID = spID;
                orderRequest.Timestamped = DateTime.Now;
                orderRequest.UpdateDescription = updateDesc;
                orderRequest.UpdateTime = updateTime;
                orderRequest.UpdateType = updateType;
                orderRequest.UserID = (userID != null) ? userID.ID:"";
                orderRequest.UserType = (userID != null) ? userID.type : -1;

                db.SyncOrderRelations.Add(orderRequest);
                db.SaveChanges();

                //sync relate extend
                orderRequestE.AccessCode = extInfo.accessCode;
                orderRequestE.ChannelID = extInfo.channelID;
                orderRequestE.ChargeMode = extInfo.chargeMode;
                orderRequestE.CycleEndTime = extInfo.cycleEndTime;
                orderRequestE.DAExpireDate = extInfo.DAExpireDate;
                orderRequestE.DAUnit = extInfo.DAUnit;
                orderRequestE.DedicatedAccountOperationType = extInfo.dedicatedAccountOperationType;
                orderRequestE.DurationOfGracePeriod = extInfo.durationOfGracePeriod;
                orderRequestE.DurationOfSuspensionPeriod = extInfo.durationOfSuspendPeriod;
                orderRequestE.Fee = extInfo.fee;
                orderRequestE.IsFreePeriod = extInfo.isFreePeriod;
                orderRequestE.Keyword = extInfo.keyword;
                orderRequestE.MDSPSUBEXPMODE = extInfo.MDSPSUBEXPMODE;
                orderRequestE.ObjectType = extInfo.objectType;
                orderRequestE.OperationCode = extInfo.operatorID;
                orderRequestE.OperatorID = extInfo.operatorID;
                orderRequestE.OrderKey = extInfo.operCode;
                orderRequestE.PayType = extInfo.payType;
                orderRequestE.RentSucess = extInfo.rentSuccess;
                orderRequestE.ServiceAvailability = extInfo.serviceAvailability;
                orderRequestE.ServicePayType = extInfo.servicePayType;
                orderRequestE.StartTime = extInfo.Starttime;
                orderRequestE.TraceUniqueID = extInfo.TraceUniqueID;
                orderRequestE.TransactionID = extInfo.transactionID;
                orderRequestE.Try = extInfo._try;
                orderRequestE.UpdateReason = extInfo.updateReason;
                orderRequestE.SyncOrderRelationID = orderRequest.ID;

                db.SyncOrderRelationExtendeds.Add(orderRequestE);
                db.SaveChanges();

                if (userID != null && string.IsNullOrEmpty(spID) == false && string.IsNullOrEmpty(productID) == false
                && string.IsNullOrEmpty(serviceID) == false && updateType > 0 && string.IsNullOrEmpty(updateTime) == false)
                {
                    product = db.SyncOrderProducts.AsEnumerable().FirstOrDefault(f => f.ProductID == productID & f.ServiceID == serviceID);
                    if (product != null && product.ID > 0)
                    {
                        if (product.IsActive.GetValueOrDefault() == 1)
                        {
                            if (product.IsSub.GetValueOrDefault() == 1)
                            {
                                if (db.SyncOrderRelations.AsEnumerable().Any(a => a.UserID == userID.ID & a.UserType == userID.type & a.Passed ==1
                                    & a.ProductID == productID & a.ServiceID == serviceID & a.UpdateType == updateType & DayDiff(UTC(a.UpdateTime), UTC(updateTime)) < product.DaysValid.GetValueOrDefault()) == false)
                                {
                                    if (updateType == 1 | db.SyncOrderRelations.AsEnumerable().Any(a => a.UserID == userID.ID & a.UserType == userID.type & a.Passed == 1
                                        & a.ProductID == productID & a.ServiceID == serviceID & a.UpdateType == updateType & DayDiff(UTC(a.UpdateTime), UTC(updateTime)) < product.DaysValid.GetValueOrDefault()))
                                    {
                                        orderRequest.Passed = 1;
                                        db.SaveChanges();

                                        //set keyword

                                        //call sub
                                        int it=db.subsa(product.Keyword, orderRequest.UserID, orderRequestE.OperationCode, Convert.ToInt32(product.Charge), product.DaysValid);
                                        db.SaveChanges();
                                        //send sms

                                    }
                                    else //exist active
                                    {
                                        ret.result = "2031";
                                        ret.resultDescription = "The subscription relationship does not exist.";
                                    }
                                }
                                else //existing active sub
                                {
                                    ret.result = "2030";
                                    ret.resultDescription = "The subscription relationship already exists.";
                                }
                            }
                            else //none sub - on demand
                            {
                                ret.result = "2034";
                                ret.resultDescription = "The service is unavailable.";
                            }
                        }
                        else //none active
                        {
                            ret.result = "2033";
                            ret.resultDescription = "The service is unavailable.";
                        }
                    }
                    else //none exist
                    {
                        ret.result = "2032";
                        ret.resultDescription = "The service does not exist.";
                    }
                }
                else //null, wrong formt etc.
                {
                    ret.result = "1211";
                    ret.resultDescription = "The field format is incorrect or the value is invalid.";
                }
            }
            catch (Exception ex)
            {
                AppUtil.LogFileWrite(ex.ToString());
                ret.result = "2500";
                ret.resultDescription = "An internal system error occurred.";
                //send mail
            }

            try
            {
                //return value
                if (orderRequestE != null && orderRequestE.ID > 0) orderResponse.SyncOrderRelationExtendedID = orderRequestE.ID;
                orderResponse.Result = ret.result;
                orderResponse.ResultDescription = ret.resultDescription;
                orderResponse.ResultInterpretation = (ret.result == "0") ? "Sucess" : db.SyncOrderErrors.AsEnumerable().FirstOrDefault(f => f.Code == ret.result).Cause;
                orderResponse.Timestamped = DateTime.Now;
                ret.extensionInfo = extensionInfo;

                db.SyncOrderRelationResponses.Add(orderResponse);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                //write flatfile and send mail
                AppUtil.LogFileWrite(ex.ToString());
            }
            return ret;
        }

        private DateTime UTC(string time)
        {
            DateTime? val = null;
            try
            { val = DateTime.ParseExact(time, TimeFormat, System.Globalization.CultureInfo.InvariantCulture); }
            catch
            { }

            //DateTime.TryParseExact(dateValue, pattern, null, DateTimeStyles.None, out parsedDate)
            return val.GetValueOrDefault();
        }

        private int DayDiff(DateTime start, DateTime end)
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

        private ExtendedParameter GetOrderExtension(NamedParameterList exOrder)
        {
            //if (exOrder != null && exOrder.item != null) AppUtil.LogFileWrite(exOrder.item.Length.ToString());
            //else AppUtil.LogFileWrite("NULL Extensionifno");

            ExtendedParameter ret = new ExtendedParameter();
            NameValueCollection keyPairs = new NameValueCollection();

            if (exOrder != null && exOrder.item != null)
            {
                foreach (NamedParameter ext in exOrder.item)
                {
                    //AppUtil.LogFileWrite(string.Format("Key::{0}=Value::{1}",ext.key, ext.value));
                    if (ext != null) keyPairs.Add(ext.key, ext.value);
                }

                var cp = new ExtendedParameter();

                foreach (var prop in typeof(ExtendedParameter).GetProperties())
                {
                    var colParam = keyPairs[prop.Name];
                    if (colParam != null)
                    {
                        prop.SetValue(cp, ValueToType(colParam, prop.PropertyType), null);
                    }
                }
                ret = cp;
            }
            else
            {
                ;
            }

            return ret;
        }

        private T GetObjectAs<T>(object source, T destinationType)
            where T : class
        {
            return Convert.ChangeType(source, typeof(T)) as T;
        }

        private object ValueToType(string value, Type propertyType)
        {
            var underlyingType = Nullable.GetUnderlyingType(propertyType);
            if (underlyingType == null)
                return Convert.ChangeType(value, propertyType, CultureInfo.InvariantCulture);
            return String.IsNullOrEmpty(value)
                   ? null
                   : Convert.ChangeType(value, underlyingType, CultureInfo.InvariantCulture);

        }

    }
    //[System.Xml.Serialization.XmlArrayAttribute(Order = 10)]


    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/data/v1_0")]

    public class UserID
    {
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ID; //{ get; set; }
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int type; //{ get; set; }
    }


    public class NamedParameterList
    {
       [XmlElement("item")]
        public NamedParameter[] item; //{ get; set; }

    }

    //[System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/data/v1_0")]

    public class NamedParameter
    {

        public string key; //{ get; set; }
        public string value; //{ get; set; }
    }

    public class ExtendedParameter
    {
        public string accessCode; //{ get; set; }
        public string operCode; //{ get; set; }
        public int chargeMode; //{ get; set; }
        public string MDSPSUBEXPMODE; //{ get; set; }
        public string objectType; //{ get; set; }
        public string Starttime; //{ get; set; }
        public bool isFreePeriod; //{ get; set; }
        public string operatorID; //{ get; set; }
        public int payType; //{ get; set; }
        public string transactionID; //{ get; set; }
        public string orderKey; //{ get; set; }
        public string keyword; //{ get; set; }
        public string durationOfGracePeriod; //{ get; set; }
        public string serviceAvailability; //{ get; set; }
        public string durationOfSuspendPeriod; //{ get; set; }
        public string servicePayType; //{ get; set; }
        public int fee; //{ get; set; }
        public string cycleEndTime; //{ get; set; }
        public int channelID; //{ get; set; }
        public string TraceUniqueID; //{ get; set; }
        public bool rentSuccess; //{ get; set; }
        public bool _try; //{ get; set; }
        public int updateReason; //{ get; set; }
        public string dedicatedAccountOperationType; //{ get; set; }
        public string DAExpireDate; //{ get; set; }
        public string DAUnit; //{ get; set; }
    }

    public class SyncOrderRequest
    {
        public UserID userID; //{ get; set; }
        public string spID; //{ get; set; }
        public string productID; //{ get; set; }
        public string serviceID; //{ get; set; }
        public string serviceList; //{ get; set; }
        public int updateType; //{ get; set; }
        public string updateTime; //{ get; set; }
        public string updateDesc; //{ get; set; }
        public string effectiveTime; //{ get; set; }
        public string expiryTime; //{ get; set; }
        //[XmlElement("extensionInfo")]
        public NamedParameterList extensionInfo; //{ get; set; }
    }
    public class SyncResponse
    {
        public string result; //{ get; set; }
        public string resultDescription; //{ get; set; }
        public NamedParameterList extensionInfo; //{ get; set; }
    }
    public enum UpdateMode
    {
        Add = 1,
        Delete = 2,
        Update = 3,
        Block = 5,
        Unblock = 6
    }

    public class AppUtil
    {
        public static void LogFileWrite(string message)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                string text = AppDomain.CurrentDomain.BaseDirectory;
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
                    xmlRequest = GetSoapEnvelope(newStream);
                    CopyStream(newStream, oldStream);
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    CopyStream(oldStream, newStream);
                    xmlResponse = GetSoapEnvelope(newStream);
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

}

