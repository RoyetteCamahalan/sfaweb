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
    public partial class expensereportmiscellaneou
    {
        public long miscexpenseid { get; set; }
        public Nullable<long> expensereportid { get; set; }
        public Nullable<int> misccodeid { get; set; }
        public Nullable<System.DateTime> expensedate { get; set; }
        public string particulars { get; set; }
        public Nullable<decimal> amount { get; set; }
        public string imgloc { get; set; }
    
        public virtual expensereport expensereport { get; set; }
        public virtual miscexpensecode miscexpensecode { get; set; }
    }
}
