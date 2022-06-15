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
    public partial class doctor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public doctor()
        {
            this.institutiondoctormaps = new HashSet<institutiondoctormap>();
            this.practivities = new HashSet<practivity>();
        }
    
        public int doc_id { get; set; }
        public string doc_code { get; set; }
        public string doc_lastname { get; set; }
        public string doc_firstname { get; set; }
        public string doc_mi { get; set; }
        public Nullable<int> specialization_id { get; set; }
        public Nullable<System.DateTime> birthdate { get; set; }
        public string contact_number { get; set; }
        public string prc_licensed { get; set; }
        public string email_address { get; set; }
        public Nullable<long> createdbyid { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<long> updatedbyid { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
        public Nullable<bool> isactive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<institutiondoctormap> institutiondoctormaps { get; set; }
        public virtual specialization specialization { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<practivity> practivities { get; set; }
    }
}
