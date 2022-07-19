using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Model
{
    public class Fundliquidations
    {
        public string dateencoded { get; set; }
        public long vendorid { get; set; }
        public long expenseid { get; set; }
        public string refno { get; set; }
        public string refdate { get; set; }
        public string amount { get; set; }
        public bool vat { get; set; }
        public string remarks { get; set; }
        public long ffo_flid { get; set; }
        public long ffo_frid { get; set; }

    }
    public class FundliquidationsObject
    {
        [JsonProperty(PropertyName = "fundliquidation")]
        public List<Fundliquidations> FundLiquidations { get; set; }
    }
}