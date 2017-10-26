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

namespace MTNCChat.CASVC {
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
    [System.Web.Services.WebServiceBindingAttribute(Name="AmountChargingBinding", Namespace="http://www.csapi.org/wsdl/parlayx/payment/amount_charging/v2_1/service")]
    public partial class AmountChargingService : System.Web.Services.Protocols.SoapHttpClientProtocol {

        [XmlElement(Namespace = "http://www.huawei.com.cn/schema/common/v2_1")]
        public RequestSOAPHeader authCred;
        
        private System.Threading.SendOrPostCallback chargeAmountOperationCompleted;
        
        private System.Threading.SendOrPostCallback refundAmountOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public AmountChargingService() {
            this.Url = string.Format(global::MTNCChat.Properties.Settings.Default.MTNCChat_CASVC_AmountChargingService, ((AppUtil.TestMode == 0) ? AppUtil.LiveIP : AppUtil.TestIP));
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
        public event chargeAmountCompletedEventHandler chargeAmountCompleted;
        
        /// <remarks/>
        public event refundAmountCompletedEventHandler refundAmountCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeader("authCred", Direction = SoapHeaderDirection.InOut)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("chargeAmountResponse", Namespace="http://www.csapi.org/schema/parlayx/payment/amount_charging/v2_1/local")]
        public chargeAmountResponse chargeAmount([System.Xml.Serialization.XmlElementAttribute("chargeAmount", Namespace="http://www.csapi.org/schema/parlayx/payment/amount_charging/v2_1/local")] chargeAmount chargeAmount1) {
            object[] results = this.Invoke("chargeAmount", new object[] {
                        chargeAmount1});
            return ((chargeAmountResponse)(results[0]));
        }
        
        /// <remarks/>
        public void chargeAmountAsync(chargeAmount chargeAmount1) {
            this.chargeAmountAsync(chargeAmount1, null);
        }
        
        /// <remarks/>
        public void chargeAmountAsync(chargeAmount chargeAmount1, object userState) {
            if ((this.chargeAmountOperationCompleted == null)) {
                this.chargeAmountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnchargeAmountOperationCompleted);
            }
            this.InvokeAsync("chargeAmount", new object[] {
                        chargeAmount1}, this.chargeAmountOperationCompleted, userState);
        }
        
        private void OnchargeAmountOperationCompleted(object arg) {
            if ((this.chargeAmountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.chargeAmountCompleted(this, new chargeAmountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapHeader("authCred", Direction = SoapHeaderDirection.InOut)]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        [return: System.Xml.Serialization.XmlElementAttribute("refundAmountResponse", Namespace="http://www.csapi.org/schema/parlayx/payment/amount_charging/v2_1/local")]
        public refundAmountResponse refundAmount([System.Xml.Serialization.XmlElementAttribute("refundAmount", Namespace="http://www.csapi.org/schema/parlayx/payment/amount_charging/v2_1/local")] refundAmount refundAmount1) {
            object[] results = this.Invoke("refundAmount", new object[] {
                        refundAmount1});
            return ((refundAmountResponse)(results[0]));
        }
        
        /// <remarks/>
        public void refundAmountAsync(refundAmount refundAmount1) {
            this.refundAmountAsync(refundAmount1, null);
        }
        
        /// <remarks/>
        public void refundAmountAsync(refundAmount refundAmount1, object userState) {
            if ((this.refundAmountOperationCompleted == null)) {
                this.refundAmountOperationCompleted = new System.Threading.SendOrPostCallback(this.OnrefundAmountOperationCompleted);
            }
            this.InvokeAsync("refundAmount", new object[] {
                        refundAmount1}, this.refundAmountOperationCompleted, userState);
        }
        
        private void OnrefundAmountOperationCompleted(object arg) {
            if ((this.refundAmountCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.refundAmountCompleted(this, new refundAmountCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/parlayx/payment/amount_charging/v2_1/local")]
    public partial class chargeAmount {
        
        private string endUserIdentifierField;
        
        private ChargingInformation chargeField;
        
        private string referenceCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="anyURI")]
        public string endUserIdentifier {
            get {
                return this.endUserIdentifierField;
            }
            set {
                this.endUserIdentifierField = value;
            }
        }
        
        /// <remarks/>
        public ChargingInformation charge {
            get {
                return this.chargeField;
            }
            set {
                this.chargeField = value;
            }
        }
        
        /// <remarks/>
        public string referenceCode {
            get {
                return this.referenceCodeField;
            }
            set {
                this.referenceCodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/parlayx/common/v2_1")]
    public partial class ChargingInformation {
        
        private string descriptionField;
        
        private string currencyField;
        
        private decimal amountField;
        
        private bool amountFieldSpecified;
        
        private string codeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string currency {
            get {
                return this.currencyField;
            }
            set {
                this.currencyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public decimal amount {
            get {
                return this.amountField;
            }
            set {
                this.amountField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool amountSpecified {
            get {
                return this.amountFieldSpecified;
            }
            set {
                this.amountFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string code {
            get {
                return this.codeField;
            }
            set {
                this.codeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/parlayx/payment/amount_charging/v2_1/local")]
    public partial class refundAmountResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/parlayx/payment/amount_charging/v2_1/local")]
    public partial class refundAmount {
        
        private string endUserIdentifierField;
        
        private ChargingInformation chargeField;
        
        private string referenceCodeField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="anyURI")]
        public string endUserIdentifier {
            get {
                return this.endUserIdentifierField;
            }
            set {
                this.endUserIdentifierField = value;
            }
        }
        
        /// <remarks/>
        public ChargingInformation charge {
            get {
                return this.chargeField;
            }
            set {
                this.chargeField = value;
            }
        }
        
        /// <remarks/>
        public string referenceCode {
            get {
                return this.referenceCodeField;
            }
            set {
                this.referenceCodeField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.57.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://www.csapi.org/schema/parlayx/payment/amount_charging/v2_1/local")]
    public partial class chargeAmountResponse {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    public delegate void chargeAmountCompletedEventHandler(object sender, chargeAmountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class chargeAmountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal chargeAmountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public chargeAmountResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((chargeAmountResponse)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    public delegate void refundAmountCompletedEventHandler(object sender, refundAmountCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.57.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class refundAmountCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal refundAmountCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public refundAmountResponse Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((refundAmountResponse)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591