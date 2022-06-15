using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace SimpleFFO.Model
{
    [Serializable]
    public class AppModels
    {
        //public const string baseurl = "https://localhost:44362";
        public const string simplesoftechconfigurl = "http://simplesoftech.com/serveripaddress/serverconfig.json";

        public const string dateformat = "MM/dd/yyyy";
        public const string datetimeformat = "MM/dd/yyyy hh:mm tt";
        public const string moneyformat = "N2";
        public const string decimaloneformat = "0.#";
        public const string decimaltwoformat = "0.##";
        public const int PrefKey_yearlyleavecount = 43;

        public const string odofolder = "ODO";
        public const string receiptfolder = "RECEIPTS";

        //public const string imageurl = "http://192.168.1.100:8082/sfaws/signatures/";
        public static string imageurl { get => System.Configuration.ConfigurationManager.AppSettings["imageurl"]; }
        public static string baseurl { get => System.Configuration.ConfigurationManager.AppSettings["baseurl"]; }
        public static string fcmsendurl { get => System.Configuration.ConfigurationManager.AppSettings["fcmsendurl"]; }
        public static string fcmsenderid { get => System.Configuration.ConfigurationManager.AppSettings["fcmsenderid"]; }
        public static string fcmserverkey { get => System.Configuration.ConfigurationManager.AppSettings["fcmserverkey"]; }

        public const string noimageurl = "images/noimage.png";


        public class Company
        {
            public const string name = "ECE";
            public const string appname = "Simple SFA";
        }
        public class Routes
        {
            public const string login = "login";
            public const string dashboard = "dashboard";
            public const string requestpractivity = "requestpractivity"; 
            public const string requestpractivitydetails = "requestpractivitydetails";
            public const string requestpractivitydetailsaction = "requestpractivitydetailsaction"; 
            public const string requeststop = "requeststop";
            public const string requeststopdetails = "requeststopdetails";
            public const string requeststopdetailsaction = "requeststopdetailsaction";
            public const string requesttup = "requesttup";
            public const string requesttupdetails = "requesttupdetails";
            public const string requesttupdetailsaction = "requesttupdetailsaction";
            public const string expensereport = "expensereport";
            public const string expensereportdetails = "expensereportdetails";
            public const string expensereportdetailsaction = "expensereportdetailsaction";

            public const string requestloadetails = "requestloadetails";
            public const string requestloa = "requestloa";
            public const string requestsalaryloan = "requestsalaryloan";
            public const string requestsalaryloandetails = "requestsalaryloandetails";
            public const string requestsalaryloandetailsaction = "requestsalaryloandetailsaction"; 

            public const string requestvehiclerepair = "vehiclerepair";
            public const string requestvehiclerepairdetails = "requestvehiclerepairdetails";
            public const string requestvehiclerepairdetailsaction = "requestvehiclerepairdetailsaction";

            public const string branchapproval = "branchapproval";
            public const string masterfiles = "masterfiles";

            public const string calldetails = "calldetails";
            public const string dailysyncdetails = "dailysyncdetails";
            public const string sfareports = "sfareports";
            public const string sfareportspsrperformance = "sfareportspsrperformance";

            public const string locationviewer = "locationviewer";
            public const string pagenotfound = "pagenotfound";

            public const string jointcalls = "jointcalls";
            public const string institutiondoctors = "institutiondoctors";
            public const string usertrails = "usertrails";
            public const string mobileapps = "mobileapps";

            public const string apis = "apis";

        }
        public class Modules
        {
            public const int practivity = 201;
            public const int stop = 202;
            public const int tup = 203;
            public const int leaverequest = 204;
            public const int vehiclerepair = 205;
            public const int expensereport = 206;
            public const int fundrequest = 207;
            public const int fundliquidation = 208;
            public const int salaryloan = 209;

            public const int companyvehicles = 301;
            public const int employees = 302;
            public const int employeetypes = 303;
            public const int repairshops = 304;
            public const int prodcuctcategories = 305;
            public const int products = 306;
            public const int users = 307;
            public const int warehouses = 308;
            public const int mdlist = 309;

            public static string getName(int id)
            {
                switch (id)
                {
                    case practivity:
                        return "Activity Proposal";
                    case stop:
                        return "S.T.O.P";
                    case tup:
                        return "T.U.P";
                    case leaverequest:
                        return "Leave of Absence";
                    case salaryloan:
                        return "Salary Loan";
                    case vehiclerepair:
                        return "Vehicle Repair";
                    case expensereport:
                        return "Expense Report";
                    case fundrequest:
                        return "Fund Request";
                    case fundliquidation:
                        return "Fund Liquidation";
                    default:
                        return "";
                }
            }
        }
        public class SubModules
        {
            public const int dashboard = 1;
            public const int sfareports = 2;
            public const int branchapproval = 3;
            public const int mdapproval = 4;
            public const int userlogs = 5;
        }
            public class Pages
        {
            public const string pageInstitutions = "institutions";
            public const string pageProducts = "products";
            public const string pageProductCategories = "productcategories";
            public const string pagesRepairShops = "repairshops";
            public const string pageVehicles = "vehicles";
            public const string pageEmployeeTypes = "employeetypes";
            public const string pageEmployees = "employees";
            public const string pageWarehouses = "warehouses";
            public const string pageUsers= "users";

            public const string pageCallReportAnalysis = "CallReportAnalysis";
            public const string pageCallReportDetails = "CallReportDetails";
            public const string pageDailyCallTracker = "DailyCallTracker";
            public const string pageDailySyncReport = "DailySyncReport";

            public static string getPageTitle(string pagename)
            {
                switch (pagename)
                {
                    case pageInstitutions:
                        return "Institutions";
                    case pageProducts:
                        return "Products";
                    case pageProductCategories:
                        return "Product Categories";
                    case pagesRepairShops:
                        return "Repair Shops";
                    case pageVehicles:
                        return "Company Vehicles";
                    case pageEmployeeTypes:
                        return "Employee Types";
                    case pageEmployees:
                        return "Employees";
                    case pageWarehouses:
                        return "Warehouses";
                    case pageUsers:
                        return "Users";
                    default:
                        return "";
                }
            }
        }
        public class EmployeeTypes
        {
            public const int bbdm = 111; //DISTRICT MANAGER
            public const int rbdm = 123; //REGIONAL SALES MANAGER
            public const int psr = 666; //PROFESSIONAL SALES REPRESENTATIVE
            public const int ptr = 555; //PROFESSIONAL TRADE REPRESENTATIVE
            public const int nsm = 124; //NATIONAL SALES MANAGER
            public const int smd = 125; //SALES AND MARKETING DIRECTOR
            public const int gm = 126; //GENERAL MANAGER
            public const int administrator = 222; //ADMINISTRATOR
        }

        public class SessionKeys
        {
            public const string warehouseid = "Warehouseid";
            public const string userfullname = "userfullname";
            public const string employeeid = "employeeid";
        }

        public class Status
        {
            public const int notapplicable = -1;
            public const int draft = 0;
            public const int cancelled = 1;
            public const int rejected = 2;
            public const int submitted = 3;
            public const int endorsed = 4;
            public const int approved = 5;
            public const int _checked = 6;
            public const int completed = 7;
            public const int forfundrequest = 21;
            public const int processing = 22;
            public const int fordisbursement = 23; 
            public const int forliquidation = 24;

            public static List<object> getListAction()
            {
                return new List<object> {
                    new { key = endorsed, value = "Endorse" },
                    new { key = approved, value ="Approve" },
                    new { key = _checked, value = "Check" },
                    new { key = fordisbursement, value = "Disburse" } };
            }
            public static string getStatus(int status)
            {
                switch (status)
                {
                    case draft:
                        return "Draft";
                    case cancelled:
                        return "Cancelled";
                    case submitted:
                        return "Submitted";
                    case rejected:
                        return "Rejected";
                    case endorsed:
                        return "For Endorsement";
                    case approved:
                        return "Approved";
                    case _checked:
                        return "Checked";
                    case completed:
                        return "Completed";
                    case forfundrequest:
                        return "For Fund Request";
                    case processing:
                        return "Processing";
                    case fordisbursement:
                        return "For Disbursement";
                    case forliquidation:
                        return "For Liquidation";
                    default:
                        return "";
                }
            }
            public static string getAction(int status)
            {
                switch (status)
                {
                    case endorsed:
                        return "Endorse";
                    case approved:
                        return "Approve";
                    case _checked:
                        return "Check";
                    case completed:
                        return "Complete";
                    case rejected:
                        return "Reject";
                    case fordisbursement:
                        return "Disburse";
                    default:
                        return "";
                }
            }
            public static string getClassColor(int status)
            {
                switch (status)
                {
                    case draft:
                        return "bg-default";
                    case cancelled:
                        return "bg-warning";
                    case submitted:
                        return "bg-primary";
                    case rejected:
                        return "bg-danger";
                    case endorsed:
                        return "bg-info";
                    case _checked:
                        return "bg-primary";
                    case approved:
                        return "bg-success";
                    case completed:
                        return "bg-success";
                    case forfundrequest:
                        return "bg-primary";
                    case processing:
                        return "bg-primary";
                    case fordisbursement:
                        return "bg-info";
                    case forliquidation:
                        return "bg-info";
                    default:
                        return "bg-default";
                }
            }
            public static string getBadge(int status)
            {
                switch (status)
                {
                    case draft:
                        return "badge bg-default";
                    case cancelled:
                        return "badge bg-warning";
                    case submitted:
                        return "badge bg-primary";
                    case rejected:
                        return "badge bg-danger";
                    case endorsed:
                        return "badge bg-info";
                    case _checked:
                        return "badge bg-success";
                    case approved:
                        return "badge bg-success";
                    case completed:
                        return "badge bg-success";
                    case forfundrequest:
                        return "badge bg-primary";
                    case processing:
                        return "badge bg-primary";
                    case fordisbursement:
                        return "badge bg-info";
                    case forliquidation:
                        return "badge bg-danger";
                    default:
                        return "badge bg-default";
                }
            }
        }
        public class DayTypes
        {
            public const string regularholiday = "2";
            public const string specialholiday = "3";
        }

        public class ItemTypes
        {
            public const int HB = 0;
            public const int OHB = 1;

            public static string getTypeName(int t)
            {
                switch (t)
                {
                    case HB:
                        return "House Brand";
                    case OHB:
                        return "Other House Brand";
                    default:
                        return "";
                }
            }
            public static List<object> getList()
            {
                return new List<object> { new { key = 0, value= "House Brand" }, new { key = 1, value = "Other House Brand" } };
            }
        }


        public class Tieup
        {
            public class TieupClass
            {
                public const int STOP = 0;
                public const int TUP = 1;
            }
            public class TieupType
            {
                public const int rebate = 0;
                public const int project = 1;


                public static List<object> getList()
                {
                    return new List<object> {
                    new { key = rebate, value = "Rebate" },
                    new { key = project, value = "Project" }};
                }
            }

            public class ClaimType
            {
                public const int advance = 0;
                public const int achievementroi = 1;
                public const int periodicrebate = 2;


                public static List<object> getList()
                {
                    return new List<object> {
                    new { key = advance, value = "Advance" },
                    new { key = achievementroi, value = "Achievement of Agreed upon percentage of ROI" },
                    new { key = periodicrebate, value = "Periodic Rebate" } };
                }
            }

            public class TieUpMode
            {
                public const int pushing = 0;
                public const int despensing_prescribing = 1;
                public const int combination = 2;


                public static List<object> getList(bool isdoctor = false)
                {
                    return new List<object> {
                    new { key = pushing, value = "Pushing" },
                    new { key = despensing_prescribing, value = isdoctor ? "Prescribing":"Despensing" },
                    new { key = combination, value = "Combination" } };
                }
            }
        }
        public class Funding
        {
            public class PaymentMode
            {
                public const int cash= 0;
                public const int banktransfer = 1;
                public const int check = 2;

                public static List<object> ToList()
                {
                    return new List<object> {
                    new { key = cash, value = "Cash" },
                    new { key = banktransfer, value = "Bank Transfer" },
                    new { key = check, value = "Check" } };
                }
                public static string Description(int mode)
                {
                    switch (mode)
                    {
                        case cash:
                            return "Cash";
                        case banktransfer:
                            return "Bank Transfer";
                        case check:
                            return "Check";
                        default:
                            return "-";
                    }
                }
            }
        }
        public class SFAReports
        {
            public const int dailycalltraccker = 1;
            public const int callreportanalysis = 2;
            public const int callreportdetails = 3;
            public const int dailysyncreport = 4;
            public const int callperformance = 5;
            public const int averagecallperday = 6;
            public const int weeklyincidental = 7;
            public const int callperformanceperclass = 8;
            public const int callperformanceperspecialty = 9;
            public const int cyclereportspecialization = 10;
            public const int jointcalls = 11;
            public const int servicecalls = 12; 
        }
        public class ErrorMessage
        {
            public const string required = "*This field is required";
            public const string recordexisting = "*This record already exist";
            public const string zeroabove = "*Must be greater than zero";
            public const string oneabove = "*Must be greater than or equal to 0";
            public const string invaliddate = "*Invalid Date";
            public const string invalidinput = "*Invalid Input";
            public const string duplicateentry = "*Duplicate Entry";
        }

        [Serializable]
        public class SimpleWebConfig
        {
            public sfadm sfadmversion { get; set; }

            [Serializable]
            public class sfadm
            {
               public string url { get; set; }
                public long version { get; set; }
                public string versioncode { get; set; }
            }
        }
    }
}