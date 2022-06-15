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
    public partial class institutiondoctormap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public institutiondoctormap()
        {
            this.calls = new HashSet<call>();
        }
    
        public long idm_id { get; set; }
        public Nullable<int> inst_id { get; set; }
        public Nullable<int> doc_id { get; set; }
        public Nullable<int> class_id { get; set; }
        public string best_time_to_call { get; set; }
        public string room_number { get; set; }
        public string default_products { get; set; }
        public Nullable<int> stage_id { get; set; }
        public Nullable<long> createdbyid { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<long> updatedbyid { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
        public Nullable<bool> isactive { get; set; }
    
        public virtual institution institution { get; set; }
        public virtual doctor doctor { get; set; }
        public virtual doctorclass doctorclass { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<call> calls { get; set; }
    }
}
