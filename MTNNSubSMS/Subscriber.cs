//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTNNSubSMS
{
    using System;
    using System.Collections.Generic;
    
    public partial class Subscriber
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string MSISDN { get; set; }
        public string Shortcode { get; set; }
        public System.DateTime SubDate { get; set; }
        public System.DateTime ExpDate { get; set; }
        public int CurrentCredit { get; set; }
        public Nullable<System.DateTime> MemberSince { get; set; }
        public Nullable<System.DateTime> LastSubDate { get; set; }
        public string LastCode { get; set; }
        public Nullable<int> TotalHit { get; set; }
        public Nullable<int> Status { get; set; }
    }
}
