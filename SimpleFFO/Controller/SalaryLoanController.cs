using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SimpleFFO.Model;

namespace SimpleFFO.Controller
{
    public class SalaryLoanController : SimpleDB
    {
        public salaryloan getSalaryLoan(long salaryloanid)
        {
            if (salaryloanid == 0)
                return new salaryloan
                {
                    salaryloanid = this.salaryloans.Select(s => s.salaryloanid).DefaultIfEmpty(0).Max() + 1
                };
            return this.salaryloans.FirstOrDefault(s => s.salaryloanid == salaryloanid);
        }

        public void SaveSalaryLoan(bool isnew, salaryloan _salaryloan)
        {
            if (isnew)
                this.salaryloans.Add(_salaryloan);

            if (_salaryloan.status == AppModels.Status.submitted)
            {
                this.Entry(_salaryloan).Reference(c => c.employee).Load();
                ApprovalController approvalController = new ApprovalController();
                _salaryloan.endorsedto = approvalController.SaveTrailOnSubmit(AppModels.Modules.salaryloan, _salaryloan.salaryloanid, _salaryloan.status ?? 0, _salaryloan.employee, "");
            }
            this.SaveChanges();
        }



        //public void SaveVehicleRepairDetails(bool isnew, vehiclerepairdetail _vehiclerepairdetail)
        //{
        //    if (isnew)
        //        this.vehiclerepairdetails.Add(_vehiclerepairdetail);
        //    this.SaveChanges();
        //}
    }
}