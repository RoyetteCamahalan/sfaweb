using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Model
{
    public class PostExpenseReports
    {
        public DateTime? dateencoded { get; set; }
        public long vendorid { get; set; }
        public long expenseid { get; set; }
        public string refno { get; set; }
        public DateTime? refdate { get; set; }
        public decimal? amount { get; set; }
        public bool? vat { get; set; }
        public string remarks { get; set; }
        public long ffo_expensereportid { get; set; }
    }

    public class ExpenseReportObject
    {
        [JsonProperty(PropertyName = "expensereport")]
        public List<PostExpenseReports> ExpenseReportPost { get; set; }
    }
}