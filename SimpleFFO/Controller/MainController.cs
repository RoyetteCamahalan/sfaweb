using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class MainController : SimpleDB
    {
        public List<appversion> GetAppversions()
        {
            return this.appversions.OrderByDescending(x => x.versionid).ToList();
        }
    }
}