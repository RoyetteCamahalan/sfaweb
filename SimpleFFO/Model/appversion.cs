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
    public partial class appversion
    {
        public long versionid { get; set; }
        public string versioncode { get; set; }
        public Nullable<System.DateTime> releasedate { get; set; }
        public string newfeatures { get; set; }
        public string bugfixes { get; set; }
    }
}
