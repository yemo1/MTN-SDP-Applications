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
using System.Diagnostics;
using System.Xml.Schema;
using System.Data.Entity.Core.Objects;



namespace MTNCMWeb
{
    /// <summary>
    /// Data Sync For MTN SA
    /// </summary>
    /// 

    [WebService(Namespace = "http://www.csapi.org/schema/parlayx/data/v1_0")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    [System.Web.Services.WebServiceBindingAttribute(Name = "DataSyncBinding", Namespace = "http://www.csapi.org/schema/parlayx/data/v1_0")]

    public class DataSync : System.Web.Services.WebService
    {
        string TimeFormat = ConfigurationManager.AppSettings["TimeFormat"];
        SyncOrderEntities db = new SyncOrderEntities();

        public NamedParameterList extenInfo = new NamedParameterList();

        [System.Web.Services.WebMethodAttribute()]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        //[XmlInclude (typeof(UserID))]
        //[XmlInclude(typeof(NamedParameterList))]
        [return: System.Xml.Serialization.XmlElementAttribute("syncOrderRelationResponse", Namespace = "http://www.csapi.org/schema/parlayx/data/sync/v1_0/local")]
        public syncOrderRelationResponse syncOrderRelation([System.Xml.Serialization.XmlElementAttribute("syncOrderRelation", Namespace = "http://www.csapi.org/schema/parlayx/data/sync/v1_0/local")] syncOrderRelation syncOrderRelation1)
        {
            //MimeHeaders mimeHeader = message.getMimeHeaders(); 
            ////change header’s attribute mimeHeader.setHeader(“Content-Type”, “text/xml”); 
            //mimeHeader.setHeader(“SOAPACTION”, “http://www.abc.com/ProcedureName”);

            SyncOrderError syncResponse = new SyncOrderError();
            SyncOrderProduct product = new SyncOrderProduct();
            SyncOrderRelation orderRequest = new MTNCMWeb.SyncOrderRelation();
            SyncOrderRelationExtended orderRequestE = new SyncOrderRelationExtended();
            SyncOrderRelationResponse orderResponse = new SyncOrderRelationResponse();
            SyncOrderRequest oReq = new SyncOrderRequest();

            syncOrderRelationResponse ret = new syncOrderRelationResponse();
            ExtendedParameter extInfo = new ExtendedParameter(); 
            NamedParameterList exIn = new NamedParameterList();

            extenInfo.item = new NamedParameter[syncOrderRelation1.extensionInfo.Length];
            
            for (int i = 0; i < syncOrderRelation1.extensionInfo.Length; i++)
            {
                extenInfo.item[i] = new NamedParameter();
                extenInfo.item[i] = syncOrderRelation1.extensionInfo[i];
            }
            //UpdateTime yyyyMMddHHmmss -UTC time

            ret.result = "0";
            ret.resultDescription = "OK";

            try
            {
                extInfo = GetOrderExtension(extenInfo);
                //log datat
                //sync relate
                orderRequest.EffectiveTime = syncOrderRelation1.effectiveTime;
                orderRequest.ExpiryTime = syncOrderRelation1.expiryTime;
                orderRequest.ProductID = syncOrderRelation1.productID;
                orderRequest.ServiceID = syncOrderRelation1.serviceID;
                orderRequest.ServiceList = syncOrderRelation1.serviceList;
                orderRequest.ServiceProviderID = syncOrderRelation1.spID;
                orderRequest.Timestamped = DateTime.Now;
                orderRequest.UpdateDescription = syncOrderRelation1.updateDesc;
                orderRequest.UpdateTime = syncOrderRelation1.updateTime;
                orderRequest.UpdateType = syncOrderRelation1.updateType;
                orderRequest.UserID = (syncOrderRelation1.userID != null) ? syncOrderRelation1.userID.ID : "";
                orderRequest.UserType = (syncOrderRelation1.userID != null) ? syncOrderRelation1.userID.type : -1;
                orderRequest.Passed = 0;

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

                if (syncOrderRelation1.userID != null && string.IsNullOrEmpty(syncOrderRelation1.spID) == false && string.IsNullOrEmpty(syncOrderRelation1.productID) == false
                && string.IsNullOrEmpty(syncOrderRelation1.serviceID) == false && syncOrderRelation1.updateType > 0 && string.IsNullOrEmpty(syncOrderRelation1.updateTime) == false)
                {
                    product = db.SyncOrderProducts.AsEnumerable().FirstOrDefault(f => f.ProductID == syncOrderRelation1.productID & f.ServiceID == syncOrderRelation1.serviceID);
                    if (product != null && product.ID > 0)
                    {
                        AppUtil.TestMode = product.TestMode.GetValueOrDefault();
                        if (product.IsActive.GetValueOrDefault() == 1)
                        {
                            if (product.IsSub.GetValueOrDefault() == 1)
                            {
                                //if (db.SyncOrderRelations.AsEnumerable().Any(a => a.UserID == syncOrderRelation1.userID.ID & a.UserType == syncOrderRelation1.userID.type & a.Passed == 1
                                //    & a.ProductID == syncOrderRelation1.productID & a.ServiceID == syncOrderRelation1.serviceID & a.UpdateType == syncOrderRelation1.updateType
                                //    & AppUtil.DayDiff(AppUtil.UTC(a.UpdateTime), AppUtil.UTC(syncOrderRelation1.updateTime)) < product.DaysValid.GetValueOrDefault()) == false)
                                //{
                                //    if (syncOrderRelation1.updateType == 1 | (db.SyncOrderRelations.AsEnumerable().Any(a => a.UserID == syncOrderRelation1.userID.ID 
                                //        & a.UserType == syncOrderRelation1.userID.type & a.Passed == 1
                                //        & a.ProductID == syncOrderRelation1.productID & a.ServiceID == syncOrderRelation1.serviceID //& a.UpdateType == syncOrderRelation1.updateType 
                                //        ) & syncOrderRelation1.updateType != 1)) //& DayDiff(UTC(a.UpdateTime), UTC(syncOrderRelation1.updateTime)) < product.DaysValid.GetValueOrDefault()
                                //    {
                                orderRequest.Passed = 1;
                                db.SaveChanges();
                                ObjectParameter rspOut = new ObjectParameter("result", typeof(int));
                                //set keyword
                                //call sub
                                var it = db.subsa(product.Keyword, orderRequest.UserID, product.AccessCode, orderRequest.ServiceID, orderRequest.ProductID,
                                    Convert.ToInt32(product.Charge), product.DaysValid, syncOrderRelation1.updateType, rspOut);
                                if (rspOut.Value.ToString() != "0")
                                {
                                    ret.result = rspOut.Value.ToString();
                                    ret.resultDescription = (ret.result == "2031") ? "The subscription relationship does not exist." :
                                        (ret.result == "2032") ? "The service does not exist." :
                                        (ret.result == "2034") ? "The service is unavailable." :
                                        "An internal system error occurred.";
                                }
                                db.SaveChanges();
                                //send sms

                                //    }
                                //    else //exist active
                                //    {
                                //        ret.result = "2031";
                                //        ret.resultDescription = "The subscription relationship does not exist.";
                                //    }
                                //}
                                //else //existing active sub
                                //{
                                //    ret.result = "2030";
                                //    ret.resultDescription = "The subscription relationship already exists.";
                                //}
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
                object mybj = syncOrderRelation1.extensionInfo;
                //ret.extensionInfo = (NamedParameter[])mybj;

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

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/data/sync/v1_0/local")]
    public class syncOrderRelation : object, System.ComponentModel.INotifyPropertyChanged
    {

        private UserID userIDField;

        private string spIDField;

        private string productIDField;

        private string serviceIDField;

        private string serviceListField;

        private int updateTypeField;

        private string updateTimeField;

        private string updateDescField;

        private string effectiveTimeField;

        private string expiryTimeField;

        private NamedParameter[] extensionInfoField;

        /// <remarks/>

        [System.Xml.Serialization.XmlElement("userID", IsNullable = false, ElementName = "userID")]
        public UserID userID
        {
            get
            {
                return this.userIDField;
            }
            set
            {
                this.userIDField = value;
                this.RaisePropertyChanged("userID");
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("spID", IsNullable = false, Order = 1)]
        public string spID
        {
            get
            {
                return this.spIDField;
            }
            set
            {
                this.spIDField = value;
                this.RaisePropertyChanged("spID");
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("productID", IsNullable = false, Order = 2)]
        public string productID
        {
            get
            {
                return this.productIDField;
            }
            set
            {
                this.productIDField = value;
                this.RaisePropertyChanged("productID");
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("serviceID", IsNullable = false, Order = 3)]
        public string serviceID
        {
            get
            {
                return this.serviceIDField;
            }
            set
            {
                this.serviceIDField = value;
                this.RaisePropertyChanged("serviceID");
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("serviceList", IsNullable = false, Order = 4)]
        public string serviceList
        {
            get
            {
                return this.serviceListField;
            }
            set
            {
                this.serviceListField = value;
                this.RaisePropertyChanged("serviceList");
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("updateType", IsNullable = false, Order = 5)]
        public int updateType
        {
            get
            {
                return this.updateTypeField;
            }
            set
            {
                this.updateTypeField = value;
                this.RaisePropertyChanged("updateType");
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("updateTime", IsNullable = false, Order = 6)]
        public string updateTime
        {
            get
            {
                return this.updateTimeField;
            }
            set
            {
                this.updateTimeField = value;
                this.RaisePropertyChanged("updateTime");
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("updateDesc", IsNullable = false, Order = 7)]
        public string updateDesc
        {
            get
            {
                return this.updateDescField;
            }
            set
            {
                this.updateDescField = value;
                this.RaisePropertyChanged("updateDesc");
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("effectiveTime", IsNullable = false, Order = 8)]
        public string effectiveTime
        {
            get
            {
                return this.effectiveTimeField;
            }
            set
            {
                this.effectiveTimeField = value;
                this.RaisePropertyChanged("effectiveTime");
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("expiryTime", IsNullable = false, Order = 9)]
        public string expiryTime
        {
            get
            {
                return this.expiryTimeField;
            }
            set
            {
                this.expiryTimeField = value;
                this.RaisePropertyChanged("expiryTime");
            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("extensionInfo",Form=XmlSchemaForm.Qualified, IsNullable = false, Order = 10)]
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form = XmlSchemaForm.Unqualified, IsNullable = false, ElementName = "item")]
        public NamedParameter[] extensionInfo
        {
            get
            {
                return this.extensionInfoField;
            }
            set
            {
                this.extensionInfoField = value;
                this.RaisePropertyChanged("extensionInfo");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/data/sync/v1_0/local", TypeName = "userID")]
    public class UserID : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string idField;

        private int typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("ID", Form = XmlSchemaForm.Unqualified, IsNullable = false, Order = 0)]

        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                this.RaisePropertyChanged("ID");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("type", IsNullable = false, Form = XmlSchemaForm.Unqualified, Order = 1)]
        public int type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
                this.RaisePropertyChanged("type");

            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/data/sync/v1_0/local")]
    public class syncOrderRelationResponse : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string resultField;

        private string resultDescriptionField;

        //private NamedParameter[] extensionInfoField;

        /// <remarks/>
       [System.Xml.Serialization.XmlElement("result", IsNullable = false)]
        public string result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
                this.RaisePropertyChanged("result");

            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("resultDescription", IsNullable = false)]
        public string resultDescription
        {
            get
            {
                return this.resultDescriptionField;
            }
            set
            {
                this.resultDescriptionField = value;
                this.RaisePropertyChanged("resultDescription");

            }
        }

        /// <remarks/>
        //[System.Xml.Serialization.XmlElement("extensionInfo", IsNullable = false)]
        //public NamedParameter[] extensionInfo
        //{
        //    get
        //    {
        //        return this.extensionInfoField;
        //    }
        //    set
        //    {
        //        this.extensionInfoField = value;
        //        this.RaisePropertyChanged("extensionInfo");
        //    }
        //}

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.csapi.org/schema/parlayx/data/sync/v1_0/local")]
    public class NamedParameter : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string keyField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("key", IsNullable = false, Form = XmlSchemaForm.Unqualified, Order = 0)]
        public string key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
                this.RaisePropertyChanged("key");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElement("value", IsNullable = false, Form = XmlSchemaForm.Unqualified, Order = 1)]
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
                this.RaisePropertyChanged("value");
            }
        }


        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

    }


    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name = "NamedParameterList", Namespace = "http://www.csapi.org/schema/parlayx/data/v1_0", ItemName = "item")] //, ItemName = "item"
    [System.SerializableAttribute()]
    public class NamedParameterList
    {
        private NamedParameter[] itemField;
        [System.Xml.Serialization.XmlElement("item", Form = XmlSchemaForm.Unqualified, IsNullable = false)]
        public NamedParameter[] item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

    }


    public class ExtendedParameter
    {
        public string accessCode { get; set; }
        public string operCode { get; set; }
        public int chargeMode { get; set; }
        public string MDSPSUBEXPMODE { get; set; }
        public string objectType { get; set; }
        public string Starttime { get; set; }
        public bool isFreePeriod { get; set; }
        public string operatorID { get; set; }
        public int payType { get; set; }
        public string transactionID { get; set; }
        public string orderKey { get; set; }
        public string keyword { get; set; }
        public string durationOfGracePeriod { get; set; }
        public string serviceAvailability { get; set; }
        public string durationOfSuspendPeriod { get; set; }
        public string servicePayType { get; set; }
        public int fee { get; set; }
        public string cycleEndTime { get; set; }
        public int channelID { get; set; }
        public string TraceUniqueID { get; set; }
        public bool rentSuccess { get; set; }
        public bool _try { get; set; }
        public int updateReason { get; set; }
        public string dedicatedAccountOperationType { get; set; }
        public string DAExpireDate { get; set; }
        public string DAUnit { get; set; }
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
    public enum UpdateMode
    {
        Add = 1,
        Delete = 2,
        Update = 3,
        Block = 5,
        Unblock = 6
    }

}

