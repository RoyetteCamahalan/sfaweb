using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class DashBoardController : SimpleDB
    {
        public long _warehouseid = 0;
        public long _employeeid = 0;
        public List<GenericObject> GetRequestList(string search = "")
        {
            List<GenericObject> requests = (from pr in this.practivities
                                     join a in this.activities on pr.activityid equals a.activitiyid
                                     where pr.warehouseid == this._warehouseid
                                     select new GenericObject()
                                     {
                                         id = pr.practivityid,
                                         date = pr.datecreated ?? DateTime.MaxValue,
                                         reference = a.activityname + " - ",
                                         status = pr.status ?? 0,
                                         statusacc = pr.statusacc ?? 0,
                                         moduleid = AppModels.Modules.practivity,
                                         dayfrom = DateTime.MinValue,
                                         totalamount = pr.totalbudget ?? 0
                                     })
                                   .Concat(from t in this.tieups
                                           join i in this.institutions on t.inst_id equals i.inst_id
                                           where t.warehouseid == this._warehouseid && t.tieupclass == AppModels.Tieup.TieupClass.STOP
                                           select new GenericObject()
                                           {
                                               id = t.tieupid,
                                               date = t.datecreated ?? DateTime.MinValue,
                                               reference = i.inst_name,
                                               status = t.status ?? 0,
                                               statusacc = AppModels.Status.notapplicable,
                                               moduleid = AppModels.Modules.stop,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = (t.totalmonthlyrebates ?? 0) * (t.duration ?? 0)
                                           })
                                   .Concat(from t in this.tieups
                                           join d in this.doctors on t.doc_id equals d.doc_id
                                           where t.warehouseid == this._warehouseid && t.tieupclass == AppModels.Tieup.TieupClass.TUP
                                           select new GenericObject()
                                           {
                                               id = t.tieupid,
                                               date = t.datecreated ?? DateTime.MinValue,
                                               reference = d.doc_firstname + " " + d.doc_lastname,
                                               status = t.status ?? 0,
                                               statusacc = AppModels.Status.notapplicable,
                                               moduleid = AppModels.Modules.tup,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = (t.totalmonthlyrebates ?? 0) * (t.duration ?? 0)
                                           })
                                   .Concat(from l in this.employeeleaves
                                           join lt in this.leavetypes on l.leavetypeid equals lt.leavetypeid
                                           where l.employeeid == this._employeeid
                                           select new GenericObject()
                                           {
                                               id = l.leaveid,
                                               date = l.datecreated ?? DateTime.MinValue,
                                               reference = lt.leavetypename + " - ",
                                               status = l.status ?? 0,
                                               statusacc = AppModels.Status.notapplicable,
                                               moduleid = AppModels.Modules.leaverequest,
                                               dayfrom = (l.dayfrom ?? DateTime.MinValue),
                                               totalamount = 0
                                           })
                                   .Concat(from s in this.salaryloans
                                           where s.employeeid == this._employeeid
                                           select new GenericObject()
                                           {
                                               id = s.salaryloanid,
                                               date = s.created_at ?? DateTime.MinValue,
                                               reference = "Salary Loan",
                                               status = s.status ?? 0,
                                               statusacc = AppModels.Status.notapplicable,
                                               moduleid = AppModels.Modules.salaryloan,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = s.amount ?? 0
                                           })
                                   .Concat(from e in this.expensereports
                                           where e.warehouseid == this._warehouseid
                                           select new GenericObject()
                                           {
                                               id = e.expensereportid,
                                               date = e.datefiled ?? DateTime.MinValue,
                                               reference = "",
                                               status = e.status ?? 0,
                                               statusacc = AppModels.Status.notapplicable,
                                               moduleid = AppModels.Modules.expensereport,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = 0
                                           })
                                   .Concat(from v in this.vehiclerepairs
                                               //join vrd in this.vehiclerepairdetails on v.vehicleid equals vrd.vehiclerepairid
                                           where v.warehouseid == this._warehouseid
                                           select new GenericObject()
                                           {
                                               id = (long)v.vehiclerepairid,
                                               date = v.datefiled ?? DateTime.MinValue,
                                               reference = "",
                                               status = v.status ?? 0,
                                               statusacc = AppModels.Status.notapplicable,
                                               moduleid = AppModels.Modules.vehiclerepair,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = v.totalamount ?? (v.avgamount ?? 0)
                                           }).OrderByDescending(o => o.date).ToList();

            return requests;
        }

        public List<GenericObject> getForApproval(long employeeid)
        {
            var lst = (from e in this.employees
                                join at in approvaltrees on new { key1 = (e.employeetypeid==111 ? e.employeeid : e.districtmanagerid), key2 = AppModels.Modules.practivity } equals new { key1 = at.districtmanagerid, key2 = (int)at.moduleid}
                                join pr in this.practivities on e.warehouseid equals pr.warehouseid
                                where at.employeeid == employeeid && (((pr.endorsedto ?? e.districtmanagerid) == employeeid && pr.status == AppModels.Status.submitted) || (pr.endorsedto == employeeid && pr.status >= AppModels.Status.submitted))
                                select new GenericObject()
                                {
                                    id = pr.practivityid,
                                    date = pr.activitydate ?? DateTime.MinValue,
                                    employeename = e.firstname + " " + e.lastname,
                                    reference = "",
                                    status = pr.status ?? 0,
                                    moduleid = AppModels.Modules.practivity,
                                    dayfrom = DateTime.MinValue,
                                    totalamount=0
                                })
                                   .Concat(from e in this.employees
                                           join at in approvaltrees on new { key1 = (e.employeetypeid == 111 ? e.employeeid : e.districtmanagerid), key2 = AppModels.Modules.stop } equals new { key1 = at.districtmanagerid, key2 = (int)at.moduleid }
                                           join t in this.tieups on e.warehouseid equals t.warehouseid
                                           join i in this.institutions on t.inst_id equals i.inst_id
                                           where at.employeeid == employeeid && t.tieupclass == AppModels.Tieup.TieupClass.STOP && (((t.endorsedto ?? e.districtmanagerid) == employeeid && t.status == AppModels.Status.submitted) || (t.endorsedto == employeeid && t.status >= AppModels.Status.submitted))
                                           select new GenericObject()
                                           {
                                               id = t.tieupid,
                                               date = t.datecreated ?? DateTime.MinValue,
                                               employeename = e.firstname + " " + e.lastname,
                                               reference = i.inst_name,
                                               status = t.status ?? 0,
                                               moduleid = AppModels.Modules.stop,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = 0
                                           })
                                   .Concat(from e in this.employees
                                           join at in approvaltrees on new { key1 = (e.employeetypeid == 111 ? e.employeeid : e.districtmanagerid), key2 = AppModels.Modules.tup } equals new { key1 = at.districtmanagerid, key2 = (int)at.moduleid }
                                           join t in this.tieups on e.warehouseid equals t.warehouseid
                                           join d in this.doctors on t.doc_id equals d.doc_id
                                           where at.employeeid == employeeid && t.tieupclass == AppModels.Tieup.TieupClass.TUP && (((t.endorsedto ?? e.districtmanagerid) == employeeid && t.status == AppModels.Status.submitted) || (t.endorsedto == employeeid && t.status >= AppModels.Status.submitted))
                                           select new GenericObject()
                                           {
                                               id = t.tieupid,
                                               date = t.datecreated ?? DateTime.MinValue,
                                               employeename = e.firstname + " " + e.lastname,
                                               reference = d.doc_firstname + " " + d.doc_lastname,
                                               status = t.status ?? 0,
                                               moduleid = AppModels.Modules.tup,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = 0
                                           })
                                   .Concat(from e in this.employees
                                           join at in approvaltrees on new { key1 = (e.employeetypeid == 111 ? e.employeeid : e.districtmanagerid), key2 = AppModels.Modules.leaverequest } equals new { key1 = at.districtmanagerid, key2 = (int)at.moduleid }
                                           join l in this.employeeleaves on e.employeeid equals l.employeeid
                                           join lt in this.leavetypes on l.leavetypeid equals lt.leavetypeid
                                           where at.employeeid == employeeid && (((l.endorsedto ?? e.districtmanagerid) == employeeid && l.status == AppModels.Status.submitted) || (l.endorsedto == employeeid && l.status >= AppModels.Status.submitted))
                                           select new GenericObject()
                                           {
                                               id = l.leaveid,
                                               date = l.datecreated ?? DateTime.MinValue,
                                               employeename = e.firstname + " " + e.lastname,
                                               reference = lt.leavetypename + " - ",
                                               status = l.status ?? 0,
                                               moduleid = AppModels.Modules.leaverequest,
                                               dayfrom = (l.dayfrom ?? DateTime.MinValue),
                                               totalamount = 0
                                           })
                                   .Concat(from e in this.employees
                                           join at in approvaltrees on new { key1 = (e.employeetypeid == 111 ? e.employeeid : e.districtmanagerid), key2 = AppModels.Modules.leaverequest } equals new { key1 = at.districtmanagerid, key2 = (int)at.moduleid }
                                           join s in this.salaryloans on e.employeeid equals s.employeeid
                                           where at.employeeid == employeeid && (((s.endorsedto ?? e.districtmanagerid) == employeeid && s.status == AppModels.Status.submitted) || (s.endorsedto == employeeid && s.status >= AppModels.Status.submitted))
                                           select new GenericObject()
                                           {
                                               id = s.salaryloanid,
                                               date = s.created_at ?? DateTime.MinValue,
                                               employeename = e.firstname + " " + e.lastname,
                                               reference =  "Salary Loan",
                                               status = s.status ?? 0,
                                               moduleid = AppModels.Modules.salaryloan,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = s.amount ?? 0
                                           })
                                   .Concat(from e in this.employees
                                           join at in approvaltrees on new { key1 = (e.employeetypeid == 111 ? e.employeeid : e.districtmanagerid), key2 = AppModels.Modules.expensereport } equals new { key1 = at.districtmanagerid, key2 = (int)at.moduleid }
                                           join er in expensereports on e.warehouseid equals er.warehouseid
                                           where at.employeeid == employeeid && (((er.endorsedto ?? e.districtmanagerid) == employeeid && er.status == AppModels.Status.submitted) || (er.endorsedto == employeeid && er.status >= AppModels.Status.submitted))
                                           select new GenericObject()
                                           {
                                               id = er.expensereportid,
                                               date = er.datefiled ?? DateTime.MinValue,
                                               employeename = e.firstname + " " + e.lastname,
                                               reference = "",
                                               status = er.status ?? 0,
                                               moduleid = AppModels.Modules.expensereport,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = 0
                                           })
                                   .Concat(from e in this.employees
                                           join at in approvaltrees on new { key1 = (e.employeetypeid == 111 ? e.employeeid : e.districtmanagerid), key2 = AppModels.Modules.expensereport } equals new { key1 = at.districtmanagerid, key2 = (int)at.moduleid }
                                           join v in this.vehiclerepairs on e.warehouseid equals v.warehouseid
                                           where at.employeeid == employeeid && (((v.endorsedto ?? e.districtmanagerid) == employeeid && v.status == AppModels.Status.submitted) || (v.endorsedto == employeeid && v.status >= AppModels.Status.submitted))
                                           select new GenericObject()
                                           {
                                               id = (long)v.vehiclerepairid,
                                               date = v.datefiled ?? DateTime.MinValue,
                                               employeename = e.firstname + " " + e.lastname,
                                               reference = v.totalamount>0 ? "Total amount": "Average amount",
                                               status = v.status ?? 0,
                                               moduleid = AppModels.Modules.vehiclerepair,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = v.totalamount ?? (v.avgamount ?? 0)
                                           })
                                   .Concat(from e in this.employees
                                           join at in approvaltrees on new { key1 = (e.employeetypeid == 111 ? e.employeeid : e.districtmanagerid), key2 = AppModels.Modules.fundrequest } equals new { key1 = at.districtmanagerid, key2 = (int)at.moduleid }
                                           join fr in fundrequests on e.warehouseid equals fr.warehouseid
                                           where at.employeeid == employeeid && (((fr.endorsedto ?? e.districtmanagerid) == employeeid && fr.status == AppModels.Status.submitted) || (fr.endorsedto == employeeid && fr.status >= AppModels.Status.submitted) || (at.employeeid == employeeid && fr.status == AppModels.Status.fordisbursement && at.status==AppModels.Status.fordisbursement))
                                           select new GenericObject()
                                           {
                                               id = fr.fundrequestid,
                                               date = fr.daterequested ?? DateTime.MinValue,
                                               employeename = e.firstname + " " + e.lastname,
                                               reference = "",
                                               status = fr.status ?? 0,
                                               moduleid = AppModels.Modules.fundrequest,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = 0
                                           })
                                   .Concat(from e in this.employees
                                           join at in approvaltrees on new { key1 = (e.employeetypeid == 111 ? e.employeeid : e.districtmanagerid), key2 = AppModels.Modules.fundliquidation } equals new { key1 = at.districtmanagerid, key2 = (int)at.moduleid }
                                           join fr in fundliquidations on e.warehouseid equals fr.warehouseid
                                           where at.employeeid == employeeid && (((fr.endorsedto ?? e.districtmanagerid) == employeeid && fr.status == AppModels.Status.submitted) || (fr.endorsedto == employeeid && fr.status >= AppModels.Status.submitted))
                                           select new GenericObject()
                                           {
                                               id = fr.fundliquidationid,
                                               date = fr.datesubmitted ?? DateTime.MinValue,
                                               employeename = e.firstname + " " + e.lastname,
                                               reference = "",
                                               status = fr.status ?? 0,
                                               moduleid = AppModels.Modules.fundliquidation,
                                               dayfrom = DateTime.MinValue,
                                               totalamount = 0
                                           }).OrderBy(o => o.date).ToList();
            return lst.GroupBy(o => new { o.id, o.moduleid }).Select(o=> o.FirstOrDefault()).ToList();
        }
        public int GetCountbyStatus(int t)
        {
            int count = 0;
            if (t == 0)
            {
                count = this.practivities.Where(pr => pr.warehouseid == this._warehouseid && pr.status == AppModels.Status.draft).Count();
                count += this.tieups.Where(ti => ti.warehouseid == this._warehouseid && ti.status == AppModels.Status.draft).Count();
            }
            else if (t == 1)
            {
                count = this.practivities.Where(pr => pr.warehouseid == this._warehouseid && (pr.status == AppModels.Status.submitted || pr.status == AppModels.Status.endorsed)).Count();
                count += this.tieups.Where(ti => ti.warehouseid == this._warehouseid && (ti.status == AppModels.Status.submitted || ti.status == AppModels.Status.endorsed)).Count();
                count += this.employeeleaves.Where(el => el.employeeid == this._employeeid && el.status == AppModels.Status.submitted).Count();
                count += this.salaryloans.Where(s => s.employeeid == this._employeeid && s.status == AppModels.Status.submitted).Count();
            }
            else if (t == 2)
            {
                count = this.practivities.Where(pr => pr.warehouseid == this._warehouseid && pr.status == AppModels.Status.approved).Count();
                count += this.tieups.Where(ti => ti.warehouseid == this._warehouseid && ti.status == AppModels.Status.approved).Count();
                count += this.employeeleaves.Where(el => el.employeeid == this._employeeid && el.status == AppModels.Status.approved).Count();
                count += this.salaryloans.Where(s => s.employeeid == this._employeeid && s.status == AppModels.Status.approved).Count();
            }
            return count;
        }
    }

}