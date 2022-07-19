using Newtonsoft.Json;
using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views.components
{
    public partial class StatusTrailComponent : System.Web.UI.UserControl
    {
        ApprovalController approvalController;
        PRActivityController prActivityController;
        TieupController tieupController;
        ExpenseReportController expenseReportController;
        LeaveController leaveController;
        SalaryLoanController salaryLoanController;
        FundController fundController;
        VehicleRepairController vehicleRepairController;
        Auth auth;
        FFOPettyCashWS.Service1 options;

        private GenericObject approvalaction
        {
            get { return (GenericObject)ViewState["approvalaction"]; }
            set { ViewState["approvalaction"] = value; }
        }
        private int moduleId {
            get { return (int)ViewState["moduleId"]; }
            set { ViewState["moduleId"] = value; }
        }
        private long requestId
        {
            get { return (long)ViewState["requestId"]; }
            set { ViewState["requestId"] = value; }
        }
        public bool hasAction
        {
            get { return Convert.ToBoolean(ViewState["hasaction"] ?? false); }
            set { ViewState["hasaction"] = value; }
        }
        public long supplierNo
        {
            get { return Convert.ToInt64(ViewState["supplierNo"] ?? 0); }
            set { ViewState["supplierNo"] = value; }
        }
        public decimal totalAmount
        {
            get { return Convert.ToInt64(ViewState["totalAmount"] ?? 0); }
            set { ViewState["totalAmount"] = value; }
        }

        public void Bind(int moduleId, long requestId, int requestStatus)
        {
            this.moduleId = moduleId;
            this.requestId = requestId;

            init();
            LoadStatusTrail();
            if (requestStatus >= AppModels.Status.submitted)
            {
                checkIfApproval();
            }                
        }
        private void init()
        {
            auth = new Auth(this.Page);
            approvalController = new ApprovalController();
            prActivityController = new PRActivityController();
            tieupController = new TieupController();
            expenseReportController = new ExpenseReportController();
            leaveController = new LeaveController();
            salaryLoanController = new SalaryLoanController();
            fundController = new FundController();
            vehicleRepairController = new VehicleRepairController();
            options = new FFOPettyCashWS.Service1();

        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        public string getRequestStatusBadge(int status)
        {
            return AppModels.Status.getBadge(status);
        }
        private void LoadStatusTrail()
        {
            dgvstatustrail.DataSource = approvalController.GetStatustrails(moduleId, requestId);
            dgvstatustrail.DataBind();
            panelstatustrail.Visible = true;
        }
        private bool checkIfApproval()
        {
            switch (this.moduleId)
            {
                case AppModels.Modules.practivity:
                    practivity pr = prActivityController.GetPRActivity(this.requestId);
                    return checkIfApproval(pr.status ?? 0, pr.warehouse.employees.First(), pr.endorsedto, false);
                case AppModels.Modules.stop:
                case AppModels.Modules.tup:
                    tieup t = tieupController.GetTieup(this.requestId);
                    return checkIfApproval(t.status ?? 0, t.warehouse.employees.First(), t.endorsedto, false);
                case AppModels.Modules.expensereport:
                    expensereport er = expenseReportController.GetExpensereport(this.requestId);
                    return checkIfApproval(er.status ?? 0, er.warehouse.employees.First(), er.endorsedto, false);
                case AppModels.Modules.leaverequest:
                    employeeleave el = leaveController.GetEmployeeleave(this.requestId);
                    return checkIfApproval(el.status ?? 0, el.employee, el.endorsedto, false);
                case AppModels.Modules.salaryloan:
                    salaryloan s = salaryLoanController.getSalaryLoan(this.requestId);
                    return checkIfApproval(s.status ?? 0, s.employee, s.endorsedto, false);
                case AppModels.Modules.vehiclerepair:
                    vehiclerepair v = vehicleRepairController.GetVehiclerepair(this.requestId);
                    return checkIfApproval(v.status ?? 0, v.warehouse.employees.First(), v.endorsedto, false);
                default:
                    return false;
            }
        }
        private bool checkIfApproval(int requestStatus, employee employee, long? endorsedTo, bool isDisburse)
        {
            approvalaction = approvalController.checkIfApproval(moduleId, requestId, requestStatus, employee, endorsedTo, auth.currentuser.employeeid, isDisburse);
            if (approvalaction.statusaction != -1)
            {
                hasAction = true;
                panelapprovalaction.Visible = true;
                btnaccept.CommandArgument = approvalaction.statusaction.ToString();
                btnaccept.Text = AppModels.Status.getAction(approvalaction.statusaction);
                btnreject.CommandArgument = AppModels.Status.rejected.ToString();
                return true;
            }
            return false;
        }

        protected void btnSaveActivityAction_Click(object sender, EventArgs e)
        {
            init();
            bool hasError = false;
            lblerroractionremarks.Text = AppModels.ErrorMessage.required;
            if (Validator.RequiredField(txtActionRemarks))
            {
                LinkButton btn = (LinkButton)sender;
                int requestStatus= Convert.ToInt32(btn.CommandArgument);
                if (!checkIfApproval())
                {
                    Response.Redirect(this.Page.Request.Url.ToString());
                    return;
                }
                switch (this.moduleId)
                {
                    case AppModels.Modules.practivity:
                        practivity pr = prActivityController.GetPRActivity(this.requestId);
                        pr.status = requestStatus;
                        if (pr.status == AppModels.Status.rejected)
                            pr.endorsedto = null;
                        else
                            pr.endorsedto = this.approvalaction.endorsenext;
                        prActivityController.SaveChanges();
                        break;
                    case AppModels.Modules.tup:
                    case AppModels.Modules.stop:
                        tieup t = tieupController.GetTieup(this.requestId);
                        t.status = requestStatus;
                        if (t.status == AppModels.Status.rejected)
                            t.endorsedto = null;
                        else
                            t.endorsedto = this.approvalaction.endorsenext;
                        tieupController.SaveChanges();
                        break;
                    case AppModels.Modules.expensereport:
                        expensereport er = expenseReportController.GetExpensereport(this.requestId);
                        er.status = requestStatus;
                        if (er.status == AppModels.Status.rejected)
                            er.endorsedto = null;
                        else
                            er.endorsedto = this.approvalaction.endorsenext;
                        expenseReportController.SaveChanges();
                        if (er.status == AppModels.Status.approved)
                        {
                            fundController.GetFundRequests(this.moduleId, this.requestId, er.warehouseid ?? 0);
                            postexpensereport(this.requestId);
                        }
                        break;

                    case AppModels.Modules.leaverequest:
                        employeeleave el = leaveController.GetEmployeeleave(this.requestId);
                        el.status = requestStatus;
                        if (el.status == AppModels.Status.rejected)
                            el.endorsedto = null;
                        else
                            el.endorsedto = this.approvalaction.endorsenext;
                        leaveController.SaveChanges();
                        break;
                    case AppModels.Modules.salaryloan:
                        salaryloan s = salaryLoanController.getSalaryLoan(this.requestId);
                        s.status = requestStatus;
                        if (s.status == AppModels.Status.rejected)
                            s.endorsedto = null;
                        else
                            s.endorsedto = this.approvalaction.endorsenext;
                        salaryLoanController.SaveChanges();
                        if (s.status == AppModels.Status.approved)
                            fundController.GetFundRequests(this.moduleId, this.requestId, s.employee.warehouseid ?? 0);
                        break;
                    case AppModels.Modules.vehiclerepair:
                        if (this.supplierNo > 0)
                        {
                            vehiclerepair v = vehicleRepairController.GetVehiclerepair(this.requestId);
                            v.status = requestStatus;
                            //v.supplierno = this.supplierNo;
                            v.totalamount = this.totalAmount;
                            if (v.status == AppModels.Status.rejected)
                                v.endorsedto = null;
                            else
                                v.endorsedto = this.approvalaction.endorsenext;
                            vehicleRepairController.SaveChanges();
                            Validator.SetError(this.txtActionRemarks, false);
                        }
                        else
                        {
                            lblerroractionremarks.Text = "Please select your preferred proposal.";
                            hasError = true;
                            Validator.SetError(this.txtActionRemarks, true);
                        }
                        break;
                }
                if (!hasError)
                {
                    approvalController.SaveTrail(this.moduleId, this.requestId, (long)auth.currentuser.employeeid, requestStatus, this.approvalaction.treelevel, txtActionRemarks.Text);
                    Response.RedirectToRoute(AppModels.Routes.dashboard);
                }
            }
            upanelactions.Update();
        }

        private void postexpensereport(long expensereportid)
        {
            List<expensereportmiscellaneou> lst = expenseReportController.GetpostExpMisc(expensereportid);
            try
            {
                var misobj = new ExpenseReportObject();
                misobj.ExpenseReportPost = new List<PostExpenseReports>();

                foreach (var gathered in lst)
                {
                    var store = new PostExpenseReports()
                    {
                        dateencoded = gathered.expensedate,
                        vendorid = (long)gathered.vendorid,
                        expenseid = (long)gathered.misccodeid,
                        refno = gathered.referenceno,
                        refdate = gathered.expensedate,
                        amount = gathered.amount,
                        vat = gathered.isvat,
                        remarks = gathered.particulars,
                        ffo_expensereportid = gathered.miscexpenseid
                    };
                    misobj.ExpenseReportPost.Add(store);
                }
                string Serialize = JsonConvert.SerializeObject(misobj);
                options.Upload_Data(auth.GetToken(),FFOPettyCashWS.myTransactCode.CPostExpenseReport,Serialize);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}