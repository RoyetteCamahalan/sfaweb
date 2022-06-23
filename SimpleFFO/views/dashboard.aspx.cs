using Newtonsoft.Json;
using SimpleFFO.Controller;
using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.views
{
    public partial class dashboard : System.Web.UI.Page
    {
        Auth auth;
        DashBoardController dashBoardController;
        SupplierController supplierController;
        List<vendorcategory> lstcategory;

        #region "Vars"
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            auth = new Auth(this);
            if (!auth.hasAuth())
                return;

            dashBoardController = new DashBoardController();
            supplierController = new SupplierController();
            if (!Page.IsPostBack)
            {
                dashBoardController._employeeid = (long)auth.currentuser.employeeid;
                dashBoardController._warehouseid = auth.warehouseid;
                Session["update"] = Server.UrlEncode(System.DateTime.Now.ToString());
                DisplayList();
                this.lblDraftCount.Text = dashBoardController.GetCountbyStatus(0).ToString();
                this.lblSubmittedCount.Text = dashBoardController.GetCountbyStatus(1).ToString();
                this.lblApprovedCount.Text = dashBoardController.GetCountbyStatus(2).ToString();
                ImplementPrivileges();
                webserviceresult();

            }
            else
            {
                tbl_main.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }
        private void webserviceresult()
        {
            FFOPettyCashWS.Service1 options = new FFOPettyCashWS.Service1();
            if(auth.currentuser.employee.employeetypeid != 222)
            {
                lstcategory = JsonConvert.DeserializeObject<List<vendorcategory>>(options.Download_Data(auth.GetToken(), FFOPettyCashWS.myTransactCode.CGetVendorCategory, "0"));
                supplierController.SaveVendorCategory(lstcategory);
            }
           
        }
        private void ImplementPrivileges()
        {
            usersubpriv usp = auth.GetSubUserpriv(AppModels.SubModules.dashboard);
            if(!(usp.canaccess ?? false))
            {
                usp = auth.GetSubUserpriv(AppModels.SubModules.sfareports);
                if(usp.canaccess ?? false)
                    Response.RedirectToRoute(AppModels.Routes.sfareports);
            }
        }
        private void DisplayList()
        {
            List<GenericObject> lsttoreview = dashBoardController.getForApproval(auth.currentuser.employeeid ?? 0);
            dgvtoreview.DataSource = lsttoreview;
            dgvtoreview.DataKeyNames = new string[] { "id", "moduleid" };
            dgvtoreview.DataBind();
            if (lsttoreview.Count == 0)
                paneltoreview.Visible = false;

            tbl_main.DataSource = dashBoardController.GetRequestList();
            tbl_main.DataKeyNames = new string[] { "id", "moduleid" };
            tbl_main.DataBind();
            tbl_main.HeaderRow.TableSection = TableRowSection.TableHeader;
        }
        public string getRequestStatus(int status)
        {
            return AppModels.Status.getStatus(status);
        }
        public string getRequestStatusBadge(int status)
        {
            return AppModels.Status.getBadge(status);
        }
        public string RequestDescription(GenericObject g)
        {
            switch (g.moduleid)
            {
                case AppModels.Modules.practivity:
                    practivity pr = this.dashBoardController.practivities.Where(p => p.practivityid == g.id).FirstOrDefault();
                    if (pr.inst_id > 0)
                    {
                        return pr.activity.activityname + " - " + pr.institution.inst_name;
                    }
                    else if (pr.doc_id > 0)
                    {
                        return pr.activity.activityname + " - " + pr.doctor.doc_firstname + " " + pr.doctor.doc_lastname;
                    }
                    break;
                case AppModels.Modules.stop:
                case AppModels.Modules.tup:
                    tieup tieup = this.dashBoardController.tieups.Where(t => t.tieupid == g.id).FirstOrDefault();
                    return AppModels.ItemTypes.getTypeName(tieup.producttype ?? 0) + " - " + g.reference;
                case AppModels.Modules.leaverequest:
                    return g.reference + g.dayfrom.ToString(AppModels.dateformat);
                case AppModels.Modules.salaryloan:
                    return g.reference + " : P " + g.totalamount.ToString(AppModels.moneyformat);
                case AppModels.Modules.vehiclerepair:
                    return g.reference + " : P " + g.totalamount.ToString(AppModels.moneyformat);
                case AppModels.Modules.expensereport:
                    expensereport e = this.dashBoardController.expensereports.Where(t => t.expensereportid == g.id).FirstOrDefault();
                    return (e.datefrom ?? DateTime.MaxValue).ToString(AppModels.dateformat) + "-" + (e.dateend ?? DateTime.MaxValue).ToString(AppModels.dateformat);
                case AppModels.Modules.fundrequest:
                    fundrequest f = this.dashBoardController.fundrequests.Find(g.id);
                    if (f.moduleid == AppModels.Modules.practivity)
                        return "P " + (f.amount ?? 0).ToString(AppModels.moneyformat) + " | Activity | " + RequestDescription(new GenericObject { id = (f.requestid ?? 0), moduleid = (f.moduleid ?? 0) });
                    else if (f.moduleid == AppModels.Modules.stop)
                        return "P " + (f.amount ?? 0).ToString(AppModels.moneyformat) + " | STOP | " + RequestDescription(new GenericObject { id = (f.requestid ?? 0), moduleid = (f.moduleid ?? 0) });
                    else if (f.moduleid == AppModels.Modules.tup)
                        return "P " + (f.amount ?? 0).ToString(AppModels.moneyformat) + " | TUP | " + RequestDescription(new GenericObject { id = (f.requestid ?? 0), moduleid = (f.moduleid ?? 0) });
                    else if (f.moduleid == AppModels.Modules.expensereport)
                        return "P " + (f.amount ?? 0).ToString(AppModels.moneyformat) + " | Expense Report | " + RequestDescription(new GenericObject { id = (f.requestid ?? 0), moduleid = (f.moduleid ?? 0) });
                    else if (f.moduleid == AppModels.Modules.salaryloan)
                        return "Salaray Loan | P " + (f.amount ?? 0).ToString(AppModels.moneyformat);
                    else
                        return "P " + (f.amount ?? 0).ToString(AppModels.moneyformat);
                case AppModels.Modules.fundliquidation:
                    fundliquidation fl = this.dashBoardController.fundliquidations.Find(g.id);
                    if (fl.moduleid == AppModels.Modules.practivity)
                        return "P " + (fl.amount ?? 0).ToString(AppModels.moneyformat) + " | Activity | " + RequestDescription(new GenericObject { id = (fl.requestid ?? 0), moduleid = (fl.moduleid ?? 0) });
                    else if (fl.moduleid == AppModels.Modules.stop)
                        return "P " + (fl.amount ?? 0).ToString(AppModels.moneyformat) + " | STOP | " + RequestDescription(new GenericObject { id = (fl.requestid ?? 0), moduleid = (fl.moduleid ?? 0) });
                    else if (fl.moduleid == AppModels.Modules.tup)
                        return "P " + (fl.amount ?? 0).ToString(AppModels.moneyformat) + " | TUP | " + RequestDescription(new GenericObject { id = (fl.requestid ?? 0), moduleid = (fl.moduleid ?? 0) });
                    else if (fl.moduleid == AppModels.Modules.vehiclerepair)
                        return "P " + (fl.amount ?? 0).ToString(AppModels.moneyformat) + " | VRR | " + RequestDescription(new GenericObject { id = (fl.requestid ?? 0), moduleid = (fl.moduleid ?? 0) });
                    else
                        return "P " + (fl.amount ?? 0).ToString(AppModels.moneyformat);
            }
            return g.reference;
        }

        public string getElementRoute(int moduleid, long id, bool withaction = false)
        {

            switch (moduleid)
            {
                case AppModels.Modules.practivity:
                    if (withaction)
                        return GetRouteUrl(AppModels.Routes.requestpractivitydetailsaction, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()), action = "funding" });
                    return GetRouteUrl(AppModels.Routes.requestpractivitydetails, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()) });
                case AppModels.Modules.stop:
                    if (withaction)
                        return GetRouteUrl(AppModels.Routes.requeststopdetailsaction, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()), action = "funding" });
                    return GetRouteUrl(AppModels.Routes.requeststopdetails, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()) });
                case AppModels.Modules.tup:
                    if (withaction)
                        return GetRouteUrl(AppModels.Routes.requesttupdetailsaction, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()), action = "funding" });
                    return GetRouteUrl(AppModels.Routes.requesttupdetails, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()) });
                case AppModels.Modules.leaverequest:
                    return GetRouteUrl(AppModels.Routes.requestloadetails, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()) });
                case AppModels.Modules.salaryloan:
                    if (withaction)
                        return GetRouteUrl(AppModels.Routes.requestsalaryloandetailsaction, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()), action = "funding" });
                    return GetRouteUrl(AppModels.Routes.requestsalaryloandetails, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()) });
                case AppModels.Modules.expensereport:
                    if (withaction)
                        return GetRouteUrl(AppModels.Routes.expensereportdetailsaction, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()), action = "funding" });
                    return GetRouteUrl(AppModels.Routes.expensereportdetails, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()) });
                case AppModels.Modules.vehiclerepair:
                    if (withaction)
                        return GetRouteUrl(AppModels.Routes.requestvehiclerepair, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()), action = "funding" });
                    return GetRouteUrl(AppModels.Routes.requestvehiclerepairdetails, new { id = Auth.AppSecurity.URLEncrypt(id.ToString()) });
                case AppModels.Modules.fundrequest:
                    fundrequest f = this.dashBoardController.fundrequests.Find(id);
                    return getElementRoute(f.moduleid ?? 0, f.requestid ?? 0, true);
                case AppModels.Modules.fundliquidation:
                    fundliquidation ff = this.dashBoardController.fundliquidations.Find(id);
                    return getElementRoute(ff.moduleid ?? 0, ff.requestid ?? 0, true);
                default:
                    return "";
            }
        }

        protected void btnaction_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect(getElementRoute(Convert.ToInt32(tbl_main.DataKeys[Convert.ToInt32(btn.CommandArgument)]["moduleid"]), Convert.ToInt64(tbl_main.DataKeys[Convert.ToInt32(btn.CommandArgument)]["id"])));
        }

        protected void btnrequestypelink_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect(getElementRoute(Convert.ToInt32(dgvtoreview.DataKeys[Convert.ToInt32(btn.CommandArgument)]["moduleid"]), Convert.ToInt64(dgvtoreview.DataKeys[Convert.ToInt32(btn.CommandArgument)]["id"])));
        }

        protected void tbl_main_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton btnfundrequest = (LinkButton)e.Row.FindControl("btnfundrequest");
                Label txtAccoutingStatus = (Label)e.Row.FindControl("txtAccoutingStatus");
                GenericObject obj = (GenericObject)e.Row.DataItem;
                if (obj.moduleid == AppModels.Modules.practivity || obj.moduleid == AppModels.Modules.stop || obj.moduleid == AppModels.Modules.tup)
                {
                    if (obj.status == AppModels.Status.approved)
                    {
                        btnfundrequest.Visible = true;
                        List<fundrequest> fundrequests = dashBoardController.fundrequests.Where(x => x.moduleid == obj.moduleid && x.requestid == obj.id).ToList();
                        List<fundliquidation> fundliquidations = dashBoardController.fundliquidations.Where(x => x.moduleid == obj.moduleid && x.requestid == obj.id).ToList();
                        decimal totaldisburse = fundrequests.Where(f => f.status == AppModels.Status.completed && (f.isdisburse ?? false)).Sum(x => x.amount).GetValueOrDefault();
                        decimal totalfundliquidation = fundliquidations.Where(f => f.status != AppModels.Status.rejected).Sum(y => y.amount).GetValueOrDefault();
                        if(fundrequests.Where(f=>f.status!=AppModels.Status.rejected && f.status!= AppModels.Status.approved && f.status != AppModels.Status.completed).Count()>0)
                        {
                            txtAccoutingStatus.Text = getRequestStatus(AppModels.Status.processing);
                            txtAccoutingStatus.CssClass = getRequestStatusBadge(AppModels.Status.processing);
                        }
                        else if (fundrequests.Where(f => f.status == AppModels.Status.approved && (f.isdisburse ?? false)==false).Count()>0)
                        {
                            txtAccoutingStatus.Text = getRequestStatus(AppModels.Status.fordisbursement);
                            txtAccoutingStatus.CssClass = getRequestStatusBadge(AppModels.Status.fordisbursement);
                        }
                        else if(totaldisburse>0 && totaldisburse > totalfundliquidation)
                        {
                            txtAccoutingStatus.Text = getRequestStatus(AppModels.Status.forliquidation);
                            txtAccoutingStatus.CssClass = getRequestStatusBadge(AppModels.Status.forliquidation);
                        }
                        else if (fundrequests.Where(f => f.status != AppModels.Status.rejected).Sum(x => x.amount).GetValueOrDefault()<obj.totalamount)
                        {
                            txtAccoutingStatus.Text = getRequestStatus(AppModels.Status.forfundrequest);
                            txtAccoutingStatus.CssClass = getRequestStatusBadge(AppModels.Status.forfundrequest);
                        }
                        else
                        {
                            txtAccoutingStatus.Text = getRequestStatus(AppModels.Status.completed);
                            txtAccoutingStatus.CssClass = getRequestStatusBadge(AppModels.Status.completed);
                        }
                    }
                    else
                    {
                        txtAccoutingStatus.Text = getRequestStatus(AppModels.Status.notapplicable);
                        txtAccoutingStatus.CssClass = getRequestStatusBadge(AppModels.Status.notapplicable);
                    }

                }else if((obj.moduleid == AppModels.Modules.expensereport || obj.moduleid == AppModels.Modules.salaryloan) && obj.status == AppModels.Status.approved)
                {
                    btnfundrequest.Visible = true;
                    List<fundrequest> fundrequests = dashBoardController.fundrequests.Where(x => x.moduleid == obj.moduleid && x.requestid == obj.id).ToList();
                    decimal totaldisburse = fundrequests.Where(f => f.status == AppModels.Status.completed && (f.isdisburse ?? false)).Sum(x => x.amount).GetValueOrDefault();
                    if (fundrequests.Where(f => f.status != AppModels.Status.rejected && (f.isdisburse ?? false)).Count() == 0)
                    {
                        txtAccoutingStatus.Text = getRequestStatus(AppModels.Status.processing);
                        txtAccoutingStatus.CssClass = getRequestStatusBadge(AppModels.Status.processing);
                    }
                    else if (fundrequests.Where(f => f.status == AppModels.Status.approved && (f.isdisburse ?? false) == false).Count() > 0)
                    {
                        txtAccoutingStatus.Text = getRequestStatus(AppModels.Status.fordisbursement);
                        txtAccoutingStatus.CssClass = getRequestStatusBadge(AppModels.Status.fordisbursement);
                    }else if (totaldisburse == 0)
                    {
                        txtAccoutingStatus.Text = getRequestStatus(AppModels.Status.processing);
                        txtAccoutingStatus.CssClass = getRequestStatusBadge(AppModels.Status.processing);
                    }
                    else
                    {
                        txtAccoutingStatus.Text = getRequestStatus(AppModels.Status.completed);
                        txtAccoutingStatus.CssClass = getRequestStatusBadge(AppModels.Status.completed);
                    }
                }
                else
                {
                    txtAccoutingStatus.Text = getRequestStatus(obj.statusacc);
                    txtAccoutingStatus.CssClass = getRequestStatusBadge(obj.statusacc);
                }
            }
        }

        protected void btnfundrequest_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Response.Redirect(getElementRoute(Convert.ToInt32(tbl_main.DataKeys[Convert.ToInt32(btn.CommandArgument)]["moduleid"]), Convert.ToInt64(tbl_main.DataKeys[Convert.ToInt32(btn.CommandArgument)]["id"]),true));
        }

        /*protected void btnfundrequest_Click(object sender, EventArgs e)
        {
            LinkButton btn = (LinkButton)sender;
            Server.Transfer(getElementRoute(Convert.ToInt32(tbl_main.DataKeys[Convert.ToInt32(btn.CommandArgument)]["moduleid"]), Convert.ToInt64(tbl_main.DataKeys[Convert.ToInt32(btn.CommandArgument)]["id"]), true));
            //Response.Redirect(getElementRoute(Convert.ToInt32(tbl_main.DataKeys[Convert.ToInt32(btn.CommandArgument)]["moduleid"]), Convert.ToInt64(tbl_main.DataKeys[Convert.ToInt32(btn.CommandArgument)]["id"]), true));
        }*/
    }
}