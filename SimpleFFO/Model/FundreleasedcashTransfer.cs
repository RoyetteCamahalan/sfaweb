using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Model
{
    public class FundreleasedcashTransfer
    {
        public string dateencoded { get; set; }
        public string amount { get; set; }
        public string remarks { get; set; }
        public long ffo_fundreleasedid { get; set; }

    }
    public class FundreleasedcashTransferObj
    {
        [JsonProperty(PropertyName = "fundreleasedcashtransfer")]
        public List<FundreleasedcashTransfer> Fundreleaseds { get; set; }
    }
}