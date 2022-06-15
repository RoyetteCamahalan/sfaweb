using Newtonsoft.Json;
using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views
{
    public partial class MobileApps : System.Web.UI.Page
    {
        #region "Vars"
        private AppModels.SimpleWebConfig conf
        {
            get { return (AppModels.SimpleWebConfig)ViewState["simpleconfig"]; }
            set { ViewState["simpleconfig"]=value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            Auth auth = new Auth(this);
            if (!auth.hasAuth())
                return;

            if (!Page.IsPostBack)
            {
                getSimpleWebConfig();
            }
        }
        private void getSimpleWebConfig()
        {
            var json = new WebClient().DownloadString(AppModels.simplesoftechconfigurl);
            conf= JsonConvert.DeserializeObject<AppModels.SimpleWebConfig>(json, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            btndownloadsfadmlink.NavigateUrl = "~/Files/SFADM/Apps/SFADM_v" + conf.sfadmversion.versioncode + ".apk";
        }

        
        protected void btndownloadsfadm_Click(object sender, EventArgs e)
        {
            Response.ContentType = "Application/apk";
            Response.AppendHeader("Content-Disposition", "attachment; filename=SFADM_v" + conf.sfadmversion.versioncode + ".apk");
            Response.TransmitFile(Server.MapPath(@"~/Files/SFADM/Apps/SFADM_v" + conf.sfadmversion.versioncode + ".apk"));
            Response.End();
        }

        protected void btndownloadsfa_Click(object sender, EventArgs e)
        {
            Response.ContentType = "Application/apk";
            Response.AppendHeader("Content-Disposition", "attachment; filename=sfav1.4.9.210529_int.apk");
            Response.TransmitFile(Server.MapPath(@"~/Files/SFA/Apps/sfav1.4.9.210529_int.apk"));
            Response.End();
        }
    }
}