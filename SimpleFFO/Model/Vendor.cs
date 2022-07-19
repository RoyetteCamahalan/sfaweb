using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Model
{

    public class Vendor
    {
        [JsonIgnore]
        public long Vendorid { get; set; }
        [JsonProperty(nameof(Vendorid))]
        private long VendoridSetter
        {
            set => Vendorid = value;
        }

        [JsonProperty(PropertyName = "vendorname")]
        public string Vendorname { get; set; }

        [JsonIgnore]
        public int Status { get; set; }
        [JsonProperty(nameof(Status))]
        private int StatusSetter
        {
            set => Status = value;
        }

        public string dateencoded { get; set; }

        public string address { get; set; }

        public string telephone { get; set; }

        public string vatno { get; set; }

        public bool vat { get; set; }

        [JsonIgnore]
        public int Branchid { get; set; }
        [JsonProperty(nameof(Branchid))]
        private int BranchidSetter
        {
            set => Branchid = value;
        }

        [JsonProperty(PropertyName = "vendorcategoryid")]
        public int VendorCategoryId { get; set; }

        //[JsonIgnore]
        //public int VendorCategoryId { get; set; }
        //[JsonProperty(nameof(VendorCategoryId))]
        //private int VendorCatergoryIdSetter
        //{
        //    set => VendorCategoryId = value;
        //}
    }

    public class VendorObject
    {
        [JsonProperty(PropertyName = "vendor")]
        public List<Vendor> Vendor { get; set; }
    }



}