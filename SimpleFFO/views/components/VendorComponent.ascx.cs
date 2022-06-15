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
    public partial class VendorComponent : System.Web.UI.UserControl
    {
        SupplierController supplierController;

        public event EventHandler Save;
        public long supplierID
        {
            get { return Convert.ToInt64(ViewState["supplierID"] ?? 0); }
            set { ViewState["supplierID"] = value; }
        }
        private void InitComponent()
        {
            supplierController = new SupplierController();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!this.Page.IsPostBack)
                LoadToCombo();
        }
        private void LoadToCombo()
        {
            EmployeeController employeeController = new EmployeeController();
            List<branch> lstshopbrancches = new List<branch> { new branch { branchid = -1, branchname = "--Select Branch--" }, new branch { branchid = 0, branchname = "All Branches" } };
            lstshopbrancches.AddRange(employeeController.GetActiveBranches());
            cmbsupplierbranches.DataSource = lstshopbrancches;
            cmbsupplierbranches.DataTextField = "branchname";
            cmbsupplierbranches.DataValueField = "branchid";
            cmbsupplierbranches.DataBind();
        }
        public void Create()
        {
            this.supplierID = 0;
            this.txtSupplierName.Text = "";
            this.cmbsupplierbranches.SelectedValue = "-1";
            this.chkIsactive.Checked = true;
            ClearValidations();
            PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnopenlibmodal", true);
            upanelmodal.Update();
        }
        public void Edit(long supplierID)
        {
            InitComponent();
            this.supplierID = supplierID;
            this.lblModalTitle.Text = "Update Record";
            supplier supplier = supplierController.getSupplier(this.supplierID);
            this.txtSupplierName.Text = supplier.suppliername;
            this.cmbsupplierbranches.SelectedValue = (supplier.branchid ?? 0).ToString();
            this.chkIsactive.Checked = supplier.isactive ?? true;
            ClearValidations();
            PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnopenlibmodal", true);
            upanelmodal.Update();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            InitComponent();
            if (PageValidate())
            {
                supplier supplier = supplierController.getSupplier(this.supplierID);
                supplier.suppliername = this.txtSupplierName.Text;
                supplier.isactive = this.chkIsactive.Checked;
                supplier.branchid = Convert.ToInt64(cmbsupplierbranches.SelectedValue);
                supplier.isrepairshop = true;
                if (this.supplierID == 0)
                {
                    supplier.created_at = DateTime.Now;
                    supplierController.suppliers.Add(supplier);
                }
                supplierController.SaveChanges();
                PageController.fnFireEvent(this.Page, PageController.EventType.click, "btnopenlibmodal", true);
                if (Save != null)
                    Save(this, EventArgs.Empty);
            }
            upanelmodal.Update();
        }

        private void ClearValidations()
        {
           Validator.SetError(txtSupplierName);
        }
        private bool PageValidate()
        {
            bool isvalid;
            this.errtxtSupplierName.Text = AppModels.ErrorMessage.required;
            isvalid = Validator.RequiredField(txtSupplierName);
            isvalid = Validator.DecZeroAbove(cmbsupplierbranches) && isvalid;
            if (isvalid)
            {
                if (supplierController.isExist(this.supplierID, txtSupplierName.Text))
                {
                    this.errtxtSupplierName.Text = AppModels.ErrorMessage.recordexisting;
                    Validator.SetError(txtSupplierName, true);
                    isvalid = false;
                }
                else
                    Validator.SetError(txtSupplierName);
            }
            return isvalid;
        }
    }
}