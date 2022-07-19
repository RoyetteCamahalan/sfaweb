using Newtonsoft.Json;
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
    public partial class MainPage : System.Web.UI.Page
    {
        InstitutionController institutionController;
        SupplierController supplierController;
        ProductController productController;
        VehicleController vehicleController;
        EmployeeController employeeController;
        Auth auth;
        List<Vendor> lstvendor;
        FFOPettyCashWS.Service1 options;

        public string myPage
        {
            get => (ViewState["myPage"] ?? "").ToString();
            set => ViewState["myPage"] = value;
        }
        public object currentid
        {
            get => ViewState["currentid"] ?? "0";
            set => ViewState["currentid"] = value;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this);
            if (!auth.hasAuth())
                return;

            institutionController = new InstitutionController();
            supplierController = new SupplierController();
            productController = new ProductController();
            vehicleController = new VehicleController();
            employeeController = new EmployeeController();
            options = new FFOPettyCashWS.Service1();
            if (!Page.IsPostBack)
            {
                myPage = (string)this.RouteData.Values["targetpage"];
                LoadtoCombo();
                LoadFilters();
                DisplayList();
                ImplementPrivileges();
                webserviceresult();
            }
            else
            {
                ShowTableHeaders();
            }
            RegisterAsyncControls();
        }

        private void RegisterAsyncControls()
        {
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnCreateNew);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(cmbfilterbranch);
        }
        private void ImplementPrivileges()
        {
            bool hasaccess = true;
            userpriv up;
            switch (this.myPage)
            {
                case AppModels.Pages.pageEmployees:
                    up = auth.GetUserpriv(AppModels.Modules.employees);
                    panelbtncreate.Visible = up.canadd ?? false;
                    tbl_employees.Columns[5].Visible = up.canedit ?? false;
                    hasaccess = (up.canadd ?? false) || (up.canedit ?? false);
                    break;
                case AppModels.Pages.pageEmployeeTypes:
                    up = auth.GetUserpriv(AppModels.Modules.employeetypes);
                    panelbtncreate.Visible = up.canadd ?? false;
                    tbl_employeetypes.Columns[3].Visible = up.canedit ?? false;
                    hasaccess = (up.canadd ?? false) || (up.canedit ?? false);
                    break;
                case AppModels.Pages.pageUsers:
                    up = auth.GetUserpriv(AppModels.Modules.users);
                    panelbtncreate.Visible = (up.canadd ?? false) || (auth.currentuser.isappsysadmin ?? false);
                    tbl_test.Columns[4].Visible = (up.canedit ?? false) || (auth.currentuser.isappsysadmin ?? false);
                    hasaccess = (up.canadd ?? false) || (up.canedit ?? false) || (auth.currentuser.isappsysadmin ?? false);
                    break;
                case AppModels.Pages.pageWarehouses:
                    up = auth.GetUserpriv(AppModels.Modules.warehouses);
                    panelbtncreate.Visible = up.canadd ?? false;
                    tbl_warehouses.Columns[5].Visible = up.canedit ?? false;
                    hasaccess = (up.canadd ?? false) || (up.canedit ?? false);
                    break;
                case AppModels.Pages.pagesRepairShops:
                    up = auth.GetUserpriv(AppModels.Modules.repairshops);
                    //panelbtncreate.Visible = up.canadd ?? false;
                    //tbl_suppliers.Columns[4].Visible = up.canedit ?? false;
                    //tbl_suppliers.Columns[3].Visible = up.canedit ?? false;

                    hasaccess = (up.canadd ?? false) || (up.canedit ?? false);
                    break;
                case AppModels.Pages.pageProducts:
                    up = auth.GetUserpriv(AppModels.Modules.products);
                    panelbtncreate.Visible = up.canadd ?? false;
                    tbl_products.Columns[4].Visible = up.canedit ?? false;
                    hasaccess = (up.canadd ?? false) || (up.canedit ?? false);
                    break;
                case AppModels.Pages.pageProductCategories:
                    up = auth.GetUserpriv(AppModels.Modules.prodcuctcategories);
                    panelbtncreate.Visible = up.canadd ?? false;
                    tbl_productcategories.Columns[2].Visible = up.canedit ?? false;
                    hasaccess = (up.canadd ?? false) || (up.canedit ?? false);
                    break;
                case AppModels.Pages.pageVehicles:
                    up = auth.GetUserpriv(AppModels.Modules.companyvehicles);
                    panelbtncreate.Visible = up.canadd ?? false;
                    tbl_vehicles.Columns[4].Visible = up.canedit ?? false;
                    tbl_vehicles.Columns[5].Visible = up.canedit ?? false;
                    hasaccess = (up.canadd ?? false) || (up.canedit ?? false);
                    break;
                default:

                    break;
            }
            if (!hasaccess)
                Response.RedirectToRoute(AppModels.Routes.pagenotfound);
        }

        protected void btnsimplepagenumber_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
        //    if (btn.CommandArgument != "-3")
        //    {
        //        this.pageIndex = Convert.ToInt32(btn.CommandArgument);
        //        this.Bind();
        //    }
        }
        public void DisplayList()
        {
            switch (this.myPage)
            {
                case AppModels.Pages.pageInstitutions:

                    List<GenericObject> objs = new List<GenericObject>();
                    objs.Add(new GenericObject { id = 1, name = "Apple" });
                    objs.Add(new GenericObject { id = 2, name = "Avocado" });
                    objs.Add(new GenericObject { id = 3, name = "Banana" });
                    objs.Add(new GenericObject { id = 4, name = "Cherry" });
                    objs.Add(new GenericObject { id = 5, name = "Cranberry" });
                    objs.Add(new GenericObject { id = 6, name = "Grapes" });
                    objs.Add(new GenericObject { id = 7, name = "Kiwi" });
                    objs.Add(new GenericObject { id = 8, name = "Mango" });
                    objs.Add(new GenericObject { id = 9, name = "Orange" });
                    objs.Add(new GenericObject { id = 10, name = "Watermelon" });
                    objs.Add(new GenericObject { id = 11, name = "Apple" });
                    objs.Add(new GenericObject { id = 12, name = "Apple" });

                    lstitems.Bind(objs);

                    lst_institutions.Bind(institutionController.GetAll(auth.warehouseid));
                    

                    //lst_institutions.Bind(institutionController.GetAll(auth.warehouseid));
                    break;
                case AppModels.Pages.pagesRepairShops:
                    tbl_suppliers.DataSource = supplierController.GetRepairShops(Convert.ToInt64(cmbfilterbranch.SelectedValue),true);
                    tbl_suppliers.DataKeyNames = new string[] { "supplierno" };
                    tbl_suppliers.DataBind();
                    break;
                case AppModels.Pages.pageProducts:
                    tbl_products.DataSource = productController.GetProductList();
                    tbl_products.DataKeyNames = new string[] { "product_id" };
                    tbl_products.DataBind();
                    break;
                case AppModels.Pages.pageProductCategories:
                    tbl_productcategories.DataSource = productController.GetItemCategories();
                    tbl_productcategories.DataKeyNames = new string[] { "itemcatcode" };
                    tbl_productcategories.DataBind();
                    break;
                case AppModels.Pages.pageVehicles:
                    tbl_vehicles.DataSource = vehicleController.GetCompanyVehicles();
                    tbl_vehicles.DataKeyNames = new string[] { "vehicleid" };
                    tbl_vehicles.DataBind();
                    break;
                case AppModels.Pages.pageEmployeeTypes:
                    tbl_employeetypes.DataSource = employeeController.GetEmployeetypes();
                    tbl_employeetypes.DataKeyNames = new string[] { "employeetypeid" };
                    tbl_employeetypes.DataBind();
                    break;
                case AppModels.Pages.pageWarehouses:
                    tbl_warehouses.DataSource = employeeController.GetWarehouses(Convert.ToInt64(cmbfilterbranch.SelectedValue));
                    tbl_warehouses.DataKeyNames = new string[] { "warehouseid" };
                    tbl_warehouses.DataBind();
                    break;
                case AppModels.Pages.pageEmployees:
                    tbl_employees.DataSource = employeeController.GetBranchEmployees(Convert.ToInt64(cmbfilterbranch.SelectedValue));
                    tbl_employees.DataKeyNames = new string[] { "employeeid" };
                    tbl_employees.DataBind();
                    break;
                case AppModels.Pages.pageUsers:
                    tbl_test.DataSource = employeeController.GetUseraccounts(Convert.ToInt64(cmbfilterbranch.SelectedValue));
                    tbl_test.DataKeyNames = new string[] { "useraccountid" };
                    tbl_test.DataBind();
                    break;
                default:

                    break;
            }
            ShowTableHeaders();
        }
        private void ShowTableHeaders()
        {
            switch (this.myPage)
            {
                case AppModels.Pages.pagesRepairShops:
                    tbl_suppliers.HeaderRow.TableSection = TableRowSection.TableHeader;
                    break;
                case AppModels.Pages.pageProducts:
                    tbl_products.HeaderRow.TableSection = TableRowSection.TableHeader;
                    break;
                case AppModels.Pages.pageProductCategories:
                    tbl_productcategories.HeaderRow.TableSection = TableRowSection.TableHeader;
                    break;
                case AppModels.Pages.pageVehicles:
                    tbl_vehicles.HeaderRow.TableSection = TableRowSection.TableHeader;
                    break;
                case AppModels.Pages.pageEmployeeTypes:
                    tbl_employeetypes.HeaderRow.TableSection = TableRowSection.TableHeader;
                    break;
                case AppModels.Pages.pageWarehouses:
                    tbl_warehouses.HeaderRow.TableSection = TableRowSection.TableHeader;
                    break;
                case AppModels.Pages.pageEmployees:
                    tbl_employees.HeaderRow.TableSection = TableRowSection.TableHeader;
                    break;
                case AppModels.Pages.pageUsers:
                    tbl_test.HeaderRow.TableSection = TableRowSection.TableHeader;
                    break;
                default:

                    break;
            }
        }
        private void LoadtoCombo()
        {
            switch (this.myPage)
            {
                case AppModels.Pages.pageInstitutions:
                    cmbIntitutionType.DataSource = institutionController.GetInstitutionTypes();
                    cmbIntitutionType.DataTextField = "institutiontypename";
                    cmbIntitutionType.DataValueField = "insttypeid";
                    cmbIntitutionType.DataBind();
                    break;
                case AppModels.Pages.pageProducts:
                    List<Object> lstItemType = new List<object>
                    {
                        new { itemtypeid = 0, itemtypename = "House Brand" },
                        new { itemtypeid = 1, itemtypename = "Other House Brand" }

                    };
                    cmbItemType.DataSource = lstItemType;
                    cmbItemType.DataTextField = "itemtypename";
                    cmbItemType.DataValueField = "itemtypeid";
                    cmbItemType.DataBind();

                    List<itemcategory> itemcategories = productController.GetItemCategories();
                    cmbItemCategory.DataSource = itemcategories;
                    cmbItemCategory.DataTextField = "itemcatdescription";
                    cmbItemCategory.DataValueField = "itemcatcode";
                    cmbItemCategory.DataBind();

                    List<packaging> packagings = productController.GetPackagings();
                    cmbPackaging.DataSource = packagings;
                    cmbPackaging.DataTextField = "description";
                    cmbPackaging.DataValueField = "code";
                    cmbPackaging.DataBind();
                    break;
                case AppModels.Pages.pageVehicles:
                    cmbAssignedTo.DataSource = vehicleController.getActiveWarehouses();
                    cmbAssignedTo.DataTextField = "fullname";
                    cmbAssignedTo.DataValueField = "warehouseid";
                    cmbAssignedTo.DataBind();
                    break;
                case AppModels.Pages.pageWarehouses:
                    cmbwarehousebranches.DataSource = employeeController.GetActiveBranches();
                    cmbwarehousebranches.DataTextField = "branchname";
                    cmbwarehousebranches.DataValueField = "branchid";
                    cmbwarehousebranches.DataBind();
                    break;
                case AppModels.Pages.pageEmployees:
                    cmbempbranches.DataSource = employeeController.GetActiveBranches();
                    cmbempbranches.DataTextField = "branchname";
                    cmbempbranches.DataValueField = "branchid";
                    cmbempbranches.DataBind();
                    cmbemptype.DataSource = employeeController.GetActiveEmployeetypes();
                    cmbemptype.DataTextField = "employeetypedescription";
                    cmbemptype.DataValueField = "employeetypeid";
                    cmbemptype.DataBind();
                    break;
                default:

                    break;
            }
        }
        private void LoadFilters()
        {
            panelfilters.Visible = false;
            switch (this.myPage)
            {
                case AppModels.Pages.pagesRepairShops:
                    cmbfilterbranch.DataSource = employeeController.GetActiveBranches();
                    cmbfilterbranch.DataTextField = "branchname";
                    cmbfilterbranch.DataValueField = "branchid";
                    cmbfilterbranch.DataBind();
                    panelfilters.Visible = true;
                    break;
                case AppModels.Pages.pageWarehouses:
                case AppModels.Pages.pageEmployees:
                case AppModels.Pages.pageUsers:
                    cmbfilterbranch.DataSource = employeeController.GetActiveBranches();
                    cmbfilterbranch.DataTextField = "branchname";
                    cmbfilterbranch.DataValueField = "branchid";
                    cmbfilterbranch.DataBind();
                    panelfilters.Visible = true;
                    break;
            }
        }
        #region "Events"

        private bool ClearValidations()
        {
            //use this validation to avoid postbacks
            bool isvalid = true;
            switch (this.myPage)
            {
                case AppModels.Pages.pageInstitutions:
                    Validator.SetError(txtInstitutionName);
                    Validator.SetError(cmbIntitutionType);
                    break;
                case AppModels.Pages.pageProducts:
                    Validator.SetError(txtProductName);
                    Validator.SetError(cmbItemType);
                    Validator.SetError(cmbItemCategory);
                    Validator.SetError(cmbPackaging);
                    Validator.SetError(txtPrice);
                    Validator.SetError(txtPinMoney);
                    break;
                case AppModels.Pages.pageProductCategories:
                    Validator.SetError(txtItemCatDesc);
                    break;
                case AppModels.Pages.pageVehicles:
                    Validator.SetError(txtvehiclename);
                    Validator.SetError(txtvehicleyear);
                    Validator.SetError(txtvehiclemodel);
                    Validator.SetError(txtplateno);
                    Validator.SetError(txtcurrentodo);
                    break;
                case AppModels.Pages.pageEmployeeTypes:
                    Validator.SetError(txtemployeetypeid);
                    Validator.SetError(txtemployeetypename);
                    break;
                case AppModels.Pages.pageWarehouses:
                    Validator.SetError(txtwarehouseid);
                    Validator.SetError(cmbwarehousebranches);
                    Validator.SetError(txtwarehousecode);
                    Validator.SetError(txtwarehousename);
                    break;
                case AppModels.Pages.pageEmployees:
                    Validator.SetError(txtemplastname);
                    Validator.SetError(txtempfirstname);
                    Validator.SetError(cmbempbranches);
                    Validator.SetError(cmbemptype);
                    break;
                case AppModels.Pages.pageUsers:
                    Validator.SetError(cmbuseremployee);
                    Validator.SetError(txtuseruname);
                    Validator.SetError(txtuserpassword);
                    break;
                default:

                    break;
            }

            return isvalid;
        }
        private bool PageValidate()
        {
            //use this validation to avoid postbacks
            bool isvalid = true;
            switch (this.myPage)
            {
                case AppModels.Pages.pageInstitutions:
                    this.errtxtInstitutionName.Text = AppModels.ErrorMessage.required;
                    isvalid = Validator.RequiredField(txtInstitutionName);
                    if (isvalid)
                    {
                        if (institutionController.isExist(Convert.ToInt32(this.currentid), txtInstitutionName.Text))
                        {
                            this.errtxtInstitutionName.Text = AppModels.ErrorMessage.recordexisting;
                            Validator.SetError(txtInstitutionName, true);
                            isvalid = false;
                        }
                        else
                            Validator.SetError(txtInstitutionName);
                    }
                    isvalid = Validator.DecOneAbove(cmbIntitutionType) && isvalid;
                    break;
                case AppModels.Pages.pageProducts:
                    this.errtxtProductName.Text = AppModels.ErrorMessage.required;
                    isvalid = Validator.RequiredField(txtProductName);
                    if (isvalid)
                    {
                        if (productController.isExist(Convert.ToInt32(this.currentid), txtProductName.Text))
                        {
                            this.errtxtProductName.Text = AppModels.ErrorMessage.recordexisting;
                            Validator.SetError(txtProductName, true);
                            isvalid = false;
                        }
                        else
                            Validator.SetError(txtProductName);
                    }
                    isvalid = Validator.CharRequired(cmbItemType) && isvalid;
                    isvalid = Validator.DecOneAbove(cmbItemCategory) && isvalid;
                    isvalid = Validator.DecOneAbove(cmbPackaging) && isvalid;
                    isvalid = Validator.DecZeroAbove(txtPrice) && isvalid;
                    isvalid = Validator.DecZeroAbove(txtPinMoney) && isvalid;
                    break;
                case AppModels.Pages.pageProductCategories:
                    this.errtxtItemCatDesc.Text = AppModels.ErrorMessage.required;
                    isvalid = Validator.RequiredField(txtItemCatDesc);
                    if (isvalid)
                    {
                        if (productController.isCategoryExist(this.currentid.ToString(), txtItemCatDesc.Text))
                        {
                            this.errtxtItemCatDesc.Text = AppModels.ErrorMessage.recordexisting;
                            Validator.SetError(txtItemCatDesc, true);
                            isvalid = false;
                        }
                        else
                            Validator.SetError(txtItemCatDesc);
                    }
                    break;
                case AppModels.Pages.pageVehicles:
                    isvalid = Validator.RequiredField(txtvehiclename);
                    isvalid = Validator.RequiredField(txtvehicleyear) && isvalid;
                    isvalid = Validator.RequiredField(txtvehiclemodel) && isvalid;
                    isvalid = Validator.RequiredField(txtplateno) && isvalid;
                    isvalid = Validator.DecZeroAbove(txtcurrentodo) && isvalid;
                    break;
                case AppModels.Pages.pageEmployeeTypes:
                    isvalid = Validator.RequiredField(txtemployeetypename) && isvalid;
                    if (Convert.ToInt32(this.currentid) == 0)
                    {
                        errortxtemployeetypeid.Text = AppModels.ErrorMessage.oneabove;
                        if (Validator.DecOneAbove(txtemployeetypeid))
                        {
                            employeetype e = employeeController.GetEmployeetype(Convert.ToInt32(txtemployeetypeid.Text));
                            if (e != null)
                            {
                                isvalid = false;
                                errortxtemployeetypeid.Text = AppModels.ErrorMessage.recordexisting;
                                Validator.SetError(txtemployeetypeid, true);
                            }
                        }
                        else
                        {
                            isvalid = false;
                        }
                    }
                    break;
                case AppModels.Pages.pageWarehouses:
                    isvalid = Validator.RequiredField(txtwarehousecode) && isvalid;
                    isvalid = Validator.DecOneAbove(cmbwarehousebranches) && isvalid;
                    isvalid = Validator.RequiredField(txtwarehousename) && isvalid;
                    if (Convert.ToInt32(this.currentid) == 0)
                    {
                        errortxtwarehouseid.Text = AppModels.ErrorMessage.oneabove;
                        if (Validator.DecOneAbove(txtwarehouseid))
                        {
                            warehouse e = employeeController.GetWarehouse(Convert.ToInt32(txtwarehouseid.Text));
                            if (e != null)
                            {
                                isvalid = false;
                                errortxtwarehouseid.Text = AppModels.ErrorMessage.recordexisting;
                                Validator.SetError(txtwarehouseid, true);
                            }
                        }
                        else
                        {
                            isvalid = false;
                        }
                    }
                    break;
                case AppModels.Pages.pageEmployees:
                    isvalid = Validator.RequiredField(txtemplastname) && isvalid;
                    isvalid = Validator.RequiredField(txtempfirstname) && isvalid;
                    isvalid = Validator.DecOneAbove(cmbempbranches) && isvalid;
                    isvalid = Validator.DecOneAbove(cmbemptype) && isvalid;
                    break;
                case AppModels.Pages.pageUsers:
                    if(Convert.ToInt64(this.currentid)==0)
                        isvalid = Validator.DecOneAbove(cmbuseremployee) && isvalid;
                    errtxtuseruname.Text = AppModels.ErrorMessage.required;
                    isvalid = Validator.RequiredField(txtuseruname) && isvalid;
                    isvalid = Validator.RequiredField(txtuserpassword) && isvalid;
                    if (isvalid)
                    {
                        if (employeeController.IsUserNameExist(txtuseruname.Text, Convert.ToInt64(this.currentid)))
                        {
                            errtxtuseruname.Text = "*Username already in use";
                            Validator.SetError(txtuseruname, true);
                            isvalid = false;
                        }
                    }
                    break;
                default:

                    break;
            }

            return isvalid;
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.currentid.ToString() == "-1")
            {
                return;
            }
            if (!PageValidate())
            {
                upanelmodal.Update();
                return;
            }

            switch (this.myPage)
            {
                case AppModels.Pages.pageInstitutions:
                    institution institution;
                    institution = institutionController.getInstitution(Convert.ToInt32(this.currentid));
                    institution.inst_name = this.txtInstitutionName.Text;
                    institution.inst_location = this.txtLocation.Text;
                    institution.isactive = this.chkIsactive.Checked;
                    institution.insttypeid = Convert.ToInt32(this.cmbIntitutionType.SelectedValue);
                    if (Convert.ToInt32(this.currentid) == 0)
                    {
                        institutionController.institutions.Add(institution);
                    }
                    institutionController.SaveChanges();
                    break;
                case AppModels.Pages.pageProducts:
                    product product = productController.getProduct(Convert.ToInt64(this.currentid));
                    product.name = this.txtProductName.Text;
                    product.itemtypeid = Convert.ToInt32(this.cmbItemType.SelectedValue);
                    product.itemcatcode = this.cmbItemCategory.SelectedValue;
                    product.packagingcode = Convert.ToInt32(this.cmbPackaging.SelectedValue);
                    product.price = Convert.ToDecimal(this.txtPrice.Text);
                    product.pinmoney = Convert.ToDecimal(this.txtPinMoney.Text);
                    if (Convert.ToInt32(this.currentid) == 0)
                    {
                        productController.products.Add(product);
                    }
                    productController.SaveChanges();
                    break;
                case AppModels.Pages.pageProductCategories:
                    itemcategory itemcategory = productController.getProductCategory(this.currentid.ToString());
                    itemcategory.itemcatdescription = this.txtItemCatDesc.Text;
                    if (Convert.ToInt32(this.currentid) == 0)
                    {
                        productController.itemcategories.Add(itemcategory);
                    }
                    productController.SaveChanges();
                    break;
                case AppModels.Pages.pageVehicles:
                    companyvehicle companyvehicle = vehicleController.GetCompanyVehicle(Convert.ToInt64(this.currentid));
                    companyvehicle.vehiclename = this.txtvehiclename.Text;
                    companyvehicle.year = this.txtvehicleyear.Text;
                    companyvehicle.model = this.txtvehiclemodel.Text;
                    companyvehicle.platenumber = this.txtplateno.Text;
                    if (this.txtcurrentodo.Text != "")
                        companyvehicle.currentodo = Convert.ToInt64(this.txtcurrentodo.Text);
                    if (Convert.ToInt32(this.currentid) == 0)
                        vehicleController.companyvehicles.Add(companyvehicle);
                    vehicleController.SaveChanges();
                    if (companyvehicle.vehicleid > 0)
                    {
                        long selectedwarehouseid = Convert.ToInt64(this.cmbAssignedTo.SelectedValue);
                        warehouse oldwarehouse = companyvehicle.warehouses.FirstOrDefault();
                        if (oldwarehouse == null && selectedwarehouseid > 0)
                        {
                            warehouse w = vehicleController.getWarehouse(selectedwarehouseid);
                            w.assignedvehicle = companyvehicle.vehicleid;
                            vehicleController.SaveChanges();
                        }
                        else if (oldwarehouse != null && oldwarehouse.assignedvehicle != selectedwarehouseid)
                        {
                            if (selectedwarehouseid > 0)
                            {
                                warehouse w = vehicleController.getWarehouse(selectedwarehouseid);
                                w.assignedvehicle = companyvehicle.vehicleid;
                            }
                            oldwarehouse.assignedvehicle = null;
                            vehicleController.SaveChanges();
                        }
                    }
                    break;
                case AppModels.Pages.pageEmployeeTypes:
                    employeetype employeetype = employeeController.GetEmployeetype(Convert.ToInt32(this.currentid));
                    employeetype.employeetypedescription = this.txtemployeetypename.Text;
                    employeetype.isactive = this.chkIsactive.Checked;
                    if (Convert.ToInt32(this.currentid) == 0)
                    {
                        employeetype.employeetypeid = Convert.ToInt32(txtemployeetypeid.Text);
                        employeetype.createdbyid = auth.currentuser.employeeid;
                        employeetype.datecreated = Utility.getServerDate();
                        employeeController.employeetypes.Add(employeetype);
                    }
                    else
                    {
                        employeetype.updatedbyid = auth.currentuser.employeeid;
                        employeetype.dateupdated = Utility.getServerDate();
                    }
                    employeeController.SaveChanges();
                    break;
                case AppModels.Pages.pageWarehouses:
                    warehouse warehouse = employeeController.GetWarehouse(Convert.ToInt32(this.currentid));
                    warehouse.warehousecode = this.txtwarehousecode.Text;
                    warehouse.warehousedescription = this.txtwarehousename.Text;
                    warehouse.isactive = this.chkIsactive.Checked;
                    if (Convert.ToInt32(this.currentid) == 0)
                    {
                        warehouse.warehouseid = Convert.ToInt32(txtwarehouseid.Text);
                        warehouse.branchid = Convert.ToInt64(cmbwarehousebranches.SelectedValue);
                        warehouse.createdbyid = auth.currentuser.employeeid;
                        warehouse.datecreated = Utility.getServerDate();
                        employeeController.warehouses.Add(warehouse);
                    }
                    else
                    {
                        warehouse.updatedbyid = auth.currentuser.employeeid;
                        warehouse.dateupdated = Utility.getServerDate();
                    }
                    employeeController.SaveChanges();
                    break;
                case AppModels.Pages.pageEmployees:
                    employee employee = employeeController.GetEmployee(Convert.ToInt64(this.currentid));
                    if (employee == null)
                    {
                        long tempid = employeeController.GetMaxEmployeeNo(Convert.ToInt64(this.cmbempbranches.SelectedValue));
                        if (tempid == 0)
                            tempid = Convert.ToInt64(this.cmbempbranches.SelectedValue + "000000");
                        tempid++;
                        employee = new employee
                        {
                            employeeid = tempid,
                            employeecode = tempid.ToString(),
                            createdbyid = auth.currentuser.employeeid,
                            datecreated = Utility.getServerDate()
                        };
                        employeeController.employees.Add(employee);
                    }
                    else
                    {
                        employee.updatedbyid = auth.currentuser.employeeid;
                        employee.dateupdated = Utility.getServerDate();
                    }
                    employee.lastname = this.txtemplastname.Text;
                    employee.firstname = this.txtempfirstname.Text;
                    employee.middlename = this.txtempmiddlename.Text;
                    employee.address = this.txtempaddress.Text;
                    employee.contactno = this.txtempcontact.Text;
                    employee.email = this.txtempemail.Text;
                    employee.gender = this.cmbempgender.SelectedValue;
                    employee.branchid = Convert.ToInt64(this.cmbempbranches.SelectedValue);
                    employee.warehouseid = Convert.ToInt64(this.cmbempwarehouse.SelectedValue);
                    employee.districtmanagerid = Convert.ToInt64(this.cmbempdm.SelectedValue);
                    employee.employeetypeid = Convert.ToInt32(this.cmbemptype.SelectedValue);
                    employee.isactive = this.chkIsactive.Checked;
                    employeeController.SaveChanges();
                    break;
                case AppModels.Pages.pageUsers:
                    useraccount u = employeeController.GetUseraccount(Convert.ToInt64(this.currentid));
                    if (u == null)
                    {
                        employee ue = employeeController.GetEmployee(Convert.ToInt64(cmbuseremployee.SelectedValue));
                        long tempuac = Convert.ToInt64(ue.branchid.ToString() + "00" + ue.warehouseid);
                        useraccount tempu = employeeController.GetUseraccount(tempuac);
                        if (tempu != null || ue.warehouseid==0)
                            tempuac = employeeController.GetMaxUserAccountID(ue.branchid ?? 0) + 1;

                        u = new useraccount
                        {
                            useraccountid = tempuac,
                            employeeid = ue.employeeid,
                            createdbyid = (int)auth.currentuser.employeeid,
                            datecreated = Utility.getServerDate()

                        };
                        employeeController.useraccounts.Add(u);
                    }
                    else
                    {
                        u.updatedbyid = (int)auth.currentuser.employeeid;
                        u.dateupdated = Utility.getServerDate();
                    }
                    u.username = txtuseruname.Text;
                    u.userpass = txtuserpassword.Text;
                    u.isactive = this.chkIsactive.Checked;
                    u.isappsysadmin = this.chkisadmin.Checked;
                    employeeController.SaveChanges();
                    break;
                default:

                    break;
            }
            DisplayList();
            upanelmaingrid.Update();
            PageController.fnFireEvent(this, PageController.EventType.click, "btnopenlibmodal", true);
            this.currentid = "-1";
            upanelmodal.Update();
        }
        protected void btnCreateNew_Click(object sender, EventArgs e)
        {
            if (this.myPage == AppModels.Pages.pagesRepairShops)
            {
                ctlvendor.Create();
                return;
            }
            this.currentid = "0";
            this.lblModalTitle.Text = "New Record";
            switch (this.myPage)
            {
                case AppModels.Pages.pageInstitutions:
                    this.txtInstitutionName.Text = "";
                    this.txtLocation.Text = "";
                    this.chkIsactive.Checked = true;
                    break;
                case AppModels.Pages.pagesRepairShops:
                    break;
                case AppModels.Pages.pageProducts:
                    this.txtProductName.Text = "";
                    this.txtPrice.Text = "0";
                    this.txtPinMoney.Text = "0";
                    break;
                case AppModels.Pages.pageProductCategories:
                    this.txtItemCatDesc.Text = "";
                    break;
                case AppModels.Pages.pageVehicles:
                    this.txtvehiclename.Text = "";
                    this.txtvehicleyear.Text = "";
                    this.txtvehiclemodel.Text = "";
                    this.txtplateno.Text = "";
                    this.txtcurrentodo.Text = "";
                    this.cmbAssignedTo.SelectedValue = "0";
                    break;
                case AppModels.Pages.pageEmployeeTypes:
                    paneltxtemployeetypeid.Visible = true;
                    this.txtemployeetypeid.Text = "";
                    this.txtemployeetypename.Text = "";
                    this.chkIsactive.Checked = true;
                    break;
                case AppModels.Pages.pageWarehouses:
                    paneltxtwarehouseid.Visible = true;
                    this.txtwarehouseid.Text = "";
                    this.txtwarehousecode.Text = "";
                    this.cmbwarehousebranches.SelectedValue = "0";
                    this.cmbwarehousebranches.Enabled = true;
                    this.txtwarehousename.Text = "";
                    this.chkIsactive.Checked = true;
                    break;
                case AppModels.Pages.pageEmployees:
                    this.txtemplastname.Text = "";
                    this.txtempfirstname.Text = "";
                    this.txtempmiddlename.Text = "";
                    this.txtempaddress.Text = "";
                    this.txtempcontact.Text = "";
                    this.txtempemail.Text = "";
                    this.cmbempbranches.SelectedValue = "0";
                    LoadEmployeeBrandDetails();
                    this.cmbempwarehouse.SelectedValue = "0";
                    this.cmbempdm.SelectedValue = "0";
                    this.cmbemptype.SelectedValue = "0";
                    this.chkIsactive.Checked = true;
                    break;
                case AppModels.Pages.pageUsers:
                    panelcmbuseremployee.Visible = true;
                    LoadUserEmployees();
                    this.txtuseruname.Text = "";
                    this.txtuserpassword.Text = "";
                    txtuserpassword.Attributes["value"] = "";
                    this.chkIsactive.Checked = true;
                    this.chkisadmin.Checked = false;
                    break;
                default:

                    break;
            }
            ClearValidations();
            PageController.fnFireEvent(this, PageController.EventType.click, "btnopenlibmodal", true);
            upanelmodal.Update();
        }
        #endregion

        protected void btnColEdit_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            if (this.myPage == AppModels.Pages.pagesRepairShops)
            {
                //ctlvendor.Edit(Convert.ToInt64(lb.CommandArgument));
                return;
            }
            this.lblModalTitle.Text = "Update Record";
            ClearValidations();
            switch (this.myPage)
            {
                case AppModels.Pages.pageInstitutions:
                    institution institution = institutionController.getInstitution(Convert.ToInt32(lb.CommandArgument));
                    this.currentid = institution.inst_id.ToString();
                    this.txtInstitutionName.Text = institution.inst_name;
                    this.txtLocation.Text = institution.inst_location;
                    this.chkIsactive.Checked = institution.isactive ?? true;
                    if (institution.insttypeid > 0)
                    {
                        this.cmbIntitutionType.SelectedValue = institution.insttypeid.ToString();
                    }
                    else
                    {
                        this.cmbIntitutionType.SelectedIndex = 0;
                    }
                    break;
                case AppModels.Pages.pageProducts:
                    product product = productController.getProduct(Convert.ToInt64(lb.CommandArgument));
                    this.currentid = product.product_id.ToString();
                    this.txtProductName.Text = product.name;
                    if (product.itemtypeid >= 0)
                        this.cmbItemType.SelectedValue = product.itemtypeid.ToString();
                    else
                        this.cmbItemType.SelectedIndex = 0;
                    //if ((product.itemcatcode ?? "0") != "0")
                    //    this.cmbItemCategory.SelectedValue = product.itemcatcode;
                    //else
                        this.cmbItemCategory.SelectedIndex = 0;
                    if (product.packagingcode > 0)
                        this.cmbPackaging.SelectedValue = product.packagingcode.ToString();
                    else
                        this.cmbPackaging.SelectedIndex = 0;
                    this.txtPrice.Text = (product.price ?? 0).ToString(AppModels.moneyformat);
                    this.txtPinMoney.Text = (product.pinmoney ?? 0).ToString(AppModels.moneyformat);
                    break;
                case AppModels.Pages.pageProductCategories:
                    itemcategory itemcategory = productController.getProductCategory(lb.CommandArgument);
                    this.currentid = itemcategory.itemcatcode;
                    this.txtItemCatDesc.Text = itemcategory.itemcatdescription;
                    upanelmodal.Update();
                    break;
                case AppModels.Pages.pageVehicles:
                    companyvehicle companyvehicle = vehicleController.GetCompanyVehicle(Convert.ToInt64(lb.CommandArgument));
                    this.currentid = companyvehicle.vehicleid.ToString();
                    this.txtvehiclename.Text = companyvehicle.vehiclename;
                    this.txtvehicleyear.Text = companyvehicle.year;
                    this.txtvehiclemodel.Text = companyvehicle.model;
                    this.txtplateno.Text = companyvehicle.platenumber;
                    this.txtcurrentodo.Text = companyvehicle.currentodo.ToString();
                    this.cmbAssignedTo.SelectedValue = companyvehicle.warehouses.FirstOrDefault() == null ? "0" : companyvehicle.warehouses.FirstOrDefault().warehouseid.ToString();
                    break;
                case AppModels.Pages.pageEmployeeTypes:
                    employeetype employeetype = employeeController.GetEmployeetype(Convert.ToInt32(lb.CommandArgument));
                    paneltxtemployeetypeid.Visible = false;
                    this.currentid = employeetype.employeetypeid.ToString();
                    this.txtemployeetypename.Text = employeetype.employeetypedescription;
                    this.chkIsactive.Checked = employeetype.isactive ?? true;
                    break;
                case AppModels.Pages.pageWarehouses:
                    warehouse warehouse = employeeController.GetWarehouse(Convert.ToInt32(lb.CommandArgument));
                    paneltxtwarehouseid.Visible = false;
                    this.currentid = warehouse.warehouseid.ToString();
                    this.txtwarehousecode.Text = warehouse.warehousecode;
                    this.txtwarehousename.Text = warehouse.warehousedescription;
                    this.cmbwarehousebranches.SelectedValue = warehouse.branchid.ToString();
                    this.cmbwarehousebranches.Enabled = false;
                    this.chkIsactive.Checked = warehouse.isactive ?? true;
                    break;
                case AppModels.Pages.pageEmployees:
                    employee employee = employeeController.GetEmployee(Convert.ToInt64(lb.CommandArgument));
                    this.currentid = employee.employeeid.ToString();
                    this.txtemplastname.Text = employee.lastname;
                    this.txtempfirstname.Text = employee.firstname;
                    this.txtempmiddlename.Text = employee.middlename;
                    this.txtempaddress.Text = employee.address;
                    this.txtempcontact.Text = employee.contactno;
                    this.txtempemail.Text = employee.email;
                    this.cmbempgender.SelectedValue = employee.gender;
                    if (cmbempbranches.Items.FindByValue(employee.branchid.ToString()) == null)
                        this.cmbempbranches.SelectedValue = "0";
                    else
                        this.cmbempbranches.SelectedValue = employee.branchid.ToString();

                    if (employee.warehouseid > 0)
                        LoadEmployeeBrandDetails(employee.warehouse);
                    else
                        LoadEmployeeBrandDetails();
                    this.cmbempwarehouse.SelectedValue = employee.warehouseid.ToString();
                    this.cmbemptype.SelectedValue = employee.employeetypeid.ToString();

                    if (cmbempdm.Items.FindByValue(employee.districtmanagerid.ToString()) == null)
                        this.cmbempdm.SelectedValue = "0";
                    else
                        this.cmbempdm.SelectedValue = employee.districtmanagerid.ToString();

                    this.chkIsactive.Checked = employee.isactive ?? true;
                    break;
                case AppModels.Pages.pageUsers:
                    useraccount u = employeeController.GetUseraccount(Convert.ToInt64(lb.CommandArgument));
                    panelcmbuseremployee.Visible = false;
                    LoadUserEmployees(u.employee);
                    this.currentid = u.useraccountid.ToString();
                    this.txtuseruname.Text = u.username;
                    this.txtuserpassword.Text = u.userpass;
                     txtuserpassword.Attributes["value"] = u.userpass;
                    this.chkIsactive.Checked = u.isactive ?? true;
                    this.chkisadmin.Checked = u.isappsysadmin ?? false;
                    break;
                default:

                    break;
            }
            PageController.fnFireEvent(this, PageController.EventType.click, "btnopenlibmodal", true);
            upanelmodal.Update();
        }

        public string getAssignedVehicle(long vehicleid)
        {
            warehouse warehouse = institutionController.warehouses.Where(w => w.assignedvehicle == vehicleid).FirstOrDefault();
            if (warehouse != null && warehouse.employees.FirstOrDefault() != null)
            {
                return warehouse.employees.First().firstname + " " + warehouse.employees.First().lastname;
            }
            return "";
        }

        protected void cmbfilterbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayList();
            upanelmaingrid.Update();
        }

        private void LoadEmployeeBrandDetails(warehouse w = null)
        {
            List<warehouse> lstwh = new List<warehouse> { new warehouse { warehouseid = 0, warehousedescription = "--Not Applicable--" } };
            lstwh.AddRange(employeeController.GetUnAssignedWarehouses(Convert.ToInt64(cmbempbranches.SelectedValue)));
            if (w != null)
                lstwh.Add(w);
            cmbempwarehouse.DataSource = lstwh;
            cmbempwarehouse.DataTextField = "warehousedescription";
            cmbempwarehouse.DataValueField = "warehouseid";
            cmbempwarehouse.DataBind();

            LoadSupervisors();
        }
        private void LoadSupervisors()
        {
            List<object> lstdm = new List<object> { new { employeeid = 0, fullname = "--Not Applicable--" } };
            lstdm.AddRange(employeeController.GetSupervisors(Convert.ToInt64(cmbempbranches.SelectedValue), Convert.ToInt32(cmbemptype.SelectedValue)));
            cmbempdm.DataSource = lstdm;
            cmbempdm.DataTextField = "fullname";
            cmbempdm.DataValueField = "employeeid";
            cmbempdm.DataBind();
        }
        protected void cmbempbranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadEmployeeBrandDetails();
            upanelmodal.Update();
        }
        private void LoadUserEmployees(employee e = null)
        {
            List<object> lstemp = new List<object> { new { employeeid = 0, fullname = "--Select--" } };
            lstemp.AddRange(employeeController.GetNoUserEmployees());
            if (e != null)
                lstemp.Add(new { e.employeeid, fullname = e.lastname + ", " + e.firstname });
            cmbuseremployee.DataSource = lstemp;
            cmbuseremployee.DataTextField = "fullname";
            cmbuseremployee.DataValueField = "employeeid";
            cmbuseremployee.DataBind();
        }

        protected void btnsaveuserpriv_Click(object sender, EventArgs e)
        {
            foreach(GridViewRow grv in tbl_userprivileges.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    userpriv up = employeeController.GetUserpriv(Convert.ToInt64(tbl_userprivileges.DataKeys[grv.DataItemIndex]["userprivno"]));
                    if (up == null)
                    {
                        up = new userpriv
                        {
                            useraccountid=Convert.ToInt64(this.currentid),
                            modcode= Convert.ToInt32(tbl_userprivileges.DataKeys[grv.DataItemIndex]["modcode"])
                        };
                        employeeController.userprivs.Add(up);
                    }
                    CheckBox chkcanrequest = (CheckBox)grv.FindControl("chkcanrequest");
                    CheckBox chkcanadd = (CheckBox)grv.FindControl("chkcanadd");
                    CheckBox chkcanedit = (CheckBox)grv.FindControl("chkcanedit");
                    up.canrequest = chkcanrequest.Checked;
                    up.canadd = chkcanadd.Checked;
                    up.canedit = chkcanedit.Checked;
                    employeeController.SaveChanges();
                }
            }
            foreach (GridViewRow grv in tbl_usersubpriv.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    usersubpriv up = employeeController.GetUsersubpriv(Convert.ToInt64(tbl_usersubpriv.DataKeys[grv.DataItemIndex]["usersubprivno"]));
                    if (up == null)
                    {
                        up = new usersubpriv
                        {
                            useraccountid = Convert.ToInt64(this.currentid),
                            submodcode = Convert.ToInt32(tbl_usersubpriv.DataKeys[grv.DataItemIndex]["submodcode"])
                        };
                        employeeController.usersubprivs.Add(up);
                    }
                    CheckBox chkcanaccess = (CheckBox)grv.FindControl("chkcanaccess");
                    up.canaccess = chkcanaccess.Checked;
                    employeeController.SaveChanges();
                }
            }
            this.currentid = "-1";
            upaneluserprivmodal.Update();
            PageController.fnFireEvent(this, PageController.EventType.click, "btnopenuserprivmodal", true);
        }

        protected void btncolUserPriv_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            useraccount u = employeeController.GetUseraccount(Convert.ToInt64(lb.CommandArgument));
            this.lbluserpriv.Text = u.employee.lastname + ", " + u.employee.firstname;
            tbl_userprivileges.DataSource = employeeController.GetUserprivs(u.useraccountid);
            tbl_userprivileges.DataKeyNames = new string[] { "userprivno", "modcode" };
            tbl_userprivileges.DataBind();

            tbl_usersubpriv.DataSource = employeeController.GetUserSubprivs(u.useraccountid);
            tbl_usersubpriv.DataKeyNames = new string[] { "usersubprivno", "submodcode" };
            tbl_usersubpriv.DataBind();
            this.currentid = u.useraccountid.ToString();
            PageController.fnFireEvent(this, PageController.EventType.click, "btnopenuserprivmodal", true);
            upaneluserprivmodal.Update();
        }

        protected void chkselectall_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkselectall.Checked)
                return;

            foreach (GridViewRow grv in tbl_userprivileges.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkcanrequest = (CheckBox)grv.FindControl("chkcanrequest");
                    CheckBox chkcanadd = (CheckBox)grv.FindControl("chkcanadd");
                    CheckBox chkcanedit = (CheckBox)grv.FindControl("chkcanedit");
                    chkcanrequest.Checked=true;
                    chkcanadd.Checked=true;
                    chkcanedit.Checked = true;
                }
            }
            foreach (GridViewRow grv in tbl_usersubpriv.Rows)
            {
                if (grv.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkcanaccess = (CheckBox)grv.FindControl("chkcanaccess");
                    chkcanaccess.Checked = true;
                }
            }
            upaneluserprivmodal.Update();
        }

        protected void cmbemptype_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSupervisors();
            upanelmodal.Update();
        }

        protected void ctlvendor_Save(object sender, EventArgs e)
        {
            DisplayList();
            upanelmaingrid.Update();
        }
        protected void btnnext_Click(object sender, EventArgs e)
        {
            //if (pageIndex < totalPageCount)
            //{
            //    pageIndex++;
            //    this.Bind();
            //}
        }

        private void webserviceresult()
        {
            if(auth.currentuser.employee.employeetypeid != 222)
            {
                lstvendor = JsonConvert.DeserializeObject<List<Vendor>>(options.Download_Data(auth.GetToken(), FFOPettyCashWS.myTransactCode.CGetVendor, "0"));
                supplierController.SaveVendors(lstvendor);
            }
        
        }
    }
}