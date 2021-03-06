//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PayslipDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceIssueType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ServiceIssueType()
        {
            this.ExporterEpbServices = new HashSet<ExporterEpbService>();
            this.ServiceIssueTypeHourlyCosts = new HashSet<ServiceIssueTypeHourlyCost>();
        }
    
        public string Id { get; set; }
        public string ServiceTypeId { get; set; }
        public string Name { get; set; }
        public string NewName { get; set; }
        public double Cost { get; set; }
        public bool IsHourlyService { get; set; }
        public System.DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool Active { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExporterEpbService> ExporterEpbServices { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ServiceIssueTypeHourlyCost> ServiceIssueTypeHourlyCosts { get; set; }
    }
}
