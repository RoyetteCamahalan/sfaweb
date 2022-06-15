using SimpleFFO.Controller;
using SimpleFFO.Model;
using Spire.Xls;
using Spire.Xls.Core.Spreadsheet.PivotTables;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views
{
    public partial class InstitutionDoctors : System.Web.UI.Page
    {
        Auth auth;
        EmployeeController employeeController;
        ReportDashBoard reportDashBoard;
        DoctorController doctorController;
        InstitutionController institutionController;

        #region "Vars"

        public int SelectedTab
        {
            get { return Convert.ToInt32(ViewState["selectedtab"] ?? 0); }
            set { ViewState["selectedtab"] = value; }
        }
        public bool isPSR
        {
            get { return Convert.ToBoolean(ViewState["isPSR"] ?? false); }
            set { ViewState["isPSR"] = value; }
        }
        public long currentid
        {
            get { return Convert.ToInt64(ViewState["currentid"] ?? 0); }
            set { ViewState["currentid"] = value; }
        }
        public bool isReactivation
        {
            get { return Convert.ToBoolean(ViewState["isReactivation"] ?? false); }
            set { ViewState["isReactivation"] = value; }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {

            auth = new Auth(this, submodcode: AppModels.SubModules.sfareports);
            if (!auth.hasAuth())
                return;

            employeeController = new EmployeeController();
            reportDashBoard = new ReportDashBoard();
            doctorController = new DoctorController();
            institutionController = new InstitutionController();

            if (!Page.IsPostBack)
            {
                LoadTree(4);
                LoadCombo();
                isPSR = auth.currentuser.employee.employeetypeid == AppModels.EmployeeTypes.psr || auth.currentuser.employee.employeetypeid == AppModels.EmployeeTypes.ptr;
                if (isPSR)
                {
                    SelectedTab = 2;
                    tablistmenu.Visible = false;
                    panelpsrlist.Visible = true;
                    DisplayList();
                }
                else
                {
                    navtabdoctors.Visible = true;
                    navtabpsrlist.Visible = false;
                    usersubpriv usp = auth.GetSubUserpriv(AppModels.SubModules.mdapproval);
                    tablistmenu.Visible = usp != null && (usp.canaccess ?? false);
                    if (tablistmenu.Visible)
                    {
                        LoadForApproval();
                    }
                }


            }
            RegisterAsyncControls();
        }
        private void RegisterAsyncControls()
        {
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnexportfile);//POST BACK; NOT ASYNC
            RegisterAsyncControlsForApproval();
            RegisterAsyncControlsPSRList();
        }
        private void RegisterAsyncControlsForApproval()
        {
            foreach (var row in lstforapproval.Items)
            {
                if (row.ItemType == ListViewItemType.DataItem)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((CheckBox)row.FindControl("chkselect"));
                }
            }
        }
        private void RegisterAsyncControlsPSRList()
        {
            foreach (var row in lstpstlist.Items)
            {
                if (row.ItemType == ListViewItemType.DataItem)
                {
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)row.FindControl("btndeletion"));
                    ScriptManager.GetCurrent(this).RegisterAsyncPostBackControl((LinkButton)row.FindControl("btnreactivate"));
                }
            }
        }
        private void LoadCombo()
        {
            List<GenericObject> lstfiltertype = new List<GenericObject>();
            lstfiltertype.Add(new GenericObject { id = 0, stringval = "View All" });
            lstfiltertype.Add(new GenericObject { id = 1, stringval = "Doctor" });
            lstfiltertype.Add(new GenericObject { id = 2, stringval = "Drugstore" });
            cmbfiltertype.DataSource = lstfiltertype;
            cmbfiltertype.DataTextField = "stringval";
            cmbfiltertype.DataValueField = "id";
            cmbfiltertype.DataBind();

            List<GenericObject> lstfilter = new List<GenericObject>();
            lstfilter.Add(new GenericObject { id = 2, stringval = "View All" });
            lstfilter.Add(new GenericObject { id = 1, stringval = "Active" });
            lstfilter.Add(new GenericObject { id = 0, stringval = "Inactive" });
            cmbfilteractive.DataSource = lstfilter;
            cmbfilteractive.DataTextField = "stringval";
            cmbfilteractive.DataValueField = "id";
            cmbfilteractive.DataBind();

            cmbspecialization.DataSource = doctorController.GetSpecializations();
            cmbspecialization.DataTextField = "name";
            cmbspecialization.DataValueField = "specialization_id";
            cmbspecialization.DataBind();

            cmbdocclass.DataSource = doctorController.GetDoctorclasses();
            cmbdocclass.DataTextField = "name";
            cmbdocclass.DataValueField = "doctor_class_id";
            cmbdocclass.DataBind();

            cmbinsttype.DataSource = institutionController.GetInstitutionTypes();
            cmbinsttype.DataTextField = "institutiontypename";
            cmbinsttype.DataValueField = "insttypeid";
            cmbinsttype.DataBind();

            LoadInstitutions();
        }
        private void LoadInstitutions()
        {
            List<institution> lstsrc = institutionController.GetAll(auth.warehouseid);
            lstsrc = lstsrc.Prepend(new institution { inst_id = -1, inst_name = "--Create New--" }).ToList();
            lstsrc = lstsrc.Prepend(new institution { inst_id = 0, inst_name = "--Select Institution--" }).ToList();
            cmbinstitution.DataSource = lstsrc;
            cmbinstitution.DataTextField = "inst_name";
            cmbinstitution.DataValueField = "inst_id";
            cmbinstitution.DataBind();
        }
        private void LoadTree(int level)
        {

            List<GenericObject> lstdms = new List<GenericObject>();
            List<branch> lstbranches = new List<branch>();
            List<GenericObject> lstpsr = new List<GenericObject>();
            if (level == 4) //rbdmchanged
            {
                employee e = auth.currentuser.employee;
                List<GenericObject> lstrbdms = new List<GenericObject>();
                if (e.employeetypeid == AppModels.EmployeeTypes.gm || e.employeetypeid == AppModels.EmployeeTypes.smd || e.employeetypeid == AppModels.EmployeeTypes.nsm || e.employeetypeid == AppModels.EmployeeTypes.administrator)
                {
                    lstrbdms.Add(new GenericObject { id = 0, employeename = "All RBDMs" });
                    lstrbdms.AddRange(employeeController.GetRBDMs());

                    lstbranches.Add(new branch { branchid = 0, branchname = "All Branches" });
                    lstbranches.AddRange(employeeController.GetActiveBranches());

                    lstdms.Add(new GenericObject { id = 0, employeename = "All DMs" });
                    lstdms.AddRange(employeeController.GetDistrictManagers(0, 0));

                    //lstpsr.Add(new GenericObject { id = 0, employeename = "All PSR/PTR" });
                    lstpsr.AddRange(employeeController.GetPSRPTR(0, 0, 0));
                    if (lstpsr.Count() != 1)
                        lstpsr = lstpsr.Prepend(new GenericObject { id = 0, employeename = "All PSR/PTR" }).ToList();
                }
                else if (e.employeetypeid == AppModels.EmployeeTypes.rbdm)
                {
                    lstrbdms.Add(new GenericObject { id = e.employeeid, employeename = e.firstname + " " + e.lastname });
                    cmbrbdm.Enabled = false;

                    lstbranches.AddRange(employeeController.GetRBDMBranches(e.employeeid));
                    if (lstbranches.Count == 1)
                    {
                        lstdms.AddRange(employeeController.GetDistrictManagers(lstbranches[0].branchid, e.employeeid));
                        cmbbranches.Enabled = false;
                    }
                    else
                    {
                        lstbranches = lstbranches.Prepend(new branch { branchid = 0, branchname = "All Branches" }).ToList();
                        lstdms.AddRange(employeeController.GetDistrictManagers(0, e.employeeid));
                    }
                    if (lstdms.Count == 1)
                    {
                        lstpsr.AddRange(employeeController.GetPSRPTR(e.branchid ?? 0, lstdms[0].id, e.employeeid));
                    }
                    else
                    {
                        lstdms = lstdms.Prepend(new GenericObject { id = 0, employeename = "All DMs" }).ToList();
                        lstpsr.AddRange(employeeController.GetPSRPTR(0, 0, e.employeeid));
                    }
                    if (lstpsr.Count() != 1)
                        lstpsr = lstpsr.Prepend(new GenericObject { id = 0, employeename = "All PSR/PTR" }).ToList();


                }
                else if (e.employeetypeid == AppModels.EmployeeTypes.bbdm)
                {
                    employee erdbdm = employeeController.GetEmployee((long)e.districtmanagerid);
                    if (erdbdm != null)
                        lstrbdms.Add(new GenericObject { id = erdbdm.employeeid, employeename = erdbdm.firstname + " " + erdbdm.lastname });

                    cmbrbdm.Enabled = false;

                    lstbranches.Add(e.branch);
                    cmbbranches.Enabled = false;

                    lstdms.Add(new GenericObject { id = e.employeeid, employeename = e.firstname + " " + e.lastname });
                    cmbbbdm.Enabled = false;

                    lstpsr.AddRange(employeeController.GetPSRPTR(e.branchid ?? 0, e.employeeid, e.districtmanagerid ?? 0));
                    if (lstpsr.Count() != 1)
                        lstpsr = lstpsr.Prepend(new GenericObject { id = 0, employeename = "All PSR/PTR" }).ToList();
                }
                else
                {
                    employee bbdm = employeeController.GetEmployee(e.districtmanagerid ?? 0);
                    employee rdbdm = employeeController.GetEmployee((long)bbdm.districtmanagerid);
                    lstrbdms.Add(new GenericObject { id = rdbdm.employeeid, employeename = rdbdm.firstname + " " + rdbdm.lastname });
                    cmbrbdm.Enabled = false;

                    lstbranches.Add(e.branch);
                    cmbbranches.Enabled = false;

                    if (bbdm != null)
                        lstdms.Add(new GenericObject { id = bbdm.employeeid, employeename = bbdm.firstname + " " + bbdm.lastname });
                    cmbbbdm.Enabled = false;

                    lstpsr.Add(new GenericObject { id = e.employeeid, employeename = e.firstname + " " + e.lastname });
                    cmbpsr.Enabled = false;
                }
                cmbrbdm.DataSource = lstrbdms;
                cmbrbdm.DataTextField = "employeename";
                cmbrbdm.DataValueField = "id";
                cmbrbdm.DataBind();

                cmbbranches.DataSource = lstbranches;
                cmbbranches.DataTextField = "branchname";
                cmbbranches.DataValueField = "branchid";
                cmbbranches.DataBind();

                cmbbbdm.DataSource = lstdms;
                cmbbbdm.DataTextField = "employeename";
                cmbbbdm.DataValueField = "id";
                cmbbbdm.DataBind();

                cmbpsr.DataSource = lstpsr;
                cmbpsr.DataTextField = "employeename";
                cmbpsr.DataValueField = "id";
                cmbpsr.DataBind();
            }
            long rbdmid = Convert.ToInt64(cmbrbdm.SelectedValue);
            if (level <= 1) //rbdmchanged
            {
                lstbranches.AddRange(employeeController.GetRBDMBranches(rbdmid));
                if (lstbranches.Count > 1)
                    lstbranches = lstbranches.Prepend(new branch { branchid = 0, branchname = "All Branches" }).ToList();

                cmbbranches.DataSource = lstbranches;
                cmbbranches.DataTextField = "branchname";
                cmbbranches.DataValueField = "branchid";
                cmbbranches.DataBind();
            }
            long branchid = Convert.ToInt64(cmbbranches.SelectedValue);
            if (level <= 2) //branchchanged
            {
                lstdms = new List<GenericObject>();
                lstdms.AddRange(employeeController.GetDistrictManagers(branchid, rbdmid));
                if (lstdms.Count > 1)
                    lstdms = lstdms.Prepend(new GenericObject() { id = 0, employeename = "All DMs" }).ToList();

                cmbbbdm.DataSource = lstdms;
                cmbbbdm.DataTextField = "employeename";
                cmbbbdm.DataValueField = "id";
                cmbbbdm.DataBind();
            }
            long bbdmid = Convert.ToInt64(cmbbbdm.SelectedValue);
            if (level <= 3) //bbdmchanged
            {
                lstpsr.AddRange(employeeController.GetPSRPTR(branchid, bbdmid, rbdmid));
                if (lstpsr.Count != 1)
                    lstpsr = lstpsr.Prepend(new GenericObject() { id = 0, employeename = "All PSR/PTR" }).ToList();

                cmbpsr.DataSource = lstpsr;
                cmbpsr.DataTextField = "employeename";
                cmbpsr.DataValueField = "id";
                cmbpsr.DataBind();

            }
            upanelreportmain.Update();
        }
        private void DisplayList(bool isexport = false)
        {
            long rbdmid = Convert.ToInt64(cmbrbdm.SelectedValue);
            long branch_id = Convert.ToInt64(cmbbranches.SelectedValue);
            long districtmanagerid = Convert.ToInt64(cmbbbdm.SelectedValue);
            long employeeid = Convert.ToInt64(cmbpsr.SelectedValue);
            long warehouseid = 0;
            if (employeeid > 0)
            {
                employee emp = employeeController.GetEmployee(employeeid);
                warehouseid = emp.warehouseid ?? 0;
            }
            DataTable dt;
            if (!isexport)
            {
                dt = reportDashBoard.GetResultReport(0, 36, branch_id, rbdmid, districtmanagerid, warehouseid, 1990, 1,
                isactive: Convert.ToInt32(cmbfilteractive.SelectedValue), mdtype: Convert.ToInt32(cmbfiltertype.SelectedValue));
                districtmanagerid = 0;
                warehouseid = 0;
                long instid = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToInt64(dt.Rows[i]["bbdmid"]) == districtmanagerid && Convert.ToInt32(dt.Rows[i]["istotal"]) == 0)
                    {
                        dt.Rows[i]["bbdm"] = "";
                    }
                    if (Convert.ToInt64(dt.Rows[i]["warehouseid"]) == warehouseid && Convert.ToInt32(dt.Rows[i]["istotal"]) == 0)
                    {
                        dt.Rows[i]["psr"] = "";
                    }
                    if (Convert.ToInt64(dt.Rows[i]["inst_id"]) == instid && Convert.ToInt32(dt.Rows[i]["istotal"]) == 0)
                    {
                        dt.Rows[i]["inst_name"] = "";
                    }
                    districtmanagerid = Convert.ToInt64(dt.Rows[i]["bbdmid"]);
                    warehouseid = Convert.ToInt64(dt.Rows[i]["warehouseid"]);
                    instid = Convert.ToInt64(dt.Rows[i]["inst_id"]);
                }
                if (isPSR)
                {
                    lstpstlist.DataSource = dt;
                    lstpstlist.Visible = true;
                    lstpstlist.DataBind();
                    RegisterAsyncControlsPSRList();
                }
                else
                {
                    tbl_masterlst.DataSource = dt;
                    tbl_masterlst.Visible = true;
                    tbl_masterlst.DataBind();
                    LoadForApproval();
                }
                auth.SaveLog("MD List", "Load MD List: RBDM-" + cmbrbdm.SelectedItem.Text + " Branch-" + cmbbranches.SelectedItem.Text + " BBDM-" + cmbbbdm.SelectedItem.Text +
                     " PSR-" + cmbpsr.SelectedItem.Text);
            }
            else
            {
                string name = "MDList" + DateTime.Now.ToString("yyyyMMdd_hhmmtt") + ".xlsx";
                dt = reportDashBoard.GetResultReport(0, 37, branch_id, rbdmid, districtmanagerid, warehouseid, 1990, 1, isactive: 2, mdtype: 0);

                Workbook workbook = new Workbook();
                workbook.LoadFromFile(Server.MapPath("~/Template/mdlstTemplate.xlsx"));

                var EditWorkbook = new Workbook();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveToStream(stream, FileFormat.Version2010);
                    stream.Seek(0, SeekOrigin.Begin);
                    EditWorkbook.LoadFromStream(stream, ExcelVersion.Version2010);
                }

                Worksheet sheet = EditWorkbook.Worksheets[1];
                sheet.InsertDataTable(dt, false, 2, 1);
                XlsPivotTable pvtmasterlst = EditWorkbook.Worksheets[0].PivotTables[0] as XlsPivotTable;
                pvtmasterlst.Cache.IsRefreshOnLoad = true;
                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();
                auth.SaveLog("MD List", "Export MD List: RBDM-" + cmbrbdm.SelectedItem.Text + " Branch-" + cmbbranches.SelectedItem.Text + " BBDM-" + cmbbbdm.SelectedItem.Text +
                     " PSR-" + cmbpsr.SelectedItem.Text);
                EditWorkbook.SaveToHttpResponse(name, Response, HttpContentType.Excel2010);
            }

        }
        private void LoadForApproval()
        {
            long rbdmid = Convert.ToInt64(cmbrbdm.SelectedValue);
            long branch_id = Convert.ToInt64(cmbbranches.SelectedValue);
            long districtmanagerid = Convert.ToInt64(cmbbbdm.SelectedValue);
            long employeeid = Convert.ToInt64(cmbpsr.SelectedValue);
            long warehouseid = 0;
            if (employeeid > 0)
            {
                employee emp = employeeController.GetEmployee(employeeid);
                warehouseid = emp.warehouseid ?? 0;
            }

            DataTable dt = reportDashBoard.GetResultReport(0, 38, branch_id, rbdmid, districtmanagerid, warehouseid, 1990, 1, isactive: 2, mdtype: 0);

            int totalcount = 0;
            foreach (DataRow row in dt.Rows)
            {
                totalcount += Convert.ToInt32(row["totalcount"]);
            }
            if (totalcount > 0)
            {
                lblforapprovalcount.Text = totalcount.ToString();
                panelapprove.Visible = true;
            }
            else
            {
                lblforapprovalcount.Text = "";
                panelapprove.Visible = false;
            }

            lstforapproval.DataSource = dt;
            lstforapproval.DataBind();
            RegisterAsyncControlsForApproval();
        }
        protected void cmbrbdm_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTree(1);
        }
        protected void cmbbranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTree(2);
        }

        protected void cmbbbdm_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTree(3);
        }
        protected void cmbpsr_SelectedIndexChanged(object sender, EventArgs e)
        {
            //switch (Convert.ToInt32(cmbreporttypes.SelectedValue))
            //{
            //    case AppModels.SFAReports.dailycalltraccker:
            //        if (cmbpsr.SelectedValue == "0")
            //        {
            //            lblprevunavailable.Text = "Preview is unavailable, please export report or filter specific PSR/PTR.";
            //            panelpreviewunavailable.Visible = true;
            //            tbl_dailycallTracker.Visible = false;
            //            btnloadinit.Visible = false;
            //        }
            //        else
            //        {
            //            panelpreviewunavailable.Visible = false;
            //            btnloadinit.Visible = true;
            //        }
            //        upanelgrids.Update(); break;
            //    case AppModels.SFAReports.masterlist:
            //        if (cmbpsr.SelectedValue == "0")
            //        {
            //            lblprevunavailable.Text = "Preview is unavailable, please export report or filter specific PSR/PTR.";
            //            panelpreviewunavailable.Visible = true;
            //            tbl_masterlst.Visible = false;
            //            btnloadinit.Visible = false;
            //            btnexportinit.Visible = true;
            //        }
            //        else
            //        {
            //            panelpreviewunavailable.Visible = false;
            //            btnloadinit.Visible = true;
            //            btnexportinit.Visible = false;
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }

        protected void btntabdoctors_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            setSelectedTab(Convert.ToInt32(btn.CommandArgument));
        }
        private void setSelectedTab(int selectedtab)
        {
            SelectedTab = selectedtab;
            if (SelectedTab == 0)
            {
                navtabdoctors.CssClass += " active";
                navtabforapproval.CssClass = navtabforapproval.CssClass.Replace(" active", "");
                navtabpsrlist.CssClass = navtabforapproval.CssClass.Replace(" active", "");
                paneldoctors.Visible = true;
                panelforapproval.Visible = false;
                panelpsrlist.Visible = false;
            }
            else if (SelectedTab == 1)
            {
                navtabdoctors.CssClass = navtabdoctors.CssClass.Replace(" active", "");
                navtabforapproval.CssClass += " active";
                navtabpsrlist.CssClass = navtabforapproval.CssClass.Replace(" active", "");
                paneldoctors.Visible = false;
                panelforapproval.Visible = true;
                panelpsrlist.Visible = false;
            }
            else if (SelectedTab == 2)
            {
                navtabdoctors.CssClass = navtabdoctors.CssClass.Replace(" active", "");
                navtabpsrlist.CssClass = navtabforapproval.CssClass.Replace(" active", "");
                navtabpsrlist.CssClass += " active";
                paneldoctors.Visible = false;
                panelforapproval.Visible = false;
                panelpsrlist.Visible = true;
            }
            upanelgrids.Update();
        }
        protected void btnLoadData(object sender, EventArgs e)
        {
            DisplayList();
            upanelgrids.Update();
            PageController.fnHideLoader(this, "panelloader");
        }
        protected void btnloadinit_Click(object sender, EventArgs e)
        {
            panelpreviewunavailable.Visible = false;
            upanelgrids.Update();
            PageController.fnShowLoader(this, "panelloader");
            PageController.fnFireEvent(this, PageController.EventType.click, ButtonLoad.ClientID, true);
        }
        protected void btnexportinit_Click(object sender, EventArgs e)
        {
            //dvProgressBar.Visible = true;
            //upanelreportmain.Update();
            //PageController.fnShowLoader(this, "panelloader");
            PageController.fnFireEvent(this, PageController.EventType.click, btnexportfile.ClientID, true);
        }
        protected void btnGenerateExcel(object sender, EventArgs e)
        {
            DisplayList(true);
            //PageController.fnHideLoader(this, "panelloader");
        }

        protected void lstforapproval_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView lst = (ListView)e.Item.FindControl("lstapprovaldetails");
                List<institutiondoctormap> lstsource = doctorController.GetForApproval(Convert.ToInt64(DataBinder.Eval(e.Item.DataItem, "warehouseid")));
                lst.DataSource = lstsource;
                lst.DataKeyNames = new string[] { "idm_id" };
                lst.DataBind();
            }
        }

        protected void chkselect_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkselect = (CheckBox)sender;
            ListViewDataItem item = (ListViewDataItem)chkselect.NamingContainer;
            ListView lst = (ListView)item.FindControl("lstapprovaldetails");
            foreach (var row in lst.Items)
            {
                if (row.ItemType == ListViewItemType.DataItem)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkselect");
                    chk.Checked = chkselect.Checked;
                }
            }
            upanelgrids.Update();
        }

        protected void btnapprove_Click(object sender, EventArgs e)
        {
            SaveForApproval(true);
        }

        protected void btndisapprove_Click(object sender, EventArgs e)
        {
            SaveForApproval(false);
        }
        private void SaveForApproval(bool isapprove)
        {
            List<institutiondoctormap> institutiondoctormaps = new List<institutiondoctormap>();
            foreach (var item in lstforapproval.Items)
            {
                if (item.ItemType == ListViewItemType.DataItem)
                {
                    ListView lst = (ListView)item.FindControl("lstapprovaldetails");
                    foreach (var row in lst.Items)
                    {
                        if (row.ItemType == ListViewItemType.DataItem)
                        {
                            CheckBox chk = (CheckBox)row.FindControl("chkselect");
                            if (chk.Checked)
                            {
                                institutiondoctormap idm = doctorController.GetInstitutiondoctormap(Convert.ToInt64(lst.DataKeys[row.DataItemIndex]["idm_id"]));
                                if (idm != null)
                                {
                                    if (idm.doctor.isactive == false && idm.doctor.updatedbyid == 0)
                                    {
                                        if (isapprove)
                                        {
                                            idm.doctor.isactive = true;
                                            idm.isactive = true;
                                            idm.updatedbyid = auth.currentuser.employeeid;
                                            idm.institution.isactive = true;
                                            idm.doctor.deleted_at = null;
                                        }
                                        else
                                            idm.updatedbyid = auth.currentuser.employeeid;
                                    }
                                    else if (idm.doctor.isactive  == true && idm.doctor.deleted_at != null)
                                    {
                                        if (isapprove)
                                            idm.doctor.isactive = false;
                                        else
                                            idm.doctor.deleted_at = null;
                                    }
                                    idm.doctor.updatedbyid = auth.currentuser.employeeid;
                                    idm.doctor.updated_at = DateTime.Now;
                                    institutiondoctormaps.Add(idm);
                                }
                            }
                        }
                    }
                }

            }
            if (institutiondoctormaps.Count > 0)
            {
                doctorController.SaveChanges();
                LoadForApproval();
                upanelgrids.Update();
                auth.SaveLog("MD List", (isapprove ? "Approval" : "Disapproval") + " of " + institutiondoctormaps.Count.ToString() + " doctors");
                PageController.fnShowAlert(this, PageController.AlertType.success, "MDU " + (isapprove ? "Approval" : "Disapproval") + " Successful.");
            }
            else
            {
                PageController.fnShowAlert(this, PageController.AlertType.error, "No Record Selected.");
            }
        }
        private void ClearValidations()
        {

        }
        private bool ValidateDoctor()
        {
            bool isvalid;
            lblerrorlastname.Text = AppModels.ErrorMessage.required;
            isvalid = Validator.RequiredField(txtdoclastname);
            isvalid = Validator.RequiredField(txtdocfirstname) && isvalid;
            if (isvalid)
            {
                if (doctorController.isNameExists(txtdoclastname.Text, txtdocfirstname.Text))
                {
                    lblerrorlastname.Text = "Doctor already exist!";
                    Validator.SetError(txtdoclastname, true);
                    isvalid = false;
                }
                else
                    Validator.SetError(txtdoclastname);
            }
            isvalid = Validator.DecOneAbove(cmbspecialization) && isvalid;
            isvalid = Validator.DecOneAbove(cmbdocclass) && isvalid;

            Validator.SetError(cmbinstitution);
            if (Convert.ToInt32(cmbinstitution.SelectedValue) == 0)
            {
                isvalid = false;
                Validator.SetError(cmbinstitution, true);
            }
            else if (Convert.ToInt32(cmbinstitution.SelectedValue) == -1)
            {
                isvalid = Validator.RequiredField(txtinstname) && isvalid;
                isvalid = Validator.RequiredField(txtinstaddress) && isvalid;
                isvalid = Validator.DecOneAbove(cmbinsttype) && isvalid;
            }
            return isvalid;
        }
        protected void btnadddoctor_Click(object sender, EventArgs e)
        {
            ClearValidations();
            PageController.fnFireEvent(this, PageController.EventType.click, "btnopenlibmodal", true);
            upanelmodal.Update();
        }

        protected void btndeletion_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            if (Convert.ToInt32(btn.CommandArgument) > 0)
            {
                this.currentid = Convert.ToInt64(btn.CommandArgument);
                lblconfirmationmessage.Text = "Are you sure to tag this as inactive?";
                this.isReactivation = false;
                btnsaveconfirmation.Text = "Yes";
                btncloseconfirmation.Text = "Cancel";
                upanelconfirmationmodal.Update();
                PageController.fnFireEvent(this, PageController.EventType.click, "btnopenconfirmationmodal", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateDoctor())
            {
                int instid = Convert.ToInt32(cmbinstitution.SelectedValue);
                if (instid == -1)
                {
                    institution institution = new institution();
                    instid = institutionController.GenerateInstID(auth.warehouseid);
                    institution.inst_id = instid;
                    institution.inst_code = "DA" + institution.inst_id.ToString();
                    institution.inst_name = txtinstname.Text;
                    institution.inst_location = txtinstaddress.Text;
                    institution.insttypeid = Convert.ToInt32(cmbinsttype.SelectedValue);
                    institution.warehouseid = auth.warehouseid;
                    institution.createdbyid = auth.currentuser.employeeid;
                    institution.created_at = DateTime.Now;
                    institution.updatedbyid = 0;
                    institution.isactive = false;
                    institution.area_id = 0;
                    institution.isattendance = false;
                    institutionController.institutions.Add(institution);
                    institutionController.SaveChanges();
                }
                doctor doctor = new doctor();
                doctor.doc_id = doctorController.GenerateDocID(auth.warehouseid);
                doctor.doc_code = "DA" + doctor.doc_id.ToString();
                doctor.doc_lastname = txtdoclastname.Text;
                doctor.doc_firstname = txtdocfirstname.Text;
                doctor.doc_mi = txtdocmiddlename.Text;
                doctor.specialization_id = Convert.ToInt32(cmbspecialization.SelectedValue);
                doctor.birthdate = Convert.ToDateTime("1900-01-01 00:00:00.000");
                doctor.contact_number = txtcontactno.Text;
                doctor.email_address = "";
                doctor.prc_licensed = txtlicenseno.Text;
                doctor.createdbyid = auth.currentuser.employeeid;
                doctor.updatedbyid = 0;
                doctor.created_at = DateTime.Now;
                doctor.isactive = false;
                doctorController.doctors.Add(doctor);
                institutiondoctormap idm = new institutiondoctormap();
                idm.idm_id = Convert.ToInt64(instid.ToString() + (Convert.ToInt32(doctor.doc_id.ToString().Replace(auth.warehouseid.ToString(), "")).ToString()));
                idm.inst_id = instid;
                idm.doc_id = doctor.doc_id;
                idm.stage_id = 0;
                idm.class_id = Convert.ToInt32(cmbdocclass.SelectedValue);
                idm.best_time_to_call = txtbesttimetocall.Text;
                idm.room_number = txtroomnumber.Text;
                idm.createdbyid = auth.currentuser.employeeid;
                idm.created_at = DateTime.Now;
                idm.isactive = false;
                idm.updatedbyid = 0;
                doctorController.institutiondoctormaps.Add(idm);
                doctorController.SaveChanges();
                DisplayList();
                upanelgrids.Update();
                txtdoclastname.Text = "";
                txtdocfirstname.Text = "";
                txtdocmiddlename.Text = "";
                txtbesttimetocall.Text = "";
                txtinstname.Text = "";
                txtinstaddress.Text = "";
                txtroomnumber.Text = "";
                txtcontactno.Text = "";
                txtlicenseno.Text = "";
                if (Convert.ToInt32(cmbinstitution.SelectedValue) == -1)
                    LoadInstitutions();
                PageController.fnFireEvent(this, PageController.EventType.click, "btnopenlibmodal", true);
                PageController.fnShowAlert(this, PageController.AlertType.success, "Doctor Successfully Added");
            }
            upanelmodal.Update();
        }

        protected void cmbinstitution_SelectedIndexChanged(object sender, EventArgs e)
        {
            panelinstitution.Visible = Convert.ToInt64(cmbinstitution.SelectedValue) == -1;
            upanelmodal.Update();
        }

        protected void btnsaveconfirmation_Click(object sender, EventArgs e)
        {
            if (currentid > 0)
            {
                doctor d = doctorController.GetDoctor((int)currentid);
                if (d != null)
                {
                    if (this.isReactivation)
                    {
                        d.updatedbyid = 0;
                        d.updated_at = DateTime.Now;
                    }
                    else
                    {
                        d.deleted_at = DateTime.Now;
                        d.updatedbyid = auth.employeeid;
                        d.updated_at = d.deleted_at;
                    }
                    doctorController.SaveChanges();
                    PageController.fnShowAlert(this, PageController.AlertType.success, "Record Successfully Updated");
                    PageController.fnFireEvent(this, PageController.EventType.click, "btnopenconfirmationmodal", true);
                    DisplayList();
                    upanelgrids.Update();
                }
            }
        }

        protected void btnreactivate_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            if (Convert.ToInt32(btn.CommandArgument) > 0)
            {
                this.currentid = Convert.ToInt64(btn.CommandArgument);
                lblconfirmationmessage.Text = "Are you sure to reactivate this record?";
                this.isReactivation = true;
                btnsaveconfirmation.Text = "Yes";
                btncloseconfirmation.Text = "Cancel";
                upanelconfirmationmodal.Update();
                PageController.fnFireEvent(this, PageController.EventType.click, "btnopenconfirmationmodal", true);
            }
        }

        //protected void lstpstlist_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{
        //    if(e.Item.ItemType== ListViewItemType.DataItem)
        //    {
        //        LinkButton btndeletion = (LinkButton)e.Item.FindControl("btndeletion");

        //    }
        //}
    }
}