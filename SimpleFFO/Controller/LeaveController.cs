using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class LeaveController : SimpleDB
    {
        private long _employeeid = 0;
        public LeaveController()
        {
            _employeeid = Convert.ToInt64(HttpContext.Current.Session[AppModels.SessionKeys.employeeid]);
        }


        public employeeleave GetEmployeeleave(long leaveid)
        {
            if (leaveid == 0)
                return new employeeleave
                {
                    leaveid = this.employeeleaves.Select(e => e.leaveid).DefaultIfEmpty(0).Max() + 1,
                    status = AppModels.Status.cancelled
                };
            return this.employeeleaves.FirstOrDefault(e => e.leaveid == leaveid);

        }

        /*Filter = 0-allleave 1- approved 2-pending*/
        public decimal getRemainingLeaveDays(long employeeid,int year, int filter)
        {
            
            decimal result = 0;
            List<employeeleave> employeeleaves;
            if (filter == 1)
            {
                employeeleaves = this.employeeleaves.Where(
                e => (e.employeeid ?? 0) == employeeid && ((e.dayfrom ?? DateTime.MinValue).Year == year || (e.dayto ?? DateTime.MinValue).Year == year) &&
                (e.status ?? 0) == AppModels.Status.approved).ToList();
            }else if (filter == 2)
            {
                employeeleaves = this.employeeleaves.Where(
                e => (e.employeeid ?? 0) == employeeid && ((e.dayfrom ?? DateTime.MinValue).Year == year || (e.dayto ?? DateTime.MinValue).Year == year) &&
                (e.status ?? 0) != AppModels.Status.submitted && (e.status ?? 0) != AppModels.Status.endorsed).ToList();
            }
            else
            {
                employeeleaves = this.employeeleaves.Where(
                e => (e.employeeid ?? 0) == employeeid && ((e.dayfrom ?? DateTime.MinValue).Year == year || (e.dayto ?? DateTime.MinValue).Year == year) &&
                (e.status ?? 0) != AppModels.Status.cancelled && (e.status ?? 0) != AppModels.Status.rejected).ToList();
            }
            result =Convert.ToDecimal( employeeleaves.Select(e => e.noofdays).Sum());
            foreach(employeeleave e in employeeleaves)
            {
                if(e.dayfrom.GetValueOrDefault().Year == (year - 1) || e.dayto.GetValueOrDefault().Year==(year + 1))
                {
                    Dictionary<string, object> res = ProcessLeave(e.dayfrom.GetValueOrDefault(), e.dayto.GetValueOrDefault(), e.hashalfday ?? false, year);
                    result = result - Convert.ToDecimal(res["leaveforotheryear"]);
                }
            }
            return result;
        }
        public decimal getYearlyLeaveDays()
        {
            var result = this.preferences.FirstOrDefault(p => p.preference_id == AppModels.PrefKey_yearlyleavecount);
            if (result != null)
                return Convert.ToDecimal(result.value);
            return 0;
        }
        public List<DateTime> getHolidays(int year)
        {
            return (from cs in this.cyclesets
                    join cd in this.cycledays on cs.cycle_set_id equals cd.cycle_set_id
                    where (cs.year == year || cs.year == (year - 1) || cs.year == (year + 1)) &&
                    (cd.date_type_id == AppModels.DayTypes.regularholiday || cd.date_type_id == AppModels.DayTypes.specialholiday)
                    select cd.cycle_number + "/" + cd.day_number + "/" + cs.year ).Cast<DateTime>().ToList();
        }
        public Dictionary<string, object> ProcessLeave(DateTime datefrom,DateTime dateto, bool hashalfday, int year)
        {
            Dictionary<string, object> data= new Dictionary< string, object>();
            decimal leavedays = 0;
            decimal covereddayoff = 0;
            decimal coveredholiday = 0;
            decimal leaveforcurrentyear = 0;
            decimal leaveforotheryear = 0;
            List<DateTime> lstHolidays = getHolidays(year);
            for (var curdate = datefrom; curdate <= dateto; curdate = curdate.AddDays(1))
            {
                bool isholiday = false;
                if (lstHolidays.Contains(curdate.Date))
                {
                    isholiday = true;
                    coveredholiday += 1;
                }
                if (curdate.DayOfWeek == DayOfWeek.Sunday)
                {
                    covereddayoff += 1;
                }
                else if (curdate.DayOfWeek == DayOfWeek.Saturday)
                {
                    leavedays = leavedays + Convert.ToDecimal(0.5);
                    covereddayoff = covereddayoff + Convert.ToDecimal(0.5);
                    if(curdate.Year==year)
                        leaveforcurrentyear= leaveforcurrentyear + Convert.ToDecimal(0.5);
                    else
                        leaveforotheryear = leaveforotheryear + Convert.ToDecimal(0.5);
                }
                else if(!isholiday)
                {
                    leavedays += 1;
                    if (curdate.Year == year)
                        leaveforcurrentyear = leaveforcurrentyear + 1;
                    else
                        leaveforotheryear = leaveforotheryear + 1;
                }
            }
            if (hashalfday)
            {
                leavedays = leavedays - Convert.ToDecimal(0.5);
                leaveforcurrentyear = leaveforcurrentyear - Convert.ToDecimal(0.5);
            }

            bool gotowork = false;
            DateTime backtowork= dateto;
            while (!gotowork)
            {
                backtowork = backtowork.AddDays(1);
                if (!(backtowork.DayOfWeek == DayOfWeek.Sunday) && !lstHolidays.Contains(backtowork.Date))
                {
                    gotowork = true;
                }

            }
            data.Add("leavedays" , leavedays);
            data.Add("covereddayoff", covereddayoff);
            data.Add("coveredholiday", coveredholiday);
            data.Add("backtowork", backtowork);
            data.Add("leaveforcurrentyear", leaveforcurrentyear);
            data.Add("leaveforotheryear", leaveforotheryear);
            return data;
        }

        public List<employeeleave> GetEmployeeleaves(long employeeid)
        {
            return this.employeeleaves.Where(el => el.employeeid == employeeid).OrderBy(el => el.datecreated).ToList();
        }
        public List<leavetype> GetLeaveTypes()
        {
            return this.leavetypes.Where(e => e.isactive ?? true).ToList();
        }


        public void SaveLeave(bool isnew,employeeleave _employeeleave)
        {
            if (isnew)
                this.employeeleaves.Add(_employeeleave);

            if (_employeeleave.status == AppModels.Status.submitted)
            {
                ApprovalController approvalController = new ApprovalController();
                _employeeleave.endorsedto = approvalController.SaveTrailOnSubmit(AppModels.Modules.leaverequest, _employeeleave.leaveid, _employeeleave.status ?? 0, _employeeleave.employee, "");
            }
            this.SaveChanges();
        }
    }

}