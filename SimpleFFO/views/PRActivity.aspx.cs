using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO
{
    public partial class PRActivity : System.Web.UI.Page
    {
        InstitutionController institutionController;
        DoctorController doctorController;
        PRActivityController prActivityController;
        ProductController productController;
        Auth auth;

        #region "Vars"
        private int moduleid { get => AppModels.Modules.practivity; }
        public bool isPageView
        {
            get { return Convert.ToBoolean(ViewState["ispageview"] ?? false); }
            set { ViewState["ispageview"] = value; }
        }
        public long practivityid
        {
            get { return Convert.ToInt64(ViewState["practivityid"]); }
            set { ViewState["practivityid"] = value; }
        }
        public long warehouseid
        {
            get { return Convert.ToInt64(ViewState["warehouseid"]); }
            set { ViewState["warehouseid"] = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this, modcode: this.moduleid);
            if (!auth.hasAuth())
                return;

            institutionController = new InstitutionController();
            doctorController = new DoctorController();
            prActivityController = new PRActivityController();
            productController = new ProductController();
            if (!Page.IsPostBack)
            {
                LoadtoCombo();
                if (Page.RouteData.Values.ContainsKey("id"))
                {
                    this.practivityid = Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["id"].ToString()));
                    LoadRecord();
                }
                else
                {
                    this.practivityid = 0;
                    this.warehouseid = auth.warehouseid;
                    this.txtdatefiled.Text = Utility.getServerDate().ToString(AppModels.dateformat);
                    initGrid();
                    DisplayList();
                }
            }
            else
            {
                tblListOFAtendees.HeaderRow.TableSection = TableRowSection.TableHeader;

            }
            RegisterAsyncControls();
        }
        #region "Page Methods"
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!Page.IsPostBack)
            {
                if (this.practivityid > 0 && Page.RouteData.Values.ContainsKey("action"))
                {
                    ctlFunding.Bind(this.moduleid, this.practivityid, this.warehouseid, Convert.ToDecimal(txtTotalBudget.Text));
                }
            }
        }
        #endregion

        #region "Methods"
        public string getRequestStatus(int status)
        {
            return AppModels.Status.getStatus(status);
        }
        public string getRequestStatusBadge(int status)
        {
            return AppModels.Status.getBadge(status);
        }
        private void LoadRecord()
        {
            practivity practivity = prActivityController.GetPRActivity(this.practivityid);
            cmbActivityType.SelectedValue = (practivity.activityid ?? 0).ToString();
            this.warehouseid = practivity.warehouseid ?? 0;
            if (practivity.inst_id > 0)
            {
                rdInstitutions.Checked = true;
                DisplayList();
                drplstChoice.SelectedValue = (practivity.inst_id ?? 0).ToString();
            }
            else
            {
                rdDoctors.Checked = true;
                DisplayList();
                drplstChoice.SelectedValue = (practivity.doc_id ?? 0).ToString();
            }
            txtdatefiled.Text = (practivity.datecreated ?? DateTime.Now).ToString(AppModels.datetimeformat);
            txtLocation.Text = practivity.location;
            dtpDate.Text = (practivity.activitydate ?? DateTime.Now).ToString(AppModels.dateformat);

            if ((practivity.status ?? 0) >= AppModels.Status.submitted)
            {
                isPageView = true;
                btnSaveDraft.Visible = false;
                btnCancel.Visible = false;
                btnSubmit.Visible = false;
                btnAddAttendee.Visible = false;
                btnAddOutcome.Visible = false;
                btnAddBudget.Visible = false;

                cmbActivityType.Enabled = false;
                rdInstitutions.Enabled = false;
                rdDoctors.Enabled = false;
                drplstChoice.Enabled = false;
                txtClassification.ReadOnly = true;
                txtLocation.ReadOnly = true;
                dtpDate.ReadOnly = true;
                dtpDate.CssClass = dtpDate.CssClass.Replace(" dtpDate", "");

                tblListOFAtendees.Columns[4].Visible = false;
                tblExpectedOutput.Columns[2].Visible = false;
                tblProposedBudget.Columns[2].Visible = false;

                ctlStatusTrail.Bind(this.moduleid, this.practivityid, practivity.status ?? 0);

            }
            else if (practivity.status == AppModels.Status.rejected)
            {
                btnSaveDraft.Visible = false;
                btnSubmit.Text = "Re-Submit";
                ctlStatusTrail.Bind(this.moduleid, this.practivityid, practivity.status ?? 0);
            }

            ViewState["Attendees"] = practivity.attendees.ToList();
            this.BindGrid(0);

            ViewState["ExpectedOutcome"] = practivity.practivityoutcomes.ToList();
            this.BindGrid(1);

            ViewState["Budget"] = practivity.practivitybudgets.ToList();
            this.BindGrid(2);

            txtTotalBudget.Text = (practivity.totalbudget ?? 0).ToString(AppModels.moneyformat);
        }
        private void initGrid()
        {
            List<attendee> attendees = new List<attendee> { new attendee() };
            ViewState["Attendees"] = attendees;
            this.BindGrid(0);

            List<practivityoutcome> practivityoutcomes = new List<practivityoutcome> { new practivityoutcome() };
            ViewState["ExpectedOutcome"] = practivityoutcomes;
            this.BindGrid(1);

            List<practivitybudget> practivitybudgets = new List<practivitybudget> { new practivitybudget() };
            ViewState["Budget"] = practivitybudgets;
            this.BindGrid(2);
        }
        private void BindGrid(int t)
        {
            if (t == 0)
            {
                tblListOFAtendees.DataSource = (List<attendee>)ViewState["Attendees"];
                tblListOFAtendees.DataKeyNames = new string[] { "attendeeid" };
                tblListOFAtendees.DataBind();
            }
            else if (t == 1)
            {
                tblExpectedOutput.DataSource = (List<practivityoutcome>)ViewState["ExpectedOutcome"];
                tblExpectedOutput.DataKeyNames = new string[] { "practivityoutcomeid" };
                tblExpectedOutput.DataBind();
            }
            else if (t == 2)
            {
                List<practivitybudget> lst = (List<practivitybudget>)ViewState["Budget"];
                tblProposedBudget.DataSource = lst;
                tblProposedBudget.DataKeyNames = new string[] { "practivitybudgetid" };
                tblProposedBudget.DataBind();
                decimal total = 0;
                foreach (practivitybudget budget in lst)
                {
                    total = (budget.amount ?? 0) + total;
                }
                txtTotalBudget.Text = total.ToString(AppModels.moneyformat);
            }
            RegisterAsyncControls();
        }
        public void LoadtoCombo()
        {
            cmbActivityType.DataSource = prActivityController.GetActivities();
            cmbActivityType.DataTextField = "activityname";
            cmbActivityType.DataValueField = "activitiyid";
            cmbActivityType.DataBind();
        }

        public void DisplayList()
        {
            if (this.rdInstitutions.Checked)
            {
                drplstChoice.DataSource = institutionController.GetAll(auth.warehouseid);
                drplstChoice.DataTextField = "inst_name";
                drplstChoice.DataValueField = "inst_id";
                drplstChoice.DataBind();

                lbldropdown.Text = "Institution ";
            }
            else
            {
                drplstChoice.DataSource = doctorController.GetDoctorsDroplst(this.warehouseid);
                drplstChoice.DataTextField = "fullname";
                drplstChoice.DataValueField = "doc_id";
                drplstChoice.DataBind();

                lbldropdown.Text = "Customer ";

            }
            if (drplstChoice.Items.Count > 0)
                ViewClassification();
        }
        private void ViewClassification()
        {
            if (drplstChoice.SelectedValue == null)
                return;
            if (this.rdInstitutions.Checked)
            {
                institution institution = institutionController.getInstitution(Convert.ToInt32(drplstChoice.SelectedValue));
                txtClassification.Text = institution.institutiontype == null ? "UnSpecified" : institution.institutiontype.institutiontypename;
            }
            else
            {
                doctor doctor = doctorController.GetDoctor(Convert.ToInt32(drplstChoice.SelectedValue));
                txtClassification.Text = doctor.specialization==null ? "UnSpecified" : doctor.specialization.name;
            }
        }
        private List<object> GetGridData(int t)
        {
            if (t == 0)
            {
                List<attendee> lst = (List<attendee>)ViewState["Attendees"];
                for (int i = 0; i < this.tblListOFAtendees.Rows.Count; i++)
                {
                    lst[i].doc_name = ((TextBox)tblListOFAtendees.Rows[i].Cells[0].FindControl("txtDocName")).Text;
                    lst[i].specialization = ((TextBox)tblListOFAtendees.Rows[i].Cells[1].FindControl("txtSpecialty")).Text;
                    lst[i].remarks = ((TextBox)tblListOFAtendees.Rows[i].Cells[3].FindControl("txtNotes")).Text;
                    lst[i].doc_id = Convert.ToInt64(((HiddenField)tblListOFAtendees.Rows[i].Cells[0].FindControl("hfdoc_id")).Value);
                    lst[i].attendeeid = Convert.ToInt64(tblListOFAtendees.DataKeys[i].Value);
                }

                return lst.Cast<object>().ToList();
            }
            else if (t == 1)
            {
                List<practivityoutcome> lst = (List<practivityoutcome>)ViewState["ExpectedOutcome"];
                for (int i = 0; i < this.tblExpectedOutput.Rows.Count; i++)
                {
                    string str = ((TextBox)tblExpectedOutput.Rows[i].Cells[1].FindControl("txtOutcome")).Text;
                    if (!decimal.TryParse(str, out _))
                    {
                        str = "0";
                    }
                    lst[i].monthlysales = decimal.Parse(str);
                    lst[i].product_id = Convert.ToInt32(((DropDownList)tblExpectedOutput.Rows[i].Cells[0].FindControl("cmbProducts")).SelectedValue);
                    lst[i].practivityoutcomeid = Convert.ToInt64(tblExpectedOutput.DataKeys[i].Value);
                }
                return lst.Cast<object>().ToList();
            }
            else
            {
                List<practivitybudget> lst = (List<practivitybudget>)ViewState["Budget"];
                for (int i = 0; i < this.tblProposedBudget.Rows.Count; i++)
                {
                    lst[i].budgettypeid = Convert.ToInt32(((DropDownList)tblProposedBudget.Rows[i].Cells[0].FindControl("cmbBudgetTypes")).SelectedValue);
                    string str = ((TextBox)tblProposedBudget.Rows[i].Cells[1].FindControl("txtAmount")).Text;
                    if (str == "0" || !decimal.TryParse(str, out _))
                    {
                        str = "0";
                    }
                    lst[i].amount = decimal.Parse(str);
                    lst[i].practivitybudgetid = Convert.ToInt64(tblProposedBudget.DataKeys[i].Value);
                }
                return lst.Cast<object>().ToList();
            }
        }
        private bool PageValidate()
        {
            bool isvalid;
            isvalid = Validator.DecOneAbove(cmbActivityType);
            isvalid = Validator.DecOneAbove(drplstChoice) && isvalid;
            isvalid = Validator.RequiredField(txtLocation) && isvalid;
            Validator.SetError(dtpDate);
            if (!DateTime.TryParse(dtpDate.Text, out _))
            {
                isvalid = false;
                Validator.SetError(dtpDate, true);
            }
            for (int i = 0; i < this.tblListOFAtendees.Rows.Count; i++)
            {
                TextBox txtDocName = ((TextBox)tblListOFAtendees.Rows[i].Cells[0].FindControl("txtDocName"));
                isvalid = Validator.RequiredField(txtDocName) && isvalid;
            }
            List<int> lstproductids = new List<int>();
            for (int i = 0; i < this.tblExpectedOutput.Rows.Count; i++)
            {
                DropDownList cmbProducts = ((DropDownList)tblExpectedOutput.Rows[i].Cells[0].FindControl("cmbProducts"));
                if (Validator.DecOneAbove(cmbProducts))
                {
                    if (lstproductids.Contains(Convert.ToInt32(cmbProducts.SelectedValue)))
                    {
                        isvalid = false;
                        Validator.SetError(cmbProducts, true);
                    }
                    else
                    {
                        Validator.SetError(cmbProducts);
                        lstproductids.Add(Convert.ToInt32(cmbProducts.SelectedValue));
                    }
                }
                else
                    isvalid = false;
                TextBox txtOutcome = ((TextBox)tblExpectedOutput.Rows[i].Cells[0].FindControl("txtOutcome"));
                isvalid = Validator.DecOneAbove(txtOutcome) && isvalid;
            }
            List<int> lstbudgetids = new List<int>();
            for (int i = 0; i < this.tblProposedBudget.Rows.Count; i++)
            {
                DropDownList cmbBudgetTypes = ((DropDownList)tblProposedBudget.Rows[i].Cells[0].FindControl("cmbBudgetTypes"));
                if (Validator.DecOneAbove(cmbBudgetTypes))
                {
                    if (lstbudgetids.Contains(Convert.ToInt32(cmbBudgetTypes.SelectedValue)))
                    {
                        isvalid = false;
                        Validator.SetError(cmbBudgetTypes, true);
                    }
                    else
                    {
                        Validator.SetError(cmbBudgetTypes);
                        lstbudgetids.Add(Convert.ToInt32(cmbBudgetTypes.SelectedValue));
                    }
                }
                else
                    isvalid = false;
                TextBox txtAmount = ((TextBox)tblProposedBudget.Rows[i].Cells[0].FindControl("txtAmount"));
                isvalid = Validator.DecOneAbove(txtAmount) && isvalid;
            }
            return isvalid;
        }

        private void Save(int status)
        {
            if (PageValidate())
            {
                long practivityid = Convert.ToInt64(ViewState["practivityid"]);
                practivity practivity = prActivityController.GetPRActivity(practivityid);
                practivity.activityid = Convert.ToInt32(cmbActivityType.SelectedValue);
                practivity.warehouseid = this.warehouseid;
                if (rdInstitutions.Checked)
                    practivity.inst_id = Convert.ToInt32(drplstChoice.SelectedValue);
                else
                    practivity.doc_id = Convert.ToInt32(drplstChoice.SelectedValue);
                practivity.location = txtLocation.Text;
                practivity.activitydate = Convert.ToDateTime(dtpDate.Text);

                List<attendee> attendees = GetGridData(0).Cast<attendee>().Select(a => { a.practivityid = practivity.practivityid; return a; }).ToList();
                List<practivityoutcome> practivityoutcomes = GetGridData(1).Cast<practivityoutcome>().Select(a => { a.practivityid = practivity.practivityid; return a; }).ToList();
                List<practivitybudget> practivitybudgets = GetGridData(2).Cast<practivitybudget>().Select(a => { a.practivityid = practivity.practivityid; return a; }).ToList();
                practivity.totalbudget = Convert.ToDecimal(txtTotalBudget.Text);
                practivity.status = status;
                prActivityController.SavePRActivity(practivityid == 0, practivity, attendees, practivityoutcomes, practivitybudgets);
                this.Response.RedirectToRoute("dashboard");
            }
            else
            {
                PageController.fnScroll(this, "is-invalid", true,true);
            }
            upanelprdetails.Update();
            upanelattendee.Update();
            upaneloutcome.Update();
            upanelbudget.Update();
        }

        #endregion

        protected void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            DisplayList();
            upanelprdetails.Update();
        }

        protected void drplstChoice_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewClassification();
            upanelprdetails.Update();
        }

        protected void btnAddAttendee_Click(object sender, EventArgs e)
        {
            if (btnAddAttendee.ID == ((Control)sender).ID)
            {
                List<attendee> lst = GetGridData(0).Cast<attendee>().ToList();
                lst.Add(new attendee());
                ViewState["Attendees"] = lst;
                this.BindGrid(0);
            }
            else if (btnAddOutcome.ID == ((Control)sender).ID)
            {
                List<practivityoutcome> lst = GetGridData(1).Cast<practivityoutcome>().ToList();
                lst.Add(new practivityoutcome());
                ViewState["ExpectedOutcome"] = lst;
                this.BindGrid(1);
                upaneloutcome.Update();
            }
            else
            {
                List<practivitybudget> lst = GetGridData(2).Cast<practivitybudget>().ToList();
                lst.Add(new practivitybudget());
                ViewState["Budget"] = lst;
                this.BindGrid(2);
                upanelbudget.Update();
            }
        }

        protected void tblListOFAtendees_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((Control)sender).ID == this.tblListOFAtendees.ID && e.CommandName == "removeitem")
            {
                List<attendee> lst = (List<attendee>)ViewState["Attendees"];
                lst.RemoveAt(Convert.ToInt32(e.CommandArgument));
                if (lst.Count == 0)
                {
                    lst.Add(new attendee());
                }
                ViewState["Attendees"] = lst;
                this.BindGrid(0);
                upanelattendee.Update();
            }
            else if (((Control)sender).ID == this.tblExpectedOutput.ID && e.CommandName == "removeitem")
            {
                List<practivityoutcome> lst = (List<practivityoutcome>)ViewState["ExpectedOutcome"];
                lst.RemoveAt(Convert.ToInt32(e.CommandArgument));
                if (lst.Count == 0)
                {
                    lst.Add(new practivityoutcome());
                }
                ViewState["ExpectedOutcome"] = lst;
                this.BindGrid(1);
                upaneloutcome.Update();
            }
            else if (((Control)sender).ID == this.tblProposedBudget.ID && e.CommandName == "removeitem")
            {
                List<practivitybudget> lst = (List<practivitybudget>)ViewState["Budget"];
                lst.RemoveAt(Convert.ToInt32(e.CommandArgument));
                if (lst.Count == 0)
                {
                    lst.Add(new practivitybudget());
                }
                ViewState["Budget"] = lst;
                this.BindGrid(2);
                upanelbudget.Update();
            }
        }

        [System.Web.Services.WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<object> GetCompletionList(long warehouseid, string prefix)
        {
            DoctorController doctorController = new DoctorController();
            return doctorController.GetDoctorsDroplst(warehouseid, prefix).Take(10).ToList();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Save(AppModels.Status.submitted);
        }

        protected void tblExpectedOutput_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && ((Control)sender).ID == tblExpectedOutput.ID)
            {
                DropDownList dropDownLst = (DropDownList)e.Row.FindControl("cmbProducts");
                List<product> products = productController.GetProductList();
                dropDownLst.DataSource = products;
                dropDownLst.DataTextField = "name";
                dropDownLst.DataValueField = "product_id";
                dropDownLst.DataBind();
                dropDownLst.SelectedValue = (DataBinder.Eval(e.Row.DataItem, "product_id") ?? 0).ToString();
                dropDownLst.Enabled = !this.isPageView;

                TextBox txtOutcome = (TextBox)e.Row.FindControl("txtOutcome");
                txtOutcome.ReadOnly = this.isPageView;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && ((Control)sender).ID == tblProposedBudget.ID)
            {
                DropDownList dropDownLst = (DropDownList)e.Row.FindControl("cmbBudgetTypes");
                dropDownLst.DataSource = prActivityController.GetBudgettypes();
                dropDownLst.DataTextField = "budgettypename";
                dropDownLst.DataValueField = "budgettypeid";
                dropDownLst.DataBind();
                dropDownLst.SelectedValue = (DataBinder.Eval(e.Row.DataItem, "budgettypeid") ?? 0).ToString();
                dropDownLst.Enabled = !this.isPageView;

                TextBox txtAmount = (TextBox)e.Row.FindControl("txtAmount");
                txtAmount.ReadOnly = this.isPageView;
            }
            else if (e.Row.RowType == DataControlRowType.DataRow && ((Control)sender).ID == tblListOFAtendees.ID)
            {
                TextBox txtDocName = (TextBox)e.Row.FindControl("txtDocName");
                txtDocName.ReadOnly = this.isPageView;

                TextBox txtSpecialty = (TextBox)e.Row.FindControl("txtSpecialty");
                txtSpecialty.ReadOnly = this.isPageView;

                TextBox txtNotes = (TextBox)e.Row.FindControl("txtNotes");
                txtNotes.ReadOnly = this.isPageView;
            }
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            Save(AppModels.Status.draft);
        }

        protected void txtAmount_TextChanged(object sender, EventArgs e)
        {
            double totalbudget = 0;
            TextBox txtAmount;
            foreach (GridViewRow gvr in tblProposedBudget.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    txtAmount = (TextBox)gvr.FindControl("txtAmount");
                    totalbudget += double.Parse((txtAmount.Text == "" ? "0" : txtAmount.Text));
                }
            }
            txtTotalBudget.Text = totalbudget.ToString(AppModels.moneyformat);
            upanelbudget.Update();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Response.RedirectToRoute("dashboard");
        }

        private void RegisterAsyncControls()
        {
            foreach (GridViewRow grv in tblExpectedOutput.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    LinkButton btn = (LinkButton)grv.FindControl("btnRemove");
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btn);
                }
            }

            foreach (GridViewRow grv in tblProposedBudget.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btnRemoveBudget"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((TextBox)grv.FindControl("txtAmount"));
                }
            }

            foreach (GridViewRow grv in tblListOFAtendees.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    LinkButton btn = (LinkButton)grv.FindControl("btnRemoveAttendee");
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btn);
                }
            }
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSaveDraft);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSubmit);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnCancel);
        }

    }
}