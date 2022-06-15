using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.Views
{
    public partial class LOA : System.Web.UI.Page
    {
        LeaveController leaveController;
        ApprovalController approvalController;
        Auth auth;
        #region "vars"
        private int moduleid { get => AppModels.Modules.leaverequest; }
        public long leaveid
        {
            get => Convert.ToInt64(ViewState["leaveid"]);
            set { ViewState["leaveid"] = value;}
        }
        public long warehouseid
        {
            get { return Convert.ToInt64(ViewState["warehouseid"]); }
            set { ViewState["warehouseid"] = value; }
        }
        public GenericObject approvalaction
        {
            get { return (GenericObject)ViewState["approvalaction"]; }
            set { ViewState["approvalaction"] = value; }
        }
        public int currentyear
        {
            get => Convert.ToInt32(ViewState["year"]);
            set
            {
                ViewState["year"] = value;
            }
        }
        public decimal yearlyleave
        {
            get => Convert.ToDecimal(ViewState["yearlyleave"]);
            set
            {
                ViewState["yearlyleave"] = value;
            }
        }
        public long employeeid
        {
            get => Convert.ToInt64(ViewState["employeeid"]);
            set
            {
                ViewState["employeeid"] = value;
            }
        }
        public int treelevel
        {
            get { return Convert.ToInt32(ViewState["treelevel"]); }
            set { ViewState["treelevel"] = value; }
        }
        public long endorsenext
        {
            get { return Convert.ToInt64(ViewState["endorsenext"]); }
            set { ViewState["endorsenext"] = value; }
        }
        public bool isPageView
        {
            get { return Convert.ToBoolean(ViewState["ispageview"] ?? false); }
            set { ViewState["ispageview"] = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this,modcode: AppModels.Modules.leaverequest);
            if (!auth.hasAuth())
                return;

            leaveController = new LeaveController();
            approvalController = new ApprovalController();
            if (!Page.IsPostBack)
            {
                this.yearlyleave = leaveController.getYearlyLeaveDays();
                this.currentyear = Utility.getServerDate().Year;
                LoadCombo();
                if (Page.RouteData.Values.ContainsKey("id"))
                {
                    this.leaveid = Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["id"].ToString()));
                    LoadRecord();
                }
                else
                {
                    this.leaveid = 0;
                    this.employeeid = auth.currentuser.employeeid ?? 0;
                    this.btnTagasCancelled.Visible = false;
                }
                DisplayUserInfo();
                DisplayLeaveList();
            }
            else
            {
                tblLeave.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
            RegisterAsyncControls();
        }
        private void RegisterAsyncControls()
        {
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnTagasCancelled);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSubmit);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnCancel);
        }
        public void DisplayUserInfo()
        {
            txtbox_fullname.Text = auth.currentuser.employee.firstname;
            txtbox_datehired.Text = auth.currentuser.employee.hiredate.ToString();
            if (this.leaveid == 0)
                txtbox_datefiled.Text = Utility.getServerDate().ToString(AppModels.dateformat);
            txtbox_role.Text = auth.currentuser.employee.lastname;
            txtbox_branch.Text = auth.currentuser.employee.branch.branchname;

            txttotaldaysapplied.Text = leaveController.getRemainingLeaveDays(this.employeeid, this.currentyear, 0).ToString(AppModels.decimaloneformat);
            txttotalapproveddays.Text = leaveController.getRemainingLeaveDays(this.employeeid, this.currentyear, 1).ToString(AppModels.decimaloneformat);
            txttotalpendingdays.Text = (Convert.ToDecimal(txttotaldaysapplied.Text) - Convert.ToDecimal(txttotalapproveddays.Text)).ToString(AppModels.decimaloneformat);
            txtleavebalance.Text = (this.yearlyleave - Convert.ToDecimal(txttotaldaysapplied.Text)).ToString(AppModels.decimaloneformat);
        }

        public void LoadCombo()
        {
            cmbTypeofLeave.DataSource = leaveController.GetLeaveTypes();
            cmbTypeofLeave.DataTextField = "leavetypename";
            cmbTypeofLeave.DataValueField = "leavetypeid";
            cmbTypeofLeave.DataBind();
        }

        public void DisplayLeaveList()
        {

            tblLeave.DataSource = leaveController.GetEmployeeleaves(this.employeeid);
            tblLeave.DataKeyNames = new string[] { "leaveid" };
            tblLeave.DataBind();
            tblLeave.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        public string getDescriptionStatus(DateTime sdate, DateTime edate)
        {
            return sdate.ToString(AppModels.dateformat) + "-" + edate.ToString(AppModels.dateformat);
        }
        public string getStatus(int status)
        {
            return AppModels.Status.getStatus(status);
        }
        public string getStatusBadge(int status)
        {
               return AppModels.Status.getBadge(status);
        }
        private void LoadRecord()
        {
            employeeleave e = leaveController.GetEmployeeleave(this.leaveid);
            this.employeeid = e.employeeid ?? 0;
            cmbTypeofLeave.SelectedValue = e.leavetypeid.ToString();
            dtpFromDate.Text = (e.dayfrom ?? DateTime.MinValue).ToString(AppModels.dateformat);
            dtpToDate.Text = (e.dayto ?? DateTime.MinValue).ToString(AppModels.dateformat);
            this.dropdownHalfDays.SelectedValue = (e.hashalfday ?? false) ? "1" : "0" ;
            txtboxLeaveDays.Text = (e.noofdays ?? 0).ToString();
            dtpToWorkDate.Text = (e.bacttowork ?? DateTime.MinValue).ToString(AppModels.dateformat);
            txtbox_reason.Text = e.reason;
            txtbox_contact.Text = e.contactdetails;
            //_employeeleave.reliever = null;
            txtbox_datefiled.Text = (e.datecreated ?? DateTime.MinValue).ToString(AppModels.dateformat);
            if(e.status==AppModels.Status.submitted && e.employeeid==auth.currentuser.employeeid)
                this.btnTagasCancelled.Visible = true;
            else
                this.btnTagasCancelled.Visible = false;


            if ((e.status ?? 0) >= AppModels.Status.submitted)
            {
                isPageView = true;
                btnCancel.Visible = false;
                btnSubmit.Visible = false;

                dtpFromDate.ReadOnly = true;
                dtpFromDate.CssClass = dtpFromDate.CssClass.Replace(" dtpFromDate", "");
                dtpToDate.ReadOnly = true;
                dtpToDate.CssClass = dtpFromDate.CssClass.Replace(" dtpToDate", "");
                dropdownHalfDays.Enabled = false;
                cmbTypeofLeave.Enabled = false;
                txtbox_reason.ReadOnly = true;
                txtbox_contact.ReadOnly = true;
                txtReliever.ReadOnly = true;

                ctlStatusTrail.Bind(this.moduleid, this.leaveid, e.status ?? 0);
            }
            else if (e.status == AppModels.Status.rejected)
            {
                btnSubmit.Text = "Re-Submit";
                ctlStatusTrail.Bind(this.moduleid, this.leaveid, e.status ?? 0);
            }
            ApplicationForm();
        }
        public string getRequestStatusBadge(int status)
        {
            return AppModels.Status.getBadge(status);
        }
        public string getElementRoute(string id)
        {
            return GetRouteUrl(AppModels.Routes.requestloadetails, new { id = Auth.AppSecurity.URLEncrypt(id) });
        }

        public void ApplicationForm()
        {
            if (!dtpFromDate.Text.Equals("") && !dtpToDate.Text.Equals(""))
            {
                Dictionary<string, object> obj = leaveController.ProcessLeave(Convert.ToDateTime(dtpFromDate.Text), Convert.ToDateTime(dtpToDate.Text), dropdownHalfDays.SelectedValue == "1", this.currentyear);
                txtboxLeaveDays.Text = obj["leavedays"].ToString();
                txbox_coveredHolidays.Text = obj["coveredholiday"].ToString();
                txtboxCoveredDayoff.Text = obj["covereddayoff"].ToString();
                dtpToWorkDate.Text = Convert.ToDateTime(obj["backtowork"]).ToString(AppModels.dateformat);
            }
        }

        private bool PageValidate()
        {
            bool isvalid=true;
            errordtpFromDate.Text = AppModels.ErrorMessage.invalidinput;
            errordtpToDate.Text = AppModels.ErrorMessage.invalidinput;
            if (!(Validator.DateValid(dtpFromDate) && Convert.ToDateTime(dtpFromDate.Text).Date >= Utility.getServerDate().Date))
            {
                isvalid = false;
                Validator.SetError(dtpFromDate, true);
            }
            if (!(Validator.DateValid(dtpToDate) && Convert.ToDateTime(dtpToDate.Text).Date >= Utility.getServerDate().Date))
            {
                isvalid = false;
                Validator.SetError(dtpToDate, true);
            }
            if(isvalid)
            {
                if (Convert.ToDateTime(dtpFromDate.Text).Date > Convert.ToDateTime(dtpToDate.Text).Date)
                {
                    isvalid = false;
                    Validator.SetError(dtpFromDate, true);
                    Validator.SetError(dtpToDate, true);
                }
            }
            if (isvalid)
            {
                decimal totaldaysapplied = leaveController.getRemainingLeaveDays(this.employeeid, this.currentyear, 0);
                if (Decimal.TryParse(txtboxLeaveDays.Text, out _))
                {
                    if (Convert.ToDecimal(txtboxLeaveDays.Text) <= 0)
                    {
                        errortxtboxLeaveDays.Text = "*Invalid Leave Days.";
                        Validator.SetError(txtboxLeaveDays, true);
                        isvalid = false;
                    }
                    else if (Convert.ToDecimal(txtboxLeaveDays.Text) > (this.yearlyleave - totaldaysapplied))
                    {
                        errortxtboxLeaveDays.Text = "*You only have " + (this.yearlyleave - totaldaysapplied) + " leave balance.";
                        Validator.SetError(txtboxLeaveDays, true);
                        isvalid = false;
                    }
                    else
                    {
                        Validator.SetError(txtboxLeaveDays);
                    }
                }
                else
                {
                    errortxtboxLeaveDays.Text = AppModels.ErrorMessage.invalidinput;
                    isvalid = false;
                }
            }
            isvalid = Validator.DecOneAbove(cmbTypeofLeave) && isvalid;
            isvalid = Validator.RequiredField(txtbox_reason) && isvalid;
            isvalid = Validator.DecZeroAbove(txtbox_contact) && isvalid;
            return isvalid;
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (PageValidate())
            {
                Submit(AppModels.Status.submitted);
            }
            else
            {
                PageController.fnScroll(this, "is-invalid", true, true);
            }
            upanelmain.Update();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Response.RedirectToRoute("dashboard");
        }
        private void Submit(int status)
        {
            employeeleave _employeeleave = leaveController.GetEmployeeleave(this.leaveid);
            if (this.leaveid == 0)
            {
                _employeeleave.employeeid = this.employeeid;
                _employeeleave.leavetypeid = Convert.ToInt32(cmbTypeofLeave.SelectedValue);
                _employeeleave.datecreated = Utility.getServerDate();
            }
            _employeeleave.dayfrom = Convert.ToDateTime(dtpFromDate.Text);
            _employeeleave.dayto = Convert.ToDateTime(dtpToDate.Text);
            _employeeleave.hashalfday = this.dropdownHalfDays.SelectedValue == "1";
            _employeeleave.noofdays = Convert.ToDecimal(txtboxLeaveDays.Text);
            _employeeleave.bacttowork = Convert.ToDateTime(dtpToWorkDate.Text);
            _employeeleave.reason = txtbox_reason.Text;
            _employeeleave.contactdetails = txtbox_contact.Text;
            _employeeleave.reliever = null;
            _employeeleave.status = status;

            leaveController.SaveLeave(this.leaveid == 0, _employeeleave);
            this.Response.RedirectToRoute("dashboard");
        }

        protected void dtpDate_TextChanged(Object sender, EventArgs e)
        {
            ApplicationForm();
        }

        protected void btnTagasCancelled_Click(object sender, EventArgs e)
        {
                Submit(AppModels.Status.cancelled);
        }

    }
}