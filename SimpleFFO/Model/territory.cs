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
    public partial class territory
    {
        public long areaid { get; set; }
        public string areaname { get; set; }
        public Nullable<long> districtid { get; set; }
        public Nullable<bool> isactive { get; set; }
        public string createdbyid { get; set; }
        public Nullable<System.DateTime> datecreated { get; set; }
        public string updatedbyid { get; set; }
        public Nullable<System.DateTime> dateupdated { get; set; }
    }
}
