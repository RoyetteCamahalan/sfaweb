using SimpleFFO.Controller;
using SimpleFFO.Model;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views
{
    public partial class UserLogs : System.Web.UI.Page
    {
        Auth auth;
        EmployeeController employeeController;
        CycleController cycleController;
        CallController callController;
        ReportDashBoard reportDashBoard;
        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this, submodcode: AppModels.SubModules.userlogs);
            if (!auth.hasAuth())
                return;

            employeeController = new EmployeeController();
            cycleController = new CycleController();
            callController = new CallController();
            reportDashBoard = new ReportDashBoard();

            if (!Page.IsPostBack)
            {
                LoadCombo();
                if (Page.RouteData.Values.ContainsKey("employeeid"))
                {
                    long employeeid = Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["employeeid"].ToString()));
                    int selectedyear = Convert.ToInt32(Page.RouteData.Values["year"]);
                    int cyclenumber = Convert.ToInt32(Page.RouteData.Values["cyclenumber"]);
                    cmbpsr.SelectedValue = employeeid.ToString();
                    cmbyear.SelectedValue = selectedyear.ToString();
                    cmbyear_SelectedIndexChanged(cmbyear, null);
                    cmbcycle.SelectedValue = cyclenumber.ToString();
                    DisplayList();
                }
            }
            ScriptManager.GetCurrent(Page).RegisterPostBackControl(btnexportfile);
        }

        #region "Methods"
        public string getCallDetailsRoute(string id)
        {
            return GetRouteUrl(AppModels.Routes.calldetails, new { id = Auth.AppSecurity.URLEncrypt(id) });
        }
        public string getCallSyncRoute(string employeeid, string date)
        {
            return GetRouteUrl(AppModels.Routes.dailysyncdetails, new { employeeid = Auth.AppSecurity.URLEncrypt(employeeid), date = Auth.AppSecurity.URLEncrypt(date) });
        }
        private void LoadCombo()
        {
            LoadTree(4);

            cmbyear.DataSource = cycleController.GetCyclesets();
            cmbyear.DataTextField = "year";
            cmbyear.DataValueField = "year";
            cmbyear.DataBind();
            cmbyear.Items.FindByValue(DateTime.Now.Year.ToString()).Selected = true;
            cmbyear_SelectedIndexChanged(cmbyear, null);
        }
        #endregion


        protected void tbl_CallReportDetails_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgsig = (Image)e.Row.FindControl("imgsig");
                imgsig.ImageUrl = AppModels.imageurl + DataBinder.Eval(e.Row.DataItem, "imageurl");
            }

        }


        private void DisplayList()
        {
            long rbdmid = Convert.ToInt64(cmbrbdm.SelectedValue);
            long branch_id = Convert.ToInt64(cmbbranches.SelectedValue);
            int selectedyear = Convert.ToInt32(cmbyear.SelectedValue);
            int cyclenumber = Convert.ToInt32(cmbcycle.SelectedValue);
            long districtmanagerid = Convert.ToInt64(cmbbbdm.SelectedValue);
            long employeeid = Convert.ToInt64(cmbpsr.SelectedValue);
            long warehouseid = 0;
            if (employeeid > 0)
            {
                employee emp = employeeController.GetEmployee(employeeid);
                warehouseid = emp.warehouseid ?? 0;
            }
            DataTable dt = reportDashBoard.GetResultReport(0, 39, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, true, false);

            tbl_main.Visible = true;
            tbl_main.DataSource = dt;
            tbl_main.DataBind();

            auth.SaveLog("User Logs", "Load User Logs");
        }
        protected void btnLoadData(object sender, EventArgs e)
        {
            DisplayList();
            upanelgrids.Update();
            PageController.fnHideLoader(this, "panelloader");
        }

        protected void btnGenerateExcel(object sender, EventArgs e)
        {
            ExportToExcel();
        }

        //private string getFileName(string predicate)
        //{
        //    string name = "Cycle" + cmbcycle.SelectedItem.Text + "_" + cmbyear.SelectedItem.Text + "_" + predicate + "_" + cmbbranches.SelectedItem.Text;
        //    if (Convert.ToInt64(cmbpsr.SelectedValue) > 0)
        //        name += "_" + cmbpsr.SelectedItem.Text;
        //    return name.Replace(" ", "_").Replace(",", "_");
        //}

        private void ExportToExcel()
        {
            //DataTable dt = new DataTable();
            //string name = string.Empty;
            //string template = string.Empty;

            //long rbdmid = Convert.ToInt64(cmbrbdm.SelectedValue);
            //long branch_id = Convert.ToInt64(cmbbranches.SelectedValue);
            //int selectedyear = Convert.ToInt32(cmbyear.SelectedValue);
            //int cyclenumber = Convert.ToInt32(cmbcycle.SelectedValue);
            //long districtmanagerid = Convert.ToInt64(cmbbbdm.SelectedValue);
            //long employeeid = Convert.ToInt64(cmbpsr.SelectedValue);
            //employee emp = employeeController.GetEmployee(employeeid);
            //long warehouseid = 0;
            //if (emp != null)
            //    warehouseid = emp.warehouseid ?? 0;
            //switch (Convert.ToInt32(cmbreporttypes.SelectedValue))
            //{

            //    case AppModels.SFAReports.callreportanalysis:


            //        name = getFileName("CallReportAnalysis") + ".xlsx";
            //        template = "QuarterlyCallReport.xlsx";
            //        dt = reportDashBoard.GetResultReport(0, 22, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);

            //        break;

            //    case AppModels.SFAReports.callreportdetails:
            //        name = getFileName("CallReportsDetails") + ".xlsx";
            //        template = "CallReportDetails.xlsx";
            //        dt = reportDashBoard.GetResultReport(0, 29, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, true, true);
            //        break;
            //    case AppModels.SFAReports.dailycalltraccker:
            //        name = getFileName("DailyCallTracker") + ".xlsx";
            //        template = "DailyCallTrackerTemplate.xlsx";
            //        dt = reportDashBoard.GetResultReport(0, 21, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
            //        break;

            //    case AppModels.SFAReports.dailysyncreport:
            //        name = getFileName("PMRSyncReport") + ".xlsx";
            //        template = "PMRSyncReport.xlsx";

            //        dt = reportDashBoard.GetResultReport(0, 30, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, true, true);
            //        break;
            //}
            //string serverpath = Server.MapPath("~/Template/" + template);


            //Workbook workbook = new Workbook();
            //workbook.LoadFromFile(serverpath);

            //var EditWorkbook = new Workbook();

            //using (var stream = new MemoryStream())
            //{
            //    workbook.SaveToStream(stream, FileFormat.Version2010);
            //    stream.Seek(0, SeekOrigin.Begin);
            //    EditWorkbook.LoadFromStream(stream, ExcelVersion.Version2010);
            //}


            //if (AppModels.SFAReports.dailycalltraccker == Convert.ToInt32(cmbreporttypes.SelectedValue))
            //{
            //    Worksheet sheet = EditWorkbook.Worksheets[1];

            //    sheet.InsertDataTable(dt, false, 2, 1);

            //    //CellRange dataRange = sheet.Range[1, 1, dt.Rows.Count + 1, 11];
            //    XlsPivotTable pt = EditWorkbook.Worksheets[0].PivotTables[0] as XlsPivotTable;
            //    //pt.ChangeDataSource(dataRange);
            //    pt.Cache.IsRefreshOnLoad = true;

            //    EditWorkbook.Worksheets[0].Range["A1"].Text = "Report created on: " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");


            //}
            //else if (AppModels.SFAReports.callreportdetails == Convert.ToInt32(cmbreporttypes.SelectedValue))
            //{
            //    Worksheet sheet = EditWorkbook.Worksheets[0];
            //    //DataTable tempdt = dt.Copy();

            //    if (dt.Columns.Contains("call_id"))
            //        dt.Columns.Remove("call_id");
            //    sheet.InsertDataTable(dt, false, 2, 1);


            //    /*for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        CellRange c1 = sheet.Range["AE" + (i + 2).ToString()];
            //        Spire.Xls.HyperLink UrlLink1 = sheet.HyperLinks.Add(c1);
            //        UrlLink1.TextToDisplay = c1.Text;
            //        UrlLink1.Type = HyperLinkType.Url;
            //        UrlLink1.Address = c1.Text;


            //        CellRange c2 = sheet.Range["H" + (i + 2).ToString()];
            //        Spire.Xls.HyperLink UrlLink2 = sheet.HyperLinks.Add(c2);
            //        UrlLink2.TextToDisplay = c2.Text;
            //        UrlLink2.Type = HyperLinkType.Url;
            //        UrlLink2.Address = AppModels.baseurl + "/" + AppModels.Routes.calldetails + "/" + Auth.AppSecurity.URLEncrypt(tempdt.Rows[i]["call_id"].ToString());
            //    }*/

            //    //sheet.AllocatedRange.AutoFitColumns();
            //    sheet.AllocatedRange.AutoFitRows();
            //}
            //else if (AppModels.SFAReports.callreportanalysis == Convert.ToInt32(cmbreporttypes.SelectedValue))
            //{
            //    DataTable dt2 = reportDashBoard.GetResultReport(0, 23, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
            //    DataTable dt3 = reportDashBoard.GetResultReport(0, 24, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
            //    DataTable dt4 = reportDashBoard.GetResultReport(0, 25, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
            //    DataTable dt5 = reportDashBoard.GetResultReport(0, 26, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
            //    DataTable dt6 = reportDashBoard.GetResultReport(0, 27, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
            //    DataTable dt7 = reportDashBoard.GetResultReport(0, 28, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);

            //    if (dt.Rows.Count > 0)
            //    {
            //        Worksheet MTD_DM_CALL = EditWorkbook.Worksheets[14];
            //        MTD_DM_CALL.InsertDataTable(dt, false, 2, 1);
            //        //CellRange data_MTD_DM = MTD_DM_CALL.Range[1, 1, dt.Rows.Count + 1, 28];
            //        XlsPivotTable pt_MTD_DM = EditWorkbook.Worksheets[1].PivotTables[0] as XlsPivotTable;
            //        //pt_MTD_DM.ChangeDataSource(data_MTD_DM);
            //        pt_MTD_DM.Cache.IsRefreshOnLoad = true;

            //    }

            //    if (dt2.Rows.Count > 0)
            //    {
            //        Worksheet DM_CALL_PERF = EditWorkbook.Worksheets[15];
            //        DM_CALL_PERF.InsertDataTable(dt2, false, 2, 1);
            //        //CellRange data_DM_CALL_PERF = DM_CALL_PERF.Range[1, 1, dt2.Rows.Count + 1, 17];
            //        XlsPivotTable pt_DM_CALL_PERF = EditWorkbook.Worksheets[2].PivotTables[0] as XlsPivotTable;
            //        //pt_DM_CALL_PERF.ChangeDataSource(data_DM_CALL_PERF);
            //        pt_DM_CALL_PERF.Cache.IsRefreshOnLoad = true;
            //    }

            //    if (dt3.Rows.Count > 0)
            //    {
            //        Worksheet MR_Ave_CALL = EditWorkbook.Worksheets[16];
            //        MR_Ave_CALL.InsertDataTable(dt3, false, 2, 1);
            //        //CellRange data_MR_Ave_CALL = MR_Ave_CALL.Range[1, 1, dt3.Rows.Count + 1, 11];
            //        XlsPivotTable pt_MR_Ave_CALL = EditWorkbook.Worksheets[3].PivotTables[0] as XlsPivotTable; // ERROR diri na side 
            //        //pt_MR_Ave_CALL.ChangeDataSource(data_MR_Ave_CALL);
            //        pt_MR_Ave_CALL.Cache.IsRefreshOnLoad = true;
            //    }

            //    if (dt4.Rows.Count > 0)
            //    {
            //        Worksheet BU_Call_Per_by_DM = EditWorkbook.Worksheets[17];
            //        BU_Call_Per_by_DM.InsertDataTable(dt4, false, 2, 1);
            //        //CellRange data_BU_Call_Per_by_DM = BU_Call_Per_by_DM.Range[1, 1, dt4.Rows.Count + 1, 16];
            //        XlsPivotTable pt_BU_Call_Per_by_DM = EditWorkbook.Worksheets[10].PivotTables[0] as XlsPivotTable;
            //        //pt_BU_Call_Per_by_DM.ChangeDataSource(data_BU_Call_Per_by_DM);
            //        pt_BU_Call_Per_by_DM.Cache.IsRefreshOnLoad = true;
            //    }

            //    if (dt5.Rows.Count > 0)
            //    {
            //        Worksheet Sample_Promats = EditWorkbook.Worksheets[18];
            //        Sample_Promats.InsertDataTable(dt5, false, 2, 1);
            //        //CellRange data_Sample_Promats = Sample_Promats.Range[1, 1, dt5.Rows.Count + 1, 17];
            //        XlsPivotTable pt_Sample_Promats = EditWorkbook.Worksheets[5].PivotTables[0] as XlsPivotTable;
            //        //pt_Sample_Promats.ChangeDataSource(data_Sample_Promats);
            //        pt_Sample_Promats.Cache.IsRefreshOnLoad = true;
            //    }

            //    if (dt6.Rows.Count > 0)
            //    {
            //        Worksheet DM_infield_with_MR = EditWorkbook.Worksheets[19];
            //        DM_infield_with_MR.InsertDataTable(dt6, false, 2, 1);
            //        //CellRange data_DM_infield_with_MR = DM_infield_with_MR.Range[1, 1, dt6.Rows.Count + 1, 16];
            //        XlsPivotTable pt_DM_infield_with_M = EditWorkbook.Worksheets[11].PivotTables[0] as XlsPivotTable;
            //        //pt_DM_infield_with_M.ChangeDataSource(data_DM_infield_with_MR);
            //        pt_DM_infield_with_M.Cache.IsRefreshOnLoad = true;
            //    }

            //    if (dt7.Rows.Count > 0)
            //    {
            //        Worksheet Work_WITH = EditWorkbook.Worksheets[20];
            //        Work_WITH.InsertDataTable(dt7, false, 2, 1);
            //        //CellRange data_Work_WITH = Work_WITH.Range[1, 1, dt7.Rows.Count + 1, 11];
            //        XlsPivotTable pt_Work_WITH = EditWorkbook.Worksheets[12].PivotTables[0] as XlsPivotTable;
            //        //pt_Work_WITH.ChangeDataSource(data_Work_WITH);
            //        pt_Work_WITH.Cache.IsRefreshOnLoad = true;


            //    }
            //    DateTime tempdate = new DateTime(selectedyear, cyclenumber, 1);
            //    for (int i = 1; i < 14; i++)
            //    {
            //        Worksheet tempworksheet = EditWorkbook.Worksheets[i];
            //        tempworksheet.Range["B5"].Text = "Report Month: " + tempdate.ToString("MMMM yyyy");
            //        tempworksheet.Range["B6"].Text = "Report created on: " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");
            //    }
            //}
            //else if (AppModels.SFAReports.dailysyncreport == Convert.ToInt32(cmbreporttypes.SelectedValue))
            //{
            //    Worksheet sheet = EditWorkbook.Worksheets[0];
            //    sheet.InsertDataTable(dt, false, 2, 1);

            //    //for (int i = 0; i < dt.Rows.Count; i++)
            //    //{
            //    //    CellRange c1 = sheet.Range["E" + (i + 2).ToString()];
            //    //    Spire.Xls.HyperLink UrlLink1 = sheet.HyperLinks.Add(c1);
            //    //    UrlLink1.TextToDisplay = c1.Text;
            //    //    UrlLink1.Type = HyperLinkType.Url;
            //    //    UrlLink1.Address = c1.Text;

            //    //    CellRange c2 = sheet.Range["G" + (i + 2).ToString()];
            //    //    Spire.Xls.HyperLink UrlLink2 = sheet.HyperLinks.Add(c2);
            //    //    UrlLink2.TextToDisplay = c2.Text;
            //    //    UrlLink2.Type = HyperLinkType.Url;
            //    //    UrlLink2.Address = c2.Text;
            //    //}
            //    sheet.AllocatedRange.AutoFitColumns();
            //    sheet.AllocatedRange.AutoFitRows();


            //}
            //auth.SaveLog("Export Report", cmbreporttypes.SelectedItem.Text);
            //btnexportfile.OnClientClick = "avascript:HideProgressBar()";
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Javascript", "javascript:HideProgressBar()", true);
            //EditWorkbook.SaveToHttpResponse(name, Response, HttpContentType.Excel2010);

        }
        protected void cmbyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbyear.SelectedIndex >= 0)
            {
                int lastmonth = 12;
                if (Convert.ToInt32(cmbyear.SelectedValue) == Utility.getServerDate().Year)
                    lastmonth = Utility.getServerDate().Month;

                List<ListItem> lstcycle = new List<ListItem>();
                lstcycle.Add(new ListItem
                {
                    Text = "All",
                    Value = "0"
                });
                for (var i = 1; i <= lastmonth; i++)
                {
                    lstcycle.Add(new ListItem
                    {
                        Text = i.ToString(),
                        Value = i.ToString()
                    });
                }
                cmbcycle.DataSource = lstcycle;
                cmbcycle.DataBind();
                cmbcycle.Items.FindByValue(lastmonth.ToString()).Selected = true;
            }
            upanelreportmain.Update();
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

                    lstpsr.Add(new GenericObject { id = 0, employeename = "All PSR/PTR" });
                    lstpsr.AddRange(employeeController.GetPSRPTR(0, 0, 0));
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

        protected void btnloadinit_Click(object sender, EventArgs e)
        {
            panelpreviewunavailable.Visible = false;
            tbl_main.Visible = false;
            upanelgrids.Update();
            PageController.fnShowLoader(this, "panelloader");
            PageController.fnFireEvent(this, PageController.EventType.click, ButtonLoad.ClientID, true);
        }

        protected void btnexportinit_Click(object sender, EventArgs e)
        {
            //dvProgressBar.Visible = true;
            //upanelreportmain.Update();
            PageController.fnFireEvent(this, PageController.EventType.click, btnexportfile.ClientID, true);
        }
    }

}