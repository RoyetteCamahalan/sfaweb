using Newtonsoft.Json;
using SimpleFFO.Controller;
using SimpleFFO.Model;
using SimpleFFO.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views.components
{
    public partial class VendorComponent : System.Web.UI.UserControl
    {
        SupplierController supplierController;
        List<Vendor> lstvendor;
        FFOPettyCashWS.Service1 options;
        Auth auth;
        MainPage mainPage;

        public event EventHandler Save;
        public long supplierID
        {
            get { return Convert.ToInt64(ViewState["supplierID"] ?? 0); }
            set { ViewState["supplierID"] = value; }
        }
        private void InitComponent()
        {
            auth = new Auth(this.Page);
            supplierController = new SupplierController();
            options = new FFOPettyCashWS.Service1();
            mainPage = new MainPage();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
                InitComponent();
        }
        private void LoadToCombo()
        {
            //    EmployeeController employeeController = new EmployeeController();
            //    List<branch> lstshopbrancches = new List<branch> { new branch { branchid = -1, branchname = "--Select Branch--" }, new branch { branchid = 0, branchname = "All Branches" } };
            //    lstshopbrancches.AddRange(employeeController.GetActiveBranches());
            //    cmbsupplierbranches.DataSource = lstshopbrancches;
            //    cmbsupplierbranches.DataTextField = "branchname";
            //    cmbsupplierbranches.DataValueField = "branchid";
            //    cmbsupplierbranches.DataBind();

        }
        public void Create()
        {
            this.supplierID = 0;
            //this.txtSupplierName.Text = "";
            //this.cmbsupplierbranches.SelectedValue = "-1";
            //this.chkIsactive.Checked = true;


            this.txtvendorname.Text = "";
            this.txtaddress.Text = "";
            this.txttelephone.Text = "";
            this.txtvatno.Text = "";

            ClearValidations();
            PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnopenlibmodal", true);
            upanelmodal.Update();
        }

        //public void Edit(long supplierID)
        //{
        //    InitComponent();
        //    this.supplierID = supplierID;
        //    this.lblModalTitle.Text = "Update Record";
        //    supplier supplier = supplierController.getSupplier(this.supplierID);
        //    this.txtSupplierName.Text = supplier.suppliername;
        //    this.cmbsupplierbranches.SelectedValue = (supplier.branchid ?? 0).ToString();
        //    //this.chkIsactive.Checked = supplier.isactive ?? true;
        //    this.chkIsactive.Checked = true;
        //    ClearValidations();
        //    PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnopenlibmodal", true);
        //    upanelmodal.Update();
        //}

        protected void btnSave_Click(object sender, EventArgs e)
        {
            InitComponent();
            //if (PageValidate())
            //{
            //supplier supplier = supplierController.getSupplier(this.supplierID);
            //supplier.suppliername = this.txtvendorname.Text;
            //supplier.isactive = this.chkIsactive.Checked;
            //supplier.isactive = 1;
            //supplier.branchid = Convert.ToInt64(cmbsupplierbranches.SelectedValue);
            //supplier.isrepairshop = true;
            //if (this.supplierID == 0)
            //{
            //    supplier.created_at = DateTime.Now;
            //    supplierController.suppliers.Add(supplier);
            //}
            //supplierController.SaveChanges();
            //PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnopenlibmodal", true);
            //if (Save != null)
            //    Save(this, EventArgs.Empty);

            if (Convert.ToInt32(cmbisvat.SelectedValue) > 0)
            {
                var vendobj = new VendorObject();
                vendobj.Vendor = new List<Vendor>();

                var gathered = new Vendor()
                {
                    dateencoded = DateTime.Now.ToString("MM/dd/yyyy"),
                    Vendorname = txtvendorname.Text,
                    address = txtaddress.Text,
                    telephone = txttelephone.Text,
                    vatno = txtvatno.Text,
                    vat = VendorValidation(),
                    VendorCategoryId = 2
                };

                vendobj.Vendor.Add(gathered);
                string finalResponse = JsonConvert.SerializeObject(vendobj);
                options.Upload_Data(auth.GetToken(), FFOPettyCashWS.myTransactCode.CPostVendor, finalResponse);

                PageController.fnShowAlert(this.Page, PageController.AlertType.success, "Record Successfully Updated");
                PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnopenlibmodal", true);
                webserviceresult();
                if (Save != null)
                    Save(this, EventArgs.Empty);
            }
            //}
            upanelmodal.Update();
        }
        private bool VendorValidation()
        {
            if (Convert.ToInt32(cmbisvat.SelectedValue) == 1)
                return true;
            return false;
        }

        private void ClearValidations()
        {
            Validator.SetError(txtvendorname);
        }

        //private bool PageValidate()
        //{
        //    bool isvalid;
        //    this.txtvendorname.Text = AppModels.ErrorMessage.required;
        //    isvalid = Validator.RequiredField(txtvendorname);
        //    //isvalid = Validator.DecZeroAbove(cmbsupplierbranches) && isvalid;
        //    if (isvalid)
        //    {
        //        if (supplierController.isExist(this.supplierID, txtvendorname.Text))
        //        {
        //            this.lblerrorvendorname.Text = AppModels.ErrorMessage.recordexisting;
        //            Validator.SetError(txtvendorname, true);
        //            isvalid = false;
        //        }
        //        else
        //            Validator.SetError(txtvendorname);
        //    }
        //    return isvalid;
        //}

        private void webserviceresult()
        {
            if (auth.currentuser.employee.employeetypeid != 222)
            {
                lstvendor = JsonConvert.DeserializeObject<List<Vendor>>(options.Download_Data(auth.GetToken(), FFOPettyCashWS.myTransactCode.CGetVendor, "0"));
                supplierController.SaveVendors(lstvendor);
            }

        }
    }
}