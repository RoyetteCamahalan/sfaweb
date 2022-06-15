using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class EmployeeController : SimpleDB
    {
        public EmployeeController()
        {

        }
        public long GetMaxEmployeeNo(long branchid)
        {
            return this.employees.Where(e => e.branchid == branchid).Select(e => e.employeeid).DefaultIfEmpty(0).Max();
        }
        public string GetEmployeeName(long? employeedid)
        {
            employee _employee = employees.Where(e => e.employeeid == employeedid).FirstOrDefault();
            if (_employee == null)
                return "None";
            return _employee.firstname ?? "None";
        }
        public employee GetEmployee(long id)
        {
            return this.employees.Where(e => e.employeeid == id).FirstOrDefault();
        }
        public employee GetEmployeeByEmployees(long id)
        {
            return this.employees.Where(e => e.warehouseid == id).FirstOrDefault();
        }
        public List<object> GetEmployees(bool isactive = false)
        {
            if (isactive)
                return this.employees.Where(e => (e.isactive ?? true) && e.employeetypeid != 555 && e.employeetypeid != 666).OrderBy(e => e.lastname).Select(e => new { e.employeeid, fullname = e.lastname + ", " + e.firstname }).ToList<object>();
            return this.employees.Where(e => e.employeetypeid != 555 && e.employeetypeid != 666).OrderBy(e => e.lastname).Select(e => new { e.employeeid, fullname = e.lastname + ", " + e.firstname }).ToList<object>();
        }
        public List<GenericObject> GetDistrictManagers(long branchid, long rbdmid)
        {
            var a = this.employees.Where(e => (e.branchid == branchid || branchid == 0) && (e.districtmanagerid == rbdmid || rbdmid == 0) && (e.isactive ?? true) && e.employeetypeid == AppModels.EmployeeTypes.bbdm).OrderBy(e => e.lastname).Select(e => new GenericObject() { id = e.employeeid, employeename = e.lastname + ", " + e.firstname }).ToList<GenericObject>();
            return a;
        }
        public List<GenericObject> GetRBDMs()
        {
            var a = this.employees.Where(e => (e.isactive ?? true) && e.employeetypeid == AppModels.EmployeeTypes.rbdm).OrderBy(e => e.lastname).Select(e => new GenericObject() { id = e.employeeid, employeename = e.lastname + ", " + e.firstname }).ToList<GenericObject>();
            return a;
        }
        public List<GenericObject> GetPSRPTR(long branchid, long districtmanagerid, long rbdmid)
        {
            return (from w in warehouses
                    join psr in employees on w.warehouseid equals psr.warehouseid
                    join dm in employees on psr.districtmanagerid equals dm.employeeid
                    where (psr.employeetypeid == AppModels.EmployeeTypes.psr || psr.employeetypeid == AppModels.EmployeeTypes.ptr) &&
                        (psr.branchid == branchid || branchid == 0) && (psr.districtmanagerid == districtmanagerid || districtmanagerid == 0) &&
                        (dm.districtmanagerid == rbdmid || rbdmid == 0) && (w.isactive ?? true)
                    select new GenericObject()
                    {
                        id = psr.employeeid,
                        employeename = psr.lastname + ", " + psr.firstname
                    }).OrderBy(g => g.employeename).ToList<GenericObject>();
        }
        public List<employee> GetDistrictEmployees(long? districtmanagerid)
        {
            return employees.Where(e => e.districtmanagerid == districtmanagerid).ToList();
        }
        public List<object> GetNoUserEmployees()
        {
            var a = this.employees.Where(e => (e.isactive ?? true) && !e.useraccounts.Any(es => es.employeeid == e.employeeid)).DefaultIfEmpty().OrderBy(e => e.lastname).Select(e => new { e.employeeid, fullname = e.lastname + ", " + e.firstname }).ToList<object>();
            return a;
        }
        public List<object> GetSupervisors(long branchid, int employeetypeid)
        {
            if (employeetypeid == AppModels.EmployeeTypes.psr || employeetypeid == AppModels.EmployeeTypes.ptr)
                return this.employees.Where(e => e.branchid == branchid && e.employeetypeid == AppModels.EmployeeTypes.bbdm).OrderBy(e => e.lastname).Select(e => new { e.employeeid, fullname = e.lastname + ", " + e.firstname }).ToList<object>();
            else if (employeetypeid == AppModels.EmployeeTypes.bbdm)
                return this.employees.Where(e => e.employeetypeid == AppModels.EmployeeTypes.rbdm).OrderBy(e => e.lastname).Select(e => new { e.employeeid, fullname = e.lastname + ", " + e.firstname }).ToList<object>();
            else
                return this.employees.Where(e => e.employeetypeid != AppModels.EmployeeTypes.rbdm && 1 == 0).OrderBy(e => e.lastname).Select(e => new { e.employeeid, fullname = e.lastname + ", " + e.firstname }).ToList<object>();
        }
        public List<employee> GetBranchEmployees(long branchid)
        {
            return this.employees.Where(e => e.branchid == branchid || branchid == 0).OrderBy(e => e.lastname).ToList();
        }
        public List<branch> GetRBDMBranches(long employeeid)
        {
            return (from b in branches
                    join e in employees on b.branchid equals e.branchid
                    where e.employeetypeid == AppModels.EmployeeTypes.bbdm  && (e.districtmanagerid == employeeid || employeeid==0) && (b.isactive ?? true) 
                    select b).Distinct().OrderBy(b => b.branchname).ToList();
        }

        public List<branch> GetBranches()
        {
            return this.branches.Where(b => b.isactive ?? true).OrderBy(b => b.branchname).ToList();
        }



        #region "EmployeeTypes"

        public employeetype GetEmployeetype(int id)
        {
            if (id == 0)
                return new employeetype();
            return this.employeetypes.Where(e => e.employeetypeid == id).FirstOrDefault();
        }
        public List<employeetype> GetEmployeetypes()
        {
            return this.employeetypes.Where(e => e.employeetypeid != 0).OrderBy(e => e.employeetypeid).ToList();
        }
        public List<employeetype> GetActiveEmployeetypes()
        {
            return this.employeetypes.Where(e => e.employeetypeid != 0 && (e.isactive ?? true)).OrderBy(e => e.employeetypeid).ToList();
        }
        #endregion

        #region "Warehouses"
        public warehouse GetWarehouse(long id)
        {
            if (id == 0)
                return new warehouse();
            return this.warehouses.Find(id);
        }
        public List<warehouse> GetWarehouses(long branchid)
        {
            return this.warehouses.Where(e => e.branchid == branchid || branchid == 0).OrderBy(e => e.warehouseid).ToList();
        }
        public List<warehouse> GetUnAssignedWarehouses(long branchid)
        {
            return this.warehouses.Where(e => e.branchid == branchid && !e.employees.Any(es => es.warehouseid == e.warehouseid)).OrderBy(e => e.warehouseid).ToList();
        }
        public List<branch> GetActiveBranches()
        {
            return this.branches.Where(e => e.isactive ?? true).OrderBy(e => e.branchname).ToList();
        }
        #endregion

        #region "Users"
        public long GetMaxUserAccountID(long branchid)
        {
            return (from e in employees
                    join u in useraccounts on e.employeeid equals u.employeeid
                    where e.branchid == branchid
                    select u).Select(e => e.useraccountid).DefaultIfEmpty(0).Max();
        }
        public useraccount GetUseraccount(long id)
        {
            return this.useraccounts.Find(id);
        }
        public List<useraccount> GetUseraccounts(long branchid)
        {
            return (from e in employees
                    join u in useraccounts on e.employeeid equals u.employeeid
                    where e.branchid == branchid || branchid == 0
                    select u).OrderBy(e => e.useraccountid).ToList();
        }
        public bool IsUserNameExist(string uname, long useraccountid)
        {
            return this.useraccounts.Where(u => u.username == uname && u.useraccountid != useraccountid).Count() > 0;
        }
        public List<object> GetUserprivs(long useraccountid)
        {
            return (from m in modules
                    join u in userprivs on new { key1 = m.module_id, key2 = useraccountid } equals new { key1 = u.modcode, key2 = u.useraccountid } into ljoin
                    where m.module_id > 200
                    from l in ljoin.DefaultIfEmpty()
                    select new
                    {
                        userprivno = l == null ? 0 : l.userprivno,
                        useraccountid = l == null ? 0 : l.useraccountid,
                        modcode = m.module_id,
                        modname = m.name,
                        canrequest = l != null && (l.canrequest ?? false),
                        canadd = l != null && (l.canadd ?? false),
                        canedit = l != null && (l.canedit ?? false),
                    }).OrderBy(x => x.modname).ToList<object>();
        }
        public List<object> GetUserSubprivs(long useraccountid)
        {
            return (from s in systemsubmodules
                    join u in usersubprivs on new { key1 = s.submodcode, key2 = useraccountid } equals new { key1 = (int)u.submodcode, key2 = u.useraccountid } into ljoin
                    from l in ljoin.DefaultIfEmpty()
                    select new
                    {
                        usersubprivno = l == null ? 0 : l.usersubprivno,
                        useraccountid = l == null ? 0 : l.useraccountid,
                        s.submodcode,
                        s.submoddescription,
                        canaccess = l != null && (l.canaccess ?? false)
                    }).OrderBy(x => x.submoddescription).ToList<object>();
        }
        public userpriv GetUserpriv(long userprivno)
        {
            return this.userprivs.Where(u => u.userprivno == userprivno).FirstOrDefault();
        }
        public usersubpriv GetUsersubpriv(long usersubprivno)
        {
            return this.usersubprivs.Where(u => u.usersubprivno == usersubprivno).FirstOrDefault();
        }
        #endregion
    }
}