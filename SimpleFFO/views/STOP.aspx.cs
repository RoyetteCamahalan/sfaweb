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
    public partial class STOP : System.Web.UI.Page
    {
        InstitutionController institutionController;
        TieupController tieupController;
        Auth auth;

        List<tieupproduct> tieupproducts;
        #region "Vars"
        private int moduleid { get => AppModels.Modules.stop; }
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

            institutionController = new InstitutionController();
            tieupController = new TieupController();

            if (!Page.IsPostBack)
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
                }
            }
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSaveDraft);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSubmit);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnCancel);
        }

        #region "methods"
        public void LoadCombo()
        {
            cmbCompany.DataSource = institutionController.GetAll(this.warehouseid);
            cmbCompany.DataTextField = "inst_name";
            cmbCompany.DataValueField = "inst_id";
            cmbCompany.DataBind();

            cmbProductType.DataSource = AppModels.ItemTypes.getList();
            cmbProductType.DataTextField = "value";
            cmbProductType.DataValueField = "key";
            cmbProductType.DataBind();

            cmbTieUpMode.DataSource = AppModels.Tieup.TieUpMode.getList();
            cmbTieUpMode.DataTextField = "value";
            cmbTieUpMode.DataValueField = "key";
            cmbTieUpMode.DataBind();

            cmbClaimType.DataSource = AppModels.Tieup.ClaimType.getList();
            cmbClaimType.DataTextField = "value";
            cmbClaimType.DataValueField = "key";
            cmbClaimType.DataBind();
        }
        public void LoadProductbyType()
        {
            tbl_products.DataSource = tieupController.GetTieupProducts(this.tieupid, Convert.ToInt32(this.cmbProductType.SelectedValue),this.isPageView);

            tbl_products.DataKeyNames = new string[] { "tieupproductid", "product_id" };
            tbl_products.DataBind();
            InitTableHeader();
            upanelproducts.Update();
            RegisterAsyncControls();
        }
        private void InitTableHeader()
        {
            if (tbl_products.HeaderRow != null)
                tbl_products.HeaderRow.TableSection = TableRowSection.TableHeader;


            /*if (tbl_products.FooterRow != null)
                tbl_products.FooterRow.TableSection = TableRowSection.TableFooter;*/
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

            decimal discount = Convert.ToDecimal(txtDiscount.Text);
            //vars
            tieupproducts = new List<tieupproduct>();

            int tieupduration = Convert.ToInt32(txtTieupduration.Text);

            foreach (GridViewRow gvr in tbl_products.Rows)
            {
                if (gvr.RowType == DataControlRowType.DataRow)
                {
                    Label lblPrice = (Label)gvr.FindControl("lblPrice");
                    TextBox txtQuantity = (TextBox)gvr.FindControl("txtQuantity");
                    Label lblRebate = (Label)gvr.FindControl("lblRebate");
                    Label lblTotalRebate = (Label)gvr.FindControl("lblTotalRebate");
                    TextBox txtPinMoney = (TextBox)gvr.FindControl("txtPinM");
                    TextBox Text_box_Total_pinMoney = (TextBox)gvr.FindControl("txtTotalPinM");
                    Label lblSaleProjection = (Label)gvr.FindControl("lblSaleProjection");

                    tieupproduct tieupproduct = new tieupproduct
                    {
                        tieupproductid = Convert.ToInt64(tbl_products.DataKeys[gvr.RowIndex]["tieupproductid"]),
                        product_id = Convert.ToInt32(tbl_products.DataKeys[gvr.RowIndex]["product_id"]),
                        price = Convert.ToDecimal(lblPrice.Text),
                        discount = discount
                    };

                    tieupproduct.rebate = ((discount / 100) * tieupproduct.price ?? 0);
                    if (discount > 0)
                    {
                        lblRebate.Text = (tieupproduct.rebate ?? 0).ToString(AppModels.moneyformat);
                    }

                    if (decimal.TryParse(txtQuantity.Text, out _))
                    {
                        tieupproduct.monthlyqty = Convert.ToInt32(txtQuantity.Text);
                        if (chkhaspinmoney.Checked)
                        {
                            tieupproduct.pinmoney = Convert.ToDecimal(txtPinMoney.Text);
                            Text_box_Total_pinMoney.Text = ((tieupproduct.pinmoney ?? 0) * (tieupproduct.monthlyqty ?? 0)).ToString(AppModels.moneyformat);
                        }
                        else
                        {
                            tieupproduct.pinmoney = 0;
                            Text_box_Total_pinMoney.Text = "";
                        }

                        lblTotalRebate.Text = ((tieupproduct.rebate ?? 0) * (tieupproduct.monthlyqty ?? 0)).ToString(AppModels.moneyformat);
                        lblSaleProjection.Text = ((tieupproduct.price ?? 0) * (tieupproduct.monthlyqty ?? 0)).ToString(AppModels.moneyformat);
                        tieupproducts.Add(tieupproduct);
                    }
                    else
                    {
                        lblTotalRebate.Text = "";
                        lblSaleProjection.Text = "";
                        Text_box_Total_pinMoney.Text = "";
                    }

                }


            }

            textTotalProjection.Text = tieupproducts.Select(t => (t.price ?? 0) * (t.monthlyqty ?? 0)).Sum().ToString(AppModels.moneyformat);
            textNumTieup.Text = tieupduration.ToString();
            textTotalROI.Text = (Convert.ToDecimal(textTotalProjection.Text) * tieupduration).ToString(AppModels.moneyformat);
            textTotalPinM.Text = tieupproducts.Select(t => (t.pinmoney ?? 0) * (t.monthlyqty ?? 0)).Sum().ToString(AppModels.moneyformat);

            txtRebatesPerMonth.Text = tieupproducts.Select(t => (t.rebate ?? 0) * (t.monthlyqty ?? 0)).Sum().ToString(AppModels.moneyformat);
            txtNoTieup.Text = tieupduration.ToString();
            txtTotalRebatesforTieup.Text = (Convert.ToDecimal(txtRebatesPerMonth.Text) * tieupduration).ToString(AppModels.moneyformat);

            if (AsyncUpdate)
            {
                upanelproducts.Update();
                UpPnlTotalROI.Update();
                UpPnlTotalRebate.Update();
            }
        }
        #region "Validator"
        protected void cvtextTotalProjection_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (decimal.TryParse(textTotalProjection.Text, out decimal res))
            {
                if (res > 0)
                    args.IsValid = true;
            }
            else
                args.IsValid = false;
        }
        #endregion

        private void LoadRecord()
        {
            tieup tieup = tieupController.GetTieup(this.tieupid);
            cmbProductType.SelectedValue = tieup.producttype.ToString();
            this.warehouseid = tieup.warehouseid ?? 0;
            LoadCombo();
            if (tieup.inst_id > 0)
                cmbCompany.SelectedValue = tieup.inst_id.ToString();
            else
                cmbCompany.SelectedValue = tieup.doc_id.ToString();
            cmbTieUpMode.SelectedValue = tieup.tieupmode.ToString();
            txtTieupduration.Text = tieup.duration.ToString();
            dtpDate.Text = (tieup.startmonth ?? DateTime.Now).ToString(AppModels.dateformat);
            cmbClaimType.SelectedValue = tieup.dealtype.ToString();
            txtNotes.Text = tieup.notes.ToString();
            txtOtherPromoOffers.Text = tieup.otherpromooffers.ToString();
            chkhaspinmoney.Checked = tieup.haspinmoney ?? false;
            txtRebateNotes.Text = tieup.rebatenotes;
            txtDurationTo.Text = DateTime.Parse(dtpDate.Text).AddMonths(tieup.duration ?? 0).ToString(AppModels.dateformat);

            if ((tieup.status ?? 0) >= AppModels.Status.submitted)
            {
                isPageView = true;
                btnSaveDraft.Visible = false;
                btnCancel.Visible = false;
                btnSubmit.Visible = false;
                cmbCompany.Enabled = false;
                txtAvgPurhase.ReadOnly = true;
                txtCompanyNotes.ReadOnly = true;
                cmbTieUpMode.Enabled = false;
                cmbClaimType.Enabled = false;
                cmbTieUpMode.Enabled = false;
                txtNotes.ReadOnly = true;
                txtTieupduration.ReadOnly = true;

                dtpDate.ReadOnly = true;
                dtpDate.CssClass = dtpDate.CssClass.Replace(" dtpDate", "");
                txtOtherPromoOffers.ReadOnly = true;

                cmbProductType.Enabled = false;
                txtDiscount.ReadOnly = true;
                chkhaspinmoney.Enabled = false;
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

        private bool PageValidate()
        {
            bool isvalid;
            isvalid = Validator.DecOneAbove(cmbCompany);
            isvalid = Validator.DecZeroAbove(cmbTieUpMode) && isvalid;
            isvalid = Validator.DecZeroAbove(cmbClaimType) && isvalid;
            Validator.SetError(dtpDate);
            if (!DateTime.TryParse(dtpDate.Text, out _))
            {
                isvalid = false;
                Validator.SetError(dtpDate, true);
            }
            isvalid = Validator.DecZeroAbove(cmbProductType) && isvalid;
            return isvalid;
        }

        public void Save(int status)
        {
            if (PageValidate())
            {
                TableResult(false);
                tieup tieup = tieupController.GetTieup(this.tieupid);
                tieup.warehouseid = auth.warehouseid;
                tieup.tieupclass = AppModels.Tieup.TieupClass.STOP;
                tieup.producttype = Convert.ToInt32(cmbProductType.SelectedValue);
                if (tieup.tieupclass == AppModels.Tieup.TieupClass.STOP)
                {
                    tieup.inst_id = Convert.ToInt64(cmbCompany.SelectedValue);
                    tieup.doc_id = 0;
                    tieup.tieuptype = 0;
                }
                else
                {
                    tieup.inst_id = 0;
                    tieup.doc_id = Convert.ToInt64(cmbCompany.SelectedValue);
                    tieup.tieuptype = 0;
                }
                tieup.tieupmode = Convert.ToInt32(cmbTieUpMode.SelectedValue);
                tieup.duration = Convert.ToInt32(txtTieupduration.Text);
                tieup.startmonth = Convert.ToDateTime(dtpDate.Text);
                tieup.dealtype = Convert.ToInt32(cmbClaimType.SelectedValue);
                tieup.tradeoutlet = "";
                tieup.hospitalpharmacy = "";
                tieup.notes = txtNotes.Text;
                tieup.otherpromooffers = txtOtherPromoOffers.Text;
                tieup.haspinmoney = this.chkhaspinmoney.Checked;
                tieup.rebatediscount = Convert.ToDecimal(txtDiscount.Text == "" ? "0" : txtDiscount.Text);
                tieup.totalmonthlyincome = Convert.ToDecimal(textTotalProjection.Text == "" ? "0" : textTotalProjection.Text);
                tieup.totalmonthlyrebates = Convert.ToDecimal(txtRebatesPerMonth.Text == "" ? "0" : txtRebatesPerMonth.Text);
                tieup.totalmonthlypin = Convert.ToDecimal(textTotalPinM.Text == "" ? "0" : textTotalPinM.Text);
                tieup.rebatenotes = txtRebateNotes.Text;
                tieup.status = status;
                if (this.tieupproducts == null)
                    this.tieupproducts = new List<tieupproduct>();
                tieupController.SaveTieUp(tieupid==0, tieup, this.tieupproducts);
                this.Response.RedirectToRoute("dashboard");
            }
            else
            {
                PageController.fnScroll(this, "is-invalid", true,true);
            }
            upanelcompanyinfo.Update();
            upaneltieupdetails.Update();
            upanelproducts.Update();
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
                institution institution = institutionController.getInstitution(Convert.ToInt32(cmbCompany.SelectedValue));
                txtPurhaserName.Text = "Not Specified";
                //txtAvgPurhase.Text = "";
                txtNameofOwner.Text = (institution.owner ?? "") == "" ? "Not Specified" : institution.owner;
                txtContactPerson.Text = institution.contactperson;
                txtContactNo.Text = institution.contactdetails;
                txtNoofOutlets.Text = (institution.noofoutlets ?? 0).ToString();
                Validator.SetError(cmbCompany);
            }
            else
            {
                txtPurhaserName.Text = "";
                //txtAvgPurhase.Text = "";
                txtNameofOwner.Text = "";
                txtContactPerson.Text = "";
                txtContactNo.Text = "";
                txtNoofOutlets.Text = "";
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
            foreach (GridViewRow grv in tbl_products.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtQuantity = (TextBox)grv.FindControl("txtQuantity");
                        txtQuantity.ReadOnly = this.isPageView;
                }
            }
        }
    }
}