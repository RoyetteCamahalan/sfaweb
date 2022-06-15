using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using SimpleFFO.Controller;
using SimpleFFO.Model;

namespace SimpleFFO.views
{
    public partial class CallSyncReportInfo : System.Web.UI.Page
    {
        CallController _callController;
        EmployeeController _employeeController;

        protected void Page_Load(object sender, EventArgs e)
        {
            _callController = new CallController();
            _employeeController = new EmployeeController();

            if (!IsPostBack)
            {
                DateTime _date = Convert.ToDateTime(Auth.AppSecurity.URLDecrypt(RouteData.Values["date"].ToString()));
                employee _employee = _employeeController.GetEmployee(Convert.ToInt64(Auth.AppSecurity.URLDecrypt(RouteData.Values["employeeid"].ToString())));

                txtbox_employcode.Text = _employee.employeecode;
                txtbox_fullname.Text = _employee.firstname;
                txtbox_date.Text = _date.ToString(AppModels.dateformat);

                DataTable result = _callController.GetDailySyncReportinfo(_employee.employeeid, _date.ToString());
                foreach (DataRow dr in result.Rows)
                {
                    txtbox_callDate.Text = Convert.ToDateTime(dr["firstcalltime"]).ToString("hh:mm tt");
                    txtbox_callDate1.Text = Convert.ToDateTime(dr["lastcalltime"]).ToString("hh:mm tt");

                    urlframe.Attributes.Add("src", Utility.getMapEmbeddedURL(dr["firstcall_longitude"].ToString(), dr["firstcall_latitude"].ToString()));
                    urlframe1.Attributes.Add("src", Utility.getMapEmbeddedURL(dr["lastcall_longitude"].ToString(), dr["lastcall_latitude"].ToString()));
                }

                grdv_doclist.DataSource = _callController.GetDailySyncReportinfoDoctinfo(_employee.employeeid, _date);
                grdv_doclist.DataBind();
                grdv_doclist.HeaderRow.TableSection = TableRowSection.TableHeader;
            }

        }
        public string getCallDetailsRoute(string id)
        {
            return GetRouteUrl(AppModels.Routes.calldetails, new { id = Auth.AppSecurity.URLEncrypt(id) });
        }
        protected void tbl_CallReportDetails_DataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Image imgsig = (Image)e.Row.FindControl("imgsig");
                imgsig.ImageUrl = AppModels.imageurl + DataBinder.Eval(e.Row.DataItem, "imageurl");
            }

        }
    }
}