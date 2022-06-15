//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SimpleFFO.Model
{
    using System;
    using System.Collections.Generic;
    
    [Serializable]
    public partial class warehouse
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public warehouse()
        {
            this.employees = new HashSet<employee>();
            this.institutions = new HashSet<institution>();
            this.expensereports = new HashSet<expensereport>();
            this.practivities = new HashSet<practivity>();
            this.tieups = new HashSet<tieup>();
            this.fundrequests = new HashSet<fundrequest>();
            this.fundliquidations = new HashSet<fundliquidation>();
            this.vehiclerepairs = new HashSet<vehiclerepair>();
        }
    
        public long warehouseid { get; set; }
        public string warehousecode { get; set; }
        public Nullable<long> branchid { get; set; }
        public string warehousedescription { get; set; }
        public Nullable<bool> isactive { get; set; }
        public Nullable<bool> isdefault { get; set; }
        public Nullable<bool> ismain { get; set; }
        public Nullable<long> createdbyid { get; set; }
        public Nullable<System.DateTime> datecreated { get; set; }
        public Nullable<long> updatedbyid { get; set; }
        public Nullable<System.DateTime> dateupdated { get; set; }
        public Nullable<long> assignedvehicle { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<employee> employees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<institution> institutions { get; set; }
        public virtual companyvehicle companyvehicle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<expensereport> expensereports { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<practivity> practivities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tieup> tieups { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fundrequest> fundrequests { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<fundliquidation> fundliquidations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<vehiclerepair> vehiclerepairs { get; set; }
        public virtual branch branch { get; set; }
    }
}