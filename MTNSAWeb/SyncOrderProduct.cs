//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MTNSAWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class SyncOrderProduct
    {
        public int ID { get; set; }
        public string Service { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Keyword { get; set; }
        public string ServiceID { get; set; }
        public string ProductID { get; set; }
        public Nullable<decimal> Charge { get; set; }
        public Nullable<int> DaysValid { get; set; }
        public Nullable<int> IsActive { get; set; }
        public Nullable<int> IsSub { get; set; }
        public string AccessCode { get; set; }
        public Nullable<int> TestMode { get; set; }
    }
}
