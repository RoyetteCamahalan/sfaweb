using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO
{
    public partial class SiteMaster : MasterPage
    {
        Auth auth;
        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this.Page, Auth.middleware.guest);
            if (!Page.IsPostBack)
            {
                ImplementPrivileges();
                LoadForApproval();
                whatsNew();
            }
            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(btnclosewhatsnew);
        }
        private void ImplementPrivileges()
        {
            if (auth.currentuser != null)
            {
                lbluser.Text = auth.currentuser.employee.firstname + " " + auth.currentuser.employee.lastname;
                usersubpriv usp = auth.GetSubUserpriv(AppModels.SubModules.dashboard);
                navitemdashboard.Visible = usp.canaccess ?? false;

                userpriv up = auth.GetUserpriv(AppModels.Modules.practivity);
                navitempractivity.Visible = up.canrequest ?? false;

                up = auth.GetUserpriv(AppModels.Modules.stop);
                navitemstop.Visible = up.canrequest ?? false;

                up = auth.GetUserpriv(AppModels.Modules.tup);
                navitemtup.Visible = up.canrequest ?? false;

                navheaderproposal.Visible = (navitempractivity.Visible || navitemstop.Visible || navitemtup.Visible);


                up = auth.GetUserpriv(AppModels.Modules.expensereport);
                navitemtexpensereport.Visible = up.canrequest ?? false;

                up = auth.GetUserpriv(AppModels.Modules.leaverequest);
                navitemloa.Visible = up.canrequest ?? false;

                up = auth.GetUserpriv(AppModels.Modules.salaryloan);
                navitemsalaryloan.Visible = up.canrequest ?? false;

                up = auth.GetUserpriv(AppModels.Modules.vehiclerepair);
                navitemvehiclerepair.Visible = up.canrequest ?? false;

                navheaderrequest.Visible = (navitemtexpensereport.Visible || navitemloa.Visible || navitemvehiclerepair.Visible);


                up = auth.GetUserpriv(AppModels.Modules.employees);
                navitememployees.Visible = (up.canadd ?? false) || (up.canedit ?? false);

                up = auth.GetUserpriv(AppModels.Modules.employeetypes);
                navitememployeetypes.Visible = (up.canadd ?? false) || (up.canedit ?? false);

                up = auth.GetUserpriv(AppModels.Modules.users);
                navitemusers.Visible = (up.canadd ?? false) || (up.canedit ?? false) || (auth.currentuser.isappsysadmin ?? false);

                up = auth.GetUserpriv(AppModels.Modules.warehouses);
                navitemwarehouses.Visible = (up.canadd ?? false) || (up.canedit ?? false);

                navheaderwarehousemaintenance.Visible = (navitememployees.Visible || navitememployeetypes.Visible || navitemusers.Visible || navitemwarehouses.Visible);


                up = auth.GetUserpriv(AppModels.Modules.products);
                navitemproducts.Visible = (up.canadd ?? false) || (up.canedit ?? false);

                up = auth.GetUserpriv(AppModels.Modules.prodcuctcategories);
                navitemproductcategories.Visible = (up.canadd ?? false) || (up.canedit ?? false);

                navheaderitems.Visible = (navitemproducts.Visible || navitemproductcategories.Visible);

                up = auth.GetUserpriv(AppModels.Modules.mdlist);
                navitemmdlist.Visible = (up.canadd ?? false) || (up.canedit ?? false);

                up = auth.GetUserpriv(AppModels.Modules.companyvehicles);
                navitemvehicles.Visible = (up.canadd ?? false) || (up.canedit ?? false);

                up = auth.GetUserpriv(AppModels.Modules.repairshops);
                navitemrepairshops.Visible = (up.canadd ?? false) || (up.canedit ?? false);

                usp = auth.GetSubUserpriv(AppModels.SubModules.branchapproval);
                navitembranchapproval.Visible = usp.canaccess ?? false;

                navheadermasterfiles.Visible = (navheaderwarehousemaintenance.Visible || navheaderitems.Visible || navitemvehicles.Visible || navitemrepairshops.Visible || navitembranchapproval.Visible);

                usp = auth.GetSubUserpriv(AppModels.SubModules.sfareports);
                navitemsfareports.Visible = usp.canaccess ?? false;

                usp = auth.GetSubUserpriv(AppModels.SubModules.userlogs);
                navitemusertrails.Visible = usp.canaccess ?? false;

                usp = auth.GetSubUserpriv(AppModels.SubModules.errorlogs);
                navitemerrorlogs.Visible = usp.canaccess ?? false;

                paneluserprofile.Visible = true;
                navheaderlogout.Visible = true;
            }
            else
            {
                navitemdashboard.Visible = false;
                navitempractivity.Visible = false;
                navitemstop.Visible = false;
                navitemtup.Visible = false;
                navheaderproposal.Visible = false;
                navitemtexpensereport.Visible = false;
                navitemloa.Visible = false;
                navitemvehiclerepair.Visible = false;
                navheaderrequest.Visible = false;
                navitememployees.Visible = false;
                navitememployeetypes.Visible = false;
                navitemusers.Visible = false;
                navitemwarehouses.Visible = false;
                navheaderwarehousemaintenance.Visible = false;
                navitemproducts.Visible = false;
                navitemproductcategories.Visible = false;
                navheaderitems.Visible = false;
                navitemvehicles.Visible = false;
                navitemrepairshops.Visible = false;
                navitembranchapproval.Visible = false;
                navheadermasterfiles.Visible = false;
                navitemsfareports.Visible = false;
                paneluserprofile.Visible = false;
                navheaderlogout.Visible = false;
                navitemerrorlogs.Visible = false;
            }
        }


        private void LoadForApproval()
        {
            if (auth.currentuser == null)
                return;

            bool ispsr = auth.currentuser.employee.employeetypeid == AppModels.EmployeeTypes.psr || auth.currentuser.employee.employeetypeid == AppModels.EmployeeTypes.ptr;
            usersubpriv usp = auth.GetSubUserpriv(AppModels.SubModules.mdapproval);
            if (ispsr || !(usp.canaccess ?? false))
                return;
            ReportDashBoard reportDashBoard = new ReportDashBoard();
            DataTable dt = reportDashBoard.GetResultReport(0, 38, 0, 0, 0, auth.currentuser.employeeid ?? 0, 1990, 0, isactive: 2, mdtype: 0);

            int totalcount = 0;
            foreach (DataRow row in dt.Rows)
            {
                totalcount += Convert.ToInt32(row["totalcount"]);
            }
            if (totalcount > 0)
            {
                lblforapprovalcount.Text = totalcount.ToString();
            }
            else
            {
                lblforapprovalcount.Text = "";
            }
        }

        private void whatsNew()
        {
            if (auth.currentuser == null || Convert.ToInt32(SessionController.getSession(SessionController.SesssionKeys.showupdates) ?? 0) == 0)
                return;

            MainController mainController = new MainController();
            this.lstNewUpdates.DataSource = mainController.GetAppversions();
            this.lstNewUpdates.DataBind();
            PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnmodal-whatsnew", true);
        }
        protected void btnsignout_Click(object sender, EventArgs e)
        {
            auth.Logout();
            Response.RedirectToRoute(AppModels.Routes.login);
        }

        protected void btnclosewhatsnew_Click(object sender, EventArgs e)
        {
            SessionController.setSession(SessionController.SesssionKeys.showupdates, 0);
            PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnclosewhatsnew_", true);
        }
    }
}