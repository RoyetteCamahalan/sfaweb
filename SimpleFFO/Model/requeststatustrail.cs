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
    public partial class requeststatustrail
    {
        public long requeststatusid { get; set; }
        public Nullable<int> requesttype { get; set; }
        public Nullable<long> tieup_activityid { get; set; }
        public Nullable<long> employeeid { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<System.DateTime> statusdate { get; set; }
        public string remarks { get; set; }
    }
}
