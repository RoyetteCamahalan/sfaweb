using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using SimpleFFO.Controller;
using SimpleFFO.Model;

namespace SimpleFFO.views
{
    public partial class ExpenseReport : System.Web.UI.Page
    {
        ExpenseReportController expenseReportController;
        ApprovalController approvalController;
        FundController fundController;
        Auth auth;

        List<transportationtype> drplistTransportTypes;
        List<Exptype> lstexpensetype;



        #region "vars"
        private int moduleid { get => AppModels.Modules.expensereport; }
        public long expensereportid
        {
            get { return Convert.ToInt64(ViewState["expensereportid"]); }
            set { ViewState["expensereportid"] = value; }
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
        public long lastodo
        {
            get { return Convert.ToInt64(ViewState["lastodo"]); }
            set { ViewState["lastodo"] = value; }
        }


        public List<expensereportmiscellaneou> lstexpensemiscellaneous
        {
            get { return (List<expensereportmiscellaneou>)ViewState["Expensereportmiscellaneous"]; }
            set { ViewState["Expensereportmiscellaneous"] = value; }
        }
        public List<expensereportdetail> lstexpensereportdetail
        {
            get { return (List<expensereportdetail>)ViewState["lstexpensereportdetail"]; }
            set { ViewState["lstexpensereportdetail"] = value; }
        }
        public bool isPageValid
        {
            get { return Convert.ToBoolean(ViewState["ispagevalid"] ?? true); }
            set { ViewState["ispagevalid"] = value; }
        }
        public bool isPageView
        {
            get { return Convert.ToBoolean(ViewState["ispageview"] ?? false); }
            set { ViewState["ispageview"] = value; }
        }
        public long vehicleid
        {
            get { return Convert.ToInt64(ViewState["vehicleid"] ?? 0); }
            set { ViewState["vehicleid"] = value; }
        }
        public statustrail fundrequeststatus
        {
            get { return (statustrail)ViewState["fundrequeststatus"]; }
            set { ViewState["fundrequeststatus"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this,modcode: AppModels.Modules.expensereport);
            if (!auth.hasAuth())
                return;

            expenseReportController = new ExpenseReportController();
            approvalController = new ApprovalController();
            fundController = new FundController();
            if (!IsPostBack)
            {
                if (Page.RouteData.Values.ContainsKey("id"))
                {
                    this.expensereportid =Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["id"].ToString()));
                }
                else
                {
                    this.expensereportid = 0;
                    this.warehouseid = auth.warehouseid;
                }
                displayData();
                webserviceresult();
            }
            RegisterAsyncControls();
        }
        #region "Page Methods"
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!Page.IsPostBack)
            {
                if (this.expensereportid > 0 && Page.RouteData.Values.ContainsKey("action"))
                {
                    ctlFunding.Bind(this.moduleid, this.expensereportid, this.warehouseid, this.lstexpensereportdetail.Select(er => er.totaldaily ?? 0).DefaultIfEmpty(0).Sum());
                }
            }
        }
        #endregion

        public string getRequestStatus(int status)
        {
            return AppModels.Status.getStatus(status);
        }
        public string getRequestStatusBadge(int status)
        {
            return AppModels.Status.getBadge(status);
        }

        public void webserviceresult()
        {
            FFOPettyCashWS.Service1 options = new FFOPettyCashWS.Service1();
            if (auth.currentuser.employee.employeetypeid != 222)
            {
                lstexpensetype = JsonConvert.DeserializeObject<List<Exptype>>(options.Download_Data(auth.GetToken(), FFOPettyCashWS.myTransactCode.CGetExpenseType, "0"));
                expenseReportController.SaveReportype(lstexpensetype);
            }
        }

        public void displayData()
        {
            employee eremployee;
            if (this.expensereportid== 0)
            {
                eremployee = auth.currentuser.employee;

                companyvehicle companyvehicle = eremployee.warehouse.companyvehicle;
                if (companyvehicle != null)
                {
                    txtbox_plateno.Text = companyvehicle.platenumber;
                    this.lastodo = companyvehicle.currentodo ?? 0;
                    this.vehicleid = companyvehicle.vehicleid;
                }
                else
                {
                    txtbox_plateno.Text = "";
                    this.lastodo = 0;
                    this.vehicleid = 0;
                }

                DateTime basedate = Utility.getFirstDayofCurrentWeek();
                txtbox_startdate.Text = basedate.ToString(AppModels.dateformat);
                this.expensereportid = expenseReportController.getWeekifExist(this.warehouseid, basedate);
                if (this.expensereportid > 0)
                    Response.RedirectToRoute(AppModels.Routes.expensereportdetails, new { id = Auth.AppSecurity.URLEncrypt(this.expensereportid.ToString()) });
                List<expensereportdetail> templist = new List<expensereportdetail>();
                for (int i =0; i < 7; i++)
                {
                    templist.Add(new expensereportdetail { date = basedate.AddDays(i) });
                }
                txtbox_thrudate.Text = basedate.AddDays(6).ToString(AppModels.dateformat);
                this.lstexpensereportdetail = templist;

                List<expensereportmiscellaneou> expensemisc = new List<expensereportmiscellaneou>();
                this.lstexpensemiscellaneous = expensemisc;
            }
            else
            {
                expensereport er = expenseReportController.GetExpensereport(this.expensereportid);
                eremployee = er.warehouse.employees.FirstOrDefault();
                companyvehicle companyvehicle = eremployee.warehouse.companyvehicle;
                if (companyvehicle != null)
                    txtbox_plateno.Text = companyvehicle.platenumber;
                else
                    txtbox_plateno.Text = "";
                this.vehicleid = er.vehicleid ?? 0;
                this.lastodo = er.previousodo ?? 0;
                this.lstexpensereportdetail = er.expensereportdetails.OrderBy(erd=> erd.date ?? DateTime.MaxValue).ToList();
                txtbox_startdate.Text = (er.datefrom ?? DateTime.MaxValue).ToString(AppModels.dateformat);
                txtbox_thrudate.Text = (er.dateend ?? DateTime.MaxValue).ToString(AppModels.dateformat);
                if((er.previousodo ?? 0)>0 && this.lastodo<= (er.previousodo ?? 0))
                    this.lastodo = er.previousodo ?? this.lastodo;
                this.warehouseid = er.warehouseid ?? 0;
                this.lstexpensemiscellaneous = er.expensereportmiscellaneous.ToList();
                if ((er.status ?? 0) >= AppModels.Status.submitted)
                {
                    isPageView = true;
                    btnSaveDraft.Visible = false;
                    btnCancel.Visible = false;
                    btnSubmit.Visible = false;
                    btnAddExpenseBreak.Visible = false;

                    ctlStatusTrail.Bind(this.moduleid, this.expensereportid, er.status ?? 0);
                }
                else if (er.status == AppModels.Status.rejected)
                {
                    btnSaveDraft.Visible = false;
                    btnSubmit.Text = "Re-Submit";
                    ctlStatusTrail.Bind(this.moduleid, this.expensereportid, er.status ?? 0);
                }
            }
            txtbox_fullname.Text = eremployee.firstname + " " + eremployee.lastname;
            txtbox_role.Text = eremployee.employeetype.employeetypedescription;


            dgvDailyExpense.DataSource = lstexpensereportdetail;
            dgvDailyExpense.DataBind();
            ComputeDailyExpense();
            this.BindGrid();
            txtbox_lastodometer.Text = this.lastodo.ToString("#,0");
            ComputeExpenseBreakdown();

        }

        protected void textboxtextChange(object sender, EventArgs e)
        {
            ComputeDailyExpense();

        }
        private void ComputeDailyExpense(bool validate = false)
        {
            GridViewRow gvfooter = dgvDailyExpense.FooterRow;
            List<expensereportdetail> expensereportdetails = lstexpensereportdetail;
            long lastodo = this.lastodo;
            foreach (GridViewRow gvr in dgvDailyExpense.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtamount = (TextBox)gvr.FindControl("txtbox_amount");
                    TextBox txtmeal = (TextBox)gvr.FindControl("txtbox_meal");
                    TextBox txtmisc = (TextBox)gvr.FindControl("txtbox_miscellaneous");
                    TextBox txttotal = (TextBox)gvr.FindControl("txtbox_total");
                    TextBox txtroute = (TextBox)gvr.FindControl("txtroute");
                    TextBox txtbox_workwith = (TextBox)gvr.FindControl("txtbox_workwith");
                    DropDownList cmbmediumtranspo = (DropDownList)gvr.FindControl("cmbmediumtranspo");
                    Literal literalimagefilename = (Literal)gvr.FindControl("literalimagefilename"); 

                    TextBox txtodometer = (TextBox)gvr.FindControl("txtbox_odometer");
                    TextBox txtpersonal = (TextBox)gvr.FindControl("txtbox_personal");
                    TextBox txtbusiness = (TextBox)gvr.FindControl("txtbox_business");

                    if (Int64.TryParse(txtodometer.Text, out _))
                        expensereportdetails[gvr.RowIndex].totalkm = Convert.ToInt64(txtodometer.Text);
                    else
                        expensereportdetails[gvr.RowIndex].totalkm = 0;

                    if (Int64.TryParse(txtpersonal.Text, out _))
                        expensereportdetails[gvr.RowIndex].personal = Convert.ToInt32(txtpersonal.Text);
                    else
                        expensereportdetails[gvr.RowIndex].personal = 0;

                     expensereportdetails[gvr.RowIndex].business = (int) expensereportdetails[gvr.RowIndex].totalkm - (int)lastodo - (int) expensereportdetails[gvr.RowIndex].personal;

                    if (Decimal.TryParse(txtamount.Text, out _))
                        expensereportdetails[gvr.RowIndex].amount = Convert.ToDecimal(txtamount.Text);
                    else
                        expensereportdetails[gvr.RowIndex].amount = 0;

                    if (Decimal.TryParse(txtmeal.Text, out _))
                        expensereportdetails[gvr.RowIndex].meals = Convert.ToDecimal(txtmeal.Text);
                    else
                         expensereportdetails[gvr.RowIndex].meals = 0;

                    if (Decimal.TryParse(txtmisc.Text, out _))
                        expensereportdetails[gvr.RowIndex].miscellaneous = Convert.ToDecimal(txtmisc.Text); 
                    else
                         expensereportdetails[gvr.RowIndex].miscellaneous = 0;

                    expensereportdetails[gvr.RowIndex].totaldaily =  expensereportdetails[gvr.RowIndex].amount +  expensereportdetails[gvr.RowIndex].meals +  expensereportdetails[gvr.RowIndex].miscellaneous;

                    if (lastodo <=  expensereportdetails[gvr.RowIndex].totalkm)
                    {
                        lastodo = ( expensereportdetails[gvr.RowIndex].totalkm ?? 0);
                        txtbusiness.Text = ( expensereportdetails[gvr.RowIndex].business ?? 0).ToString(AppModels.moneyformat);
                    }
                    else
                    {
                        txtbusiness.Text = 0.ToString(AppModels.moneyformat);
                         expensereportdetails[gvr.RowIndex].business = 0;
                    }
                    if (validate)
                    {
                        if ( expensereportdetails[gvr.RowIndex].totalkm < lastodo)
                        {
                            Validator.SetError(txtodometer, true);
                            isPageValid = false;
                        }
                        else
                        {
                            Validator.SetError(txtodometer);
                        }

                        if (!Validator.RequiredField(txtroute))
                        {
                            isPageValid = false;
                        }

                    }
                    expensereportdetails[gvr.RowIndex].route = txtroute.Text;
                    expensereportdetails[gvr.RowIndex].workwith = txtbox_workwith.Text;
                    expensereportdetails[gvr.RowIndex].transpotypeid = Convert.ToInt32(cmbmediumtranspo.SelectedValue);
                    expensereportdetails[gvr.RowIndex].imgloc = literalimagefilename.Text; 
                    txttotal.Text = ( expensereportdetails[gvr.RowIndex].totaldaily ?? 0).ToString(AppModels.moneyformat);

                }
            }

            TextBox ftpersonal = (TextBox)gvfooter.FindControl("txtbox_totalPersonal");
            TextBox ftbusiness = (TextBox)gvfooter.FindControl("txtbox_totalbusiness");
            TextBox ftamount = (TextBox)gvfooter.FindControl("txtbox_totalamount");
            TextBox ftmeal = (TextBox)gvfooter.FindControl("txtbox_totalmeals");
            TextBox ftmisc = (TextBox)gvfooter.FindControl("txtbox_miscellanous");
            TextBox fttotal = (TextBox)gvfooter.FindControl("txtbox_dailytotal");
            this.lstexpensereportdetail = expensereportdetails;

            ftpersonal.Text = expensereportdetails.Select(t => (t.personal ?? 0)).Sum().ToString(AppModels.moneyformat);
            ftbusiness.Text = expensereportdetails.Select(t => (t.business ?? 0)).Sum().ToString(AppModels.moneyformat);

            ftamount.Text = expensereportdetails.Select(t => t.amount).Sum().ToString(AppModels.moneyformat);
            ftmeal.Text = expensereportdetails.Select(t => (t.meals ?? 0)).Sum().ToString(AppModels.moneyformat);
            ftmisc.Text = expensereportdetails.Select(t => t.miscellaneous ?? 0).Sum().ToString(AppModels.moneyformat);
            fttotal.Text = expensereportdetails.Select(t => t.totaldaily ?? 0).Sum().ToString(AppModels.moneyformat);
            panelmaingrid.Update();
        }
        protected void textboxtextChangeExpenseBreak(object sender, EventArgs e)
        {
            ComputeExpenseBreakdown();
        }

        private void ComputeExpenseBreakdown()
        {
            List<expensereportmiscellaneou> em = GetGridData();
            this.lstexpensemiscellaneous = em;
            GridViewRow foter = GvexpenseBreak.FooterRow;
            if (foter != null)
            {
                TextBox fttxtTotalAmount = (TextBox)foter.FindControl("txtbox_TotalamountEB");
                fttxtTotalAmount.Text = em.Select(ei => (ei.amount ?? 0)).Sum().ToString("#,0");
            }
            UpdatepnlExpenseBreak.Update();
        }

        protected void GvexpenseBreak_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (((Control)sender).ID == this.GvexpenseBreak.ID && e.CommandName == "removeitem")
            {
                List<expensereportmiscellaneou> lst = GetGridData();
                lst.RemoveAt(Convert.ToInt32(e.CommandArgument));
                if (lst.Count == 0)
                {
                    lst.Add(new expensereportmiscellaneou());
                }
                this.lstexpensemiscellaneous = lst;
                this.BindGrid();
                UpdatepnlExpenseBreak.Update();
            }



        }

        protected void GvexpenseBreak_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList dropDownLst = (DropDownList)e.Row.FindControl("combomisc");
                LinkButton btn = (LinkButton)e.Row.FindControl("btnRemoveEB");
                List<miscexpensecode> lst = expenseReportController.GetListMiscType();
                dropDownLst.DataSource = lst;
                dropDownLst.DataTextField = "misccodename";
                dropDownLst.DataValueField = "misccodeid";
                dropDownLst.DataBind();
                dropDownLst.SelectedValue = (DataBinder.Eval(e.Row.DataItem, "misccodeid") ?? 0).ToString();

                ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btn);

                if (this.isPageView)
                {
                    dropDownLst.Enabled = false;

                    LinkButton btnupload = (LinkButton)e.Row.FindControl("btnupload");
                    btnupload.Visible = !this.isPageView;

                    TextBox dtpDate = (TextBox)e.Row.FindControl("dtpDate");
                    dtpDate.ReadOnly = true;
                    dtpDate.CssClass = dtpDate.CssClass.Replace(" dtpDate","");

                    TextBox txtbox_particulars = (TextBox)e.Row.FindControl("txtbox_particulars");
                    txtbox_particulars.ReadOnly = true;

                    TextBox txtbox_amountEB = (TextBox)e.Row.FindControl("txtbox_amountEB");
                    txtbox_amountEB.ReadOnly = true;
                }

                if ((DataBinder.Eval(e.Row.DataItem, "imgloc") ?? "").ToString() != "")
                {
                    Image imgpreview = (Image)e.Row.FindControl("imgpreview");
                    imgpreview.ImageUrl = Utility.getImage(this.warehouseid, DataBinder.Eval(e.Row.DataItem, "imgloc").ToString(),Utility.enfiletype.odo);
                    imgpreview.Visible = true;
                }
            }

        }

        protected void AddExpenseClicked(object sender, EventArgs e)
        {
            List<expensereportmiscellaneou> lst = GetGridData();
            lst.Add(new expensereportmiscellaneou());
            this.lstexpensemiscellaneous = lst;
            this.BindGrid();
            RegisterAsyncControls();
        }

        public List<expensereportmiscellaneou> GetGridData()
        {
            List<expensereportmiscellaneou> lst = this.lstexpensemiscellaneous;
            for (int i = 0; i < this.GvexpenseBreak.Rows.Count; i++)
            {
                TextBox txtbox_amountEB = (TextBox)GvexpenseBreak.Rows[i].FindControl("txtbox_amountEB");
                TextBox dtpDate = (TextBox)GvexpenseBreak.Rows[i].FindControl("dtpDate");
                TextBox txtbox_particulars = (TextBox)GvexpenseBreak.Rows[i].FindControl("txtbox_particulars");
                DropDownList combomisc = (DropDownList)GvexpenseBreak.Rows[i].FindControl("combomisc");
                Literal literalimagefilename = (Literal)GvexpenseBreak.Rows[i].FindControl("literalimagefilename");
                if (Validator.DecOneAbove(txtbox_amountEB))

                    lst[i].amount = Convert.ToDecimal(txtbox_amountEB.Text);
                else
                {
                    lst[i].amount = 0;
                    this.isPageValid = false;
                }
                if (Validator.DateValid(dtpDate))
                {
                    if(Convert.ToDateTime(txtbox_startdate.Text) <= Convert.ToDateTime(dtpDate.Text) && Convert.ToDateTime(txtbox_thrudate.Text) >= Convert.ToDateTime(dtpDate.Text))
                        lst[i].expensedate = Convert.ToDateTime(dtpDate.Text);
                    else
                    {
                        lst[i].expensedate = null;
                        this.isPageValid = false;
                        Validator.SetError(dtpDate,true);
                    }
                }
                else
                {
                    lst[i].expensedate = null;
                    this.isPageValid = false;
                    Validator.SetError(dtpDate, true);
                }
                if (Validator.CharRequired(txtbox_particulars))
                {
                    lst[i].particulars = txtbox_particulars.Text;
                }
                else
                {
                    lst[i].particulars = "";
                    this.isPageValid = false;
                }
                if (Validator.DecZeroAbove(combomisc))
                {
                    lst[i].misccodeid = Convert.ToInt32(combomisc.SelectedValue);
                }
                else
                {
                    lst[i].misccodeid = 0;
                    this.isPageValid = false;
                }
                lst[i].imgloc = literalimagefilename.Text;
            }
            this.lstexpensemiscellaneous=lst;
            return lst.ToList();
        }

        private void BindGrid()
        {
            GvexpenseBreak.DataSource = this.lstexpensemiscellaneous;
            GvexpenseBreak.DataKeyNames = new string[] { "miscexpenseid" };
            GvexpenseBreak.DataBind();
        }

        private void RegisterAsyncControls()
        {
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnCancel);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSaveDraft);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSubmit);

            if (this.isPageView)
                return;

            foreach (GridViewRow grv in dgvDailyExpense.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((TextBox)grv.FindControl("txtbox_odometer"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((TextBox)grv.FindControl("txtbox_personal"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((TextBox)grv.FindControl("txtbox_amount"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((TextBox)grv.FindControl("txtbox_meal"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((TextBox)grv.FindControl("txtbox_miscellaneous"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btnupload"));
                    ScriptManager.GetCurrent(this).RegisterPostBackControl((Button)grv.FindControl("btnsaveimagemisc"));
                }
            }
            foreach (GridViewRow grv in GvexpenseBreak.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((TextBox)grv.FindControl("txtbox_amountEB"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btnupload"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)grv.FindControl("btnRemoveEB"));
                    ScriptManager.GetCurrent(this).RegisterPostBackControl((Button)grv.FindControl("btnsaveimagemisc")); 
                }
            }
        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "launchOpenDialog('" + btn.CommandArgument + "');", true);
        }

        protected void btnsaveimages_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                foreach (GridViewRow grv in dgvDailyExpense.Rows)
                {
                    if (grv.RowType == DataControlRowType.DataRow && grv.RowIndex == Convert.ToInt32(btn.CommandArgument))
                    {
                        Image img = (Image)grv.FindControl("imgpreview");
                        FileUpload fu = (FileUpload)grv.FindControl("fuodometer");
                        Literal l = (Literal)grv.FindControl("literalimagefilename");
                        if (fu.PostedFile != null && fu.PostedFile.ContentLength > 0 && fu.PostedFile.FileName != "")
                        {
                                l.Text = Utility.saveImage(this, this.warehouseid,fu,Utility.enfiletype.odo);
                                img.ImageUrl = Utility.getImage(this.warehouseid,l.Text,Utility.enfiletype.odo);
                                img.Visible = true;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        protected void btnsaveimagemisc_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;

                foreach (GridViewRow grv in GvexpenseBreak.Rows)
                {
                    if (grv.RowType == DataControlRowType.DataRow && grv.RowIndex == Convert.ToInt32(btn.CommandArgument))
                    {
                        Image img = (Image)grv.FindControl("imgpreview");
                        FileUpload fu = (FileUpload)grv.FindControl("fuodometer");
                        Literal l = (Literal)grv.FindControl("literalimagefilename");
                        if (fu.PostedFile != null && fu.PostedFile.ContentLength > 0)
                        {
                            string imgName = Utility.RandomString(15) + fu.FileName;
                            string imgPath = Utility.getFolder(this.warehouseid,Utility.enfiletype.odo);
                            Directory.CreateDirectory(Server.MapPath(imgPath));
                            imgPath = imgPath + "/" + imgName;
                            int imgSize = fu.PostedFile.ContentLength;
                            if (fu.PostedFile != null)
                            {

                                fu.SaveAs(Server.MapPath(imgPath));
                                l.Text = imgName;
                                img.ImageUrl = "~/" + imgPath;
                                img.Visible = true;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            this.isPageValid = true;
            ComputeDailyExpense();
            Save(AppModels.Status.draft);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            this.isPageValid = true;
            ComputeDailyExpense(true);
            Save(AppModels.Status.submitted);
        }
        private void Save(int status)
        {
            GetGridData();
            if (!this.isPageValid)
            {
                PageController.fnScroll(this, "is-invalid", true, true);
                return;
            }
            expensereport e = expenseReportController.GetExpensereport(this.expensereportid);
            e.status = status;
            if (this.expensereportid == 0)
            {
                e.datefrom = Convert.ToDateTime(txtbox_startdate.Text);
                e.dateend = Convert.ToDateTime(txtbox_thrudate.Text);
                e.previousodo = this.lastodo ;
                e.datefiled = DateTime.Now;
                e.warehouseid = this.warehouseid;
                e.vehicleid = this.vehicleid;
            }
            e.totalexpense = this.lstexpensereportdetail.Select(er => er.totaldaily ?? 0).DefaultIfEmpty(0).Sum();
            e.totalmisc = this.lstexpensereportdetail.Select(er => er.miscellaneous ?? 0).DefaultIfEmpty(0).Sum();
            expenseReportController.SaveExpensReport(this.expensereportid == 0, e, this.lstexpensereportdetail, this.lstexpensemiscellaneous);
            this.Response.RedirectToRoute("dashboard");
        }

        protected void dgvDailyExpense_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList selectedList = (DropDownList)e.Row.FindControl("cmbmediumtranspo");
                drplistTransportTypes = expenseReportController.GetTrasnportTypes();
                selectedList.DataSource = drplistTransportTypes;
                selectedList.DataTextField = "transpotypename";
                selectedList.DataValueField = "transpotypeid";
                selectedList.DataBind();
                selectedList.SelectedValue = (DataBinder.Eval(e.Row.DataItem, "transpotypeid") ?? 0).ToString();

                if (this.isPageView)
                {
                    selectedList.Enabled = false;

                    LinkButton btnupload = (LinkButton)e.Row.FindControl("btnupload");
                    btnupload.Visible = !this.isPageView;

                    TextBox txtroute = (TextBox)e.Row.FindControl("txtroute"); 
                    txtroute.ReadOnly = true;

                    TextBox txtbox_workwith = (TextBox)e.Row.FindControl("txtbox_workwith");
                    txtbox_workwith.ReadOnly = true;

                    TextBox txtbox_odometer = (TextBox)e.Row.FindControl("txtbox_odometer");
                    txtbox_odometer.ReadOnly = true;

                    TextBox txtbox_personal = (TextBox)e.Row.FindControl("txtbox_personal");
                    txtbox_personal.ReadOnly = true;

                    TextBox txtbox_amount = (TextBox)e.Row.FindControl("txtbox_amount");
                    txtbox_amount.ReadOnly = true;

                    TextBox txtbox_meal = (TextBox)e.Row.FindControl("txtbox_meal");
                    txtbox_meal.ReadOnly = true;

                    TextBox txtbox_miscellaneous = (TextBox)e.Row.FindControl("txtbox_miscellaneous");
                    txtbox_miscellaneous.ReadOnly = true;
                }
                if ((DataBinder.Eval(e.Row.DataItem, "imgloc") ?? "").ToString() != "")
                {
                    Image imgpreview = (Image)e.Row.FindControl("imgpreview");
                    imgpreview.ImageUrl = Utility.getImage(this.warehouseid, DataBinder.Eval(e.Row.DataItem, "imgloc").ToString(),Utility.enfiletype.odo);
                    imgpreview.Visible=true;
                }                
            }
        }
    }
}