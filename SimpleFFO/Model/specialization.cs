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
    public partial class specialization
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public specialization()
        {
            this.doctors = new HashSet<doctor>();
        }
    
        public int specialization_id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public Nullable<bool> ismedical { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<doctor> doctors { get; set; }
    }
}
