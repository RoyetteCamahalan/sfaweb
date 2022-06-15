using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Model
{
    [Serializable]
    public class GenericObject
    {
        public long id
        {
            get; set;
        }
        public int moduleid
        {
            get; set;
        }
        public string modulename
        {
            get { return AppModels.Modules.getName(this.moduleid); }
        }
        public string employeename
        {
            get; set;
        }
        public DateTime date
        {
            get; set;
        }
        public string reference
        {
            get; set;
        }

        public int status
        {
            get; set;
        }
        public int statusacc
        {
            get; set;
        }
        public DateTime dayfrom
        {
            get; set;
        }
        public decimal totalamount
        {
            get; set;
        }


        public int treelevel
        {
            get; set;
        }
        public int statusaction
        {
            get; set;
        }
        public long endorsenext
        {
            get; set;
        }

        public string rowid
        {
            get; set;
        }
        public int qty
        {
            get; set;
        }
        public decimal price
        {
            get; set;
        }
        public string stringval
        {
            get; set;
        }

        public int specialization_id
        {
            get; set;
        }
        public string name
        {
            get; set;
        }
    }
}