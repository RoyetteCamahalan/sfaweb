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
    public partial class practivityoutcome
    {
        public long practivityoutcomeid { get; set; }
        public Nullable<long> practivityid { get; set; }
        public Nullable<int> product_id { get; set; }
        public Nullable<decimal> monthlysales { get; set; }
    
        public virtual practivity practivity { get; set; }
        public virtual product product { get; set; }
    }
}
