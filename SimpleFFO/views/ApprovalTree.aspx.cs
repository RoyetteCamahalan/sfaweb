using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace SimpleFFO.views
{
    public partial class ApprovalTree : System.Web.UI.Page
    {
        Auth auth;
        ApprovalController approvalController;
        EmployeeController employeeController;

        #region "Vars"
        private List<approvaltree> lsttemptree
        {
            get
            {
                if (ViewState["lsttemptree"] == null)
                    return new List<approvaltree>();
                return (List<approvaltree>)ViewState["lsttemptree"];
            }
            set { ViewState["lsttemptree"] = value; }
        }
        private int currentModule
        {
            get { return (int)(ViewState["currentModule"] ?? 0); }
            set { ViewState["currentModule"] = value;  }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this,submodcode: AppModels.SubModules.branchapproval);
            if (!auth.hasAuth())
                return;

            approvalController = new ApprovalController();
            employeeController = new EmployeeController();
            if (!Page.IsPostBack)
            {
                Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());
                LoadCombo();
                DisplayList();
            }
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ViewState["update"] = Session["update"];
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            RegisteAsync();
        }
        private void LoadCombo()
        {
            this.cmbdistrictmanager.DataSource = employeeController.GetDistrictManagers(0,0);
            this.cmbdistrictmanager.DataTextField = "employeename";
            this.cmbdistrictmanager.DataValueField = "id";
            this.cmbdistrictmanager.DataBind();

            this.cmbEmployees.DataSource = employeeController.GetEmployees(true);
            this.cmbEmployees.DataTextField = "fullname";
            this.cmbEmployees.DataValueField = "employeeid";
            this.cmbEmployees.DataBind();
        }
        private void DisplayList()
        {

            lstmodules.DataSource = approvalController.GetApprovalModules();
            lstmodules.DataKeyNames = new string[] { "module_id" };
            lstmodules.DataBind();
        }
        private void RegisteAsync()
        {

            foreach (ListViewDataItem item in lstmodules.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)item.FindControl("btnedit"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)item.FindControl("btnaddemployee"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)item.FindControl("btnaddemployee"));

                    ListView lstdetail = (ListView)item.FindControl("lstdetail");
                    foreach (ListViewDataItem emp in lstdetail.Items)
                    {
                        if (emp.ItemType == ListViewItemType.DataItem)
                        {
                            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)emp.FindControl("btnup"));
                            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)emp.FindControl("btndown"));
                            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)emp.FindControl("btnremove"));
                        }
                    }
                }
            }
        }

        protected void btnaddemployee_Click(object sender, EventArgs e)
        {
            this.currentModule = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            panelmodal.Update();
            PageController.fnFireEvent(this, PageController.EventType.click, "btnShowModal",true);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Validator.DecOneAbove(cmbEmployees))
            {
                managetree(1, this.currentModule);
            }
            panelmodal.Update();
            PageController.fnFireEvent(this, PageController.EventType.click, "btnShowModal", true);
        }
        private void managetree(int action, int moduleid, int idx=0)
        {
            foreach (ListViewDataItem item in lstmodules.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem && Convert.ToInt32(lstmodules.DataKeys[item.DataItemIndex]["module_id"]) == moduleid)
                {
                    ListView lstdetail = (ListView)item.FindControl("lstdetail");
                    List<approvaltree> temp = new List<approvaltree>();
                    UpdatePanel panellstdetailcontainer = (UpdatePanel)item.FindControl("panellstdetailcontainer");
                    LinkButton btnaddemployee = (LinkButton)item.FindControl("btnaddemployee");
                    HtmlControl btnediticon = (HtmlControl)item.FindControl("btnediticon");
                    bool haserror = false;
                    foreach (ListViewDataItem emp in lstdetail.Items)
                    {
                        if (emp.ItemType == ListViewItemType.DataItem)
                        {
                            DropDownList cmbstatus = (DropDownList)emp.FindControl("cmbstatus"); 
                            temp.Add(new approvaltree
                            {
                                approvaltreeid = Convert.ToInt64(lstdetail.DataKeys[emp.DataItemIndex]["approvaltreeid"]),
                                employeeid = Convert.ToInt64(lstdetail.DataKeys[emp.DataItemIndex]["employeeid"]),
                                treelevel = emp.DataItemIndex + 1,
                                districtmanagerid = Convert.ToInt64(cmbdistrictmanager.SelectedValue),
                                moduleid = Convert.ToInt32(moduleid),
                                status = Convert.ToInt32(cmbstatus.SelectedValue)
                            });
                            if (action == 2)
                            {
                                if (lstdetail.Attributes["class"].Contains("onedit"))
                                {
                                    if (!Validator.DecZeroAbove(cmbstatus))
                                    {
                                        haserror = true;
                                    }
                                }
                            }
                            if((action == 0 && lstdetail.Attributes["class"].Contains("onedit")) || (action==2 && !lstdetail.Attributes["class"].Contains("onedit")))
                            {
                                    if (emp.DataItemIndex != 0)
                                    {
                                        LinkButton btnup = (LinkButton)emp.FindControl("btnup");
                                        btnup.Visible = true;
                                    }
                                    if (emp.DataItemIndex != lstdetail.Items.Count - 1)
                                    {
                                        LinkButton btndown = (LinkButton)emp.FindControl("btndown");
                                        btndown.Visible = true;
                                    }
                                    cmbstatus.Enabled = true;
                                LinkButton btnremove = (LinkButton)emp.FindControl("btnremove");
                                btnremove.Visible = true;
                            }
                        }
                    }
                    if (action == 2 && !haserror)
                    {
                        if (lstdetail.Attributes["class"].Contains("onedit"))
                        {
                            approvalController.SaveTree(Convert.ToInt32(moduleid), Convert.ToInt64(cmbdistrictmanager.SelectedValue), temp);
                            this.lsttemptree = temp;
                            BindDetails(lstdetail);

                            lstdetail.Attributes["class"] = lstdetail.Attributes["class"].Replace(" oneditmode", "");
                            btnediticon.Attributes["class"] = "fas fa-pen";
                            btnaddemployee.Visible = false;


                            foreach (ListViewDataItem emp in lstdetail.Items)
                            {
                                if (emp.ItemType == ListViewItemType.DataItem)
                                {
                                    DropDownList cmbstatus = (DropDownList)emp.FindControl("cmbstatus");
                                    cmbstatus.Enabled = false;
                                    LinkButton btnup = (LinkButton)emp.FindControl("btnup");
                                    btnup.Visible = false;
                                    LinkButton btndown = (LinkButton)emp.FindControl("btndown");
                                    btndown.Visible = false;
                                    LinkButton btnremove = (LinkButton)emp.FindControl("btnremove");
                                    btnremove.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            lstdetail.Attributes["class"] += " oneditmode";
                            btnediticon.Attributes["class"] = "fas fa-check";
                            btnaddemployee.Visible = true;
                        }
                    }
                    else if (action == 1)
                    {
                        temp.Add(new approvaltree
                        {
                            approvaltreeid = 0,
                            employeeid = Convert.ToInt64(cmbEmployees.SelectedValue),
                            treelevel = temp.Count(),
                            districtmanagerid = Convert.ToInt64(cmbdistrictmanager.SelectedValue),
                            moduleid = Convert.ToInt32(moduleid),
                        });
                        this.lsttemptree = temp;
                        BindDetails(lstdetail);
                        managetree(0, moduleid);
                    }else if (action == 3)
                    {
                        var t = temp[idx];
                        temp.RemoveAt(idx);
                        temp.Insert(idx-1,t);
                        this.lsttemptree = temp;
                        BindDetails(lstdetail);
                        managetree(0, moduleid);
                    }
                    else if (action == 4)
                    {
                        var t = temp[idx];
                        temp.RemoveAt(idx);
                        temp.Insert(idx+1, t);
                        this.lsttemptree = temp;
                        BindDetails(lstdetail);
                        managetree(0, moduleid);
                    }
                    else if (action == 5)
                    {
                        temp.RemoveAt(idx);
                        this.lsttemptree = temp;
                        BindDetails(lstdetail);
                        managetree(0, moduleid);
                    }
                    panellstdetailcontainer.Update();
                }
            }
            panelmodal.Update();
        }

        protected void lstmodules_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView lstdetail = (ListView)e.Item.FindControl("lstdetail");
                this.lsttemptree = approvalController.GetApprovaltree(Convert.ToInt64(cmbdistrictmanager.SelectedValue), Convert.ToInt32(lstmodules.DataKeys[e.Item.DataItemIndex]["module_id"]));
                BindDetails(lstdetail);
            }
        }
        private void BindDetails(ListView lstdetail)
        {
            lstdetail.DataSource = this.lsttemptree;
            lstdetail.DataKeyNames = new string[] { "approvaltreeid", "employeeid" };
            lstdetail.DataBind();
        }

        protected void lstdetails_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView lstdetail = (ListView)sender;
                Literal lblemployeename = (Literal)e.Item.FindControl("lblemployeename");
                DropDownList cmbstatus = (DropDownList)e.Item.FindControl("cmbstatus");
                cmbstatus.DataSource = AppModels.Status.getListAction();
                cmbstatus.DataTextField = "value";
                cmbstatus.DataValueField = "key";
                cmbstatus.DataBind();
                cmbstatus.SelectedValue = (DataBinder.Eval(e.Item.DataItem, "status") ?? 0).ToString();

                employee employee = employeeController.GetEmployee(Convert.ToInt64(lstdetail.DataKeys[e.Item.DataItemIndex]["employeeid"]));
                lblemployeename.Text = employee.lastname + ", " + employee.firstname;
            }
        }

        protected void cmbBranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayList();
        }

        protected void btnedit_Click(object sender, EventArgs e)
        {
            LinkButton btnedit = (LinkButton)sender;
            managetree(2, Convert.ToInt32(btnedit.CommandArgument));
        }
        protected void btnup_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var args = btn.CommandArgument.Split('_');
            managetree(3,Convert.ToInt32(args[0]),Convert.ToInt32(args[1]));
        }

        protected void btndown_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var args = btn.CommandArgument.Split('_');
            managetree(4, Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
        }

        protected void btnremove_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            var args = btn.CommandArgument.Split('_');
            managetree(5, Convert.ToInt32(args[0]), Convert.ToInt32(args[1]));
        }
    }
}