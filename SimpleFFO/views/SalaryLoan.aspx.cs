using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;

namespace SimpleFFO.views
{
    public partial class SalaryLoan : System.Web.UI.Page
    {
        Auth auth;
        SalaryLoanController _salaryLoanController;

        #region "Vars"
        private int moduleid { get => AppModels.Modules.salaryloan; }
        private long salaryloanid
        {
            get { return Convert.ToInt64(ViewState["salaryloanid"] ?? "0"); }
            set { ViewState["salaryloanid"]=value; }
        }
        public long warehouseid
        {
            get { return Convert.ToInt64(ViewState["warehouseid"]); }
            set { ViewState["warehouseid"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this, modcode: AppModels.Modules.salaryloan);
            if (!auth.hasAuth())
                return;

            _salaryLoanController = new SalaryLoanController();

            salaryloanid = 0;

            if (!IsPostBack)
            {
                if (Page.RouteData.Values.ContainsKey("id"))
                {
                    this.salaryloanid = Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["id"].ToString()));
                    LoadRecord();
                }
                else
                {
                    this.salaryloanid = 0;
                    txtbox_fullname.Text = auth.currentuser.employee.firstname;
                    txtbox_datehired.Text = auth.currentuser.employee.hiredate?.ToString(" dd MMMM yyyy");
                    txtbox_dateapplied.Text = DateTime.Now.ToString("dd MMMM yyyy");
                }
            }
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!Page.IsPostBack)
            {
                if (this.salaryloanid > 0 && Page.RouteData.Values.ContainsKey("action"))
                {
                    ctlFunding.Bind(this.moduleid, this.salaryloanid, this.warehouseid, Convert.ToDecimal(txtbox_loanamount.Text));
                }
            }
        }
        protected void TextboxTextChanged(object sender, EventArgs e)
        {
            string IsNegative = "";
            try
            {
                string number = Convert.ToDouble(txtbox_loanamount.Text).ToString();
                double deductionperday;

                if (number.Contains("-"))
                {
                    IsNegative = "Minus ";
                    number = number.Substring(1, number.Length - 1);
                }
                if (number.Equals("0"))
                {
                    txtbox_loadamountinwords.Text = "Zero Only";
                }
                else
                {
                    txtbox_loadamountinwords.Text = IsNegative + Utility.ConvertToWords(number);
                }

                deductionperday = (Convert.ToDouble(number) / Convert.ToInt32(txtboxpayableinMonth.Text) / 2);
                deductionperday = Math.Round(deductionperday, 2);
                ViewState["deduction"] = deductionperday;
                txtboxdeductionperpayday.Text = deductionperday.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Save(int status)
        {
            if (salaryloanid == 0)
            {
                salaryloan _salaryloan = _salaryLoanController.getSalaryLoan(this.salaryloanid);
                _salaryloan.employeeid = auth.currentuser.employee.employeeid;
                _salaryloan.amount = Convert.ToDecimal(txtbox_loanamount.Text);
                _salaryloan.duration = Convert.ToInt32(txtboxpayableinMonth.Text);
                _salaryloan.deduction = Convert.ToDecimal(ViewState["deduction"]);
                _salaryloan.purpose = txtCompanyNotes.Text;
                _salaryloan.createdbyid = auth.currentuser.employee.employeeid;
                _salaryloan.created_at = DateTime.Now;
                _salaryloan.status = status;

                _salaryLoanController.SaveSalaryLoan(this.salaryloanid == 0, _salaryloan);
            }
        }

        protected void Submit(object sender, EventArgs e)
        {
            Save(AppModels.Status.submitted);
            this.Response.RedirectToRoute("dashboard");
        }

        protected void Cancel(object sender, EventArgs e)
        {
            this.Response.RedirectToRoute("dashboard");
        }

        private void LoadRecord()
        {
            salaryloan _salaryloan = _salaryLoanController.getSalaryLoan(salaryloanid);
            this.warehouseid = _salaryloan.employee.warehouseid ?? 0;
            txtbox_loanamount.Enabled = false;
            txtCompanyNotes.Enabled = false;

            btnSubmit.Visible = false;
            btnCancel.Visible = false;

            txtbox_fullname.Text = _salaryloan.employee.firstname;
            txtbox_loanamount.Text = Convert.ToDecimal(_salaryloan.amount).ToString("#.00");
            txtbox_loadamountinwords.Text = Utility.ConvertToWords(_salaryloan.amount.ToString());
            txtCompanyNotes.Text = _salaryloan.purpose;
            txtboxdeductionperpayday.Text = Convert.ToDecimal(_salaryloan.deduction).ToString("#.00");
            txtboxpayableinMonth.Text = _salaryloan.duration.ToString();
            txtbox_datehired.Text = Convert.ToDateTime(_salaryloan.employee.datecreated).ToString("dd MMMM yyyy");
            txtbox_dateapplied.Text = Convert.ToDateTime(_salaryloan.created_at).ToString("dd MMMM yyyy");

            ctlStatusTrail.Bind(this.moduleid, this.salaryloanid, _salaryloan.status ?? 0);

        }


    }
}