﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTNCChat.DSync {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="UserID", Namespace="http://www.csapi.org/schema/parlayx/data/v1_0")]
    [System.SerializableAttribute()]
    public partial class UserID : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IDField;
        
        private int typeField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string ID {
            get {
                return this.IDField;
            }
            set {
                if ((object.ReferenceEquals(this.IDField, value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(IsRequired=true)]
        public int type {
            get {
                return this.typeField;
            }
            set {
                if ((this.typeField.Equals(value) != true)) {
                    this.typeField = value;
                    this.RaisePropertyChanged("type");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.CollectionDataContractAttribute(Name="NamedParameterList", Namespace="http://www.csapi.org/schema/parlayx/data/v1_0", ItemName="item")]
    [System.SerializableAttribute()]
    public class NamedParameterList : System.Collections.Generic.List<MTNCChat.DSync.NamedParameter> {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="NamedParameter", Namespace="http://www.csapi.org/schema/parlayx/data/v1_0")]
    [System.SerializableAttribute()]
    public partial class NamedParameter : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string keyField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string valueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string key {
            get {
                return this.keyField;
            }
            set {
                if ((object.ReferenceEquals(this.keyField, value) != true)) {
                    this.keyField = value;
                    this.RaisePropertyChanged("key");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string value {
            get {
                return this.valueField;
            }
            set {
                if ((object.ReferenceEquals(this.valueField, value) != true)) {
                    this.valueField = value;
                    this.RaisePropertyChanged("value");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="SyncResponse", Namespace="http://www.csapi.org/schema/parlayx/data/v1_0")]
    [System.SerializableAttribute()]
    public partial class SyncResponse : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string resultField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string resultDescriptionField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private MTNCChat.DSync.NamedParameterList extensionInfoField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string result {
            get {
                return this.resultField;
            }
            set {
                if ((object.ReferenceEquals(this.resultField, value) != true)) {
                    this.resultField = value;
                    this.RaisePropertyChanged("result");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false)]
        public string resultDescription {
            get {
                return this.resultDescriptionField;
            }
            set {
                if ((object.ReferenceEquals(this.resultDescriptionField, value) != true)) {
                    this.resultDescriptionField = value;
                    this.RaisePropertyChanged("resultDescription");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public MTNCChat.DSync.NamedParameterList extensionInfo {
            get {
                return this.extensionInfoField;
            }
            set {
                if ((object.ReferenceEquals(this.extensionInfoField, value) != true)) {
                    this.extensionInfoField = value;
                    this.RaisePropertyChanged("extensionInfo");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.csapi.org/schema/parlayx/data/v1_0", ConfigurationName="DSync.DataSyncSoap")]
    public interface DataSyncSoap {
        
        // CODEGEN: Generating message contract since element name userID from namespace http://www.csapi.org/schema/parlayx/data/v1_0 is not marked nillable
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        MTNCChat.DSync.syncOrderRelationResponse syncOrderRelation(MTNCChat.DSync.syncOrderRelationRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="", ReplyAction="*")]
        System.Threading.Tasks.Task<MTNCChat.DSync.syncOrderRelationResponse> syncOrderRelationAsync(MTNCChat.DSync.syncOrderRelationRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class syncOrderRelationRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="syncOrderRelation", Namespace="http://www.csapi.org/schema/parlayx/data/v1_0", Order=0)]
        public MTNCChat.DSync.syncOrderRelationRequestBody Body;
        
        public syncOrderRelationRequest() {
        }
        
        public syncOrderRelationRequest(MTNCChat.DSync.syncOrderRelationRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.csapi.org/schema/parlayx/data/v1_0")]
    public partial class syncOrderRelationRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public MTNCChat.DSync.UserID userID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=1)]
        public string spID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=2)]
        public string productID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=3)]
        public string serviceID;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=4)]
        public string serviceList;
        
        [System.Runtime.Serialization.DataMemberAttribute(Order=5)]
        public int updateType;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=6)]
        public string updateTime;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=7)]
        public string updateDesc;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=8)]
        public string effectiveTime;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=9)]
        public string expiryTime;
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=10)]
        public MTNCChat.DSync.NamedParameterList extensionInfo;
        
        public syncOrderRelationRequestBody() {
        }
        
        public syncOrderRelationRequestBody(MTNCChat.DSync.UserID userID, string spID, string productID, string serviceID, string serviceList, int updateType, string updateTime, string updateDesc, string effectiveTime, string expiryTime, MTNCChat.DSync.NamedParameterList extensionInfo) {
            this.userID = userID;
            this.spID = spID;
            this.productID = productID;
            this.serviceID = serviceID;
            this.serviceList = serviceList;
            this.updateType = updateType;
            this.updateTime = updateTime;
            this.updateDesc = updateDesc;
            this.effectiveTime = effectiveTime;
            this.expiryTime = expiryTime;
            this.extensionInfo = extensionInfo;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class syncOrderRelationResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="syncOrderRelationResponse", Namespace="http://www.csapi.org/schema/parlayx/data/v1_0", Order=0)]
        public MTNCChat.DSync.syncOrderRelationResponseBody Body;
        
        public syncOrderRelationResponse() {
        }
        
        public syncOrderRelationResponse(MTNCChat.DSync.syncOrderRelationResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.csapi.org/schema/parlayx/data/v1_0")]
    public partial class syncOrderRelationResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public MTNCChat.DSync.SyncResponse syncOrderRelationResult;
        
        public syncOrderRelationResponseBody() {
        }
        
        public syncOrderRelationResponseBody(MTNCChat.DSync.SyncResponse syncOrderRelationResult) {
            this.syncOrderRelationResult = syncOrderRelationResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface DataSyncSoapChannel : MTNCChat.DSync.DataSyncSoap, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class DataSyncSoapClient : System.ServiceModel.ClientBase<MTNCChat.DSync.DataSyncSoap>, MTNCChat.DSync.DataSyncSoap {
        
        public DataSyncSoapClient() {
        }
        
        public DataSyncSoapClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public DataSyncSoapClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataSyncSoapClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public DataSyncSoapClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MTNCChat.DSync.syncOrderRelationResponse MTNCChat.DSync.DataSyncSoap.syncOrderRelation(MTNCChat.DSync.syncOrderRelationRequest request) {
            return base.Channel.syncOrderRelation(request);
        }
        
        public MTNCChat.DSync.SyncResponse syncOrderRelation(MTNCChat.DSync.UserID userID, string spID, string productID, string serviceID, string serviceList, int updateType, string updateTime, string updateDesc, string effectiveTime, string expiryTime, MTNCChat.DSync.NamedParameterList extensionInfo) {
            MTNCChat.DSync.syncOrderRelationRequest inValue = new MTNCChat.DSync.syncOrderRelationRequest();
            inValue.Body = new MTNCChat.DSync.syncOrderRelationRequestBody();
            inValue.Body.userID = userID;
            inValue.Body.spID = spID;
            inValue.Body.productID = productID;
            inValue.Body.serviceID = serviceID;
            inValue.Body.serviceList = serviceList;
            inValue.Body.updateType = updateType;
            inValue.Body.updateTime = updateTime;
            inValue.Body.updateDesc = updateDesc;
            inValue.Body.effectiveTime = effectiveTime;
            inValue.Body.expiryTime = expiryTime;
            inValue.Body.extensionInfo = extensionInfo;
            MTNCChat.DSync.syncOrderRelationResponse retVal = ((MTNCChat.DSync.DataSyncSoap)(this)).syncOrderRelation(inValue);
            return retVal.Body.syncOrderRelationResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<MTNCChat.DSync.syncOrderRelationResponse> MTNCChat.DSync.DataSyncSoap.syncOrderRelationAsync(MTNCChat.DSync.syncOrderRelationRequest request) {
            return base.Channel.syncOrderRelationAsync(request);
        }
        
        public System.Threading.Tasks.Task<MTNCChat.DSync.syncOrderRelationResponse> syncOrderRelationAsync(MTNCChat.DSync.UserID userID, string spID, string productID, string serviceID, string serviceList, int updateType, string updateTime, string updateDesc, string effectiveTime, string expiryTime, MTNCChat.DSync.NamedParameterList extensionInfo) {
            MTNCChat.DSync.syncOrderRelationRequest inValue = new MTNCChat.DSync.syncOrderRelationRequest();
            inValue.Body = new MTNCChat.DSync.syncOrderRelationRequestBody();
            inValue.Body.userID = userID;
            inValue.Body.spID = spID;
            inValue.Body.productID = productID;
            inValue.Body.serviceID = serviceID;
            inValue.Body.serviceList = serviceList;
            inValue.Body.updateType = updateType;
            inValue.Body.updateTime = updateTime;
            inValue.Body.updateDesc = updateDesc;
            inValue.Body.effectiveTime = effectiveTime;
            inValue.Body.expiryTime = expiryTime;
            inValue.Body.extensionInfo = extensionInfo;
            return ((MTNCChat.DSync.DataSyncSoap)(this)).syncOrderRelationAsync(inValue);
        }
    }
}
