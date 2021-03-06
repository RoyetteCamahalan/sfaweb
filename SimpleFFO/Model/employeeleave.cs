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
    public partial class employeeleave
    {
        public long leaveid { get; set; }
        public Nullable<long> employeeid { get; set; }
        public Nullable<int> leavetypeid { get; set; }
        public Nullable<System.DateTime> dayfrom { get; set; }
        public Nullable<System.DateTime> dayto { get; set; }
        public Nullable<decimal> noofdays { get; set; }
        public Nullable<System.DateTime> bacttowork { get; set; }
        public string reason { get; set; }
        public string contactdetails { get; set; }
        public Nullable<long> reliever { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<bool> hashalfday { get; set; }
        public Nullable<System.DateTime> datecreated { get; set; }
        public Nullable<long> endorsedto { get; set; }
    
        public virtual leavetype leavetype { get; set; }
        public virtual employee employee { get; set; }
    }
}
