using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views
{
    public partial class Login : System.Web.UI.Page
    {
        Auth auth;
        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this, Auth.middleware.guest);
            if (!Page.IsPostBack)
            {
                if (auth.warehouseid > 0)
                {
                    Response.RedirectToRoute(AppModels.Routes.dashboard);
                }
            }
        }
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            if (txtusername.Text == "" || txtpassword.Text == "")
            {
                lblerror.Text = "*Username and Password Required!";
                lblerror.Visible = true;
                return;
            }
            lblerror.Visible = false;
            if (auth.authenticate(txtusername.Text, txtpassword.Text))
            {
                auth.SaveLog("User Login", "Successfully Logged In");
                Response.RedirectToRoute(AppModels.Routes.dashboard);
            }
            else
            {
                lblerror.Text = "*Invalid Username or Password!";
                lblerror.Visible = true;
            }
        }
    }
}