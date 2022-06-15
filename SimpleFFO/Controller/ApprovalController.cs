using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class ApprovalController : SimpleDB
    {
        public List<module> GetApprovalModules()
        {
            return this.modules.Where(m => m.module_id >= 201 && m.module_id <= 209).ToList();
        }
        public List<approvaltree> GetApprovaltree(long employeeid, int moduleid)
        {
            employee e = this.employees.Find(employeeid);
            if (e.employeetypeid != 111)
                employeeid = e.districtmanagerid ?? 0;
            return this.approvaltrees.Where(a => a.districtmanagerid== employeeid && a.moduleid==moduleid).OrderBy(a=>a.treelevel).ToList();
        }
        public void SaveTree(int moduleid,long districtmanagerid, List<approvaltree> approvaltrees)
        {
            //remove items from list
            foreach (var existingChild in this.approvaltrees.Where(a=>a.moduleid==moduleid && a.districtmanagerid == districtmanagerid).ToList())
            {
                if (!approvaltrees.Any(c => c.approvaltreeid == existingChild.approvaltreeid))
                    this.approvaltrees.Remove(existingChild);
            }
            //add/update list
            foreach (approvaltree a in approvaltrees)
            {

                if (a.approvaltreeid == 0)
                    this.approvaltrees.Add(a);
                else
                {
                    this.Entry(this.approvaltrees.Where(p => p.approvaltreeid == a.approvaltreeid).First()).CurrentValues.SetValues(a);
                }
            }
            this.SaveChanges();
        }

        public List<statustrail> GetStatustrails(int moduleid, long id)
        {
            return this.statustrails.Where(st => st.moduleid == moduleid && st.requestid == id).OrderBy(st=> st.traildate).ToList();
        }


        public GenericObject checkIfApproval(int moduleid, long requestid, int requeststatus, employee requestemployee, long? endorsedto, long? currentemployeeid, bool isdisburse)
        {
            GenericObject genericObject = new GenericObject();
            List<approvaltree> lsttree = this.GetApprovaltree(requestemployee.employeeid, moduleid);
            if (isdisburse && lsttree.Where(l=> l.employeeid==currentemployeeid && l.status==AppModels.Status.fordisbursement).Count()>0)
            {
                genericObject.statusaction = AppModels.Status.fordisbursement;
                genericObject.treelevel = lsttree.Where(l => l.employeeid == currentemployeeid && l.status == AppModels.Status.fordisbursement).FirstOrDefault().treelevel ?? 0;
                genericObject.endorsenext = 0;
                return genericObject;
            }
            List<statustrail> lsttrail = this.GetStatustrails(moduleid, requestid);
            foreach (statustrail trail in lsttrail)
            {
                if (trail.statusid == AppModels.Status.rejected)
                {
                    lsttree = this.GetApprovaltree(requestemployee.employeeid, moduleid);
                }
                else
                {
                    approvaltree temptree = lsttree.Where(lt => lt.employeeid == trail.employeeid).FirstOrDefault();
                    if (temptree != null)
                        lsttree.Remove(temptree);
                }
            }
            genericObject.statusaction = -1;
            foreach (approvaltree tree in lsttree)
            {
                if (tree.employeeid == currentemployeeid)
                {
                    if (endorsedto == null && requeststatus == AppModels.Status.submitted && requestemployee.districtmanagerid == tree.employeeid)
                    {
                        genericObject.statusaction = tree.status ?? 0;
                        genericObject.treelevel = tree.treelevel ?? 0;
                    }
                    if (endorsedto == tree.employeeid)
                    {
                        genericObject.statusaction = tree.status ?? 0;
                        genericObject.treelevel = tree.treelevel ?? 0;
                    }
                }
                else if (genericObject.statusaction != -1)
                {
                    genericObject.endorsenext = (long)tree.employeeid;
                    break;
                }
            }
            return genericObject;
        }

        public long SaveTrailOnSubmit(int moduleid, long requestid, int status, employee employee, string remarks)
        {
            statustrail st = new statustrail
            {
                moduleid = moduleid,
                requestid = requestid,
                statusid = status,
                traildate = DateTime.Now,
                employeeid = employee.employeeid,
                remarks = remarks,
                treelevel = 0
            };
            this.statustrails.Add(st);
            this.SaveChanges();

            var districtmanagerid = employee.districtmanagerid;
            if (employee.employeetypeid == AppModels.EmployeeTypes.bbdm)
                districtmanagerid = employee.employeeid;
            else if (employee.employeetypeid != AppModels.EmployeeTypes.psr && employee.employeetypeid != AppModels.EmployeeTypes.psr)
            {
                employee bbdm= employees.Where(x => x.branchid == employee.branchid && x.employeetypeid == AppModels.EmployeeTypes.bbdm).FirstOrDefault();
                districtmanagerid = bbdm == null ? 0 : bbdm.employeeid;
            }
            List<approvaltree> lsttrees = this.approvaltrees.Where(t => t.districtmanagerid == districtmanagerid && t.moduleid == moduleid).OrderBy(t => t.treelevel).ToList();
            foreach (approvaltree tree in lsttrees)
            {
                if (tree.employeeid != employee.employeeid)
                {
                    return tree.employeeid ?? 0;
                }
            }
            return 0;
        }
        public void SaveTrail(int moduleid, long requestid, long employeeid,int status,int treelevel,string remarks)
        {
            statustrail st = new statustrail
            {
                moduleid = moduleid,
                requestid = requestid,
                statusid = status,
                traildate = DateTime.Now,
                employeeid = employeeid,
                remarks = remarks,
                treelevel = treelevel
            };
            this.statustrails.Add(st);
            this.SaveChanges();
        }
        public void SaveTrail(statustrail st)
        {
            this.statustrails.Add(st);
            this.SaveChanges();
        }
    }
}