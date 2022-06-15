using SimpleFFO.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views
{
    public partial class LocationViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(Page.RouteData.Values.ContainsKey("latitude") && Page.RouteData.Values.ContainsKey("longitude"))
                {
                    string longitude = Page.RouteData.Values["longitude"].ToString();
                    string latitude = Page.RouteData.Values["latitude"].ToString();
                    urlframe.Attributes.Add("src", Utility.getMapEmbeddedURL(longitude.Replace("_","."), latitude.Replace("_", ".")));
                }
            }
        }
    }
}