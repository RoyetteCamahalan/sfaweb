using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.Views
{
    public partial class TUP : System.Web.UI.Page
    {
        DoctorController doctorController;
        TieupController tieupController;
        Auth auth;

        List<tieupproduct> tieupproducts;
        #region "Vars"
        private int moduleid { get => AppModels.Modules.tup; }
        public bool isPageView
        {
            get { return Convert.ToBoolean(ViewState["ispageview"] ?? false); }
            set { ViewState["ispageview"] = value; }
        }
        public long tieupid
        {
            get { return Convert.ToInt64(ViewState["tieupid"]); }
            set { ViewState["tieupid"] = value; }
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

            doctorController = new DoctorController();
            tieupController = new TieupController();

            if (!IsPostBack)
            {
                if (Page.RouteData.Values.ContainsKey("id"))
                {
                    this.tieupid = Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["id"].ToString()));
                    LoadRecord();
                    LoadProductbyType();
                    TableResult();
                }
                else
                {
                    this.tieupid = 0;
                    this.warehouseid = auth.warehouseid;
                    LoadCombo();
                    LoadProductbyType();
                }
            }
            else
            {
                InitTableHeader();
            }
            RegisterAsyncControls();
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!Page.IsPostBack)
            {
                if (this.tieupid > 0 && Page.RouteData.Values.ContainsKey("action"))
                {
                    ctlFunding.Bind(this.moduleid, this.tieupid, this.warehouseid, Convert.ToDecimal(txtTotalRebatesforTieup.Text));
                }
            }
        }
        private void RegisterAsyncControls()
        {
            foreach (GridViewRow grv in tbl_products.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((TextBox)grv.FindControl("txtQuantity"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((TextBox)grv.FindControl("txtItemDiscount"));
                }
            }

            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSaveDraft);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSubmit);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnCancel);
        }
        #region "methods"
        public string getRequestStatus(int status)
        {
            return AppModels.Status.getStatus(status);
        }
        public string getRequestStatusBadge(int status)
        {
            return AppModels.Status.getBadge(status);
        }
        public void LoadCombo()
        {
            cmbCompany.DataSource = doctorController.GetDoctorsDroplst(this.warehouseid);
            cmbCompany.DataTextField = "fullname";
            cmbCompany.DataValueField = "doc_id";
            cmbCompany.DataBind();

            cmbProductType.DataSource = AppModels.ItemTypes.getList();
            cmbProductType.DataTextField = "value";
            cmbProductType.DataValueField = "key";
            cmbProductType.DataBind();

            cmbTieUpMode.DataSource = AppModels.Tieup.TieUpMode.getList(true);
            cmbTieUpMode.DataTextField = "value";
            cmbTieUpMode.DataValueField = "key";
            cmbTieUpMode.DataBind();

            cmbClaimType.DataSource = AppModels.Tieup.ClaimType.getList();
            cmbClaimType.DataTextField = "value";
            cmbClaimType.DataValueField = "key";
            cmbClaimType.DataBind();

            cmbTieupType.DataSource = AppModels.Tieup.TieupType.getList();
            cmbTieupType.DataTextField = "value";
            cmbTieupType.DataValueField = "key";
            cmbTieupType.DataBind();
        }
        public void LoadProductbyType()
        {
            tbl_products.DataSource = tieupController.GetTieupProducts(this.tieupid, Convert.ToInt32(this.cmbProductType.SelectedValue), this.isPageView);

            tbl_products.DataKeyNames = new string[] { "tieupproductid", "product_id" };
            tbl_products.DataBind();
            InitTableHeader();
            upanelproducts.Update();
            RegisterAsyncControls();
        }
        private void LoadRecord()
        {
            tieup tieup = tieupController.GetTieup(this.tieupid);
            this.warehouseid = tieup.warehouseid ?? 0;
            LoadCombo();
            cmbProductType.SelectedValue = tieup.producttype.ToString();
            if (tieup.inst_id > 0)
                cmbCompany.SelectedValue = tieup.inst_id.ToString();
            else
                cmbCompany.SelectedValue = tieup.doc_id.ToString();
            cmbTieUpMode.SelectedValue = tieup.tieupmode.ToString();
            txtTieupduration.Text = tieup.duration.ToString();
            dtpDate.Text = (tieup.startmonth ?? DateTime.Now).ToString(AppModels.dateformat);
            cmbClaimType.SelectedValue = tieup.dealtype.ToString();
            cmbTieupType.SelectedValue = tieup.tieuptype.ToString();
            txtTradeOutlet.Text = tieup.tradeoutlet;
            txtHospitalPharmacy.Text = tieup.hospitalpharmacy;
            txtNotes.Text = tieup.notes.ToString();
            txtRebateNotes.Text = tieup.rebatenotes;
            txtDurationTo.Text = DateTime.Parse(dtpDate.Text).AddMonths(tieup.duration ?? 0).ToString(AppModels.dateformat);

            if ((tieup.status ?? 0) >= AppModels.Status.submitted)
            {
                isPageView = true;
                btnSaveDraft.Visible = false;
                btnCancel.Visible = false;
                btnSubmit.Visible = false;
                cmbCompany.Enabled = false;
                txtCompanyNotes.ReadOnly = true;
                cmbTieUpMode.Enabled = false;
                cmbClaimType.Enabled = false;
                cmbTieUpMode.Enabled = false;
                cmbTieupType.Enabled = false;
                txtNotes.ReadOnly = true;
                txtTieupduration.ReadOnly = true;

                dtpDate.ReadOnly = true;
                dtpDate.CssClass = dtpDate.CssClass.Replace(" dtpDate", "");
                txtDespensingCustomer.ReadOnly = true;
                txtTradeOutlet.ReadOnly = true;
                txtHospitalPharmacy.ReadOnly = true;

                cmbProductType.Enabled = false;
                txtRebateNotes.ReadOnly = true;

                ctlStatusTrail.Bind(this.moduleid, this.tieupid, tieup.status ?? 0);
            }
            else if (tieup.status == AppModels.Status.rejected)
            {
                btnSaveDraft.Visible = false;
                btnSubmit.Text = "Re-Submit";
                ctlStatusTrail.Bind(this.moduleid, this.tieupid, tieup.status ?? 0);
            }

            cmbCompany_SelectedIndexChanged(cmbCompany, null);
        }
        private void InitTableHeader()
        {
            if (tbl_products.HeaderRow != null)
                tbl_products.HeaderRow.TableSection = TableRowSection.TableHeader;


            if (tbl_products.FooterRow != null)
                tbl_products.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        #endregion
        protected void textbox_TextChange(object sender, EventArgs e)
        {
            TableResult();
        }


        public void TableResult(bool AsyncUpdate=true)
        {
            if (tbl_products.Rows.Count == 0)
                return;
            
            //vars
            tieupproducts = new List<tieupproduct>();

            int tieupduration = Convert.ToInt32(txtTieupduration.Text);

            foreach (GridViewRow gvr in tbl_products.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    Label lblPrice = (Label)gvr.FindControl("lblPrice");
                    TextBox txtQuantity = (TextBox)gvr.FindControl("txtQuantity");
                    Label lblSaleProjection = (Label)gvr.FindControl("lblSaleProjection");
                    TextBox txtItemDiscount = (TextBox)gvr.FindControl("txtItemDiscount");
                    TextBox txtNetPrice = (TextBox)gvr.FindControl("txtNetPrice");

                    tieupproduct tieupproduct = new tieupproduct
                    {
                        tieupproductid = Convert.ToInt64(tbl_products.DataKeys[gvr.RowIndex]["tieupproductid"]),
                        product_id = Convert.ToInt32(tbl_products.DataKeys[gvr.RowIndex]["product_id"]),
                        price = Convert.ToDecimal(lblPrice.Text),
                        rebate = 0
                    };

                    if (decimal.TryParse(txtQuantity.Text, out _))
                    {
                        tieupproduct.monthlyqty = Convert.ToInt32(txtQuantity.Text);
                        lblSaleProjection.Text= ((tieupproduct.monthlyqty ?? 0) * (tieupproduct.price ?? 0)).ToString(AppModels.moneyformat);
                    }
                    else
                    {
                        lblSaleProjection.Text = "";
                    }
                    if (decimal.TryParse(txtItemDiscount.Text, out _))
                    {
                        tieupproduct.discount = Convert.ToDecimal(txtItemDiscount.Text);
                    }
                    if(tieupproduct.monthlyqty>0 && tieupproduct.discount > 0)
                    {
                        tieupproduct.rebate = (tieupproduct.price ?? 0) * (tieupproduct.monthlyqty ?? 0) * (100-(tieupproduct.discount ?? 0)) / 100;
                        txtNetPrice.Text = (tieupproduct.rebate ?? 0).ToString(AppModels.moneyformat);
                        tieupproducts.Add(tieupproduct);
                    }
                    else
                    {
                        txtNetPrice.Text = "";
                    }
                }
            }

            txtTotalMonthlySales.Text = tieupproducts.Select(t => (t.price ?? 0) * (t.monthlyqty ?? 0)).Sum().ToString(AppModels.moneyformat);
            txtTieUpDuration2.Text = tieupduration.ToString();
            textTotalROI.Text = (Convert.ToDecimal(txtTotalMonthlySales.Text) * tieupduration).ToString(AppModels.moneyformat);

            textTotalMonthlyProjection.Text = txtTotalMonthlySales.Text;
            txtNetProductRebate.Text = tieupproducts.Select(t => t.rebate ?? 0).Sum().ToString(AppModels.moneyformat);
            txtTotalMonthlyRebate.Text = (Convert.ToDecimal(txtTotalMonthlySales.Text) - Convert.ToDecimal(txtNetProductRebate.Text)).ToString(AppModels.moneyformat);
            txtTieUpDuration3.Text = tieupduration.ToString();
            txtTotalRebatesforTieup.Text = (Convert.ToDecimal(txtTotalMonthlyRebate.Text) * tieupduration).ToString(AppModels.moneyformat);

            if (AsyncUpdate)
            {
                upanelproducts.Update();
                UpPnlTotalROI.Update();
                UpPnlTotalRebate.Update();
            }
        }
        private bool PageValidate()
        {
            bool isvalid;
            isvalid = Validator.DecOneAbove(cmbCompany);
            isvalid = Validator.DecZeroAbove(cmbTieUpMode) && isvalid;
            isvalid = Validator.DecZeroAbove(cmbClaimType) && isvalid;
            isvalid = Validator.DecZeroAbove(cmbTieupType) && isvalid;
            Validator.SetError(dtpDate);
            if (!DateTime.TryParse(dtpDate.Text, out _))
            {
                isvalid = false;
                Validator.SetError(dtpDate, true);
            }
            isvalid = Validator.DecZeroAbove(cmbProductType) && isvalid;
            isvalid = Validator.DecOneAbove(txtTotalMonthlySales) && isvalid;
            return isvalid;
        }
        public void Save(int status)
        {
            if (PageValidate())
            {
                TableResult(false);

                tieup tieup = tieupController.GetTieup(this.tieupid);
                tieup.warehouseid = auth.warehouseid;
                tieup.tieupclass = AppModels.Tieup.TieupClass.TUP;
                tieup.producttype = Convert.ToInt32(cmbProductType.SelectedValue);
                tieup.inst_id = 0;
                    tieup.doc_id = Convert.ToInt64(cmbCompany.SelectedValue);
                tieup.tieupmode = Convert.ToInt32(cmbTieUpMode.SelectedValue);
                tieup.duration = Convert.ToInt32(txtTieupduration.Text);
                tieup.startmonth = Convert.ToDateTime(dtpDate.Text);
                tieup.dealtype = Convert.ToInt32(cmbClaimType.SelectedValue);
                tieup.tieuptype = Convert.ToInt32(cmbTieupType.SelectedValue);
                tieup.tradeoutlet = txtTradeOutlet.Text;
                tieup.hospitalpharmacy = txtHospitalPharmacy.Text;
                tieup.notes = txtNotes.Text;
                tieup.haspinmoney = false;
                tieup.rebatediscount = 0;
                tieup.totalmonthlyincome = Convert.ToDecimal(txtTotalMonthlySales.Text);
                tieup.totalmonthlyrebates = Convert.ToDecimal(txtTotalMonthlyRebate.Text);
                tieup.totalmonthlypin = 0;
                tieup.rebatenotes = txtRebateNotes.Text;
                tieup.status = status;
                tieupController.SaveTieUp(tieupid==0, tieup, this.tieupproducts);
                this.Response.RedirectToRoute("dashboard");
            }
            else
            {
                PageController.fnScroll(this, "is-invalid", true, true);
            }
            upanelcompanyinfo.Update();
            upaneltieupdetails.Update();
            upanelproducts.Update();
            UpPnlTotalROI.Update();
        }
        protected void dtpDate_TextChanged(object sender, EventArgs e)
        {
            TableResult();
            if (DateTime.TryParse(dtpDate.Text, out _) && Int32.TryParse(txtTieupduration.Text, out _))
            {
                txtDurationTo.Text = DateTime.Parse(dtpDate.Text).AddMonths(Int32.Parse(txtTieupduration.Text)).ToString(AppModels.dateformat);
                upaneltieupdetails.Update();
            }
        }
        protected void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadProductbyType();
        }

        protected void cmbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbCompany.SelectedValue) > 0)
            {
                doctor doctor = doctorController.GetDoctor(Convert.ToInt32(cmbCompany.SelectedValue));
                txtSpecialty.Text = doctor.specialization.name;
                txtPractice.Text = "";
                txtclinicAddress.Text = doctor.institutiondoctormaps.First().institution.inst_name;
                txtPatientType.Text = "";
                txtContactNo.Text = doctor.institutiondoctormaps.First().institution.contactdetails ?? "";
                Validator.SetError(cmbCompany);
            }
            else
            {
                txtSpecialty.Text = "";
                txtPractice.Text = "";
                txtclinicAddress.Text = "";
                txtPatientType.Text = "";
                txtContactNo.Text = "";
                Validator.SetError(cmbCompany,true);
            }
            upanelcompanyinfo.Update();

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Response.RedirectToRoute("dashboard");
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            Save(AppModels.Status.draft);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Save(AppModels.Status.submitted);
        }

        protected void chkhaspinmoney_CheckedChanged(object sender, EventArgs e)
        {
            TableResult();
        }

        protected void tbl_products_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
                txtQuantity.ReadOnly = this.isPageView;

                TextBox txtItemDiscount = (TextBox)e.Row.FindControl("txtItemDiscount");
                txtItemDiscount.ReadOnly = this.isPageView;
            }
        }
    }
}