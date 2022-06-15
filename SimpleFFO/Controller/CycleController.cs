using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class CycleController : SimpleDB
    {
        public List<cycleset> GetCyclesets()
        {
            return this.cyclesets.Where(cs => cs.isactive ?? true).ToList();
        }
        public List<object> DisplayYearMonthInfo(long cycledayid)
        {
            List<object> request = (from cd in this.cycledays
                                    join cs in this.cyclesets on cd.cycle_set_id equals cs.cycle_set_id
                                    where cd.cycle_day_id == cycledayid
                                    select new
                                    {
                                        month = cd.cycle_number,
                                        cs.year
                                    }).ToList<object>();
            return request;
        }
        public cycleday GetCycleDay(int id)
        {
            return this.cycledays.Find(id);
        }
    }
}