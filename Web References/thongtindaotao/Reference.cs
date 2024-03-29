﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18449
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.18449.
// 
#pragma warning disable 1591

namespace WebService_use.thongtindaotao {
    using System;
    using System.Web.Services;
    using System.Diagnostics;
    using System.Web.Services.Protocols;
    using System.Xml.Serialization;
    using System.ComponentModel;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Web.Services.WebServiceBindingAttribute(Name="ServiceSoap", Namespace="http://tempuri.org/")]
    public partial class Service : System.Web.Services.Protocols.SoapHttpClientProtocol {
        
        private System.Threading.SendOrPostCallback XemThongTinOperationCompleted;
        
        private System.Threading.SendOrPostCallback XemLichThiOperationCompleted;
        
        private System.Threading.SendOrPostCallback XemDiemOperationCompleted;
        
        private System.Threading.SendOrPostCallback XemTKBOperationCompleted;
        
        private bool useDefaultCredentialsSetExplicitly;
        
        /// <remarks/>
        public Service() {
            this.Url = global::WebService_use.Properties.Settings.Default.WebService_use_thongtindaotao_Service;
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
        public event XemThongTinCompletedEventHandler XemThongTinCompleted;
        
        /// <remarks/>
        public event XemLichThiCompletedEventHandler XemLichThiCompleted;
        
        /// <remarks/>
        public event XemDiemCompletedEventHandler XemDiemCompleted;
        
        /// <remarks/>
        public event XemTKBCompletedEventHandler XemTKBCompleted;
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/XemThongTin", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string XemThongTin(string mssv) {
            object[] results = this.Invoke("XemThongTin", new object[] {
                        mssv});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void XemThongTinAsync(string mssv) {
            this.XemThongTinAsync(mssv, null);
        }
        
        /// <remarks/>
        public void XemThongTinAsync(string mssv, object userState) {
            if ((this.XemThongTinOperationCompleted == null)) {
                this.XemThongTinOperationCompleted = new System.Threading.SendOrPostCallback(this.OnXemThongTinOperationCompleted);
            }
            this.InvokeAsync("XemThongTin", new object[] {
                        mssv}, this.XemThongTinOperationCompleted, userState);
        }
        
        private void OnXemThongTinOperationCompleted(object arg) {
            if ((this.XemThongTinCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.XemThongTinCompleted(this, new XemThongTinCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/XemLichThi", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string XemLichThi(string mssv) {
            object[] results = this.Invoke("XemLichThi", new object[] {
                        mssv});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void XemLichThiAsync(string mssv) {
            this.XemLichThiAsync(mssv, null);
        }
        
        /// <remarks/>
        public void XemLichThiAsync(string mssv, object userState) {
            if ((this.XemLichThiOperationCompleted == null)) {
                this.XemLichThiOperationCompleted = new System.Threading.SendOrPostCallback(this.OnXemLichThiOperationCompleted);
            }
            this.InvokeAsync("XemLichThi", new object[] {
                        mssv}, this.XemLichThiOperationCompleted, userState);
        }
        
        private void OnXemLichThiOperationCompleted(object arg) {
            if ((this.XemLichThiCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.XemLichThiCompleted(this, new XemLichThiCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/XemDiem", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string XemDiem(string mssv) {
            object[] results = this.Invoke("XemDiem", new object[] {
                        mssv});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void XemDiemAsync(string mssv) {
            this.XemDiemAsync(mssv, null);
        }
        
        /// <remarks/>
        public void XemDiemAsync(string mssv, object userState) {
            if ((this.XemDiemOperationCompleted == null)) {
                this.XemDiemOperationCompleted = new System.Threading.SendOrPostCallback(this.OnXemDiemOperationCompleted);
            }
            this.InvokeAsync("XemDiem", new object[] {
                        mssv}, this.XemDiemOperationCompleted, userState);
        }
        
        private void OnXemDiemOperationCompleted(object arg) {
            if ((this.XemDiemCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.XemDiemCompleted(this, new XemDiemCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }
        
        /// <remarks/>
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://tempuri.org/XemTKB", RequestNamespace="http://tempuri.org/", ResponseNamespace="http://tempuri.org/", Use=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string XemTKB(string mssv) {
            object[] results = this.Invoke("XemTKB", new object[] {
                        mssv});
            return ((string)(results[0]));
        }
        
        /// <remarks/>
        public void XemTKBAsync(string mssv) {
            this.XemTKBAsync(mssv, null);
        }
        
        /// <remarks/>
        public void XemTKBAsync(string mssv, object userState) {
            if ((this.XemTKBOperationCompleted == null)) {
                this.XemTKBOperationCompleted = new System.Threading.SendOrPostCallback(this.OnXemTKBOperationCompleted);
            }
            this.InvokeAsync("XemTKB", new object[] {
                        mssv}, this.XemTKBOperationCompleted, userState);
        }
        
        private void OnXemTKBOperationCompleted(object arg) {
            if ((this.XemTKBCompleted != null)) {
                System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
                this.XemTKBCompleted(this, new XemTKBCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
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
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void XemThongTinCompletedEventHandler(object sender, XemThongTinCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class XemThongTinCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal XemThongTinCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void XemLichThiCompletedEventHandler(object sender, XemLichThiCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class XemLichThiCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal XemLichThiCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void XemDiemCompletedEventHandler(object sender, XemDiemCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class XemDiemCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal XemDiemCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    public delegate void XemTKBCompletedEventHandler(object sender, XemTKBCompletedEventArgs e);
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.17929")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class XemTKBCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs {
        
        private object[] results;
        
        internal XemTKBCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) : 
                base(exception, cancelled, userState) {
            this.results = results;
        }
        
        /// <remarks/>
        public string Result {
            get {
                this.RaiseExceptionIfNecessary();
                return ((string)(this.results[0]));
            }
        }
    }
}

#pragma warning restore 1591