﻿using SimpleFFO.Model;
using System;
using System.Collections.Generic;
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
            return this.miscexpensecodes.Where(m => m.isactive ?? true).OrderBy(m => m.misccodename).ToList();
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
    }
}