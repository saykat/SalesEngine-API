//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GspDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class FormAIssue
    {
        public System.Guid Id { get; set; }
        public System.DateTime IssueDate { get; set; }
        public string ExporterNo { get; set; }
        public string ExporterName { get; set; }
        public string FormRange { get; set; }
        public int Total { get; set; }
        public System.Guid ExporterId { get; set; }
        public System.Guid DistributorId { get; set; }
        public Nullable<System.Guid> DistributorUserId { get; set; }
    
        public virtual Distributor Distributor { get; set; }
        public virtual Exporter Exporter { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}