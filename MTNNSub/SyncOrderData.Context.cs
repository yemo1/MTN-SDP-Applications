﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class SyncOrderEntities : DbContext
    {
        public SyncOrderEntities()
            : base("name=SyncOrderEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<SyncOrderRelation> SyncOrderRelations { get; set; }
        public virtual DbSet<SyncOrderRelationExtended> SyncOrderRelationExtendeds { get; set; }
        public virtual DbSet<SyncOrderRelationResponse> SyncOrderRelationResponses { get; set; }
        public virtual DbSet<SyncOrderError> SyncOrderErrors { get; set; }
        public virtual DbSet<SyncOrderProduct> SyncOrderProducts { get; set; }
        public virtual DbSet<mmSender> mmSenders { get; set; }
        public virtual DbSet<AppLog> AppLogs { get; set; }
        public virtual DbSet<Subscriber> Subscribers { get; set; }
        public virtual DbSet<ChargingTransaction> ChargingTransactions { get; set; }
    
        public virtual int subsa(string message, string msisdn, string shortcode, string network, string productID, Nullable<int> charge, Nullable<int> numdays, Nullable<int> updateType, ObjectParameter result)
        {
            var messageParameter = message != null ?
                new ObjectParameter("message", message) :
                new ObjectParameter("message", typeof(string));
    
            var msisdnParameter = msisdn != null ?
                new ObjectParameter("msisdn", msisdn) :
                new ObjectParameter("msisdn", typeof(string));
    
            var shortcodeParameter = shortcode != null ?
                new ObjectParameter("shortcode", shortcode) :
                new ObjectParameter("shortcode", typeof(string));
    
            var networkParameter = network != null ?
                new ObjectParameter("Network", network) :
                new ObjectParameter("Network", typeof(string));
    
            var productIDParameter = productID != null ?
                new ObjectParameter("ProductID", productID) :
                new ObjectParameter("ProductID", typeof(string));
    
            var chargeParameter = charge.HasValue ?
                new ObjectParameter("charge", charge) :
                new ObjectParameter("charge", typeof(int));
    
            var numdaysParameter = numdays.HasValue ?
                new ObjectParameter("numdays", numdays) :
                new ObjectParameter("numdays", typeof(int));
    
            var updateTypeParameter = updateType.HasValue ?
                new ObjectParameter("updateType", updateType) :
                new ObjectParameter("updateType", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("subsa", messageParameter, msisdnParameter, shortcodeParameter, networkParameter, productIDParameter, chargeParameter, numdaysParameter, updateTypeParameter, result);
        }
    
        public virtual int otpsub(string token, string mSISDN, string svcID, string pID, Nullable<System.DateTime> tokenexpiry, ObjectParameter refKey)
        {
            var tokenParameter = token != null ?
                new ObjectParameter("token", token) :
                new ObjectParameter("token", typeof(string));
    
            var mSISDNParameter = mSISDN != null ?
                new ObjectParameter("MSISDN", mSISDN) :
                new ObjectParameter("MSISDN", typeof(string));
    
            var svcIDParameter = svcID != null ?
                new ObjectParameter("svcID", svcID) :
                new ObjectParameter("svcID", typeof(string));
    
            var pIDParameter = pID != null ?
                new ObjectParameter("pID", pID) :
                new ObjectParameter("pID", typeof(string));
    
            var tokenexpiryParameter = tokenexpiry.HasValue ?
                new ObjectParameter("tokenexpiry", tokenexpiry) :
                new ObjectParameter("tokenexpiry", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("otpsub", tokenParameter, mSISDNParameter, svcIDParameter, pIDParameter, tokenexpiryParameter, refKey);
        }
    
        public virtual int receive(string message, string msisdn, string shortcode, string network, ObjectParameter result)
        {
            var messageParameter = message != null ?
                new ObjectParameter("message", message) :
                new ObjectParameter("message", typeof(string));
    
            var msisdnParameter = msisdn != null ?
                new ObjectParameter("msisdn", msisdn) :
                new ObjectParameter("msisdn", typeof(string));
    
            var shortcodeParameter = shortcode != null ?
                new ObjectParameter("shortcode", shortcode) :
                new ObjectParameter("shortcode", typeof(string));
    
            var networkParameter = network != null ?
                new ObjectParameter("Network", network) :
                new ObjectParameter("Network", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("receive", messageParameter, msisdnParameter, shortcodeParameter, networkParameter, result);
        }
    }
}
