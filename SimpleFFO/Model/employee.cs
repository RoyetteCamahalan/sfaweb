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
    public partial class employee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public employee()
        {
            this.approvaltrees = new HashSet<approvaltree>();
            this.statustrails = new HashSet<statustrail>();
            this.employeeleaves = new HashSet<employeeleave>();
            this.useraccounts = new HashSet<useraccount>();
            this.salaryloans = new HashSet<salaryloan>();
        }
    
        public long employeeid { get; set; }
        public string employeecode { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string address { get; set; }
        public string contactno { get; set; }
        public Nullable<long> branchid { get; set; }
        public Nullable<long> immediatesupervisorid { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public Nullable<System.DateTime> birthdate { get; set; }
        public Nullable<System.DateTime> hiredate { get; set; }
        public string notes { get; set; }
        public string designation { get; set; }
        public Nullable<bool> isactive { get; set; }
        public Nullable<int> employeetypeid { get; set; }
        public byte[] photo { get; set; }
        public Nullable<long> createdbyid { get; set; }
        public Nullable<System.DateTime> datecreated { get; set; }
        public Nullable<long> updatedbyid { get; set; }
        public Nullable<System.DateTime> dateupdated { get; set; }
        public Nullable<bool> syncstatus { get; set; }
        public Nullable<System.DateTime> syncdate { get; set; }
        public Nullable<long> districtmanagerid { get; set; }
        public Nullable<long> warehouseid { get; set; }
        public string ftpfolder { get; set; }
        public Nullable<int> employmentstatus { get; set; }
    
        public virtual branch branch { get; set; }
        public virtual warehouse warehouse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<approvaltree> approvaltrees { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<statustrail> statustrails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<employeeleave> employeeleaves { get; set; }
        public virtual employeetype employeetype { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<useraccount> useraccounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<salaryloan> salaryloans { get; set; }
    }
}
