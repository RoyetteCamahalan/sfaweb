using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class FundController : SimpleDB
    {
        public fundrequest GetFundRequest(long id)
        {
            if (id == 0)
                return new fundrequest { fundrequestid = this.fundrequests.Select(s => s.fundrequestid).DefaultIfEmpty(0).Max() + 1 };
            return this.fundrequests.FirstOrDefault(i => i.fundrequestid == id);
        }
        public List<fundrequest> GetFundRequests(int moduleid, long requestid, long warehouseid)
        {
            List<fundrequest> lstrequest =  this.fundrequests.Where(f => f.moduleid == moduleid && f.requestid == requestid).OrderBy(f=> f.daterequested).ToList();

            if (lstrequest.Count == 0 && (moduleid == AppModels.Modules.expensereport || moduleid == AppModels.Modules.salaryloan))
            {
                int status = 0;
                decimal amount = 0;
                if (moduleid == AppModels.Modules.expensereport)
                {
                    ExpenseReportController expenseReportController = new ExpenseReportController();
                    expensereport ex = expenseReportController.GetExpensereport(requestid);
                    status = ex.status ?? 0;
                    amount = ex.totalexpense ?? 0;
                }
                else if (moduleid == AppModels.Modules.salaryloan)
                {
                    SalaryLoanController salaryLoanController = new SalaryLoanController();
                    salaryloan s = salaryLoanController.getSalaryLoan(requestid);
                    status = s.status ?? 0;
                    amount = s.amount ?? 0;
                }
                if (status == AppModels.Status.approved && amount > 0)
                {
                    fundrequest req = this.GetFundRequest(0);
                    req.warehouseid = warehouseid;
                    req.moduleid = moduleid;
                    req.requestid = requestid;
                    req.dateneeded = Utility.getServerDate();
                    req.paymentmode = -1;
                    req.paymentref = "";
                    req.status = AppModels.Status.submitted;
                    req.amount = amount;
                    req.daterequested = Utility.getServerDate();
                    this.SaveFundRequest(true, req);
                    lstrequest = this.fundrequests.Where(f => f.moduleid == moduleid && f.requestid == requestid).OrderBy(f => f.daterequested).ToList();
                }
            }
            return lstrequest;
        }
        public fundliquidation GetFundLiquidation(long id)
        {
            if (id == 0)
                return new fundliquidation { fundliquidationid = this.fundliquidations.Select(s => s.fundliquidationid).DefaultIfEmpty(0).Max() + 1 };
            return this.fundliquidations.FirstOrDefault(i => i.fundliquidationid == id);
        }
        public List<fundliquidation> GetFundLiquidations(int moduleid, long requestid)
        {
            return this.fundliquidations.Where(f => f.moduleid == moduleid && f.requestid == requestid).OrderBy(f => f.datesubmitted).ToList();
        }

        public void SaveFundRequest(bool isnew,fundrequest fundrequest)
        {
            if (isnew)
            {
                this.fundrequests.Add(fundrequest);

                this.Entry(fundrequest).Reference(c => c.warehouse).Load();
                /*statustrail st = new statustrail
                {
                    moduleid = AppModels.Modules.fundrequest,
                    requestid = fundrequest.fundrequestid,
                    statusid = fundrequest.status,
                    traildate = DateTime.Now,
                    employeeid = fundrequest.warehouse.employees.First().employeeid,
                    remarks = "",
                    treelevel = 0
                };
                this.statustrails.Add(st);*/

                var districtmanagerid = fundrequest.warehouse.employees.First().districtmanagerid;
                if (fundrequest.warehouse.employees.First().employeetypeid == 111)
                {
                    districtmanagerid = fundrequest.warehouse.employees.First().employeeid;
                }
                if (fundrequest.warehouse.employees.First().employeetypeid == AppModels.EmployeeTypes.bbdm)
                    districtmanagerid = fundrequest.warehouse.employees.First().employeeid;
                else if (fundrequest.warehouse.employees.First().employeetypeid != AppModels.EmployeeTypes.psr && fundrequest.warehouse.employees.First().employeetypeid != AppModels.EmployeeTypes.psr)
                {
                    employee bbdm = employees.Where(x => x.branchid == fundrequest.warehouse.employees.First().branchid && x.employeetypeid == AppModels.EmployeeTypes.bbdm).FirstOrDefault();
                    districtmanagerid = bbdm == null ? 0 : bbdm.employeeid;
                }
                List<approvaltree> lsttrees = this.approvaltrees.Where(t => t.districtmanagerid == districtmanagerid && t.moduleid == AppModels.Modules.fundrequest).OrderBy(t => t.treelevel).ToList();
                foreach (approvaltree tree in lsttrees)
                {
                    if (tree.employeeid != fundrequest.warehouse.employees.First().employeeid)
                    {
                        fundrequest.endorsedto = tree.employeeid;
                        break;
                    }
                }
            }
            this.SaveChanges();
        }
        public void SaveFundLiquidation(bool isnew, fundliquidation fundliquidation)
        {
            if (isnew)
            {
                this.fundliquidations.Add(fundliquidation);

                this.Entry(fundliquidation).Reference(c => c.warehouse).Load();
                ApprovalController approvalController = new ApprovalController();
                fundliquidation.endorsedto = approvalController.SaveTrailOnSubmit(AppModels.Modules.fundliquidation, fundliquidation.fundliquidationid, fundliquidation.status ?? 0, fundliquidation.warehouse.employees.First(), "");
            }
            this.SaveChanges();
        }

    }
}