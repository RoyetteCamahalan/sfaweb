using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;
using SimpleFFO.Model;

namespace SimpleFFO
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            //settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);

            routes.MapHttpRoute(AppModels.Routes.apis, "api/{controller}/{action}/{id}", defaults: new { id = System.Web.Http.RouteParameter.Optional });

            //Master Files
            RouteTable.Routes.MapPageRoute(AppModels.Routes.masterfiles, "masterfiles/{targetpage}", "~/views/MainPage.aspx");
            //SFA Report Files
            RouteTable.Routes.MapPageRoute(AppModels.Routes.sfareports, "sfareports", "~/views/SFAReports.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.sfareportspsrperformance, "sfareportspsrperformance/{employeeid}/{year}/{cyclenumber}", "~/views/SFAReports.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.calldetails, "calldetails/{id}", "~/views/CallReportDetailInfo.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.dailysyncdetails, "dailysyncdetails/{employeeid}/{date}", "~/views/CallSyncReportInfo.aspx");

            //Request
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestpractivity, "request/practivity", "~/views/PRActivity.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestpractivitydetails, "request/practivity/{id}", "~/views/PRActivity.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestpractivitydetailsaction, "request/practivity/{id}/{action}", "~/views/PRActivity.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requeststop, "request/stop", "~/views/STOP.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requeststopdetails, "request/stop/{id}", "~/views/STOP.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requeststopdetailsaction, "request/stop/{id}/{action}", "~/views/STOP.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requesttup, "request/tup", "~/views/TUP.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requesttupdetails, "request/tup/{id}", "~/views/TUP.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requesttupdetailsaction, "request/tup/{id}/{action}", "~/views/TUP.aspx");

            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestloa, "request/leaveofabsence", "~/views/LOA.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestloadetails, "request/leaveofabsence/{id}", "~/views/LOA.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestsalaryloan, "request/salaryloan", "~/views/SalaryLoan.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestsalaryloandetails, "request/salaryloan/{id}", "~/views/SalaryLoan.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestsalaryloandetailsaction, "request/salaryloan/{id}/{action}", "~/views/SalaryLoan.aspx");


            RouteTable.Routes.MapPageRoute(AppModels.Routes.expensereport, "expensereport", "~/views/ExpenseReport.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.expensereportdetails, "expensereports/{id}", "~/views/ExpenseReport.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.expensereportdetailsaction, "expensereports/{id}/{action}", "~/views/ExpenseReport.aspx");

            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestvehiclerepair, "request/vehiclerepair", "~/views/RepairVehicle.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestvehiclerepairdetails, "request/vehiclerepair/{id}", "~/views/RepairVehicle.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.requestvehiclerepairdetailsaction, "request/vehiclerepair/{id}/{action}", "~/views/RepairVehicle.aspx");

            RouteTable.Routes.MapPageRoute(AppModels.Routes.branchapproval, "branchapproval", "~/views/ApprovalTree.aspx");

            RouteTable.Routes.MapPageRoute(AppModels.Routes.dashboard, "dashboard", "~/views/dashboard.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.login, "login", "~/views/Login.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.pagenotfound, "404", "~/views/404pagenotfound.aspx");

            RouteTable.Routes.MapPageRoute(AppModels.Routes.locationviewer, "locationviewer/{latitude}/{longitude}", "~/views/LocationViewer.aspx");

            RouteTable.Routes.MapPageRoute("sfaws", "sfaws", "~/sfaws/sfawebservice.asmx");


            RouteTable.Routes.MapPageRoute("default", "default", "~/views/dashboard.aspx");
            RouteTable.Routes.MapPageRoute("home", "home", "~/views/dashboard.aspx");
            RouteTable.Routes.MapPageRoute("index", "index", "~/views/dashboard.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.jointcalls, "callid/{id}", "~/views/JointCallinfo.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.institutiondoctors, "institutiondoctors", "~/views/InstitutionDoctors.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.usertrails, "usertrails", "~/views/UserLogs.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.errorlogs, "errorlogs", "~/views/ErrorLogs.aspx");
            RouteTable.Routes.MapPageRoute(AppModels.Routes.mobileapps, "mobileapps", "~/views/MobileApps.aspx");
        }
    }
}
