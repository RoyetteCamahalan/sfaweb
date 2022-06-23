using Newtonsoft.Json;
using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace SimpleFFO.views.components
{
    public partial class FundingComponent : System.Web.UI.UserControl
    {
        FundController fundController;
        ApprovalController approvalController;
        SupplierController supplierController;

        DoctorController doctorController;
        InstitutionController institutionController;
        PRActivityController practivitycontroller;
        TieupController tieupcontroller;


        Auth auth;
        List<Vendor> lstvendor;
        FFOPettyCashWS.Service1 options;

        private int moduleId
        {
            get { return (int)ViewState["moduleId"]; }
            set { ViewState["moduleId"] = value; }
        }
        private long requestId
        {
            get { return (long)ViewState["requestId"]; }
            set { ViewState["requestId"] = value; }
        }
        private long warehouseId
        {
            get { return (long)ViewState["warehouseId"]; }
            set { ViewState["warehouseId"] = value; }
        }
        private decimal totalBudget
        {
            get { return (decimal)ViewState["totalBudget"]; }
            set { ViewState["totalBudget"] = value; }
        }
        public statustrail fundrequeststatus
        {
            get { return (statustrail)ViewState["fundrequeststatus"]; }
            set { ViewState["fundrequeststatus"] = value; }
        }
        public string pageSessionState
        {
            get => (Session["pageSessionState"] ?? "").ToString();
            set => Session["pageSessionState"] = value;
        }
        public void Bind(int moduleId, long requestId, long warehouseId, decimal totalBudget)
        {
            this.moduleId = moduleId;
            this.requestId = requestId;
            this.warehouseId = warehouseId;
            this.totalBudget = totalBudget;

            init();
            LoadFundRequest();
            LoadFundLiquidation();
            webserviceresult();
            PageController.fnScroll(this.Page, panelfundrequest.ClientID, false, false);
        }
        private void init()
        {
            auth = new Auth(this.Page);
            fundController = new FundController();
            approvalController = new ApprovalController();
            supplierController = new SupplierController();

            doctorController = new DoctorController();
            institutionController = new InstitutionController();
            practivitycontroller = new PRActivityController();
            tieupcontroller = new TieupController();

            options = new FFOPettyCashWS.Service1();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                cmbpaymentmode.DataSource = AppModels.Funding.PaymentMode.ToList();
                cmbpaymentmode.DataTextField = "value";
                cmbpaymentmode.DataValueField = "key";
                cmbpaymentmode.DataBind();
            }
            RegisterAsyncControls();
        }
        public string getRequestStatusBadge(int status)
        {
            return AppModels.Status.getBadge(status);
        }
        public string getRequestStatus(int status)
        {
            return AppModels.Status.getStatus(status);
        }
        private void RegisterAsyncControls()
        {
            foreach (GridViewRow grv in dgvfundrequest.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btnshowdisbursement"));
                    ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btnapprovefundreq"));
                    ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btndeclinefundreq"));
                    ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btnroutingfundreq"));
                }
            }
            foreach (GridViewRow grv in dgvfundliquidation.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btnapprovefundliq"));
                    ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btndeclinefundliq"));
                    ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btnroutingfundliq"));
                }
            }
            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(btnaddfundrequest);
            ScriptManager.GetCurrent(this.Page).RegisterAsyncPostBackControl(btnaddfundliquidation);
        }
        #region "FundingRequest"
        private void LoadFundRequest()
        {
            List<fundrequest> lstrequest = fundController.GetFundRequests(this.moduleId, this.requestId, this.warehouseId);
            dgvfundrequest.DataSource = lstrequest;
            dgvfundrequest.DataKeyNames = new string[] { "fundrequestid" };
            dgvfundrequest.DataBind();
            panelfundrequest.Visible = true;

            lbltotalfundreleased.Text = (lstrequest.Where(r => r.status == AppModels.Status.completed).Select(r => r.amount).DefaultIfEmpty(0).Sum() ?? 0).ToString(AppModels.moneyformat);
            lbltotalfundrequested.Text = (lstrequest.Where(r => r.status != AppModels.Status.rejected).Select(r => r.amount).DefaultIfEmpty(0).Sum() ?? 0).ToString(AppModels.moneyformat);
            lblfundbudget.Text = this.totalBudget.ToString(AppModels.moneyformat);
            if (this.moduleId == AppModels.Modules.expensereport)
            {
                btnaddfundrequest.Visible = false;
                panelrequestheader.Visible = false;
            }
            else if (this.warehouseId != auth.currentuser.employee.warehouseid || Convert.ToDecimal(lbltotalfundrequested.Text) >= Convert.ToDecimal(lblfundbudget.Text))
                btnaddfundrequest.Visible = false;
            else
                btnaddfundrequest.Visible = true;

            RegisterAsyncControls();
        }
        protected void btnaddfundrequest_Click(object sender, EventArgs e)
        {
            panelfundrequestentry.Visible = true;
            panelfundrequestentry.CssClass = panelfundrequestentry.CssClass.Replace(" collapsed-card", "");
            upanelfundrequest.Update();
        }

        protected void btncancelfundrequest_Click(object sender, EventArgs e)
        {
            panelfundrequestentry.Visible = false;
            panelfundrequestentry.CssClass += " collapsed-card";
            upanelfundrequest.Update();
        }
        private void ClearFundRequestEntry()
        {
            this.txtdateneeded.Text = "";
            this.cmbpaymentmode.SelectedValue = "-1";
            this.txtpayee.Text = "";
            this.txtfundrequestamount.Text = "";
        }

        protected void btnsavefundrequest_Click(object sender, EventArgs e)
        {
            init();
            bool isvalid = true;
            isvalid = Validator.DateValid(txtdateneeded);
            isvalid = Validator.DecZeroAbove(cmbpaymentmode) && isvalid;
            errortxtfundrequestamount.Text = AppModels.ErrorMessage.oneabove;
            isvalid = Validator.DecOneAbove(txtfundrequestamount) && isvalid;
            if (isvalid)
            {
                if (Convert.ToInt32(cmbpaymentmode.SelectedValue) != AppModels.Funding.PaymentMode.cash)
                    isvalid = Validator.RequiredField(txtpayee);
                else
                    Validator.SetError(txtpayee);
                if (Convert.ToDateTime(txtdateneeded.Text) < Utility.getServerDate())
                {
                    isvalid = false;
                    Validator.SetError(txtdateneeded, true);
                }
                else
                    Validator.SetError(txtdateneeded);

                List<fundrequest> requests = fundController.GetFundRequests(this.moduleId, this.requestId, this.warehouseId);
                decimal totalrequest = requests.Where(r => r.status != AppModels.Status.rejected).Select(r => r.amount).DefaultIfEmpty(0).Sum() ?? 0;
                decimal currentrequest = Convert.ToDecimal(txtfundrequestamount.Text);
                errortxtfundrequestamount.Text = "Amount exceed total budget!";
                if ((this.totalBudget - totalrequest) < currentrequest)
                {
                    isvalid = false;
                    Validator.SetError(txtfundrequestamount, true);
                }
                else
                    Validator.SetError(txtfundrequestamount);

                if (isvalid)
                {
                    fundrequest req = fundController.GetFundRequest(0);
                    req.warehouseid = this.warehouseId;
                    req.moduleid = this.moduleId;
                    req.requestid = this.requestId;
                    req.dateneeded = Convert.ToDateTime(txtdateneeded.Text);
                    req.paymentmode = Convert.ToInt32(cmbpaymentmode.SelectedValue);
                    req.paymentref = txtpayee.Text;
                    req.status = AppModels.Status.submitted;
                    req.amount = Convert.ToDecimal(txtfundrequestamount.Text);
                    req.daterequested = Utility.getServerDate();
                    fundController.SaveFundRequest(true, req);
                    panelfundrequestentry.Visible = false;
                    panelfundrequestentry.CssClass = panelfundrequestentry.CssClass.Replace(" collapsed-card", "");
                    LoadFundRequest();
                    ClearFundRequestEntry();
                    upanelfundrequestdashboard.Update();
                }
            }
            upanelfundrequest.Update();
        }
        protected void btnsavedisbursement_Click(object sender, EventArgs e)
        {
            init();
            if (Validator.RequiredField(txtdisbursementreference) && Validator.RequiredField(txtdisbursementremarks))
            {
                statustrail st = fundrequeststatus;
                fundrequest f = fundController.GetFundRequest(st.requestid ?? 0);
                f.status = AppModels.Status.completed;
                f.disbursementdate = Utility.getServerDate();
                f.disbursementref = txtdisbursementreference.Text;
                f.paymentmode = Convert.ToInt32(cmbdisburmentmode.SelectedValue);
                f.disbursementremarks = txtdisbursementremarks.Text;
                f.isdisburse = true;
                f.endorsedto = 0;
                st.statusid = AppModels.Status.completed;
                st.remarks = f.disbursementremarks;
                st.traildate = DateTime.Now;
                postwebserviceresult(f);
                fundController.SaveChanges();
                approvalController.SaveTrail(st);
                LoadFundRequest();
                upanelfundrequestdashboard.Update();
                PageController.fnFireEvent(this.Page, PageController.EventType.click, "btndisbursementmodal", true);
            }
            upaneldisbursement.Update();
        }
        #endregion
        #region "FundingCommon"
        protected void btnroutingfund_Click(object sender, EventArgs e)
        {
            init();
            LinkButton btn = (LinkButton)sender;
            if (btn.ID == "btnroutingfundliq")
                dgvmodalrouting.DataSource = approvalController.GetStatustrails(AppModels.Modules.fundliquidation, Convert.ToInt64(btn.CommandArgument));
            else
            {
                List<statustrail> lst = approvalController.GetStatustrails(this.moduleId, this.requestId);
                lst.AddRange(approvalController.GetStatustrails(AppModels.Modules.fundrequest, Convert.ToInt64(btn.CommandArgument)));
                dgvmodalrouting.DataSource = lst;
            }
            dgvmodalrouting.DataBind();
            upanelmodalrouting.Update();
            PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnroutingmodal", true);
        }
        protected void dgvfund_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GenericObject _approvalaction;
                LinkButton btnapprovefund;
                LinkButton btndeclinefund;
                if (((GridView)sender).ClientID == dgvfundrequest.ClientID)
                {
                    fundrequest f = fundController.GetFundRequest(Convert.ToInt64(dgvfundrequest.DataKeys[e.Row.DataItemIndex]["fundrequestid"]));
                    if ((f.isdisburse ?? false) && f.status == AppModels.Status.approved)
                        _approvalaction = approvalController.checkIfApproval(AppModels.Modules.fundrequest, f.fundrequestid, f.status ?? 0, f.warehouse.employees.First(), f.endorsedto, auth.currentuser.employeeid, true);
                    else
                        _approvalaction = approvalController.checkIfApproval(AppModels.Modules.fundrequest, f.fundrequestid, f.status ?? 0, f.warehouse.employees.First(), f.endorsedto, auth.currentuser.employeeid, false);
                    btnapprovefund = (LinkButton)e.Row.FindControl("btnapprovefundreq");
                    btndeclinefund = (LinkButton)e.Row.FindControl("btndeclinefundreq");
                    LinkButton btnshowdisbursement = (LinkButton)e.Row.FindControl("btnshowdisbursement");
                    btnshowdisbursement.Visible = (_approvalaction.statusaction == AppModels.Status.fordisbursement || f.status == AppModels.Status.completed);
                }
                else
                {
                    fundliquidation f = fundController.GetFundLiquidation(Convert.ToInt64(dgvfundliquidation.DataKeys[e.Row.DataItemIndex]["fundliquidationid"]));
                    _approvalaction = approvalController.checkIfApproval(AppModels.Modules.fundliquidation, f.fundliquidationid, f.status ?? 0, f.warehouse.employees.First(), f.endorsedto, auth.currentuser.employeeid, false);
                    btnapprovefund = (LinkButton)e.Row.FindControl("btnapprovefundliq");
                    btndeclinefund = (LinkButton)e.Row.FindControl("btndeclinefundliq");

                    if ((DataBinder.Eval(e.Row.DataItem, "receiptimagepath") ?? "").ToString() != "")
                    {
                        Image imgpreview = (Image)e.Row.FindControl("imgpreview");
                        imgpreview.ImageUrl = Utility.getImage(this.warehouseId, DataBinder.Eval(e.Row.DataItem, "receiptimagepath").ToString(), Utility.enfiletype.disbursementreceipt);
                        imgpreview.Visible = true;
                    }
                }
                if (_approvalaction.statusaction != -1)
                {
                    btnapprovefund.Visible = true;
                    btnapprovefund.Text = AppModels.Status.getAction(_approvalaction.statusaction);
                    btndeclinefund.Visible = _approvalaction.statusaction != AppModels.Status.fordisbursement;
                }
            }
        }
        protected void btnapprovefund_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            if (btn.ID == "btnapprovefundreq")
                showfundapproval(AppModels.Modules.fundrequest, Convert.ToInt64(btn.CommandArgument), true);
            else
                showfundapproval(AppModels.Modules.fundliquidation, Convert.ToInt64(btn.CommandArgument), true);
        }
        protected void btndeclinefund_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            if (btn.ClientID == "btndeclinefundreq")
                showfundapproval(AppModels.Modules.fundrequest, Convert.ToInt64(btn.CommandArgument), false);
            else
                showfundapproval(AppModels.Modules.fundliquidation, Convert.ToInt64(btn.CommandArgument), false);
        }
        private void showfundapproval(int moduleid, long requestid, bool isapprove)
        {
            init();
            statustrail st = new statustrail
            {
                moduleid = moduleid,
                requestid = requestid
            };
            int paymentmode = 0;
            GenericObject _approvalaction;
            if (moduleid == AppModels.Modules.fundrequest)
            {
                fundrequest f = fundController.GetFundRequest(requestid);
                paymentmode = f.paymentmode ?? 0;
                if ((f.isdisburse ?? false) && f.status == AppModels.Status.approved)
                    _approvalaction = approvalController.checkIfApproval(AppModels.Modules.fundrequest, f.fundrequestid, f.status ?? 0, f.warehouse.employees.First(), f.endorsedto, auth.currentuser.employeeid, true);
                else
                    _approvalaction = approvalController.checkIfApproval(AppModels.Modules.fundrequest, f.fundrequestid, f.status ?? 0, f.warehouse.employees.First(), f.endorsedto, auth.currentuser.employeeid, false);
            }
            else
            {
                fundliquidation f = fundController.GetFundLiquidation(requestid);
                _approvalaction = approvalController.checkIfApproval(moduleid, f.fundliquidationid, f.status ?? 0, f.warehouse.employees.First(), f.endorsedto, auth.currentuser.employeeid, false);
            }
            if (_approvalaction.statusaction == AppModels.Status.fordisbursement)
            {
                st.statusid = AppModels.Status.fordisbursement;
                st.employeeid = auth.currentuser.employeeid;
                cmbdisburmentmode.DataSource = AppModels.Funding.PaymentMode.ToList();
                cmbdisburmentmode.DataTextField = "value";
                cmbdisburmentmode.DataValueField = "key";
                cmbdisburmentmode.DataBind();
                cmbdisburmentmode.SelectedValue = paymentmode.ToString();
                txtdisbursementdate.Text = Utility.getServerDate().ToString(AppModels.dateformat);
                txtdisbursementreference.Text = "";
                txtdisbursementremarks.Text = "";
                cmbdisburmentmode.Enabled = true;
                txtdisbursementdate.ReadOnly = false;
                txtdisbursementreference.ReadOnly = false;
                txtdisbursementremarks.ReadOnly = false;
                this.fundrequeststatus = st;
                btnsavedisbursement.CssClass = "btn btn-flat " + AppModels.Status.getClassColor(st.statusid ?? 0);
                btnsavedisbursement.Visible = true;
                upaneldisbursement.Update();
                PageController.fnFireEvent(this.Page, PageController.EventType.click, "btndisbursementmodal", true);
            }
            else if (_approvalaction.statusaction != -1)
            {
                st.statusid = isapprove ? _approvalaction.statusaction : AppModels.Status.rejected;
                st.employeeid = isapprove ? _approvalaction.endorsenext : 0;
                btnSaveFundAction.Text = AppModels.Status.getAction(st.statusid ?? 0);
                btnSaveFundAction.CssClass = "btn btn-flat " + AppModels.Status.getClassColor(st.statusid ?? 0);
                this.fundrequeststatus = st;
                upanelfundrequestmodal.Update();
                PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnfundrequestmodal", true);
            }
            else
            {
                LoadFundRequest();
                upanelfundrequestdashboard.Update();
            }
        }

        protected void btnSaveFundAction_Click(object sender, EventArgs e)
        {
            init();
            if (Validator.RequiredField(txtremarks))
            {
                statustrail st = fundrequeststatus;
                st.traildate = DateTime.Now;
                st.remarks = txtremarks.Text;

                if (st.moduleid == AppModels.Modules.fundrequest)
                {
                    fundrequest f = fundController.GetFundRequest(st.requestid ?? 0);
                    f.status = st.statusid;
                    f.endorsedto = st.employeeid;
                    fundController.SaveChanges();
                    LoadFundRequest();
                    upanelfundrequestdashboard.Update();
                }
                else
                {
                    fundliquidation f = fundController.GetFundLiquidation(st.requestid ?? 0);
                    f.status = st.statusid;
                    f.endorsedto = st.employeeid;
                    fundController.SaveChanges();
                    LoadFundLiquidation();
                    upanelfunliquidationdashboard.Update();
                }
                st.employeeid = auth.currentuser.employeeid;
                approvalController.SaveTrail(st);
                txtremarks.Text = "";
                PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnfundrequestmodal", true);
            }
            upanelfundrequestmodal.Update();
        }

        #endregion
        #region "FundingLiquidation"
        private void LoadFundLiquidation()
        {
            if (this.moduleId == AppModels.Modules.expensereport || this.moduleId == AppModels.Modules.salaryloan)
                return;
            List<fundliquidation> lstliquidation = fundController.GetFundLiquidations(this.moduleId, this.requestId);
            dgvfundliquidation.DataSource = lstliquidation;
            dgvfundliquidation.DataKeyNames = new string[] { "fundliquidationid" };
            dgvfundliquidation.DataBind();
            panelfundliquidation.Visible = true;

            lbltotalliquidated.Text = (lstliquidation.Where(r => r.status != AppModels.Status.rejected).Select(r => r.amount).DefaultIfEmpty(0).Sum() ?? 0).ToString(AppModels.moneyformat);
            lblforliquidation.Text = (Convert.ToDecimal(lbltotalfundreleased.Text) - Convert.ToDecimal(lbltotalliquidated.Text)).ToString(AppModels.moneyformat);

            if (this.warehouseId != auth.currentuser.employee.warehouseid || Convert.ToDecimal(lblforliquidation.Text) <= 0)
                btnaddfundliquidation.Visible = false;
            else
                btnaddfundliquidation.Visible = true;

            RegisterAsyncControls();
        }
        protected void btnaddfundliquidation_Click(object sender, EventArgs e)
        {
            //webserviceresult();

            panelfundliqidationentry.Visible = true;
            panelfundliqidationentry.CssClass = panelfundliqidationentry.CssClass.Replace(" collapsed-card", "");
            txtdatesubmitted.Text = DateTime.Now.ToString(AppModels.dateformat);
            pageSessionState = "";
            upanelfundliquidation.Update();
        }
        protected void btncancelfundliquidation_Click(object sender, EventArgs e)
        {
            panelfundliqidationentry.Visible = false;
            panelfundliqidationentry.CssClass += " collapsed-card";
            pageSessionState = System.DateTime.Now.ToString();
            upanelfundliquidation.Update();
        }
        private void ClearFundLiquidationEntry()
        {
            this.txtactualdate.Text = "";
            this.txtremarks.Text = "-1";
            this.txtfundliquidationamount.Text = "";
            this.literalimagefilename.Text = "";
            this.imgpreview.ImageUrl = "";
        }
        protected void btnsavefundliquidation_Click(object sender, EventArgs e)
        {
            init();
            if (panelfundliqidationentry.CssClass.Contains("collapsed-card"))
                return;

            bool isvalid = true;
            isvalid = Validator.DateValid(txtactualdate);
            errortxtfundliquidationamount.Text = AppModels.ErrorMessage.oneabove;
            isvalid = Validator.DecOneAbove(txtfundliquidationamount) && isvalid;
            if (isvalid)
            {
                List<fundliquidation> liquidations = fundController.GetFundLiquidations(this.moduleId, this.requestId);
                List<fundrequest> requests = fundController.GetFundRequests(this.moduleId, this.requestId, this.warehouseId);
                decimal totalreleased = requests.Where(r => r.status == AppModels.Status.completed).Select(r => r.amount).DefaultIfEmpty(0).Sum() ?? 0;
                decimal totalliquidation = liquidations.Where(r => r.status != AppModels.Status.rejected).Select(r => r.amount).DefaultIfEmpty(0).Sum() ?? 0;
                decimal currentliquidation = Convert.ToDecimal(txtfundliquidationamount.Text);
                errortxtfundliquidationamount.Text = "Amount must not exceed for liquidation amount!";
                if ((totalreleased - totalliquidation) < currentliquidation)
                {
                    isvalid = false;
                    Validator.SetError(txtfundliquidationamount, true);
                }
                else
                    Validator.SetError(txtfundliquidationamount);

                if (literalimagefilename.Text == "")
                {
                    isvalid = false;
                    errorimagerequired.Visible = true;
                }
                else
                    errorimagerequired.Visible = false;

                if (isvalid)
                {
                    fundliquidation liq = fundController.GetFundLiquidation(0);
                    liq.warehouseid = this.warehouseId;
                    liq.moduleid = this.moduleId;
                    liq.requestid = this.requestId;
                    liq.datesubmitted = Utility.getServerDate();
                    liq.actualdate = Convert.ToDateTime(txtactualdate.Text);
                    liq.remarks = txtfundliquidationremarks.Text;
                    liq.status = AppModels.Status.submitted;
                    liq.amount = Convert.ToDecimal(txtfundliquidationamount.Text);
                    liq.receiptimagepath = literalimagefilename.Text;
                    fundController.SaveFundLiquidation(true, liq);
                    panelfundliqidationentry.Visible = false;
                    panelfundliqidationentry.CssClass = panelfundliqidationentry.CssClass.Replace(" collapsed-card", "");
                    LoadFundLiquidation();
                    pageSessionState = System.DateTime.Now.ToString();
                    ClearFundLiquidationEntry();
                    upanelfunliquidationdashboard.Update();
                }
            }
            upanelfundliquidation.Update();
        }
        protected void btnaddvendor_click(object sender, EventArgs e)
        {
            PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnopenvenmodal", true);
            upanelmodel.Update();
        }

        protected void SubmitVendor(object sender, EventArgs e)
        {
            init();
            if (Convert.ToInt32(cmbisvat.SelectedValue) > 0)
            {
                var vendObj = new VendorObject();
                vendObj.Vendor = new List<Vendor>();

                var gathered = new Vendor()
                {
                    dateencoded = DateTime.Now.ToString("MM/dd/yyyy"),
                    Vendorname = txtvendorname.Text,
                    address = txtaddress.Text,
                    telephone = txttelephone.Text,
                    vatno = txtvatno.Text,
                    vat = VendorValidation(),
                    VendorCategoryId = 1
                };

                vendObj.Vendor.Add(gathered);

                string finalResponse = JsonConvert.SerializeObject(vendObj);
                options.Upload_Data(auth.GetToken(),FFOPettyCashWS.myTransactCode.CPostVendor,finalResponse);
                PageController.fnShowAlert(this.Page, PageController.AlertType.success, "Record Successfully Updated");
                PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnopenvenmodal", true);
                webserviceresult();
            }
        }
        private bool VendorValidation()
        {
            if (Convert.ToInt32(cmbisvat.SelectedValue) == 1)
                return true;
            return false;
        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            init();
            try
            {
                if (pageSessionState == "")
                {
                    if (fureceipt.PostedFile != null && fureceipt.PostedFile.ContentLength > 0 && fureceipt.PostedFile.FileName != "")
                    {
                        literalimagefilename.Text = Utility.saveImage(this.Page, this.warehouseId, fureceipt, Utility.enfiletype.disbursementreceipt);
                        imgpreview.ImageUrl = Utility.getImage(this.warehouseId, literalimagefilename.Text, Utility.enfiletype.disbursementreceipt);
                        imgpreview.Visible = true;
                    }
                }
                else
                {
                    panelfundliqidationentry.Visible = false;
                    ClearFundLiquidationEntry();
                    LoadFundLiquidation();
                    panelfundliqidationentry.CssClass += " collapsed-card";
                }
            }
            catch (Exception)
            {

            }
        }
        private void webserviceresult()
        {
            if (auth.currentuser.employee.employeetypeid != 222)
            {
                lstvendor = JsonConvert.DeserializeObject<List<Vendor>>(options.Download_Data(auth.GetToken(), FFOPettyCashWS.myTransactCode.CGetVendor, "0"));
                supplierController.SaveVendors(lstvendor);
            }
         

            cmbvendors.DataSource = supplierController.GetLiquidationSupplier();
            cmbvendors.DataTextField = "suppliername";
            cmbvendors.DataValueField = "supplierno";
            cmbvendors.DataBind();
        }

        private void postwebserviceresult(fundrequest f) 
        {
            practivity practivity;
            tieup tieup;
            doctor doctor;
            institution institution;

            int fundtype = 0;
            int inst_id = 0;
            int doc_id = 0;

            string doc_name = "";
            string inst_name = "";


            if (moduleId == 201)
            {
                fundtype = 1;
                practivity = practivitycontroller.GetPRActivity((long)f.requestid);
                doc_id = practivity.doc_id ?? 0;
                inst_id = practivity.inst_id ?? 0;

                doctor = doctorController.GetDoctor(doc_id);
                institution = institutionController.getInstitution(inst_id);

                doc_name = doctor.doc_firstname + " " + doctor.doc_lastname;
                inst_name = institution.inst_name;

            }else if (moduleId == 202)
            {
                fundtype = 2;
                tieup = tieupcontroller.GetTieup((long)f.requestid);
                doc_id = Convert.ToInt32(tieup.doc_id);
                inst_id = Convert.ToInt32(tieup.inst_id);

                doctor = doctorController.GetDoctor(doc_id);
                institution = institutionController.getInstitution(inst_id);

                doc_name = doctor.doc_firstname + " " + doctor.doc_lastname;
                inst_name = institution.inst_name;
            }
            else if(moduleId == 203)
            {
                fundtype = 3;
                tieup = tieupcontroller.GetTieup((long)f.requestid);
                doc_id = Convert.ToInt32(tieup.doc_id);
                inst_id = Convert.ToInt32(tieup.inst_id);

                doctor = doctorController.GetDoctor(doc_id);
                institution = institutionController.getInstitution(inst_id);

                doc_name = doctor.doc_firstname + " " + doctor.doc_lastname;
                inst_name = institution.inst_name;
            }

            if (f.paymentmode == 0)
                f.disbursementref = "";

            var fundobj = new FundreleasedObject();
            fundobj.Fundreleaseds = new List<Fundreleased>();

            var gathered = new Fundreleased()
            {
                dateencoded = DateTime.Now.ToString("MM/dd/yyyy"),
                frid_ffo = f.fundrequestid,
                payeeid = doc_id,
                payee = doc_name,
                institutionid = inst_id,
                institutionname = inst_name,
                mode = Convert.ToInt32(f.paymentmode),
                tuprefno = "N/A", // default
                fundtypeid = fundtype,
                checkno = f.disbursementref,
                checkdate = Convert.ToDateTime(f.disbursementdate).ToString("MM/dd/yyyy"),
                amount = f.amount.ToString(),
                remarks = f.disbursementremarks
            };
            fundobj.Fundreleaseds.Add(gathered);
            string finalResponse = JsonConvert.SerializeObject(fundobj);
            options.Upload_Data(auth.GetToken(), FFOPettyCashWS.myTransactCode.CPostFundReleased, finalResponse);

        }

        protected void btnshowdisbursement_Click(object sender, EventArgs e)
        {
            init();
            LinkButton btn = (LinkButton)sender;
            fundrequest f = fundController.GetFundRequest(Convert.ToInt64(btn.CommandArgument));
            cmbdisburmentmode.DataSource = AppModels.Funding.PaymentMode.ToList();
            cmbdisburmentmode.DataTextField = "value";
            cmbdisburmentmode.DataValueField = "key";
            cmbdisburmentmode.DataBind();
            cmbdisburmentmode.SelectedValue = (f.paymentmode ?? 0).ToString();
            txtdisbursementdate.Text = (f.disbursementdate ?? DateTime.MaxValue).ToString(AppModels.dateformat);
            txtdisbursementreference.Text = f.disbursementref;
            txtdisbursementremarks.Text = f.disbursementremarks;
            cmbdisburmentmode.Enabled = false;
            txtdisbursementdate.ReadOnly = true;
            txtdisbursementreference.ReadOnly = true;
            txtdisbursementremarks.ReadOnly = true;
            btnsavedisbursement.Visible = false;
            upaneldisbursement.Update();
            PageController.fnFireEvent(this.Page, PageController.EventType.click, "btndisbursementmodal", true);
        }
        #endregion
    }
}