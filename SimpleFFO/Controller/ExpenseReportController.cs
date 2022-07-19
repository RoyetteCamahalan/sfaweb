using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.Controller
{
    public class ExpenseReportController : SimpleDB
    {
        public List<transportationtype> GetTrasnportTypes()
        {
            return this.transportationtypes.Where(t => t.isactive ?? true).ToList();
        }
        public long GetCurrentOdometer(int warehouseid)
        {
            companyvehicle vehicle = warehouses.Where(w => w.warehouseid == warehouseid).First().companyvehicle;
            if (vehicle == null)
                return 0;
            else
                return vehicle.currentodo ?? 0;
        }
        public List<miscexpensecode> GetListMiscType()
        {
            int[] active = { 1, 2 };

            return this.miscexpensecodes.Where(m => active.Contains((int)m.isactive)).OrderBy(m => m.misccodename).ToList();
        }

        public expensereport GetExpensereport(long id)
        {
            if (id == 0)
                return new expensereport { 
                    expensereportid= this.expensereports.Select(s => s.expensereportid).DefaultIfEmpty(0).Max() + 1
                };
            return expensereports.Find(id);
        }
  

        public expensereport GetLastExpensereport(long vehicleid)
        {
            return this.expensereports.Where(e => e.vehicleid==vehicleid && e.status != AppModels.Status.rejected).OrderByDescending(e => e.dateend).FirstOrDefault();
        }

        public long getWeekifExist(long warehouseid,DateTime sdate)
        {
            expensereport er = this.expensereports.Where(e => e.warehouseid == warehouseid && (e.datefrom ?? DateTime.MaxValue) == sdate.Date).FirstOrDefault();
            if (er == null)
                return 0;
            else
                return er.expensereportid;
        }
        public void SaveReportype(List<Exptype> list)
        {
            bool validation;

            foreach (var data in list)
            {
                if (validation = confimation(data.expenseid))
                {
                    miscexpensecode result = new miscexpensecode
                    {
                        misccodeid = data.expenseid,
                        misccodename = data.expensetype,
                        isactive = data.status,
                        createdbyid = 0,
                        created_at = DateTime.Now

                    };
                    this.miscexpensecodes.Add(result);
                }
                this.SaveChanges();
            }
        }
        private bool confimation(long id)
        {
            var query = this.miscexpensecodes.Where(m => m.misccodeid == id).Select(m => m.misccodeid).ToList();
            if (query.Count > 0)
                return false;
            return true;
        }

        public void SaveExpensReport(bool isnew, expensereport expensereport, List<expensereportdetail> expensereportdetails, List<expensereportmiscellaneou> expensereportmiscellaneous)
        {
            //remove items from list
            foreach (var existingChild in expensereport.expensereportdetails.ToList())
            {
                if (!expensereportdetails.Any(c => c.expensereportdetailid == existingChild.expensereportdetailid))
                    this.expensereportdetails.Remove(existingChild);
            }
            foreach (var existingChild in expensereport.expensereportmiscellaneous.ToList())
            {
                if (!expensereportmiscellaneous.Any(c => c.miscexpenseid == existingChild.miscexpenseid))
                    this.expensereportmiscellaneous.Remove(existingChild);
            }

            //add/update list
            foreach (expensereportdetail a in expensereportdetails)
            {
                a.expensereportid = expensereport.expensereportid;
                if (a.expensereportdetailid == 0)
                    expensereport.expensereportdetails.Add(a);
                else
                {
                    this.Entry(expensereport.expensereportdetails.Where(p => p.expensereportdetailid == a.expensereportdetailid).First()).CurrentValues.SetValues(a);
                }
            }
            foreach (expensereportmiscellaneou a in expensereportmiscellaneous)
            {
                a.expensereportid = expensereport.expensereportid;
                if (a.miscexpenseid == 0)
                    expensereport.expensereportmiscellaneous.Add(a);
                else
                {
                    this.Entry(expensereport.expensereportmiscellaneous.Where(p => p.miscexpenseid == a.miscexpenseid).First()).CurrentValues.SetValues(a);
                }
            }

            if (isnew)
                this.expensereports.Add(expensereport);


            if (expensereport.status == AppModels.Status.submitted)
            {
                this.Entry(expensereport).Reference(c => c.warehouse).Load();
                statustrail st = new statustrail
                {
                    moduleid = AppModels.Modules.expensereport,
                    requestid = expensereport.expensereportid,
                    statusid = expensereport.status,
                    traildate = DateTime.Now,
                    employeeid = expensereport.warehouse.employees.First().employeeid,
                    remarks = "",
                    treelevel = 0
                };
                this.statustrails.Add(st);

                var districtmanagerid = expensereport.warehouse.employees.First().districtmanagerid;
                if (expensereport.warehouse.employees.First().employeetypeid == 111)
                {
                    districtmanagerid = expensereport.warehouse.employees.First().employeeid;
                }
                List<approvaltree> lsttrees = this.approvaltrees.Where(t => t.districtmanagerid == districtmanagerid && t.moduleid == AppModels.Modules.expensereport).OrderBy(t => t.treelevel).ToList();
                foreach (approvaltree tree in lsttrees)
                {
                    if (tree.employeeid != expensereport.warehouse.employees.First().employeeid)
                    {
                        expensereport.endorsedto = tree.employeeid;
                        break;
                    }
                }
                //Update lastodo
                if (expensereport.vehicleid > 0)
                {
                    expensereport laster = GetLastExpensereport(expensereport.vehicleid ?? 0);
                    if (laster == null || (laster != null && (laster.dateend ?? DateTime.MinValue).Date <= (expensereport.dateend ?? DateTime.MinValue)))
                    {
                        companyvehicle c = this.companyvehicles.Find(expensereport.vehicleid);
                        c.currentodo = expensereport.expensereportdetails.Select(er => er.totalkm ?? 0).DefaultIfEmpty(0).Max();
                    }
                }
            }
            this.SaveChanges();
        }


        public List<PostExpenseReports> GetPostExpense(long expensereportid)
        {
            List<PostExpenseReports> expense = (from er in this.expensereports
                                                join ed in this.expensereportdetails on er.expensereportid equals ed.expensereportid
                                                join fr in this.fundrequests on er.expensereportid equals fr.requestid
                                                join em in this.expensereportmiscellaneous on new { key1 = ed.expensereportid, key2 = ed.date} equals new { key1 = em.expensereportid, key2 = em.expensedate } into result
                                                from ex in result.DefaultIfEmpty()
                                                where er.expensereportid == expensereportid
                                                orderby ed.date
                                                select new PostExpenseReports()
                                                {
                                                     dateencoded = ed.date,
                                                     vendorid = 0,
                                                     expenseid = 0,
                                                     refno = fr.disbursementref,
                                                     refdate = fr.disbursementdate,
                                                     amount = ed.totaldaily,
                                                     vat = ex.isvat,
                                                     remarks = ex.particulars,
                                                     ffo_expensereportid = er.expensereportid
                                                }).ToList<PostExpenseReports>();
            return expense;
        }


        public List<supplier> GetVendors(int branchid)
        {
            return this.suppliers.Where(s => s.branchid == branchid).ToList();
        }

        public List<expensereportmiscellaneou> GetpostExpMisc(long expensereportid)
        {
            return this.expensereportmiscellaneous.Where(m => m.expensereportid == expensereportid).ToList();
        }


    }
}


//SELECT ed.date dateencoded, 0 vendorid,0 expenseid, fr.disbursementref refno,
// fr.disbursementdate refdate, ed.totaldaily amount,1 vat, em.particulars remarks, er.expensereportid FROM expensereports er
//INNER JOIN expensereportdetails ed on er.expensereportid = ed.expensereportid
//left JOIN expensereportmiscellaneous em on er.expensereportid = em.expensereportid AND ed.date = em.expensedate
//INNER JOIN fundrequests fr on er.expensereportid = fr.requestid and 206 = fr.moduleid
//ORDER BY dateencoded