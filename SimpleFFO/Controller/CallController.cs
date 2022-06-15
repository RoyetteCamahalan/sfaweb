using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class CallController : SimpleDB
    {
        public call GetCall(long id)
        {
            return this.calls.Find(id);
        }
        public List<call> GetCalls(long warehouseid, int year, int cycle_number)
        {
            return (from cs in cyclesets
                    join cd in cycledays on cs.cycle_set_id equals cd.cycle_set_id
                    join c in calls on cd.cycle_day_id equals (c.reschedule_day_id > 0 ? c.reschedule_day_id : c.cycle_day_id)
                    where c.warehouse_id==warehouseid && cs.year==year && cd.cycle_number==cycle_number && c.status_id!=0
                    select c).OrderBy(x=> x.start_datetime).ToList();
        }

        public DataTable GetDailySyncReportinfo(long employee_id, string _date)
        {
            DataSet retVal = new DataSet();
            SqlConnection sqlConn = (SqlConnection)this.Database.Connection;
            SqlCommand cmdReport = new SqlCommand("spReportDashboard", sqlConn);
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);

            using (cmdReport)
            {
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.Parameters.Add(new SqlParameter("@operation", 0));
                cmdReport.Parameters.Add(new SqlParameter("@soperation", 41));
                cmdReport.Parameters.Add(new SqlParameter("@date", _date));
                cmdReport.Parameters.Add(new SqlParameter("@employeeid", employee_id));
                daReport.Fill(retVal);
            }
            if (retVal.Tables.Count > 0)
                return retVal.Tables[0];
            return new DataTable();
        }
        public List<object> GetDailySyncReportinfoDoctinfo(long employeeid, DateTime date)
        {
            List<object> request = new List<object>();

            request = (from cs in this.cyclesets
                       join cd in this.cycledays on cs.cycle_set_id equals cd.cycle_set_id
                       join c in this.calls on cd.cycle_day_id equals c.reschedule_day_id > 0 ? c.reschedule_day_id : c.cycle_day_id
                       //join cn in this.callnotes on c.call_id equals cn.call_id
                       join idm in this.institutiondoctormaps on c.inst_doc_id equals idm.idm_id
                       join d in this.doctors on idm.doc_id equals d.doc_id
                       join e in this.employees on c.warehouse_id equals e.warehouseid
                       join s in this.signatures on c.call_id equals s.call_id into s1
                       from s2 in s1.DefaultIfEmpty()
                       join cn in this.callnotes on c.call_id equals cn.call_id into cn1
                       from cn2 in cn1.DefaultIfEmpty()
                       where e.employeeid == employeeid && (cd.date ?? DateTime.MinValue) == date.Date
                       select new
                       {
                           c.call_id,
                           full_name = d.doc_lastname + " " + d.doc_firstname + " " + d.doc_mi,
                           c.start_datetime,
                           c.end_datetime,
                           call_type = (c.planned == true && c.status_id > 0 ? "PLANNED" :
                                        c.planned == true && c.status_id == 0 ? "Undeclared Missed" :
                                        c.planned == false && c.status_id >= 0 ? "Incidental" : string.Empty),
                           signaturetype = (s2.path.Contains("cam") ? "CAMERA" :
                                            s2.path.Contains("sig") ? "SIGNATURE" :
                                            s2.path.Contains("quicksig") ? "QUICK SIGNATURE" :
                                            s2.path.Contains("quickcam") ? "QUICK CAMERA" : string.Empty),
                           imageurl = s2.path,
                           notes = (cn2.notes != string.Empty ? cn2.notes : "NONE")
                           //cn2.notes

                       }).OrderBy(cd => cd.start_datetime).ToList<object>();

            return request;
        }
    }
}