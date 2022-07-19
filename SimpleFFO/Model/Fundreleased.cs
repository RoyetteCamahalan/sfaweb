using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Model
{
    public class Fundreleased
    {
        public string dateencoded { get; set; }
        public long frid_ffo { get; set; }
        public long payeeid { get; set; }
        public string payee { get; set; }
        public long institutionid { get; set; }
        public string institutionname { get; set; }
        public int mode { get; set; }
        public string tuprefno { get; set; }
        public int fundtypeid { get; set; }
        public string checkno { get; set; }
        public string checkdate { get; set; }
        public string amount { get; set; }
        public string remarks { get; set; }

    }
    public class FundreleasedObject
    {
        [JsonProperty(PropertyName = "fundreleased")]
        public List<Fundreleased> Fundreleaseds { get; set; }
    }
}