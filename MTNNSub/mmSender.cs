//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTNNSub
{
    using System;
    using System.Collections.Generic;
    
    public partial class mmSender
    {
        public int ID { get; set; }
        public string Originator { get; set; }
        public string Destination { get; set; }
        public string TextMessage { get; set; }
        public string ServiceTo { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<int> mStatus { get; set; }
        public Nullable<int> mState { get; set; }
        public Nullable<int> mIndex { get; set; }
        public Nullable<int> mOption { get; set; }
        public string mErrorDesc { get; set; }
        public string sLastStatus { get; set; }
        public Nullable<int> sErrorCode { get; set; }
        public string sErrorText { get; set; }
        public string sLastTimeStamp { get; set; }
        public string sErrorDesc { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public Nullable<System.DateTime> Datestamp { get; set; }
    }
}
