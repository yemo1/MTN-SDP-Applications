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
    
    public partial class SyncOrderRelationExtended
    {
        public SyncOrderRelationExtended()
        {
            this.SyncOrderRelationResponses = new HashSet<SyncOrderRelationResponse>();
        }
    
        public int ID { get; set; }
        public Nullable<int> SyncOrderRelationID { get; set; }
        public string AccessCode { get; set; }
        public string OperationCode { get; set; }
        public Nullable<int> ChargeMode { get; set; }
        public string MDSPSUBEXPMODE { get; set; }
        public string ObjectType { get; set; }
        public string StartTime { get; set; }
        public Nullable<bool> IsFreePeriod { get; set; }
        public string OperatorID { get; set; }
        public Nullable<int> PayType { get; set; }
        public string TransactionID { get; set; }
        public string OrderKey { get; set; }
        public string Keyword { get; set; }
        public string DurationOfGracePeriod { get; set; }
        public string ServiceAvailability { get; set; }
        public string DurationOfSuspensionPeriod { get; set; }
        public string ServicePayType { get; set; }
        public Nullable<int> Fee { get; set; }
        public string CycleEndTime { get; set; }
        public Nullable<int> ChannelID { get; set; }
        public string TraceUniqueID { get; set; }
        public Nullable<bool> RentSucess { get; set; }
        public Nullable<bool> Try { get; set; }
        public Nullable<int> UpdateReason { get; set; }
        public string DedicatedAccountOperationType { get; set; }
        public string DAExpireDate { get; set; }
        public string DAUnit { get; set; }
    
        public virtual SyncOrderRelation SyncOrderRelation { get; set; }
        public virtual ICollection<SyncOrderRelationResponse> SyncOrderRelationResponses { get; set; }
    }
}
