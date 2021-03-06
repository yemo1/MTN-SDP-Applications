﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.0.
// 
#pragma warning disable 1591

namespace MTNCInfo.SNSVC {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="SmsNotificationManagerBinding", Namespace="http://www.csapi.org/wsdl/parlayx/sms/notification_manager/v2_3/service")]
    public partial class SmsNotificationManagerService : System.Web.Services.Protocols.SoapHttpClientProtocol {

        [XmlElement(Namespace = "http://www.huawei.com.cn/schema/common/v2_1")]
        public RequestSOAPHeader authCred;
        
        private System.Threading.SendOrPostCallback startSmsNotificationOperationCompleted;
        
        private System.Threading.SendOrPostCallback stopSmsNotificationOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public SmsNotificationManagerService() {
            this.Url = string.Format(global::MTNCInfo.Properties.Settings.Default.MTNCInfo_SNSVC_SmsNotificationManagerService, ((AppUtil.TestMode == 0) ? AppUtil.LiveIP : AppUtil.TestIP));
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
        public event startSmsNotificationCompletedEventHandler startSmsNotificationCompleted;
        
        /// <remarks/>
        public event stopSmsNotificationCompletedEventHandler stopSmsNotificationCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeader("authCred", Direction = SoapHeaderDirection.InOut)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("startSmsNotificationResponse", Namespace="http://www.csapi.org/schema/parlayx/sms/notification_manager/v2_3/local")]
        public startSmsNotificationResponse startSmsNotification([System.Xml.Serialization.XmlElementAttribute("startSmsNotification", Namespace="http://www.csapi.org/schema/parlayx/sms/notification_manager/v2_3/local")] startSmsNotification startSmsNotification1) {
            object[] results = this.Invoke("startSmsNotification", new object[] {
                        startSmsNotification1});
            return ((startSmsNotificationResponse)(results[0]));
        }
        
        /// <remarks/>
        public void startSmsNotificationAsync(startSmsNotification startSmsNotification1) {
            this.startSmsNotificationAsync(startSmsNotification1, null);
        }
        
        /// <remarks/>
        public void startSmsNotificationAsync(startSmsNotification startSmsNotification1, object userState) {
            if ((this.startSmsNotificationOperationCompleted == null)) {
                this.startSmsNotificationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnstartSmsNotificationOperationCompleted);
            }
            this.InvokeAsync("startSmsNotification", new object[] {
                        startSmsNotification1}, this.startSmsNotificationOperationCompleted, userState);
        }
        
        private void OnstartSmsNotificationOperationCompleted(object arg) {
            if ((this.startSmsNotificationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.startSmsNotificationCompleted(this, new startSmsNotificationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeader("authCred", Direction = SoapHeaderDirection.InOut)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("stopSmsNotificationResponse", Namespace="http://www.csapi.org/schema/parlayx/sms/notification_manager/v2_3/local")]
        public stopSmsNotificationResponse stopSmsNotification([System.Xml.Serialization.XmlElementAttribute("stopSmsNotification", Namespace="http://www.csapi.org/schema/parlayx/sms/notification_manager/v2_3/local")] stopSmsNotification stopSmsNotification1) {
            object[] results = this.Invoke("stopSmsNotification", new object[] {
                        stopSmsNotification1});
            return ((stopSmsNotificationResponse)(results[0]));
        }
        
        /// <remarks/>
        public void stopSmsNotificationAsync(stopSmsNotification stopSmsNotification1) {
            this.stopSmsNotificationAsync(stopSmsNotification1, null);
        }
        
        /// <remarks/>
        public void stopSmsNotificationAsync(stopSmsNotification stopSmsNotification1, object userState) {
            if ((this.stopSmsNotificationOperationCompleted == null)) {
                this.stopSmsNotificationOperationCompleted = new System.Threading.SendOrPostCallback(this.OnstopSmsNotificationOperationCompleted);
            }
            this.InvokeAsync("stopSmsNotification", new object[] {
                        stopSmsNotification1}, this.stopSmsNotificationOperationCompleted, userState);
        }
        
        private void OnstopSmsNotificationOperationCompleted(object arg) {
            if ((this.stopSmsNotificationCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.stopSmsNotificationCompleted(this, new stopSmsNotificationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/parlayx/sms/notification_manager/v2_3/local")]
    public partial class startSmsNotification {
        
        private SimpleReference referenceField;
        
        private string smsServiceActivationNumberField;
        
        private string criteriaField;
        
        /// <remarks/>
        public SimpleReference reference {
            get {
                return this.referenceField;
            }
            set {
                this.referenceField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="anyURI")]
        public string smsServiceActivationNumber {
            get {
                return this.smsServiceActivationNumberField;
            }
            set {
                this.smsServiceActivationNumberField = value;
            }
        }
        
        /// <remarks/>
        public string criteria {
            get {
                return this.criteriaField;
            }
            set {
                this.criteriaField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/parlayx/common/v2_1")]
    public partial class SimpleReference {
        
        private string endpointField;
        
        private string interfaceNameField;
        
        private string correlatorField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType="anyURI")]
        public string endpoint {
            get {
                return this.endpointField;
            }
            set {
                this.endpointField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string interfaceName {
            get {
                return this.interfaceNameField;
            }
            set {
                this.interfaceNameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string correlator {
            get {
                return this.correlatorField;
            }
            set {
                this.correlatorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/parlayx/sms/notification_manager/v2_3/local")]
    public partial class stopSmsNotificationResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/parlayx/sms/notification_manager/v2_3/local")]
    public partial class stopSmsNotification {
        
        private string correlatorField;
        
        /// <remarks/>
        public string correlator {
            get {
                return this.correlatorField;
            }
            set {
                this.correlatorField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/parlayx/sms/notification_manager/v2_3/local")]
    public partial class startSmsNotificationResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    public delegate void startSmsNotificationCompletedEventHandler(object sender, startSmsNotificationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class startSmsNotificationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal startSmsNotificationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public startSmsNotificationResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((startSmsNotificationResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    public delegate void stopSmsNotificationCompletedEventHandler(object sender, stopSmsNotificationCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class stopSmsNotificationCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal stopSmsNotificationCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public stopSmsNotificationResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((stopSmsNotificationResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591