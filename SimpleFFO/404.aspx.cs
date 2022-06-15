using SimpleFFO.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO
{
    public partial class _404 : System.Web.UI.Page
    {
        Auth auth;
        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this,Auth.middleware.guest);
            if (Request.QueryString["aspxerrorpath"] != null)
            {
                auth.SaveLog("Error Log", Request.QueryString["aspxerrorpath"]);
            }

        }
    }
}