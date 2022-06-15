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
    public partial class JointCallInfo : System.Web.UI.Page
    {
        JointCallController jointcallcontroller;
        EmployeeController employeeController;
        CycleController cycleController;

        #region "Vars"
        private long call_id
        {
            get { return (long)ViewState["call_id"]; }
            set { ViewState["call_id"] = value;  }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            jointcallcontroller = new JointCallController();
            employeeController = new EmployeeController();
            cycleController = new CycleController();

            if (!IsPostBack)
            {
                if (Page.RouteData.Values.ContainsKey("id"))
                {
                    this.call_id = Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["id"].ToString()));
                    jointcall jointcall = jointcallcontroller.GetjointCall(call_id);
                    cycleday cycleday = cycleController.GetCycleDay(Convert.ToInt32(jointcall.cycle_day_id));


                    if (jointcall.warehouse_id > 0)
                    {
                        divservice.Visible = false;
                        employee employee = employeeController.GetEmployeeByEmployees(Convert.ToInt64(jointcall.warehouse_id));

                        List<object> callmatslst = jointcallcontroller.GetCallMaterials(call_id);
                        List<object> callprdlst = jointcallcontroller.GetCallProducts(call_id);

                        if (jointcall == null)
                            return;

                        txtbox_fullname.Text = employee.firstname;
                        txtbox_branchname.Text = employee.branch.branchname;

                        txtbox_callDate.Text = (jointcall.start_datetime ?? DateTime.MinValue).ToString("dddd, dd MMMM yyyy");
                        txtbox_started.Text = (jointcall.start_datetime ?? DateTime.MinValue).ToString("hh:mm tt");
                        txtbox_ended.Text = (jointcall.end_datetime ?? DateTime.MinValue).ToString("hh:mm tt");
                        txtbox_planned.Text = (jointcall.planned ?? false) ? "Yes" : "No";

                        if (jointcall.signatures.FirstOrDefault() != null)
                            imgSRC.ImageUrl = AppModels.imageurl + jointcall.signatures.First().path;
                        else
                            imgSRC.ImageUrl = AppModels.noimageurl;

                        grddoclst.DataSource = jointcall.serviceattendees.ToList();
                        grddoclst.DataBind();

                        grdmaterialst.DataSource = callmatslst;
                        grdmaterialst.DataBind();

                        grdproductlst.DataSource = callprdlst;
                        grdproductlst.DataBind();

                        grdnotes.DataSource = jointcall.callnotes.ToList();
                        grdnotes.DataBind();

                        lstrating.DataSource = jointcall.callevalnotes.OrderBy(x => x.evaluationtype.evaltypepriority).ToList();
                        lstrating.DataBind();

                        urlframe.Attributes.Add("src", Utility.getMapEmbeddedURL(jointcall.longitude, jointcall.latitude));
                    }
                    else
                    {
                        row1.Visible = false;
                        row2.Visible = false;
                        divmats.Visible = false;
                        divloc.Visible = false;
                        dvrate.Visible = false;
                        divservice.Visible = true;

                        service service = jointcallcontroller.GetServicecall(Convert.ToInt32(jointcall.serviceid));

                        txtbox_servicename.Text = service.servicename;
                        txtbox_servicedate.Text = cycleday.date.ToString();
                        txtbox_observation.Text = jointcall.observation;

                        servicelst.DataSource = jointcall.serviceattendees.ToList(); ;
                        servicelst.DataTextField = "others";
                        servicelst.DataValueField = "attendeeid";
                        servicelst.DataBind();
                    }


                }
            }

        }

        protected void lstrating_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if(e.Item.ItemType == ListViewItemType.DataItem)
            {
                ListView lst = (ListView)e.Item.FindControl("lstcallevals");
                lst.DataSource = jointcallcontroller.GetJointRate(this.call_id, Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "evaltypeid")));
                lst.DataBind();
            }
        }
    }
}