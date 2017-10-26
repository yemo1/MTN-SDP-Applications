﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.34014.
// 
#pragma warning disable 1591

namespace MTNCInfo.OTPSVC {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SubscriberConsentBinding", Namespace="http://www.csapi.org/wsdl/subscriberconsnet/data/v1_0/service")]
    public partial class SubscriberConsentService : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback notifySubscriberConsentResultOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SubscriberConsentService() {
            this.Url = string.Format(global::MTNCInfo.Properties.Settings.Default.MTNNWeb_OTPSVC_SubscriberConsentService, ((AppUtil.TestMode == 0) ? AppUtil.LiveIP : AppUtil.TestIP));
            if ((this.IsLocalFileSystemWebService(this.Url) == true)) {
                this.UseDefaultCredentials = true;
                this.useDefaultCredentialsSetExplicitly = false;
            }
            else {
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        public new string Url {
            get {
                return base.Url;
            }
            set {
                if ((((this.IsLocalFileSystemWebService(base.Url) == true) 
                            && (this.useDefaultCredentialsSetExplicitly == false)) 
                            && (this.IsLocalFileSystemWebService(value) == false))) {
                    base.UseDefaultCredentials = false;
                }
                base.Url = value;
            }
        }
        
        public new bool UseDefaultCredentials {
            get {
                return base.UseDefaultCredentials;
            }
            set {
                base.UseDefaultCredentials = value;
                this.useDefaultCredentialsSetExplicitly = true;
            }
        }
        
        /// <remarks/>
        public event notifySubscriberConsentResultCompletedEventHandler notifySubscriberConsentResultCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("notifySubscriberConsentResultResponse", Namespace="http://www.csapi.org/schema/subscriberconsnet/data/v1_0/local")]
        public notifySubscriberConsentResultResponse notifySubscriberConsentResult([System.Xml.Serialization.XmlElementAttribute("notifySubscriberConsentResult", Namespace="http://www.csapi.org/schema/subscriberconsnet/data/v1_0/local")] notifySubscriberConsentResult notifySubscriberConsentResult1) {
            object[] results = this.Invoke("notifySubscriberConsentResult", new object[] {
                        notifySubscriberConsentResult1});
            return ((notifySubscriberConsentResultResponse)(results[0]));
        }
        
        /// <remarks/>
        public void notifySubscriberConsentResultAsync(notifySubscriberConsentResult notifySubscriberConsentResult1) {
            this.notifySubscriberConsentResultAsync(notifySubscriberConsentResult1, null);
        }
        
        /// <remarks/>
        public void notifySubscriberConsentResultAsync(notifySubscriberConsentResult notifySubscriberConsentResult1, object userState) {
            if ((this.notifySubscriberConsentResultOperationCompleted == null)) {
                this.notifySubscriberConsentResultOperationCompleted = new System.Threading.SendOrPostCallback(this.OnnotifySubscriberConsentResultOperationCompleted);
            }
            this.InvokeAsync("notifySubscriberConsentResult", new object[] {
                        notifySubscriberConsentResult1}, this.notifySubscriberConsentResultOperationCompleted, userState);
        }
        
        private void OnnotifySubscriberConsentResultOperationCompleted(object arg) {
            if ((this.notifySubscriberConsentResultCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.notifySubscriberConsentResultCompleted(this, new notifySubscriberConsentResultCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        public new void CancelAsync(object userState) {
            base.CancelAsync(userState);
        }
        
        private bool IsLocalFileSystemWebService(string url) {
            if (((url == null) 
                        || (url == string.Empty))) {
                return false;
            }
            System.Uri wsUri = new System.Uri(url);
            if (((wsUri.Port >= 1024) 
                        && (string.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) == 0))) {
                return true;
            }
            return false;
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/subscriberconsnet/data/v1_0/local")]
    public partial class notifySubscriberConsentResult {
        
        private subscriberID subscriberIDField;
        
        private string partnerIDField;
        
        private string serviceIDField;
        
        private int capabilityTypeField;
        
        private int consentResultField;
        
        private string accessTokenField;
        
        private TokenType tokenTypeField;
        
        private bool tokenTypeFieldSpecified;
        
        private string tokenExpiryTimeField;
        
        private NamedParameter[] extensionInfoField;
        
        /// <remarks/>
        public subscriberID subscriberID {
            get {
                return this.subscriberIDField;
            }
            set {
                this.subscriberIDField = value;
            }
        }
        
        /// <remarks/>
        public string partnerID {
            get {
                return this.partnerIDField;
            }
            set {
                this.partnerIDField = value;
            }
        }
        
        /// <remarks/>
        public string serviceID {
            get {
                return this.serviceIDField;
            }
            set {
                this.serviceIDField = value;
            }
        }
        
        /// <remarks/>
        public int capabilityType {
            get {
                return this.capabilityTypeField;
            }
            set {
                this.capabilityTypeField = value;
            }
        }
        
        /// <remarks/>
        public int consentResult {
            get {
                return this.consentResultField;
            }
            set {
                this.consentResultField = value;
            }
        }
        
        /// <remarks/>
        public string accessToken {
            get {
                return this.accessTokenField;
            }
            set {
                this.accessTokenField = value;
            }
        }
        
        /// <remarks/>
        public TokenType tokenType {
            get {
                return this.tokenTypeField;
            }
            set {
                this.tokenTypeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool tokenTypeSpecified {
            get {
                return this.tokenTypeFieldSpecified;
            }
            set {
                this.tokenTypeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string tokenExpiryTime {
            get {
                return this.tokenExpiryTimeField;
            }
            set {
                this.tokenExpiryTimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public NamedParameter[] extensionInfo {
            get {
                return this.extensionInfoField;
            }
            set {
                this.extensionInfoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/subscriberconsnet/data/v1_0")]
    public partial class subscriberID {
        
        private string idField;
        
        private int typeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string ID {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int type {
            get {
                return this.typeField;
            }
            set {
                this.typeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/subscriberconsnet/data/v1_0/local")]
    public partial class notifySubscriberConsentResultResponse {
        
        private int resultField;
        
        private string resultDescriptionField;
        
        private NamedParameter[] extensionInfoField;
        
        /// <remarks/>
        public int result {
            get {
                return this.resultField;
            }
            set {
                this.resultField = value;
            }
        }
        
        /// <remarks/>
        public string resultDescription {
            get {
                return this.resultDescriptionField;
            }
            set {
                this.resultDescriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("item", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable=false)]
        public NamedParameter[] extensionInfo {
            get {
                return this.extensionInfoField;
            }
            set {
                this.extensionInfoField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/subscriberconsnet/data/v1_0")]
    public partial class NamedParameter {
        
        private string keyField;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string key {
            get {
                return this.keyField;
            }
            set {
                this.keyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.34230")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/subscriberconsnet/data/v1_0")]
    public enum TokenType {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("0")]
        Item0,
        
        /// <remarks/>
        [System.Xml.Serialization.XmlEnumAttribute("1")]
        Item1,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    public delegate void notifySubscriberConsentResultCompletedEventHandler(object sender, notifySubscriberConsentResultCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.33440")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class notifySubscriberConsentResultCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal notifySubscriberConsentResultCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public notifySubscriberConsentResultResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((notifySubscriberConsentResultResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591