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
    public partial class expensereport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public expensereport()
        {
            this.expensereportdetails = new HashSet<expensereportdetail>();
            this.expensereportmiscellaneous = new HashSet<expensereportmiscellaneou>();
        }
    
        public long expensereportid { get; set; }
        public Nullable<long> warehouseid { get; set; }
        public Nullable<System.DateTime> datefrom { get; set; }
        public Nullable<System.DateTime> dateend { get; set; }
        public Nullable<decimal> totalexpense { get; set; }
        public Nullable<decimal> totalmisc { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<long> currentodo { get; set; }
        public Nullable<long> personalmileage { get; set; }
        public Nullable<long> businessmileage { get; set; }
        public string odoimagepath { get; set; }
        public Nullable<System.DateTime> datefiled { get; set; }
        public Nullable<long> previousodo { get; set; }
        public Nullable<long> endorsedto { get; set; }
        public Nullable<long> vehicleid { get; set; }
        public Nullable<bool> isvat { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<expensereportdetail> expensereportdetails { get; set; }
        public virtual warehouse warehouse { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<expensereportmiscellaneou> expensereportmiscellaneous { get; set; }
    }
}
