using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class ReportDashBoard : SimpleDB
    {
        public List<adminreport> GetAdminReports(int employyeetype)
        {
            if (AppModels.EmployeeTypes.psr == employyeetype || AppModels.EmployeeTypes.psr==employyeetype)
                return this.adminreports.Where(a => a.isactive == true && a.isforpsr == true).OrderBy(a => a.reportorder ?? 9).ToList();
            else
                return this.adminreports.Where(a => a.isactive ?? true).OrderBy(a => a.reportorder ?? 10).ToList();
        }
        public DataTable GetResultReport(int operation,int soperation, long branchid, long rbdmid, long bbdmid, long warehouseid, int cycleset, 
            int cyclenumber, bool isweb=true, bool isexport=false, int isactive = 2, int mdtype = 0, int specializationid=0)
        {
            DataSet retVal = new DataSet();
            SqlConnection sqlConn = (SqlConnection)this.Database.Connection;
            SqlCommand cmdReport = new SqlCommand("spReportDashboard", sqlConn)
            {
                CommandTimeout = 0
            };
            SqlDataAdapter daReport = new SqlDataAdapter(cmdReport);
            
            using (cmdReport)
            {
                cmdReport.CommandType = CommandType.StoredProcedure;
                cmdReport.Parameters.AddWithValue("operation", operation);
                cmdReport.Parameters.AddWithValue("soperation", soperation);
                cmdReport.Parameters.AddWithValue("branchid", branchid);
                cmdReport.Parameters.AddWithValue("bbdmid", bbdmid);
                cmdReport.Parameters.AddWithValue("rbdmid", rbdmid);
                cmdReport.Parameters.AddWithValue("warehouseid", warehouseid);
                cmdReport.Parameters.AddWithValue("cycleset", cycleset);
                cmdReport.Parameters.AddWithValue("cyclenumber", cyclenumber);
                cmdReport.Parameters.AddWithValue("isweb", isweb);
                cmdReport.Parameters.AddWithValue("isexport", isexport);
                cmdReport.Parameters.AddWithValue("isactive", isactive);
                cmdReport.Parameters.AddWithValue("mdtype", mdtype);
                cmdReport.Parameters.AddWithValue("specializationid", specializationid);
                cmdReport.Parameters.AddWithValue("baseurl", AppModels.baseurl); 
                daReport.Fill(retVal);
            }
            if (retVal.Tables.Count > 0)
                return retVal.Tables[0];
            return new DataTable();
        }


        public List<object> GetIncidentalCallsLst(long warehouseid, int cyclenumber, int year)
        {
            List<object> result = new List<object>();

            result = (from idm in this.institutiondoctormaps
                      join d in this.doctors on idm.doc_id equals d.doc_id
                      join c in this.calls on idm.idm_id equals c.inst_doc_id
                      join cd in this.cycledays on c.cycle_day_id equals cd.cycle_day_id
                      join cs in this.cyclesets on cd.cycle_set_id equals cs.cycle_set_id
                      where c.warehouse_id == warehouseid && cd.cycle_number == cyclenumber && cs.year == year && c.planned == false
                      select new
                      {
                          c.call_id,
                          c.inst_doc_id,
                          c.cycle_day_id,
                          c.warehouse_id,
                          c.status_id,
                          c.planned,
                          c.makeup,
                          c.start_datetime,
                          c.end_datetime,
                          c.latitude,
                          c.longitude,
                          c.reschedule_day_id,
                          c.signed_day_id,
                          c.retry_count,
                          c.joint_call,
                          c.quick_sign,
                          c.location_datetime,
                          c.createdbyid,
                          c.created_at,
                          c.updatedbyid,
                          c.updated_at,
                          c.deleted_at
                      }

                ).ToList<object>();

            return result;
        }
        public List<object> GetMissedCalledLst(long warehouseid, int cyclenumber, int year)
        {
            List<object> result = new List<object>();

            result = (from cs in this.cyclesets
                      join cd in this.cycledays on cs.cycle_set_id equals cd.cycle_set_id
                      join c in this.calls on cd.cycle_day_id equals c.reschedule_day_id > 0 ? c.reschedule_day_id : c.cycle_day_id
                      join ms in this.missedcalls on c.call_id equals ms.call_id
                      where c.warehouse_id == warehouseid && cd.cycle_number == cyclenumber && cs.year == year
                      select new
                      {
                          ms.missed_call_id,
                          ms.call_id,
                          ms.reason_id,
                          ms.remarks,
                          ms.sent,
                          ms.status_approved
                      }).ToList<object>();

            return result;
        }
    }
}