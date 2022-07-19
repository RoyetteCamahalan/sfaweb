using SimpleFFO.Controller;
using SimpleFFO.Model;
using Spire.Xls;
using Spire.Xls.Core.Spreadsheet.PivotTables;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views
{
    public partial class SFAReports : System.Web.UI.Page
    {
        Auth auth;
        EmployeeController employeeController;
        CycleController cycleController;
        CallController callController;
        ReportDashBoard reportDashBoard;
        DoctorController doctorController;

        #region "Vars"
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this, submodcode: AppModels.SubModules.sfareports);
            if (!auth.hasAuth())
                return;

            employeeController = new EmployeeController();
            cycleController = new CycleController();
            callController = new CallController();
            reportDashBoard = new ReportDashBoard();
            doctorController = new DoctorController();

            if (!Page.IsPostBack)
            {
                LoadCombo();
                if (Page.RouteData.Values.ContainsKey("employeeid"))
                {
                    long employeeid = Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["employeeid"].ToString()));
                    int selectedyear = Convert.ToInt32(Page.RouteData.Values["year"]);
                    int cyclenumber = Convert.ToInt32(Page.RouteData.Values["cyclenumber"]);
                    cmbreporttypes.SelectedValue = AppModels.SFAReports.callperformance.ToString();
                    cmbreporttypes_SelectedIndexChanged(cmbreporttypes, null);
                    cmbpsr.SelectedValue = employeeid.ToString();
                    cmbyear.SelectedValue = selectedyear.ToString();
                    cmbyear_SelectedIndexChanged(cmbyear, null);
                    cmbcycle.SelectedValue = cyclenumber.ToString();
                    DisplayList();
                }
            }
            ShowTableHeaders();
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
            cmbreporttypes.DataSource = reportDashBoard.GetAdminReports(auth.currentuser.employee.employeetypeid ?? 0);
            cmbreporttypes.DataValueField = "reportid";
            cmbreporttypes.DataTextField = "reportname";
            cmbreporttypes.DataBind();

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
            DataTable dt;

            string dmname = "";
            int totalcalls = 0;
            int noofdays = 0;
            int grandtotalcalls = 0;
            int grandtotalnoofdays = 0;
            switch (Convert.ToInt32(cmbreporttypes.SelectedValue))
            {
                case AppModels.SFAReports.callreportanalysis:
                    panelpreviewunavailable.Visible = true;
                    lblprevunavailable.Text = "Preview is unavailable, please export report!";
                    break;

                case AppModels.SFAReports.callreportdetails:
                     dt = reportDashBoard.GetResultReport(0, 29, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, true,false);

                    lst_callreportdetails.Visible = true;
                    lst_callreportdetails.Bind(dt);
                    lst_callreportdetails.searchFields = new string[] { "pmr_name" };

                    break;
                case AppModels.SFAReports.dailycalltraccker:
                    LoadDailyCallTracker(warehouseid, selectedyear, cyclenumber);

                    break;
                case AppModels.SFAReports.dailysyncreport:
                    lst_syncreport.Bind(reportDashBoard.GetResultReport(0, 30, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber));
                    lst_syncreport.Visible = true;
                    lst_syncreport.DataBind();
                    break;
                case AppModels.SFAReports.averagecallperday:
                    dt = reportDashBoard.GetResultReport(0, 31, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
                    dt.Columns.Add("istotal");
                    districtmanagerid = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["istotal"] = "0";
                        if ((Convert.ToInt64(dt.Rows[i]["districtmanagerid"]) != districtmanagerid && districtmanagerid > 0))
                        {
                            DataRow dtr = dt.NewRow();
                            dtr["districtmanagerid"] = districtmanagerid;
                            dtr["bbdm"] = "-- Total   " + dmname;
                            dtr["totalcalls"] = totalcalls;
                            dtr["noofdays"] = noofdays;
                            if (noofdays == 0)
                                dtr["avgcallperday"] = 0;
                            else
                                dtr["avgcallperday"] = Math.Round(Convert.ToDecimal(totalcalls) / Convert.ToDecimal(noofdays), 0);
                            dtr["istotal"] = "1";
                            dt.Rows.InsertAt(dtr, i);
                            i++;

                            grandtotalcalls += totalcalls;
                            grandtotalnoofdays += noofdays;
                            totalcalls = 0;
                            noofdays = 0;
                        }
                        else if (Convert.ToInt64(dt.Rows[i]["districtmanagerid"]) == districtmanagerid)
                        {
                            dmname = dt.Rows[i]["bbdm"].ToString();
                            dt.Rows[i]["bbdm"] = "";
                        }
                        else
                        {
                            dmname = dt.Rows[i]["bbdm"].ToString();
                        }
                        districtmanagerid = Convert.ToInt64(dt.Rows[i]["districtmanagerid"]);
                        totalcalls += Convert.ToInt32(dt.Rows[i]["totalcalls"]);
                        noofdays += Convert.ToInt32(dt.Rows[i]["noofdays"]);
                        if (Convert.ToDecimal(dt.Rows[i]["noofdays"]) > 0)
                        {
                            dt.Rows[i]["avgcallperday"] = Math.Round(Convert.ToDecimal(dt.Rows[i]["totalcalls"]) / (Convert.ToDecimal(dt.Rows[i]["noofdays"])), 0);
                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        DataRow dtr = dt.NewRow();
                        dtr["districtmanagerid"] = districtmanagerid;
                        dtr["bbdm"] = "-- Total   " + dmname;
                        dtr["totalcalls"] = totalcalls;
                        dtr["noofdays"] = noofdays;
                        if (noofdays == 0)
                            dtr["avgcallperday"] = 0;
                        else
                            dtr["avgcallperday"] = Math.Round(Convert.ToDecimal(totalcalls) / Convert.ToDecimal(noofdays), 0);
                        dtr["istotal"] = "1";
                        dt.Rows.Add(dtr);

                        grandtotalcalls += totalcalls;
                        grandtotalnoofdays += noofdays;

                        dtr = dt.NewRow();
                        dtr["districtmanagerid"] = districtmanagerid;
                        dtr["bbdm"] = "GRAND TOTAL   ";
                        dtr["totalcalls"] = grandtotalcalls;
                        dtr["noofdays"] = grandtotalnoofdays;
                        if (grandtotalnoofdays == 0)
                            dtr["avgcallperday"] = 0;
                        else
                            dtr["avgcallperday"] = Math.Round(Convert.ToDecimal(grandtotalcalls) / Convert.ToDecimal(grandtotalnoofdays), 0);
                        dtr["istotal"] = "1";
                        dt.Rows.Add(dtr);
                    }
                    lst_averagecallperday.DataSource=dt;
                    lst_averagecallperday.Visible = true;
                    lst_averagecallperday.DataBind();
                    break;
                case AppModels.SFAReports.weeklyincidental:
                    dt = reportDashBoard.GetResultReport(0, 23, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, isexport: true);
                    LoadWeeklyIncidental(dt);
                    break;
                case AppModels.SFAReports.callperformance:
                    dt = reportDashBoard.GetResultReport(0, 32, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, isexport: true);
                    if (warehouseid > 0)
                        LoadCallPerformanceperPSR(dt, selectedyear, cyclenumber, warehouseid);
                    else
                        LoadCallPerformance(dt, selectedyear, cyclenumber);
                    break;
                case AppModels.SFAReports.callperformanceperclass:
                    dt = reportDashBoard.GetResultReport(0, 33, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, isexport: true);
                    LoadCallPerformanceperGroup(dt);
                    break;
                case AppModels.SFAReports.callperformanceperspecialty:
                    dt = reportDashBoard.GetResultReport(0, 34, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, isexport: true);
                    LoadCallPerformanceperGroup(dt);
                    break;
                case AppModels.SFAReports.cyclereportspecialization:
                    int specializationid = Convert.ToInt32(cmbspecialization.SelectedValue);
                    dt = reportDashBoard.GetResultReport(0, 8, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, isexport: true, specializationid: specializationid);
                    LoadCyclePerformanceReport(dt);
                    break;
                case AppModels.SFAReports.jointcalls:
                    dt = reportDashBoard.GetResultReport(0, 35, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, isexport: true);
                    tbl_jointcalls.DataSource = dt;
                    tbl_jointcalls.Visible = true;
                    tbl_jointcalls.DataBind();
                    break;
                case AppModels.SFAReports.servicecalls:
                    dt = reportDashBoard.GetResultReport(0, 40, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber,  isexport: true);
                    tbl_jointcalls.DataSource = dt;
                    tbl_jointcalls.Visible = true;
                    tbl_jointcalls.DataBind();
                    break;
                case AppModels.SFAReports.receiving:
                    dt = reportDashBoard.GetResultReport(0, 43, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber,true,false);
                    panelreceivingperpsr.Visible = true;
                    lst_receiving.Visible = true;
                    lst_receiving.Bind(dt);
                    lst_receiving.searchFields = new string[] { "product_name" };
                    break;
                case AppModels.SFAReports.dispense:
                    dt = reportDashBoard.GetResultReport(0, 44, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, true, false);
                    panelcallmaterialsperpsr.Visible = true;
                    lst_callmaterials.Visible = true;
                    lst_callmaterials.Bind(dt);
                    lst_callmaterials.searchFields = new string[] { "name" };

                    break;

            }
            auth.SaveLog("Preview Report",cmbreporttypes.SelectedItem.Text);
            ShowTableHeaders();
        }

        private void LoadDailyCallTracker(long warehouseid, int selectedyear, int cyclenumber)
        {
            tbl_dailycallTracker.Columns.Clear();

            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("docid"));
            dt.Columns.Add(new DataColumn("docname"));
            dt.Columns.Add(new DataColumn("tempdocname"));
            dt.Columns.Add(new DataColumn("specialization"));
            dt.Columns.Add(new DataColumn("class_code"));
            dt.Columns.Add(new DataColumn("planned"));
            dt.Columns.Add(new DataColumn("total"));

            BoundField bfield = new BoundField { HeaderText = "Doctor Name", DataField = "docname" };
            bfield.HeaderStyle.CssClass = "text-center";
            tbl_dailycallTracker.Columns.Add(bfield);

            bfield = new BoundField { HeaderText = "Specialization", DataField = "specialization" };
            bfield.ItemStyle.CssClass = "text-center";
            bfield.HeaderStyle.CssClass = "text-center";
            tbl_dailycallTracker.Columns.Add(bfield);

            bfield = new BoundField { HeaderText = "Class Code", DataField = "class_code" };
            bfield.ItemStyle.CssClass = "text-center";
            bfield.HeaderStyle.CssClass = "text-center";
            tbl_dailycallTracker.Columns.Add(bfield);

            bfield = new BoundField { HeaderText = "Planned?", DataField = "planned" };
            bfield.ItemStyle.CssClass = "text-center";
            bfield.HeaderStyle.CssClass = "text-center";
            tbl_dailycallTracker.Columns.Add(bfield);

            dt.Rows.Add();
            dt.Rows[dt.Rows.Count - 1]["docid"] = "-1";
            dt.Rows[dt.Rows.Count - 1]["docname"] = "Total";
            dt.Rows[dt.Rows.Count - 1]["tempdocname"] = "zzzzzzzzzzzTotal";
            dt.Rows[dt.Rows.Count - 1]["specialization"] = "";
            dt.Rows[dt.Rows.Count - 1]["class_code"] = "";
            dt.Rows[dt.Rows.Count - 1]["planned"] = "";

            List<call> lstcalls = callController.GetCalls(warehouseid, selectedyear, cyclenumber);
            foreach (call c in lstcalls)
            {
                bool isdoctorexist = false;
                string colname = (c.start_datetime ?? DateTime.MaxValue).ToString("MMM dd");
                for (int i = 0; i < dt.Rows.Count - 1; i++)
                {
                    if (dt.Rows[i]["docid"].ToString() == c.institutiondoctormap.doc_id.ToString())
                    {
                        isdoctorexist = true;
                        if (!dt.Columns.Contains(colname))
                        {
                            dt.Columns.Add(colname);
                            bfield = new BoundField();
                            bfield.HeaderStyle.CssClass = "dailycallheadercell";
                            bfield.ItemStyle.CssClass = "text-center";
                            bfield.HeaderText = colname;
                            bfield.DataField = colname;
                            tbl_dailycallTracker.Columns.Add(bfield);
                        }

                        dt.Rows[i][colname] = dt.Rows[i][colname] is DBNull ? 1 : Convert.ToInt32(dt.Rows[i][colname]) + 1;
                        dt.Rows[i]["total"] = dt.Rows[i]["total"] is DBNull ? 1 : Convert.ToInt32(dt.Rows[i]["total"]) + 1;
                        dt.Rows[0][colname] = dt.Rows[0][colname] is DBNull ? 1 : Convert.ToInt32(dt.Rows[0][colname]) + 1;
                        dt.Rows[0]["total"] = dt.Rows[0]["total"] is DBNull ? 1 : Convert.ToInt32(dt.Rows[0]["total"]) + 1;

                        if (dt.Rows[i]["planned"].ToString() == "No" && (c.planned ?? false))
                            dt.Rows[i]["planned"] = "Yes";
                        break;
                    }
                }
                if (!isdoctorexist)
                {
                    dt.Rows.Add();
                    dt.Rows[dt.Rows.Count - 1]["docid"] = c.institutiondoctormap.doc_id;
                    dt.Rows[dt.Rows.Count - 1]["docname"] = c.institutiondoctormap.doctor.doc_firstname + " " + c.institutiondoctormap.doctor.doc_lastname;
                    dt.Rows[dt.Rows.Count - 1]["tempdocname"] = dt.Rows[dt.Rows.Count - 1]["docname"];
                    dt.Rows[dt.Rows.Count - 1]["specialization"] = c.institutiondoctormap.doctor.specialization.name;
                    dt.Rows[dt.Rows.Count - 1]["class_code"] = c.institutiondoctormap.doctorclass.code;
                    dt.Rows[dt.Rows.Count - 1]["planned"] = (c.planned ?? false) ? "Yes" : "No";
                    if (!dt.Columns.Contains(colname))
                    {
                        dt.Columns.Add(colname);
                        bfield = new BoundField { HeaderText = colname, DataField = colname };
                        bfield.HeaderStyle.CssClass = "dailycallheadercell";
                        bfield.ItemStyle.CssClass = "text-center";
                        tbl_dailycallTracker.Columns.Add(bfield);
                    }
                    dt.Rows[dt.Rows.Count - 1][colname] = "1";
                    dt.Rows[dt.Rows.Count - 1]["total"] = "1";
                    dt.Rows[0][colname] = dt.Rows[0][colname] is DBNull ? 1 : Convert.ToInt32(dt.Rows[0][colname]) + 1;
                    dt.Rows[0]["total"] = dt.Rows[0]["total"] is DBNull ? 1 : Convert.ToInt32(dt.Rows[0]["total"]) + 1;
                }
            }
            bfield = new BoundField { HeaderText = "Total", DataField = "total" };
            bfield.ItemStyle.CssClass = "text-center font600 simple-highlight";
            bfield.HeaderStyle.CssClass = "text-center";
            tbl_dailycallTracker.Columns.Add(bfield);

            dt.DefaultView.Sort = "tempdocname";
            tbl_dailycallTracker.DataSource = dt.DefaultView.ToTable(); //callController.GetCallDailyCallTracker(warehouseid, branch_id, employeeid, districtmanagerid, selectedyear, cyclenumber, "");
            tbl_dailycallTracker.Visible = true;
            tbl_dailycallTracker.DataBind();
        }

        private void LoadWeeklyIncidental(DataTable dt)
        {
            long districtmanagerid = 0;
            string dmname = "";
            int week1 = 0;
            int week2 = 0;
            int week3 = 0;
            int week4 = 0;
            int week5 = 0;
            int week6 = 0;
            int totalweek1 = 0;
            int totalweek2 = 0;
            int totalweek3 = 0;
            int totalweek4 = 0;
            int totalweek5 = 0;
            int totalweek6 = 0;

            dt.Columns.Add("istotal");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["istotal"] = "0";
                if ((Convert.ToInt64(dt.Rows[i]["districtmanagerid"]) != districtmanagerid && districtmanagerid > 0))
                {
                    DataRow dtr = dt.NewRow();
                    dtr["districtmanagerid"] = districtmanagerid;
                    dtr["bbdm"] = "-- Total   " + dmname;
                    dtr["week1"] = week1;
                    dtr["week2"] = week2;
                    dtr["week3"] = week3;
                    dtr["week4"] = week4;
                    dtr["week5"] = week5;
                    dtr["week6"] = week5;
                    dtr["total"] = week1 + week2 + week3 + week4 + week5 + week6;
                    dtr["istotal"] = "1";
                    dt.Rows.InsertAt(dtr, i);
                    i++;

                    totalweek1 += week1;
                    totalweek2 += week2;
                    totalweek3 += week3;
                    totalweek4 += week4;
                    totalweek5 += week5;
                    totalweek6 += week6;
                    week1 = 0;
                    week2 = 0;
                    week3 = 0;
                    week4 = 0;
                    week5 = 0;
                    week6 = 0;
                }
                else if (Convert.ToInt64(dt.Rows[i]["districtmanagerid"]) == districtmanagerid)
                {
                    dmname = dt.Rows[i]["bbdm"].ToString();
                    dt.Rows[i]["bbdm"] = "";
                }
                else
                {
                    dmname = dt.Rows[i]["bbdm"].ToString();
                }
                districtmanagerid = Convert.ToInt64(dt.Rows[i]["districtmanagerid"]);
                week1 += Convert.ToInt32(dt.Rows[i]["week1"]);
                week2 += Convert.ToInt32(dt.Rows[i]["week2"]);
                week3 += Convert.ToInt32(dt.Rows[i]["week3"]);
                week4 += Convert.ToInt32(dt.Rows[i]["week4"]);
                week5 += Convert.ToInt32(dt.Rows[i]["week5"]);
                week6 += Convert.ToInt32(dt.Rows[i]["week6"]);
            }
            if (dt.Rows.Count > 0)
            {
                DataRow dtr = dt.NewRow();
                dtr["districtmanagerid"] = districtmanagerid;
                dtr["bbdm"] = "-- Total   " + dmname;
                dtr["week1"] = week1;
                dtr["week2"] = week2;
                dtr["week3"] = week3;
                dtr["week4"] = week4;
                dtr["week5"] = week5;
                dtr["week6"] = week6;
                dtr["total"] = week1 + week2 + week3 + week4 + week5 + week6;
                dtr["istotal"] = "1";
                dt.Rows.Add(dtr);

                totalweek1 += week1;
                totalweek2 += week2;
                totalweek3 += week3;
                totalweek4 += week4;
                totalweek5 += week5;
                totalweek6 += week6;


                dtr = dt.NewRow();
                dtr["districtmanagerid"] = districtmanagerid;
                dtr["bbdm"] = "GRAND TOTAL   ";
                dtr["week1"] = totalweek1;
                dtr["week2"] = totalweek2;
                dtr["week3"] = totalweek3;
                dtr["week4"] = totalweek4;
                dtr["week5"] = totalweek5;
                dtr["week6"] = totalweek6;
                dtr["total"] = totalweek1 + totalweek2 + totalweek3 + totalweek4 + totalweek5 + totalweek6;
                dtr["istotal"] = "1";
                dt.Rows.Add(dtr);
            }
            tbl_weeklyincidental.Columns[6].Visible = totalweek5 > 0;
            tbl_weeklyincidental.Columns[7].Visible = totalweek6 > 0;
            tbl_weeklyincidental.DataSource = dt;
            tbl_weeklyincidental.Visible = true;
            tbl_weeklyincidental.DataBind();
        }

        private void LoadCallPerformance(DataTable dt, int selectedyear, int cyclenumber)
        {
            long rbdmid = 0;
            long bbdmid = 0;
            decimal plancount1;
            decimal totalmd1;
            decimal plancount2;
            decimal totalmd2;
            decimal plancount3;
            decimal totalmd3;
            dt.Columns.Add("callrate1");
            dt.Columns.Add("callreach1");
            dt.Columns.Add("callfreq1");
            dt.Columns.Add("callrate2");
            dt.Columns.Add("callreach2");
            dt.Columns.Add("callfreq2");
            dt.Columns.Add("callrate3");
            dt.Columns.Add("callreach3");
            dt.Columns.Add("callfreq3");
            dt.Columns.Add("psrroute");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt64(dt.Rows[i]["rbdmid"]) == rbdmid && Convert.ToInt32(dt.Rows[i]["istotal"]) == 0)
                {
                    dt.Rows[i]["rbdm"] = "";
                }
                rbdmid = Convert.ToInt64(dt.Rows[i]["rbdmid"]);
                if (Convert.ToInt64(dt.Rows[i]["bbdmid"]) == bbdmid && Convert.ToInt32(dt.Rows[i]["istotal"]) == 0)
                {
                    dt.Rows[i]["bbdm"] = "";
                }
                bbdmid = Convert.ToInt64(dt.Rows[i]["bbdmid"]);
                if (Convert.ToInt64(dt.Rows[i]["warehouseid"]) > 0)
                    dt.Rows[i]["psrroute"] = GetRouteUrl(AppModels.Routes.sfareportspsrperformance, new { employeeid = Auth.AppSecurity.URLEncrypt(dt.Rows[i]["employeeid"].ToString()), year = selectedyear, cyclenumber });
                else
                    dt.Rows[i]["psrroute"] = "";

                plancount1 = Utility.ToDecimal(dt.Rows[i]["plancount1"]);
                totalmd1 = Utility.ToDecimal(dt.Rows[i]["totalmd1"]);
                plancount2 = Utility.ToDecimal(dt.Rows[i]["plancount2"]);
                totalmd2 = Utility.ToDecimal(dt.Rows[i]["totalmd2"]);
                plancount3 = Utility.ToDecimal(dt.Rows[i]["plancount3"]);
                totalmd3 = Utility.ToDecimal(dt.Rows[i]["totalmd3"]);

                if (plancount1 == 0)
                {
                    dt.Rows[i]["callrate1"] = "0";
                    dt.Rows[i]["plancount1"] = plancount1;
                }
                else
                    dt.Rows[i]["callrate1"] = (Utility.ToDecimal(dt.Rows[i]["callcount1"]) / plancount1) * 100;
                if (totalmd1 == 0)
                {
                    dt.Rows[i]["callreach1"] = "0";
                    dt.Rows[i]["callfreq1"] = "0";
                    dt.Rows[i]["totalmd1"] = "0"; //set to zero to avoid null in aspx
                }
                else
                {
                    dt.Rows[i]["callreach1"] = (Utility.ToDecimal(dt.Rows[i]["totalreach1"]) / totalmd1) * 100;
                    dt.Rows[i]["callfreq1"] = (Utility.ToDecimal(dt.Rows[i]["totalfreq1"]) / totalmd1) * 100;
                }

                if (plancount2 == 0)
                {
                    dt.Rows[i]["callrate2"] = "0";
                    dt.Rows[i]["plancount2"] = plancount2; //set to zero to avoid null in aspx
                }
                else
                    dt.Rows[i]["callrate2"] = (Utility.ToDecimal(dt.Rows[i]["callcount2"]) / plancount2) * 100;
                if (totalmd2 == 0)
                {
                    dt.Rows[i]["callreach2"] = "0";
                    dt.Rows[i]["callfreq2"] = "0";
                    dt.Rows[i]["totalmd2"] = "0"; //set to zero to avoid null in aspx
                }
                else
                {
                    dt.Rows[i]["callreach2"] = (Utility.ToDecimal(dt.Rows[i]["totalreach2"]) / totalmd2) * 100;
                    dt.Rows[i]["callfreq2"] = (Utility.ToDecimal(dt.Rows[i]["totalfreq2"]) / totalmd2) * 100;
                }

                if (plancount3 == 0)
                {
                    dt.Rows[i]["callrate3"] = "0";
                    dt.Rows[i]["plancount3"] = plancount3; //set to zero to avoid null in aspx
                }
                else
                    dt.Rows[i]["callrate3"] = (Utility.ToDecimal(dt.Rows[i]["callcount3"]) / plancount3) * 100;
                if (totalmd3 == 0)
                {
                    dt.Rows[i]["callreach3"] = "0";
                    dt.Rows[i]["callfreq3"] = "0";
                    dt.Rows[i]["totalmd3"] = "0"; //set to zero to avoid null in aspx
                }
                else
                {
                    dt.Rows[i]["callreach3"] = (Utility.ToDecimal(dt.Rows[i]["totalreach3"]) / totalmd3) * 100;
                    dt.Rows[i]["callfreq3"] = (Utility.ToDecimal(dt.Rows[i]["totalfreq3"]) / totalmd3) * 100;
                }

            }

            lst_callperformance.DataSource = dt;
            lst_callperformance.Visible = true;
            lst_callperformance.DataBind();

            Literal lblcallperformancec1 = (Literal)lst_callperformance.FindControl("lblcallperformancec1");
            Literal lblcallperformancec2 = (Literal)lst_callperformance.FindControl("lblcallperformancec2");
            Literal lblcallperformancec3 = (Literal)lst_callperformance.FindControl("lblcallperformancec3");
            DateTime tempdate = new DateTime(selectedyear, cyclenumber, 1);
            lblcallperformancec1.Text = tempdate.AddMonths(-2).ToString("MMMM yyyy");
            lblcallperformancec2.Text = tempdate.AddMonths(-1).ToString("MMMM yyyy");
            lblcallperformancec3.Text = tempdate.ToString("MMMM yyyy");


        }
        private void LoadCallPerformanceperPSR(DataTable dt, int selectedyear, int cyclenumber, long warehouseid)
        {
            decimal callcount1;
            decimal plancount1;
            decimal callcount2;
            decimal plancount2;
            decimal callcount3;
            decimal plancount3;
            decimal totalcallmd3 = 0;
            decimal totalreachmd3 = 0;
            decimal totalfreqmd3 = 0;
            decimal totalplancount3 = 0;
            decimal totalmd3 = 0;
            dt.Columns.Add("month1");
            dt.Columns.Add("month2");
            dt.Columns.Add("month3");
            dt.Columns.Add("average");
            for (int i = 0; i < dt.Rows.Count; i++)
            {

                callcount1 = Utility.ToDecimal(dt.Rows[i]["callcount1"]);
                plancount1 = Utility.ToDecimal(dt.Rows[i]["plancount1"]);
                callcount2 = Utility.ToDecimal(dt.Rows[i]["callcount2"]);
                plancount2 = Utility.ToDecimal(dt.Rows[i]["plancount2"]);
                callcount3 = Utility.ToDecimal(dt.Rows[i]["callcount3"]);
                plancount3 = Utility.ToDecimal(dt.Rows[i]["plancount3"]);
                dt.Rows[i]["month1"] = callcount1.ToString() + "/" + plancount1.ToString();
                dt.Rows[i]["month2"] = callcount2.ToString() + "/" + plancount2.ToString();
                dt.Rows[i]["month3"] = callcount3.ToString() + "/" + plancount3.ToString();
                if (plancount1 + plancount2 + plancount3 == 0)
                    dt.Rows[i]["average"] = "0%";
                else
                    dt.Rows[i]["average"] = Math.Round(((callcount1 + callcount2 + callcount3) / (plancount1 + plancount2 + plancount3)) * 100, 2).ToString() + "%";

                if (plancount3 > 0)
                {
                    totalcallmd3 += callcount3;
                    totalplancount3 += plancount3;
                    totalmd3++;

                    if (callcount3 > 0)
                        totalreachmd3++;
                    if (callcount3 == plancount3)
                        totalfreqmd3++;
                }

            }
            if (totalplancount3 == 0)
                crateperc.InnerText = "0%";
            else
                crateperc.InnerText = Math.Round((totalcallmd3 / totalplancount3) * 100, 2).ToString() + "%";

            if (totalmd3 == 0)
            {
                creachperc.InnerText = "0%";
                cfreuquencyperc.InnerText = "0%";
            }
            else
            {
                creachperc.InnerText = Math.Round((totalreachmd3 / totalmd3) * 100, 2).ToString() + "%";
                cfreuquencyperc.InnerText = Math.Round((totalfreqmd3 / totalmd3) * 100, 2).ToString() + "%";
            }
            lblcallrate.InnerText = totalcallmd3.ToString() + "/" + totalplancount3.ToString();
            lblcallreach.InnerText = totalreachmd3.ToString() + "/" + totalmd3.ToString();
            lblcallfreq.InnerText = totalfreqmd3.ToString() + "/" + totalmd3.ToString();

            List<object> incidental = reportDashBoard.GetIncidentalCallsLst(warehouseid, cyclenumber, selectedyear);
            List<object> missedcall = reportDashBoard.GetMissedCalledLst(warehouseid, cyclenumber, selectedyear);
            incidentalcount.InnerText = incidental.Count().ToString();
            missedcallcount.InnerText = missedcall.Count().ToString();

            panelcallperformanceperpsr.Visible = true;

            lst_performancereport.Visible = true;
            lst_performancereport.Bind(dt);
        }
        private void LoadCallPerformanceperGroup(DataTable dt)
        {
            long branchid = 0;
            //decimal callcount;
            decimal plancount;
            decimal totalmd;
            dt.Columns.Add("callrate");
            dt.Columns.Add("callreach");
            dt.Columns.Add("callfreq");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToInt64(dt.Rows[i]["branchid"]) == branchid && Convert.ToInt32(dt.Rows[i]["istotal"]) == 0)
                {
                    dt.Rows[i]["branchname"] = "";
                }
                branchid = Convert.ToInt64(dt.Rows[i]["branchid"]);

                //callcount = Utility.ToDecimal(dt.Rows[i]["callcount"]);
                plancount = Utility.ToDecimal(dt.Rows[i]["plancount"]);
                totalmd = Utility.ToDecimal(dt.Rows[i]["mdlist"]);
                if (plancount == 0)
                    dt.Rows[i]["callrate"] = "0%";
                else
                    dt.Rows[i]["callrate"] = Math.Round((Utility.ToDecimal(dt.Rows[i]["callcount"]) / plancount) * 100, 2).ToString() + "%";
                if (totalmd == 0)
                {
                    dt.Rows[i]["callreach"] = "0%";
                    dt.Rows[i]["callfreq"] = "0%";
                }
                else
                {
                    dt.Rows[i]["callreach"] = Math.Round((Utility.ToDecimal(dt.Rows[i]["mdcovered"]) / totalmd) * 100, 2).ToString() + "%";
                    dt.Rows[i]["callfreq"] = Math.Round((Utility.ToDecimal(dt.Rows[i]["rigthfreq"]) / totalmd) * 100, 2).ToString() + "%";
                }
            }

            tbl_performanceperclass.DataSource = dt;
            tbl_performanceperclass.Visible = true;
            tbl_performanceperclass.DataBind();
        }

        private void LoadCyclePerformanceReport(DataTable dt)
        {
            string specialization = null;
            string year = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToString(dt.Rows[i]["name"]) == specialization && Convert.ToInt32(dt.Rows[i]["istotal"]) == 0)
                {
                    dt.Rows[i]["name"] = "";
                }
                else
                {
                    specialization = Convert.ToString(dt.Rows[i]["name"]);
                }
                if (Convert.ToString(dt.Rows[i]["cycleyear"]) == year && Convert.ToInt32(dt.Rows[i]["istotal"]) == 0)
                {
                    dt.Rows[i]["cycleyear"] = DBNull.Value;
                }
                else
                {
                    year = Convert.ToString(dt.Rows[i]["cycleyear"]);
                }
            }
            tbl_cyclespecializationreport.DataSource = dt;
            tbl_cyclespecializationreport.Visible = true;
            tbl_cyclespecializationreport.DataBind();
        }
        private void ShowTableHeaders()
        {
            switch (Convert.ToInt32(cmbreporttypes.SelectedValue))
            {
                /* case AppModels.SFAReports.callreportdetails:
                     if (tbl_CallReportDetails.HeaderRow != null)
                         tbl_CallReportDetails.HeaderRow.TableSection = TableRowSection.TableHeader;
                     break;*/
                case AppModels.SFAReports.dailycalltraccker:
                    if (tbl_dailycallTracker.HeaderRow != null)
                        tbl_dailycallTracker.HeaderRow.TableSection = TableRowSection.TableHeader;
                    break;
                case AppModels.SFAReports.weeklyincidental:
                    if (tbl_weeklyincidental.HeaderRow != null)
                        tbl_weeklyincidental.HeaderRow.TableSection = TableRowSection.TableHeader;
                    break;
            }
        }
        protected void btnLoadData(object sender, EventArgs e)
        {
            DisplayList();
            upanelgrids.Update();
            PageController.fnHideLoader(this, "panelloader");
        }

        protected void btnGenerateExcel(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(cmbreporttypes.SelectedValue))
            {
                case AppModels.SFAReports.callreportanalysis:
                case AppModels.SFAReports.callreportdetails:
                case AppModels.SFAReports.dailycalltraccker:
                case AppModels.SFAReports.dailysyncreport:
                case AppModels.SFAReports.cyclereportspecialization:
                    ExportToExcel();
                    break;
            }
        }

        private string getFileName(string predicate)
        {
            string name = "Cycle" + cmbcycle.SelectedItem.Text + "_" + cmbyear.SelectedItem.Text + "_" + predicate + "_" + cmbbranches.SelectedItem.Text;

            if (Convert.ToInt32(cmbreporttypes.SelectedValue) == AppModels.SFAReports.cyclereportspecialization)
                name = predicate + "_" + cmbyear.SelectedItem.Text + "_" + cmbspecialization.SelectedItem.Text + "_" + cmbbranches.SelectedItem.Text;

            if (Convert.ToInt64(cmbpsr.SelectedValue) > 0)
                name += "_" + cmbpsr.SelectedItem.Text;
            return name.Replace(" ", "_").Replace(",", "_");


        }

        private void ExportToExcel()
        {
            DataTable dt = new DataTable();
            string name = string.Empty;
            string template = string.Empty;

            long rbdmid = Convert.ToInt64(cmbrbdm.SelectedValue);
            long branch_id = Convert.ToInt64(cmbbranches.SelectedValue);
            int selectedyear = Convert.ToInt32(cmbyear.SelectedValue);
            int cyclenumber = Convert.ToInt32(cmbcycle.SelectedValue);
            long districtmanagerid = Convert.ToInt64(cmbbbdm.SelectedValue);
            long employeeid = Convert.ToInt64(cmbpsr.SelectedValue);
            int specializationid = 0;
            employee emp = employeeController.GetEmployee(employeeid);
            long warehouseid = 0;
            if (emp != null)
                warehouseid = emp.warehouseid ?? 0;
            if (Convert.ToInt32(cmbreporttypes.SelectedValue) == AppModels.SFAReports.cyclereportspecialization)
            {
                specializationid = Convert.ToInt32(cmbspecialization.SelectedValue);
            }
            switch (Convert.ToInt32(cmbreporttypes.SelectedValue))
            {

                case AppModels.SFAReports.callreportanalysis:


                    name = getFileName("CallReportAnalysis") + ".xlsx";
                    template = "QuarterlyCallReport.xlsx";
                    dt = reportDashBoard.GetResultReport(0, 22, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);

                    break;

                case AppModels.SFAReports.callreportdetails:
                    name = getFileName("CallReportsDetails") + ".xlsx";
                    template = "CallReportDetails.xlsx";
                    dt = reportDashBoard.GetResultReport(0, 29, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber,true,true);
                    break;
                case AppModels.SFAReports.dailycalltraccker:
                    name = getFileName("DailyCallTracker") + ".xlsx";
                    template = "DailyCallTrackerTemplate.xlsx";
                    dt = reportDashBoard.GetResultReport(0, 21, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
                    break;

                case AppModels.SFAReports.dailysyncreport:
                    name = getFileName("PMRSyncReport") + ".xlsx";
                    template = "PMRSyncReport.xlsx";

                    dt = reportDashBoard.GetResultReport(0, 30, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, true, true);
                    break;
                case AppModels.SFAReports.cyclereportspecialization:
                    name = getFileName("YearlySpecializationPerformance") + ".xlsx";
                    template = "CycleSpecialization.xlsx";
                    dt = reportDashBoard.GetResultReport(0, 8, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber, isexport: true, specializationid: specializationid);
                    break;
            }
            string serverpath = Server.MapPath("~/Template/" + template);


            Workbook workbook = new Workbook();
            workbook.LoadFromFile(serverpath);

            var EditWorkbook = new Workbook();

            using (var stream = new MemoryStream())
            {
                workbook.SaveToStream(stream, FileFormat.Version2010);
                stream.Seek(0, SeekOrigin.Begin);
                EditWorkbook.LoadFromStream(stream, ExcelVersion.Version2010);
            }


            if (AppModels.SFAReports.dailycalltraccker == Convert.ToInt32(cmbreporttypes.SelectedValue))
            {
                Worksheet sheet = EditWorkbook.Worksheets[1];

                sheet.InsertDataTable(dt, false, 2, 1);

                //CellRange dataRange = sheet.Range[1, 1, dt.Rows.Count + 1, 11];
                XlsPivotTable pt = EditWorkbook.Worksheets[0].PivotTables[0] as XlsPivotTable;
                //pt.ChangeDataSource(dataRange);
                pt.Cache.IsRefreshOnLoad = true;

                EditWorkbook.Worksheets[0].Range["A1"].Text = "Report created on: " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");


            }
            else if (AppModels.SFAReports.callreportdetails == Convert.ToInt32(cmbreporttypes.SelectedValue))
            {
                Worksheet sheet = EditWorkbook.Worksheets[0];
                //DataTable tempdt = dt.Copy();

                if (dt.Columns.Contains("call_id"))
                    dt.Columns.Remove("call_id");
                sheet.InsertDataTable(dt, false, 2, 1);


                /*for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CellRange c1 = sheet.Range["AE" + (i + 2).ToString()];
                    Spire.Xls.HyperLink UrlLink1 = sheet.HyperLinks.Add(c1);
                    UrlLink1.TextToDisplay = c1.Text;
                    UrlLink1.Type = HyperLinkType.Url;
                    UrlLink1.Address = c1.Text;


                    CellRange c2 = sheet.Range["H" + (i + 2).ToString()];
                    Spire.Xls.HyperLink UrlLink2 = sheet.HyperLinks.Add(c2);
                    UrlLink2.TextToDisplay = c2.Text;
                    UrlLink2.Type = HyperLinkType.Url;
                    UrlLink2.Address = AppModels.baseurl + "/" + AppModels.Routes.calldetails + "/" + Auth.AppSecurity.URLEncrypt(tempdt.Rows[i]["call_id"].ToString());
                }*/

                //sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();
            }
            else if (AppModels.SFAReports.callreportanalysis == Convert.ToInt32(cmbreporttypes.SelectedValue))
            {
                DataTable dt2 = reportDashBoard.GetResultReport(0, 23, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
                DataTable dt3 = reportDashBoard.GetResultReport(0, 24, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
                DataTable dt4 = reportDashBoard.GetResultReport(0, 25, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
                DataTable dt5 = reportDashBoard.GetResultReport(0, 26, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
                DataTable dt6 = reportDashBoard.GetResultReport(0, 27, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);
                DataTable dt7 = reportDashBoard.GetResultReport(0, 28, branch_id, rbdmid, districtmanagerid, warehouseid, selectedyear, cyclenumber);

                if (dt.Rows.Count > 0)
                {
                    Worksheet MTD_DM_CALL = EditWorkbook.Worksheets[14];
                    MTD_DM_CALL.InsertDataTable(dt, false, 2, 1);
                    //CellRange data_MTD_DM = MTD_DM_CALL.Range[1, 1, dt.Rows.Count + 1, 28];
                    XlsPivotTable pt_MTD_DM = EditWorkbook.Worksheets[1].PivotTables[0] as XlsPivotTable;
                    //pt_MTD_DM.ChangeDataSource(data_MTD_DM);
                    pt_MTD_DM.Cache.IsRefreshOnLoad = true;

                }

                if (dt2.Rows.Count > 0)
                {
                    Worksheet DM_CALL_PERF = EditWorkbook.Worksheets[15];
                    DM_CALL_PERF.InsertDataTable(dt2, false, 2, 1);
                    //CellRange data_DM_CALL_PERF = DM_CALL_PERF.Range[1, 1, dt2.Rows.Count + 1, 17];
                    XlsPivotTable pt_DM_CALL_PERF = EditWorkbook.Worksheets[2].PivotTables[0] as XlsPivotTable;
                    //pt_DM_CALL_PERF.ChangeDataSource(data_DM_CALL_PERF);
                    pt_DM_CALL_PERF.Cache.IsRefreshOnLoad = true;
                }

                if (dt3.Rows.Count > 0)
                {
                    Worksheet MR_Ave_CALL = EditWorkbook.Worksheets[16];
                    MR_Ave_CALL.InsertDataTable(dt3, false, 2, 1);
                    //CellRange data_MR_Ave_CALL = MR_Ave_CALL.Range[1, 1, dt3.Rows.Count + 1, 11];
                    XlsPivotTable pt_MR_Ave_CALL = EditWorkbook.Worksheets[3].PivotTables[0] as XlsPivotTable; // ERROR diri na side 
                    //pt_MR_Ave_CALL.ChangeDataSource(data_MR_Ave_CALL);
                    pt_MR_Ave_CALL.Cache.IsRefreshOnLoad = true;
                }

                if (dt4.Rows.Count > 0)
                {
                    Worksheet BU_Call_Per_by_DM = EditWorkbook.Worksheets[17];
                    BU_Call_Per_by_DM.InsertDataTable(dt4, false, 2, 1);
                    //CellRange data_BU_Call_Per_by_DM = BU_Call_Per_by_DM.Range[1, 1, dt4.Rows.Count + 1, 16];
                    XlsPivotTable pt_BU_Call_Per_by_DM = EditWorkbook.Worksheets[10].PivotTables[0] as XlsPivotTable;
                    //pt_BU_Call_Per_by_DM.ChangeDataSource(data_BU_Call_Per_by_DM);
                    pt_BU_Call_Per_by_DM.Cache.IsRefreshOnLoad = true;
                }

                if (dt5.Rows.Count > 0)
                {
                    Worksheet Sample_Promats = EditWorkbook.Worksheets[18];
                    Sample_Promats.InsertDataTable(dt5, false, 2, 1);
                    //CellRange data_Sample_Promats = Sample_Promats.Range[1, 1, dt5.Rows.Count + 1, 17];
                    XlsPivotTable pt_Sample_Promats = EditWorkbook.Worksheets[5].PivotTables[0] as XlsPivotTable;
                    //pt_Sample_Promats.ChangeDataSource(data_Sample_Promats);
                    pt_Sample_Promats.Cache.IsRefreshOnLoad = true;
                }

                if (dt6.Rows.Count > 0)
                {
                    Worksheet DM_infield_with_MR = EditWorkbook.Worksheets[19];
                    DM_infield_with_MR.InsertDataTable(dt6, false, 2, 1);
                    //CellRange data_DM_infield_with_MR = DM_infield_with_MR.Range[1, 1, dt6.Rows.Count + 1, 16];
                    XlsPivotTable pt_DM_infield_with_M = EditWorkbook.Worksheets[11].PivotTables[0] as XlsPivotTable;
                    //pt_DM_infield_with_M.ChangeDataSource(data_DM_infield_with_MR);
                    pt_DM_infield_with_M.Cache.IsRefreshOnLoad = true;
                }

                if (dt7.Rows.Count > 0)
                {
                    Worksheet Work_WITH = EditWorkbook.Worksheets[20];
                    Work_WITH.InsertDataTable(dt7, false, 2, 1);
                    //CellRange data_Work_WITH = Work_WITH.Range[1, 1, dt7.Rows.Count + 1, 11];
                    XlsPivotTable pt_Work_WITH = EditWorkbook.Worksheets[12].PivotTables[0] as XlsPivotTable;
                    //pt_Work_WITH.ChangeDataSource(data_Work_WITH);
                    pt_Work_WITH.Cache.IsRefreshOnLoad = true;


                }
                DateTime tempdate = new DateTime(selectedyear, cyclenumber, 1);
                for (int i = 1; i < 14; i++)
                {
                    Worksheet tempworksheet = EditWorkbook.Worksheets[i];
                    tempworksheet.Range["B5"].Text = "Report Month: " + tempdate.ToString("MMMM yyyy");
                    tempworksheet.Range["B6"].Text = "Report created on: " + DateTime.Now.ToString("MMMM dd, yyyy hh:mm tt");
                }
            }
            else if (AppModels.SFAReports.dailysyncreport == Convert.ToInt32(cmbreporttypes.SelectedValue))
            {
                Worksheet sheet = EditWorkbook.Worksheets[0];
                sheet.InsertDataTable(dt, false, 2, 1);

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    CellRange c1 = sheet.Range["E" + (i + 2).ToString()];
                //    Spire.Xls.HyperLink UrlLink1 = sheet.HyperLinks.Add(c1);
                //    UrlLink1.TextToDisplay = c1.Text;
                //    UrlLink1.Type = HyperLinkType.Url;
                //    UrlLink1.Address = c1.Text;

                //    CellRange c2 = sheet.Range["G" + (i + 2).ToString()];
                //    Spire.Xls.HyperLink UrlLink2 = sheet.HyperLinks.Add(c2);
                //    UrlLink2.TextToDisplay = c2.Text;
                //    UrlLink2.Type = HyperLinkType.Url;
                //    UrlLink2.Address = c2.Text;
                //}
                sheet.AllocatedRange.AutoFitColumns();
                sheet.AllocatedRange.AutoFitRows();


            }
            else if (AppModels.SFAReports.cyclereportspecialization == Convert.ToInt32(cmbreporttypes.SelectedValue))
            {
                string[] ColumnsToBeDeleted = { "specialization_id", "istotal", "typeid", "typeid2" };
                foreach (string Colname in ColumnsToBeDeleted)
                {
                    if (dt.Columns.Contains(Colname))
                        dt.Columns.Remove(Colname);
                }
                Worksheet sheet = EditWorkbook.Worksheets[0];
                sheet.InsertDataTable(dt, false, 3, 1);
            }
            auth.SaveLog("Export Report", String.Format("{0}: RBDM:{1} Branch:{2} BBDM:{3} PSR:{4} Year:{5} Cycle:{6}", cmbreporttypes.SelectedItem.Text, cmbrbdm.SelectedItem.Text,
                cmbbranches.SelectedItem.Text, cmbbbdm.SelectedItem.Text, cmbpsr.SelectedItem.Text, selectedyear, cyclenumber));
            //btnexportfile.OnClientClick = "avascript:HideProgressBar()";
            //ScriptManager.RegisterStartupScript(this, this.Page.GetType(), "Javascript", "javascript:HideProgressBar()", true);
            EditWorkbook.SaveToHttpResponse(name, Response, HttpContentType.Excel2010);

        }
        protected void cmbyear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbyear.SelectedIndex >= 0)
            {
                int lastmonth = 12;
                if (Convert.ToInt32(cmbyear.SelectedValue) == Utility.getServerDate().Year)
                    lastmonth = Utility.getServerDate().Month;

                List<ListItem> lstcycle = new List<ListItem>();
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

        protected void cmbreporttypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnexportinit.Visible = true;
            btnloadinit.Visible = true;
            panelpreviewunavailable.Visible = false;
            cmbyear.Enabled = true;
            cmbcycle.Enabled = true;
            panelspecialization.Visible = false;
            switch (Convert.ToInt32(cmbreporttypes.SelectedValue))
            {
                case AppModels.SFAReports.callreportanalysis:
                    DisplayList();
                    btnloadinit.Visible = false;
                    break;
                case AppModels.SFAReports.dailycalltraccker:
                    if (cmbpsr.SelectedValue == "0")
                    {
                        lblprevunavailable.Text = "Preview is unavailable, please export report or filter specific PSR/PTR.";
                        panelpreviewunavailable.Visible = true;
                        btnloadinit.Visible = false;
                    }
                    else
                    {
                        panelpreviewunavailable.Visible = false;
                        btnloadinit.Visible = true;
                    }
                    break;
                case AppModels.SFAReports.averagecallperday:
                case AppModels.SFAReports.weeklyincidental:
                case AppModels.SFAReports.callperformance:
                case AppModels.SFAReports.callperformanceperclass:
                case AppModels.SFAReports.callperformanceperspecialty:
                    btnexportinit.Visible = false;
                    break;
                case AppModels.SFAReports.cyclereportspecialization:
                    cmbcycle.Enabled = false;
                    btnloadinit.Visible = true;
                    btnexportinit.Visible = true;
                    panelspecialization.Visible = true;
                    
                    employee info = employeeController.GetEmployee(Convert.ToInt64(cmbpsr.SelectedValue));

                    List<GenericObject> result = doctorController.GetSpecializationPerWarehouse(0);
                    result = result.Prepend(new GenericObject { specialization_id = 0, name = "Select a psr first" }).ToList();
                    cmbspecialization.DataSource = result;
                    cmbspecialization.DataTextField = "name";
                    cmbspecialization.DataValueField = "specialization_id";
                    cmbspecialization.DataBind();

                    //cmbpsr_SelectedIndexChanged(cmbpsr, null);
                    break;

                case AppModels.SFAReports.receiving:
                    btnexportinit.Visible = false;
                    break;
                case AppModels.SFAReports.dispense:
                    btnexportinit.Visible = false;
                    break;
                default:
                    btnloadinit.Visible = true;
                    panelpreviewunavailable.Visible = false;
                    break;
            }
            lst_callreportdetails.Visible = false;
            tbl_dailycallTracker.Visible = false;
            lst_syncreport.Visible = false;
            lst_averagecallperday.Visible = false;
            tbl_weeklyincidental.Visible = false;
            lst_callperformance.Visible = false;
            panelcallperformanceperpsr.Visible = false;
            lst_performancereport.Visible = false;
            tbl_performanceperclass.Visible = false;
            tbl_cyclespecializationreport.Visible = false;
            panelreceivingperpsr.Visible = false;
            lst_receiving.Visible = false;
            panelcallmaterialsperpsr.Visible = false;
            lst_callmaterials.Visible = false;
            upanelgrids.Update();
        }

        protected void btnloadinit_Click(object sender, EventArgs e)
        {
            panelpreviewunavailable.Visible = false;
            lst_callreportdetails.Visible = false;
            tbl_dailycallTracker.Visible = false;
            lst_syncreport.Visible = false;
            lst_averagecallperday.Visible = false;
            tbl_weeklyincidental.Visible = false;
            lst_callperformance.Visible = false;
            panelcallperformanceperpsr.Visible = false;
            lst_performancereport.Visible = false;
            tbl_performanceperclass.Visible = false;
            panelreceivingperpsr.Visible = false;
            panelcallmaterialsperpsr.Visible = false;
            lst_callmaterials.Visible = false;
            lst_receiving.Visible = false;
            upanelgrids.Update();
            PageController.fnShowLoader(this, "panelloader");
            PageController.fnFireEvent(this, PageController.EventType.click, ButtonLoad.ClientID, true);
        }

        protected void cmbpsr_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Convert.ToInt32(cmbreporttypes.SelectedValue))
            {
                case AppModels.SFAReports.dailycalltraccker:
                    if (cmbpsr.SelectedValue == "0")
                    {
                        lblprevunavailable.Text = "Preview is unavailable, please export report or filter specific PSR/PTR.";
                        panelpreviewunavailable.Visible = true;
                        tbl_dailycallTracker.Visible = false;
                        btnloadinit.Visible = false;
                    }
                    else
                    {
                        panelpreviewunavailable.Visible = false;
                        btnloadinit.Visible = true;
                    }
                    upanelgrids.Update(); break;
                case AppModels.SFAReports.cyclereportspecialization:
                    if (cmbpsr.SelectedValue == "0")
                    {
                        lblprevunavailable.Text = "Preview is unavailable, please filter specific PSR/PTR.";
                        panelpreviewunavailable.Visible = true;
                        tbl_cyclespecializationreport.Visible = false;
                        btnloadinit.Visible = false;
                        btnexportinit.Visible = false;
                    }
                    else
                    {
                        panelpreviewunavailable.Visible = false;
                        btnloadinit.Visible = true;
                        btnexportinit.Visible = true;

                        employee info = employeeController.GetEmployee(Convert.ToInt64(cmbpsr.SelectedValue));

                        List<GenericObject> result = doctorController.GetSpecializationPerWarehouse(info == null ? 0 : (long)info.warehouseid);
                        result = result.Prepend(new GenericObject { specialization_id = 0, name = "All specialization" }).ToList();
                        cmbspecialization.DataSource = result;
                        cmbspecialization.DataTextField = "name";
                        cmbspecialization.DataValueField = "specialization_id";
                        cmbspecialization.DataBind();
                    }
                    upanelgrids.Update();
                    break;
                default:
                    break;
            }
        }

        protected void tbl_averagecallperday_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (DataBinder.Eval(e.Row.DataItem, "istotal").ToString() == "1")
                {
                    e.Row.CssClass += " font600 simple-highlight";
                }
            }
        }
        protected void btnexportinit_Click(object sender, EventArgs e)
        {
            //dvProgressBar.Visible = true;
            //upanelreportmain.Update();
            PageController.fnFireEvent(this, PageController.EventType.click, btnexportfile.ClientID, true);
        }

        public string getjointcallRoute(string id)
        {
            return GetRouteUrl(AppModels.Routes.jointcalls, new { id = Auth.AppSecurity.URLEncrypt(id) });
        }

        protected void lst_performancereport_LayoutCreated(object sender, EventArgs e)
        {
            ListView lst = (ListView)sender;
            DateTime result = new DateTime(Convert.ToInt32(cmbyear.SelectedValue), Convert.ToInt32(cmbcycle.SelectedValue), 1);

            ((Literal)lst.FindControl("lst_performancereport_m1")).Text = result.AddMonths(-2).ToString("MMMM yyyy");
            ((Literal)lst.FindControl("lst_performancereport_m2")).Text = result.AddMonths(-1).ToString("MMMM yyyy");
            ((Literal)lst.FindControl("lst_performancereport_m3")).Text = result.ToString("MMMM yyyy");
        }

        protected void lst_receiving_layoutCreated(object sender, EventArgs e)
        {

        }

        protected void lst_callmaterials_layoutCreated(object sender,EventArgs e)
        {

        }
    }

}