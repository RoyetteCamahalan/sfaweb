using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views
{
    public partial class RepairVehicle : System.Web.UI.Page
    {
        Auth auth;
        //VehicleController vehicleController;
        VehicleRepairController vehicleRepairController;
        SupplierController supplierController;
        EmployeeController employeeController;

        #region "Vars"
        private int moduleid { get => AppModels.Modules.vehiclerepair; }
        public bool isPageView
        {
            get { return Convert.ToBoolean(ViewState["ispageview"] ?? false); }
            set { ViewState["ispageview"] = value; }
        }
        public List<vehiclerepairdetail> vehiclerepairdetails
        {
            get { return (List<vehiclerepairdetail>)ViewState["vehiclerepairdetails"]; }
            set { ViewState["vehiclerepairdetails"] = value; }
        }
        public List<long> supplierorder
        {
            get { return (List<long>)ViewState["supplierorder"]; }
            set { ViewState["supplierorder"] = value; }
        }
        public long warehouseid
        {
            get { return Convert.ToInt64(ViewState["warehouseid"]); }
            set { ViewState["warehouseid"] = value; }
        }
        public companyvehicle companyvehicle
        {
            get { return (companyvehicle)ViewState["companyvehicle"]; }
            set { ViewState["companyvehicle"] = value; }
        }
        public decimal avgtotal
        {
            get { return Convert.ToDecimal(ViewState["avgtotal"]); }
            set { ViewState["avgtotal"] = value; }
        }
        public int status
        {
            get { return Convert.ToInt32(ViewState["requeststatus"] ?? 0); }
            set { ViewState["requeststatus"] = value; }
        }
        #endregion

        public long vehiclerepairid
        {
            get => Convert.ToInt64(ViewState["vehiclerepairid"]);
            set { ViewState["vehiclerepairid"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this, modcode: AppModels.Modules.vehiclerepair);
            if (!auth.hasAuth())
                return;

            //vehicleController = new VehicleController();
            vehicleRepairController = new VehicleRepairController();
            supplierController = new SupplierController();
            employeeController = new EmployeeController();


            if (!Page.IsPostBack)
            {

                if (Page.RouteData.Values.ContainsKey("id"))
                {
                    this.vehiclerepairid = Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["id"].ToString()));
                    LoadRecord();
                }
                else
                {
                    this.vehiclerepairid = 0;
                    this.warehouseid = auth.warehouseid;
                    this.vehiclerepairdetails = new List<vehiclerepairdetail>();
                    LoadCombo();
                }
                DisplayInfo();

            }
            RegisterAsynchControls();
        }
        private void RegisterAsynchControls()
        {
            foreach (ListViewDataItem litem in lstrowparticulars.Items)
            {
                if (litem.ItemType == ListViewItemType.DataItem)
                {
                    ListView lstsuppliers = (ListView)litem.FindControl("lstsuppliers");
                    if(!isPageView)
                        ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)litem.FindControl("btnremove"));
                    foreach (ListViewDataItem rowitem in lstsuppliers.Items)
                    {
                        if (rowitem.ItemType == ListViewItemType.DataItem)
                        {
                            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((TextBox)rowitem.FindControl("txtprice"));
                        }
                    }
                }
            }
            foreach (ListViewDataItem litem in lstfooter.Items)
            {
                if (litem.ItemType == ListViewItemType.DataItem)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((CheckBox)litem.FindControl("chkselected"));
                }
            }
            
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnSubmit);
            ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl(btnCancel);
        }

        #region "Page Methods"
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!Page.IsPostBack)
            {
                if (this.vehiclerepairid > 0 && Page.RouteData.Values.ContainsKey("action"))
                {
                    //ctlFunding.Bind(this.moduleid, this.vehiclerepairid, this.warehouseid, Convert.ToDecimal(txtTotalBudget.Text));
                }
            }
        }
        #endregion


        #region "methods"
        protected void DisplayInfo()
        {
            warehouse w= employeeController.GetWarehouse(this.warehouseid);
            companyvehicle companyvehicle = w.companyvehicle;
            txtbox_fullname.Text = w.employees.First().firstname + " " + w.employees.First().lastname;
            if(this.vehiclerepairid == 0)
                txtbox_datefiled.Text = Utility.getServerDate().ToString(AppModels.dateformat);
            if (companyvehicle != null)
            {
                this.companyvehicle = companyvehicle;
                txtbox_companycar.Text = companyvehicle.vehiclename;
                txtbox_platenumber.Text = companyvehicle.platenumber;
                txtbox_yearmodel.Text = companyvehicle.year;
                Validator.SetError(txtbox_companycar);
            }
            else
            {
                this.companyvehicle = new companyvehicle();
                txtbox_companycar.Text = "";
                txtbox_platenumber.Text = "";
                txtbox_yearmodel.Text = "";
                if(this.vehiclerepairid == 0)
                {
                    Validator.SetError(txtbox_companycar, true);
                    btnSubmit.Visible = false;
                    btnCancel.Visible = false;
                    paneldetails.Visible = false;
                }
            }
        }
        protected void LoadCombo()
        {
            cmbRepairshop.DataSource = supplierController.GetRepairShops(employeeController.GetWarehouse(this.warehouseid).branchid ?? 0,false);
            cmbRepairshop.DataTextField = "suppliername";
            cmbRepairshop.DataValueField = "supplierno";
            cmbRepairshop.DataBind();
        }
        #endregion
        public void LoadGrid()
        {
            List<vehiclerepairdetail> lstdata = this.vehiclerepairdetails;
            if (lstdata.Count == 0)
            {
                tablecontainer.Visible = false;
            }
            else
            {
                supplierorder = new List<long>();
                List<object> lstheaderobj = new List<object>();
                foreach (long supplierno in lstdata.Select(l => l.supplierno).Distinct().ToList())
                {
                    supplier supplier = supplierController.getSupplier(supplierno);
                    supplierorder.Add(supplierno);
                    lstheaderobj.Add(new { supplierno, colheadername = supplier.suppliername });
                }
                lstheader.DataSource = lstheaderobj;
                lstheader.DataBind();
                lstfooter.DataSource = lstheaderobj;
                lstfooter.DataKeyNames = new string[] { "supplierno" };
                lstfooter.DataBind();
                tablecontainer.Visible = true;

                List<GenericObject> lstrows = new List<GenericObject>();
                foreach (vehiclerepairdetail vd in lstdata)
                {
                    bool isexist = false;
                    foreach (var row in lstrows)
                    {
                        if (row.rowid == vd.rowuuid)
                        {
                            row.qty = vd.qty ?? 0;
                            row.price = vd.price ?? 0;
                            isexist = true;
                        }
                    }
                    if (!isexist)
                    {
                        GenericObject newobj = new GenericObject {
                            rowid = vd.rowuuid,
                            reference = vd.particulars,
                            qty = vd.qty ?? 0,
                            price= vd.price ?? 0
                        };
                        lstrows.Add(newobj);
                    }
                }
                lstrowparticulars.DataSource = lstrows;
                lstrowparticulars.DataKeyNames = new string[] { "rowid" };
                lstrowparticulars.DataBind();
                GetSubTotal();
                RegisterAsynchControls();
            }

        }

        private void LoadRecord()
        {
            vehiclerepair v = vehicleRepairController.GetVehiclerepair(this.vehiclerepairid);
            this.warehouseid = v.warehouseid ?? 0;
            txtbox_datefiled.Text =(v.datefiled ?? DateTime.MaxValue).ToString(AppModels.dateformat);
            this.vehiclerepairdetails = v.vehiclerepairdetails.ToList();
            this.status = v.status ?? 0;
            if(this.status >= AppModels.Status.submitted || this.status == AppModels.Status.cancelled)
            {
                isPageView = true;
                panelshopheader.Visible = false;
                btnSubmit.Visible = false;
                btnCancel.Visible = false;

                if((v.status ?? 0) >= AppModels.Status.submitted && (v.status ?? 0) < AppModels.Status.approved && v.warehouseid == auth.warehouseid)
                btnTagasCancelled.Visible = true;
            }
            ctlStatusTrail.Bind(this.moduleid, this.vehiclerepairid, v.status ?? 0);
            ctlStatusTrail.supplierNo = v.supplierno ?? 0;
            ctlStatusTrail.totalAmount = v.totalamount ?? 0;
            LoadGrid();
            GetSubTotal();
        }
        private bool CollectData(bool isvalidate)
        {
            bool haserror = false;
            List<vehiclerepairdetail> newlstdata = new List<vehiclerepairdetail>();
            List<vehiclerepairdetail> lstdata = this.vehiclerepairdetails;
            foreach (ListViewDataItem litem in lstrowparticulars.Items)
            {
                if (litem.ItemType == ListViewItemType.DataItem)
                {
                    string rowid = lstrowparticulars.DataKeys[litem.DataItemIndex]["rowid"].ToString();
                    TextBox txtparticular = (TextBox)litem.FindControl("txtparticular");
                    TextBox txtquantity = (TextBox)litem.FindControl("txtquantity");
                    if (isvalidate)
                    {
                        if (!Validator.RequiredField(txtparticular))
                            haserror = true;
                        if (!Validator.DecZeroAbove(txtquantity))
                            haserror = true;
                    }
                    ListView lstsuppliers = (ListView)litem.FindControl("lstsuppliers");
                    foreach (ListViewDataItem rowitem in lstsuppliers.Items)
                    {
                        if (rowitem.ItemType == ListViewItemType.DataItem)
                        {
                            long supplierno = Convert.ToInt64(lstsuppliers.DataKeys[rowitem.DataItemIndex]["supplierno"]);
                            TextBox txtprice = (TextBox)rowitem.FindControl("txtprice");
                            vehiclerepairdetail vd = vehiclerepairdetails.Where(d => d.rowuuid == rowid && d.supplierno == supplierno).FirstOrDefault();
                            if (vd == null)
                            {
                                vd = new vehiclerepairdetail { supplierno = supplierno, rowuuid = rowid };
                            }
                            vd.particulars = txtparticular.Text;
                            if (Validator.DecZeroAbove(txtquantity, false))
                            {
                                vd.qty = Convert.ToInt32(txtquantity.Text);
                            }
                            if (Validator.DecZeroAbove(txtprice, isvalidate))
                            {
                                vd.price = Convert.ToDecimal(txtprice.Text);
                            }
                            else
                            {
                                haserror = true;
                            }
                            newlstdata.Add(vd);
                        }
                    }
                }
            }
            this.vehiclerepairdetails = newlstdata;
            return !haserror;
        }
        private void GenerateData()
        {
            CollectData(false);
            List<long> lstsupplierid = new List<long>();
            foreach (ListItem s in cmbRepairshop.Items)
            {
                if (s.Selected)
                {
                    lstsupplierid.Add(Convert.ToInt64(s.Value));
                }
            }
            List<vehiclerepairdetail> lstdata = this.vehiclerepairdetails;
            for (int i = lstdata.Count - 1; i >= 0; i--)
            {
                if (!lstsupplierid.Contains(lstdata[i].supplierno ?? 0))
                {
                    lstdata.RemoveAt(i);
                }
            }
            foreach (var id in lstsupplierid)
            {
                if (lstdata.Where(v => v.supplierno == id).Count() == 0)
                {
                    vehiclerepairdetail vd = new vehiclerepairdetail();
                    if (lstdata.Count > 0)
                    {
                        vd.particulars = lstdata.First().particulars;
                        vd.rowuuid = lstdata.First().rowuuid;
                        vd.qty = lstdata.First().qty;
                    }
                    else
                    {
                        vd.particulars = "";
                        vd.rowuuid = Utility.RandomString(10);
                        vd.qty = 0;
                    }
                    vd.supplierno = id;
                    vd.price = 0;
                    lstdata.Add(vd);
                }
            }
            this.vehiclerepairdetails = lstdata;
        }
        private void GetSubTotal()
        {
            List<GenericObject> lstsuppliertotal = new List<GenericObject>();
            foreach (ListViewDataItem litem in lstrowparticulars.Items)
            {
                if (litem.ItemType == ListViewItemType.DataItem)
                {
                    string rowid = lstrowparticulars.DataKeys[litem.DataItemIndex]["rowid"].ToString();
                    ListView lstsuppliers = (ListView)litem.FindControl("lstsuppliers");
                    foreach (ListViewDataItem rowitem in lstsuppliers.Items)
                    {
                        if (rowitem.ItemType == ListViewItemType.DataItem)
                        {
                            long supplierno = Convert.ToInt64(lstsuppliers.DataKeys[rowitem.DataItemIndex]["supplierno"]);
                            TextBox txtprice = (TextBox)rowitem.FindControl("txtprice");
                            bool isexist = false;
                            foreach (GenericObject obj in lstsuppliertotal)
                            {
                                if (supplierno == obj.id && Validator.DecZeroAbove(txtprice, false))
                                {
                                    obj.totalamount += Convert.ToDecimal(txtprice.Text);
                                    isexist = true;
                                }
                            }
                            if (!isexist && Validator.DecZeroAbove(txtprice, false))
                            {
                                lstsuppliertotal.Add(new GenericObject { id = supplierno, totalamount = Convert.ToDecimal(txtprice.Text) });
                            }
                        }
                    }
                }
            }
            foreach (ListViewDataItem litem in lstfooter.Items)
            {
                if (litem.ItemType == ListViewItemType.DataItem)
                {
                    long supplierno = Convert.ToInt64(lstfooter.DataKeys[litem.DataItemIndex]["supplierno"]);
                    foreach (GenericObject obj in lstsuppliertotal)
                    {
                        if (supplierno == obj.id)
                        {
                            TextBox txttotalprice = (TextBox)litem.FindControl("txttotalprice");
                            txttotalprice.Text = obj.totalamount.ToString(AppModels.moneyformat);
                        }
                    }
                }
            }
            this.avgtotal = lstsuppliertotal.Select(f => f.totalamount).Average();
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateData();
            LoadGrid();
            UpdatePnlRepairDetail.Update();
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            CollectData(false);
            List<vehiclerepairdetail> lstdata = this.vehiclerepairdetails;
            if (lstdata.Count > 0)
            {
                vehiclerepairdetail vd = new vehiclerepairdetail
                {
                    particulars = "",
                    rowuuid = Utility.RandomString(10),
                    qty = 1,
                    supplierno = lstdata.First().supplierno,
                    price = 0
                };
                lstdata.Add(vd);
                this.vehiclerepairdetails = lstdata;
                LoadGrid();
            }
            UpdatePnlRepairDetail.Update();
        }

        protected void lstrowparticulars_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                string rowid = lstrowparticulars.DataKeys[e.Item.DataItemIndex]["rowid"].ToString();
                ListView lstsuppliers = (ListView)e.Item.FindControl("lstsuppliers");
                List<vehiclerepairdetail> dtrow = new List<vehiclerepairdetail>();
                if (isPageView)
                {
                    TextBox txtparticular = (TextBox)e.Item.FindControl("txtparticular");
                    TextBox txtquantity = (TextBox)e.Item.FindControl("txtquantity");
                    Label lblparticular = (Label)e.Item.FindControl("lblparticular");
                    Label lblquantity = (Label)e.Item.FindControl("lblquantity");
                    txtparticular.Visible = false;
                    txtquantity.Visible = false;
                    lblparticular.Visible = true;
                    lblquantity.Visible = true;
                }
                foreach(long supplierno in supplierorder)
                {
                    vehiclerepairdetail vd = vehiclerepairdetails.Where(d => d.rowuuid == rowid && d.supplierno == supplierno).FirstOrDefault();
                    if (vd == null)
                    {
                        vd = new vehiclerepairdetail { supplierno=supplierno,rowuuid=rowid, price=0};
                    }
                    dtrow.Add(vd);
                }
                lstsuppliers.DataSource = dtrow;
                lstsuppliers.DataKeyNames = new string[] { "supplierno" };
                lstsuppliers.DataBind();
            }
        }

        protected void btnremove_Click(object sender, EventArgs e)
        {
            CollectData(false);
            LinkButton btn = (LinkButton)sender;
            vehiclerepairdetails.RemoveAll(v => v.rowuuid == btn.CommandArgument);
            if(vehiclerepairdetails.Count==0)
                GenerateData();
            LoadGrid();
            UpdatePnlRepairDetail.Update();
        }

        protected void txtprice_TextChanged(object sender, EventArgs e)
        {
            GetSubTotal();
            UpdatePnlRepairDetail.Update();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!CollectData(true))
            {
                UpdatePnlRepairDetail.Update();
                return;
            }else if (vehiclerepairdetails.Count == 0)
            {
                Validator.SetError(cmbRepairshop, true);
                UpdatePnlRepairDetail.Update();
                return;
            }
            Validator.SetError(cmbRepairshop);
            GetSubTotal();
            vehiclerepair _vehiclerepair = vehicleRepairController.GetVehiclerepair(this.vehiclerepairid);

            if (this.vehiclerepairid == 0)
            {
                _vehiclerepair.warehouseid = this.warehouseid;
                _vehiclerepair.vehicleid = this.companyvehicle.vehicleid;
                _vehiclerepair.datefiled = Utility.getServerDate();
                _vehiclerepair.avgamount = this.avgtotal;
                _vehiclerepair.odo = this.companyvehicle.currentodo;
                _vehiclerepair.status = AppModels.Status.submitted;

                vehicleRepairController.SaveVehicleRepair(this.vehiclerepairid == 0,_vehiclerepair, this.vehiclerepairdetails);
                this.Response.RedirectToRoute("dashboard");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            this.Response.RedirectToRoute("dashboard");
        }

        protected void lstsuppliers_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (isPageView && e.Item.ItemType == ListViewItemType.DataItem)
            {
                TextBox txtprice = (TextBox)e.Item.FindControl("txtprice");
                Label lblprice = (Label)e.Item.FindControl("lblprice");
                txtprice.Visible = false;
                lblprice.Visible = true;
            }
        }

        protected void lstfooter_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (isPageView && e.Item.ItemType == ListViewItemType.DataItem)
            {
                Panel panelchkselected = (Panel)e.Item.FindControl("panelchkselected");
                CheckBox chkselected = (CheckBox)e.Item.FindControl("chkselected");
                chkselected.Checked = ctlStatusTrail.supplierNo == Convert.ToInt64(lstfooter.DataKeys[e.Item.DataItemIndex]["supplierno"]);
                if (this.status >= AppModels.Status.submitted && this.status < AppModels.Status.approved)
                {
                    panelchkselected.Visible = true;
                    chkselected.Enabled = ctlStatusTrail.hasAction;
                }
                else
                {
                    panelchkselected.Visible = false;
                }
                
            }
        }

        protected void btnTagasCancelled_Click(object sender, EventArgs e)
        {
            vehiclerepair v = vehicleRepairController.GetVehiclerepair(this.vehiclerepairid);
            v.status = AppModels.Status.cancelled;
            v.endorsedto = 0;
            vehicleRepairController.SaveChanges();
            ApprovalController approvalController = new ApprovalController();
            approvalController.SaveTrail(this.moduleid, v.vehiclerepairid, v.warehouse.employees.First().employeeid, v.status ?? 0,0, "");
            this.Response.RedirectToRoute("dashboard");
        }

        protected void chkselected_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkselected = (CheckBox)sender;
            ctlStatusTrail.supplierNo = 0;
            ctlStatusTrail.totalAmount = 0;
            if (chkselected.Checked)
            {
                foreach (ListViewDataItem litem in lstfooter.Items)
                {
                    if (litem.ItemType == ListViewItemType.DataItem)
                    {
                        CheckBox chk = (CheckBox)litem.FindControl("chkselected");
                        if (chk.ClientID != chkselected.ClientID)
                            chk.Checked = false;
                        else
                        {
                            ctlStatusTrail.supplierNo = Convert.ToInt64(lstfooter.DataKeys[litem.DataItemIndex]["supplierno"]);
                            TextBox txtprice = (TextBox)litem.FindControl("txtprice");
                            ctlStatusTrail.totalAmount = Convert.ToDecimal(txtprice.Text);
                        }
                    }
                }
                UpdatePnlRepairDetail.Update();
            }
        }
    }
}