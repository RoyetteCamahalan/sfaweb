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
    public partial class callmaterial
    {
        public long call_material_id { get; set; }
        public long call_id { get; set; }
        public int product_id { get; set; }
        public int material_id { get; set; }
        public Nullable<int> material_count { get; set; }
        public Nullable<int> detail_aid_tag { get; set; }
        public Nullable<System.DateTime> deleted_at { get; set; }
    }
}
