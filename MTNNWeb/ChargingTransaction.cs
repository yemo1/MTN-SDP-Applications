//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTNNWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class ChargingTransaction
    {
        public int ID { get; set; }
        public string MSISDN { get; set; }
        public string Code { get; set; }
        public Nullable<int> ChargeAmount { get; set; }
        public string ChargeType { get; set; }
        public string Request { get; set; }
        public string Response { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> Timestamped { get; set; }
    }
}
