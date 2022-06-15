using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class SessionController
    {
        public class SesssionKeys
        {
            public const string employeeid = "employeeid";
            public const string showupdates = "showupdates";
        }

        public static void setSession(string key, object value) {
            HttpContext.Current.Session[key] = value;
        }
        public static object getSession(string key)
        {
            return HttpContext.Current.Session[key];
        }
    }
}