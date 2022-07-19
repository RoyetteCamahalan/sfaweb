using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views
{
    public partial class ErrorLogs : System.Web.UI.Page
    {
        Auth auth;
        EmployeeController employeeController;
        CycleController cycleController;
        CallController callController;
        ReportDashBoard reportDashBoard;

        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this, submodcode: AppModels.SubModules.userlogs);
            if (!auth.hasAuth())
                return;

            employeeController = new EmployeeController();
            cycleController = new CycleController();
            callController = new CallController();
            reportDashBoard = new ReportDashBoard();

            if (!Page.IsPostBack)
            {
                LoadCombo();
                if (Page.RouteData.Values.ContainsKey("employeeid"))
                {
                    long employeeid = Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["employeeid"].ToString()));
                    int selectedyear = Convert.ToInt32(Page.RouteData.Values["year"]);
                    int cyclenumber = Convert.ToInt32(Page.RouteData.Values["cyclenumber"]);
                    cmbpsr.SelectedValue = employeeid.ToString();
                    cmbyear.SelectedValue = selectedyear.ToString();
                    cmbyear_SelectedIndexChanged(cmbyear, null);
                    cmbcycle.SelectedValue = cyclenumber.ToString();
                    DisplayList();
                }
            }
            //ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnexportfile);
        }
        #region "Methods"
        public string getCallDetailsRoute(string id)
        {
            return GetRouteUrl(AppModels.Routes.calldetails, new { id = Auth.AppSecurity.URLEncrypt(id) });
        }
        public string getCallSyncRoute(string employeeid, string date)
        {
            return GetRouteUrl(AppModels.Routes.dailysyncdetails, new { employeeid = Auth.AppSecurity.URLEncrypt(employeeid), date = Auth.AppSecurity.URLEncrypt(date) });
        }
        private void LoadCombo()
        {
            LoadTree(4);

            cmbyear.DataSource = cycleController.GetCyclesets();
            cmbyear.DataTextField = "year";
            cmbyear.DataValueField = "year";
            cmbyear.DataBind();
            cmbyear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
            cmbyear_SelectedIndexChanged(cmbyear, null);
        }
        #endregion


        private void DisplayList()
        {
            long rbdmid = Convert.ToInt64(cmbrbdm.SelectedValue);
            long branch_id = Convert.ToInt64(cmbbranches.SelectedValue);
            int selectedyear = Convert.ToInt32(cmbyear.SelectedValue);
            int cyclenumber = Convert.ToInt32(cmbcycle.SelectedValue);
            long districtmanagerid = Convert.ToInt64(cmbpsr.SelectedValue);
            long employeeid = Convert.ToInt64(cmbpsr.SelectedValue);
            long warehouseid = 0;
            if (employeeid > 0)
            {
                employee emp = employeeController.GetEmployee(employeeid);
                warehouseid = emp.warehouseid ?? 0;
            }
            DataTable dt = reportDashBoard.GetResultReport(0, 42, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, true, false);

            tbl_main.Visible = true;
            tbl_main.DataSource = dt;
            tbl_main.DataBind();

            auth.SaveLog("User Logs", "Load User Logs");
        }

        private void LoadTree(int level)
        {
            List<GenericObject> lstdms = new List<GenericObject>();
            List<branch> lstbranches = new List<branch>();
            List<GenericObject> lstpsr = new List<GenericObject>();
            if (level == 4)
            {
                employee e = auth.currentuser.employee;
                List<GenericObject> lstrbdms = new List<GenericObject>();
                if (e.employeeid == AppModels.EmployeeTypes.gm || e.employeetypeid == AppModels.EmployeeTypes.smd || e.employeetypeid == AppModels.EmployeeTypes.nsm ||
                    e.employeetypeid == AppModels.EmployeeTypes.administrator)
                {
                    lstrbdms.Add(new GenericObject { id = 0, employeename = "All RBDMS" });
                    lstrbdms.AddRange(employeeController.GetRBDMs());

                    lstbranches.Add(new branch { branchid = 0, branchname = "All Branches" });
                    lstbranches.AddRange(employeeController.GetActiveBranches());

                    lstdms.Add(new GenericObject { id = 0, employeename = "All DMs" });
                    lstdms.AddRange(employeeController.GetDistrictManagers(0, 0));

                    lstpsr.Add(new GenericObject { id = 0, employeename = "All PSR/PTR" });
                    lstpsr.AddRange(employeeController.GetPSRPTR(0, 0, 0));
                }
                else if (e.employeetypeid == AppModels.EmployeeTypes.rbdm)
                {
                    lstrbdms.Add(new GenericObject { id = e.employeeid, employeename = e.firstname + " " + e.lastname });
                    cmbrbdm.Enabled = false;

                    lstbranches.AddRange(employeeController.GetRBDMBranches(e.employeeid));
                    if (lstbranches.Count == 1)
                    {
                        lstdms.AddRange(employeeController.GetDistrictManagers(lstbranches[0].branchid, e.employeeid));
                        cmbbranches.Enabled = false;
                    }
                    else
                    {
                        lstbranches = lstbranches.Prepend(new branch { branchid = 0, branchname = "All Branches" }).ToList();
                        lstdms.AddRange(employeeController.GetDistrictManagers(0, e.employeeid));
                    }
                    if (lstdms.Count == 1)
                    {
                        lstpsr.AddRange(employeeController.GetPSRPTR(e.branchid ?? 0, lstdms[0].id, e.employeeid));
                    }
                    else
                    {
                        lstdms = lstdms.Prepend(new GenericObject { id = 0, employeename = "All DMs" }).ToList();
                        lstpsr.AddRange(employeeController.GetPSRPTR(0, 0, e.employeeid));
                    }
                    if (lstpsr.Count() != 1)
                        lstpsr = lstpsr.Prepend(new GenericObject { id = 0, employeename = "All PSR/PTR" }).ToList();
                }
                else if (e.employeetypeid == AppModels.EmployeeTypes.bbdm)
                {
                    employee erdbdm = employeeController.GetEmployee((long)e.districtmanagerid);
                    if (erdbdm != null)
                        lstrbdms.Add(new GenericObject { id = erdbdm.employeeid, employeename = erdbdm.firstname + " " + erdbdm.lastname });

                    cmbrbdm.Enabled = false;

                    lstbranches.Add(e.branch);
                    cmbbranches.Enabled = false;

                    lstdms.Add(new GenericObject { id = e.employeeid, employeename = e.firstname + " " + e.lastname });
                    cmbbbdm.Enabled = false;

                    lstpsr.AddRange(employeeController.GetPSRPTR(e.branchid ?? 0, e.employeeid, e.districtmanagerid ?? 0));
                    if (lstpsr.Count() != 1)
                        lstpsr = lstpsr.Prepend(new GenericObject { id = 0, employeename = "All PSR/PTR" }).ToList();
                }
                else
                {
                    employee bbdm = employeeController.GetEmployee(e.districtmanagerid ?? 0);
                    employee rdbdm = employeeController.GetEmployee((long)bbdm.districtmanagerid);
                    lstrbdms.Add(new GenericObject { id = rdbdm.employeeid, employeename = rdbdm.firstname + " " + rdbdm.lastname });
                    cmbrbdm.Enabled = false;

                    lstbranches.Add(e.branch);
                    cmbbranches.Enabled = false;

                    if (bbdm != null)
                        lstdms.Add(new GenericObject { id = bbdm.employeeid, employeename = bbdm.firstname + " " + bbdm.lastname });
                    cmbbbdm.Enabled = false;

                    lstpsr.Add(new GenericObject { id = e.employeeid, employeename = e.firstname + " " + e.lastname });
                    cmbpsr.Enabled = false;

                }
                cmbrbdm.DataSource = lstrbdms;
                cmbrbdm.DataTextField = "employeename";
                cmbrbdm.DataValueField = "id";
                cmbrbdm.DataBind();

                cmbbranches.DataSource = lstbranches;
                cmbbranches.DataTextField = "branchname";
                cmbbranches.DataValueField = "branchid";
                cmbbranches.DataBind();

                cmbbbdm.DataSource = lstdms;
                cmbbbdm.DataSource = lstdms;
                cmbbbdm.DataTextField = "employeename";
                cmbbbdm.DataValueField = "id";
                cmbbbdm.DataBind();

                cmbpsr.DataSource = lstpsr;
                cmbpsr.DataTextField = "employeename";
                cmbpsr.DataValueField = "id";
                cmbpsr.DataBind();
            }
            long rbdmid = Convert.ToInt64(cmbrbdm.SelectedValue);
            if (level <= 1) //rbdmchanged
            {
                lstbranches.AddRange(employeeController.GetRBDMBranches(rbdmid));
                if (lstbranches.Count > 1)
                    lstbranches = lstbranches.Prepend(new branch { branchid = 0, branchname = "All Branches" }).ToList();

                cmbbranches.DataSource = lstbranches;
                cmbbranches.DataTextField = "branchname";
                cmbbranches.DataValueField = "branchid";
                cmbbranches.DataBind();
            }
            long branchid = Convert.ToInt64(cmbbranches.SelectedValue);
            if (level <= 2) //branchchanged
            {
                lstdms = new List<GenericObject>();
                lstdms.AddRange(employeeController.GetDistrictManagers(branchid, rbdmid));
                if (lstdms.Count > 1)
                    lstdms = lstdms.Prepend(new GenericObject() { id = 0, employeename = "All DMs" }).ToList();

                cmbbbdm.DataSource = lstdms;
                cmbbbdm.DataTextField = "employeename";
                cmbbbdm.DataValueField = "id";
                cmbbbdm.DataBind();
            }
            long bbdmid = Convert.ToInt64(cmbbbdm.SelectedValue);
            if (level <= 3) //bbdmchanged
            {
                lstpsr.AddRange(employeeController.GetPSRPTR(branchid, bbdmid, rbdmid));
                if (lstpsr.Count != 1)
                    lstpsr = lstpsr.Prepend(new GenericObject() { id = 0, employeename = "All PSR/PTR" }).ToList();

                cmbpsr.DataSource = lstpsr;
                cmbpsr.DataTextField = "employeename";
                cmbpsr.DataValueField = "id";
                cmbpsr.DataBind();

            }
            upanelreportmain.Update();
        }

        protected void cmbyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbyear.SelectedIndex >= 0)
            {
                int lastmonth = 12;
                if (Convert.ToInt32(cmbyear.SelectedValue) == Utility.getServerDate().Year)
                    lastmonth = Utility.getServerDate().Month;

                List<ListItem> lstcycle = new List<ListItem>();
                lstcycle.Add(new ListItem
                {
                    Text = "All",
                    Value = "0"
                });
                for (var i = 1; i <= lastmonth; i++)
                {
                    lstcycle.Add(new ListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    });
                }
                cmbcycle.DataSource = lstcycle;
                cmbcycle.DataBind();
                cmbcycle.Items.FindByValue(lastmonth.ToString()).Selected = true;
            }
            upanelreportmain.Update();
        }

        protected void cmbrbdm_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTree(1);
        }

        protected void cmbbranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTree(2);
        }

        protected void cmbbbdm_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTree(3);
        }

        protected void btnloadinit_Click(object sender, EventArgs e)
        {
            panelpreviewunavailable.Visible = false;
            tbl_main.Visible = false;
            upanelgrids.Update();
            PageController.fnShowLoader(this, "panelloader");
            PageController.fnFireEvent(this, PageController.EventType.click, ButtonLoad.ClientID, true);
        }



        protected void btnLoadData(object data, EventArgs e)
        {
            DisplayList();
            upanelgrids.Update();
            PageController.fnHideLoader(this, "panelloader");
        }

        //protected void btnexportinit_Click(object sender,EventArgs e)
        //{
        //    PageController.fnFireEvent(this, PageController.EventType.click, btnexportfile.ClientID, true);
        //}



    }
}