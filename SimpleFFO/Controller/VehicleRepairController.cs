using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class VehicleRepairController : SimpleDB
    {
        public vehiclerepair GetVehiclerepair(long vehiclerepairid)
        {
            if (vehiclerepairid == 0)
                return new vehiclerepair
                {
                    vehiclerepairid = this.vehiclerepairs.Select(v => v.vehiclerepairid).DefaultIfEmpty(0).Max() + 1,
                    status = AppModels.Status.cancelled
                };
            return this.vehiclerepairs.FirstOrDefault(v => v.vehiclerepairid == vehiclerepairid);
        }

        public void SaveVehicleRepair(bool isnew,vehiclerepair _vehiclerepair, List<vehiclerepairdetail> lstdetails)
        {
            foreach (var existingChild in _vehiclerepair.vehiclerepairdetails.ToList())
            {
                if (!vehiclerepairdetails.Any(c => c.vehiclerepairdetailid == existingChild.vehiclerepairdetailid))
                    this.vehiclerepairdetails.Remove(existingChild);
            }
            foreach (vehiclerepairdetail a in lstdetails)
            {

                if (a.vehiclerepairdetailid == 0)
                    _vehiclerepair.vehiclerepairdetails.Add(a);
                else
                {
                    this.Entry(_vehiclerepair.vehiclerepairdetails.Where(p => p.vehiclerepairdetailid == a.vehiclerepairdetailid).First()).CurrentValues.SetValues(a);
                }
            }
            if (isnew)
                this.vehiclerepairs.Add(_vehiclerepair); 
            
            if (_vehiclerepair.status == AppModels.Status.submitted)
            {
                this.Entry(_vehiclerepair).Reference(c => c.warehouse).Load();
                statustrail st = new statustrail
                {
                    moduleid = AppModels.Modules.vehiclerepair,
                    requestid = _vehiclerepair.vehiclerepairid,
                    statusid = _vehiclerepair.status,
                    traildate = DateTime.Now,
                    employeeid = _vehiclerepair.warehouse.employees.First().employeeid,
                    remarks = "",
                    treelevel = 0
                };
                this.statustrails.Add(st);

                var districtmanagerid = _vehiclerepair.warehouse.employees.First().districtmanagerid;
                if (_vehiclerepair.warehouse.employees.First().employeetypeid == 111)
                {
                    districtmanagerid = _vehiclerepair.warehouse.employees.First().employeeid;
                }
                List<approvaltree> lsttrees = this.approvaltrees.Where(t => t.districtmanagerid == districtmanagerid && t.moduleid == AppModels.Modules.vehiclerepair).OrderBy(t => t.treelevel).ToList();
                foreach (approvaltree tree in lsttrees)
                {
                    if (tree.employeeid != _vehiclerepair.warehouse.employees.First().employeeid)
                    {
                        _vehiclerepair.endorsedto = tree.employeeid;
                        break;
                    }
                }

            }
            this.SaveChanges();
        }
        public vehiclerepairdetail GetVehiclerepairdetail(long vehiclerepairdetailid)
        {
            if (vehiclerepairdetailid == 0)
                return new vehiclerepairdetail
                {
                    vehiclerepairdetailid = this.vehiclerepairdetails.Select(v => v.vehiclerepairdetailid).DefaultIfEmpty(0).Max() + 1
                };
            return this.vehiclerepairdetails.FirstOrDefault(v => v.vehiclerepairdetailid == vehiclerepairdetailid);
        }

        public void SaveVehicleRepairDetails(bool isnew, vehiclerepairdetail _vehiclerepairdetail)
        {
            if (isnew)
                this.vehiclerepairdetails.Add(_vehiclerepairdetail);
            this.SaveChanges();
        }


        public List<vehiclerepairdetail> GetVehicleRepairDetailRecord(long vehiclerepairid)
        {
            return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid).OrderBy(v => v.supplierno).ToList();
        }
        public List<vehiclerepairdetail> GetVehicleRepairDetailRecord2(long vehiclerepairid, string[] obtainparticular, long[] obtainedvehiclerepairid, int rowindex)
        {
            //return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid).OrderBy(v => v.supplierno).ToList();

            var resultID = this.vehiclerepairdetails.Where(vrd => obtainedvehiclerepairid.Contains(vrd.vehiclerepairdetailid)).
                Select(vrd => vrd.vehiclerepairdetailid).ToList();

            var resultparticulars = this.vehiclerepairdetails.Where(vrd => obtainparticular.Contains(vrd.particulars)).
                Select(vrd => vrd.particulars).ToList();


            if (rowindex > 1)
            {
                return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && !resultparticulars.Contains(vrd.particulars) &&
                        !resultID.Contains(vrd.vehiclerepairdetailid)).ToList();

            }
            else
            {
                if (obtainparticular.Length > 1)
                {
                    return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && resultparticulars.Contains(vrd.particulars) &&
                        !resultID.Contains(vrd.vehiclerepairdetailid)).ToList();
                }
                else
                {
                    return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && !resultparticulars.Contains(vrd.particulars) &&
                        !resultID.Contains(vrd.vehiclerepairdetailid)).ToList();
                }
            }
        }



        public List<vehiclerepairdetail> GetVehicleRepairDetailFilter(long vehiclerepairid, long[] vehiclerepairdetailid, string[] obtainparticulars)
        {
            var resultID = this.vehiclerepairdetails.Where(vrd => vehiclerepairdetailid.Contains(vrd.vehiclerepairdetailid)).
                 Select(vrd => vrd.vehiclerepairdetailid).ToList();

            var resultparticulars = this.vehiclerepairdetails.Where(vrd => obtainparticulars.Contains(vrd.particulars)).
                Select(vrd => vrd.particulars).ToList();

            if (obtainparticulars.Length > 1)
            {
                return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && resultparticulars.Contains(vrd.particulars) && !resultID.Contains(vrd.vehiclerepairdetailid)).ToList();
                //return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && resultparticulars.Contains(vrd.particulars)).ToList();

            }
            else
            {
                return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && !resultparticulars.Contains(vrd.particulars) && !resultID.Contains(vrd.vehiclerepairdetailid)).ToList();

            }


        }

        public List<vehiclerepairdetail> GetVehiclerepairDetailFilter2(long vehiclerepairid, long[] supplierno, string[] test)
        {
            var result = this.suppliers.Where(s => supplierno.Contains(s.supplierno)).
               Select(s => s.supplierno).ToList();

            var resultparticulars = this.vehiclerepairdetails.Where(vrd => test.Contains(vrd.particulars)).
                Select(vrd => vrd.particulars).ToList();

            //return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && !result.Contains((long)vrd.supplierno) && !vrd.particulars.Contains(test)).OrderBy(v => v.supplierno).ToList();

            return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && !result.Contains((long)vrd.supplierno) && !resultparticulars.Contains(vrd.particulars)).OrderBy(v => v.supplierno).ToList();

        }

        public List<vehiclerepairdetail> GetVehiclerepairDetailFilter3(long vehiclerepairid, long supplierno, string[] test)
        {
            var result = this.vehiclerepairdetails.Where(v => v.vehiclerepairid == vehiclerepairid && v.supplierno == supplierno
                && test.Contains(v.particulars)).Select(v => v.particulars).ToList();

            //return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && vrd.supplierno == supplierno && !result.Contains(vrd.particulars))
            //    .OrderBy(v => v.supplierno).ToList();

            return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && !result.Contains(vrd.particulars))
              .OrderBy(v => v.supplierno).ToList();
        }


        public List<vehiclerepairdetail> GetParticulars(long vehiclerepairid)
        {
            return this.vehiclerepairdetails.Where(v => v.vehiclerepairid == vehiclerepairid).OrderBy(v => v.particulars).ToList();
        }

        public List<vehiclerepairdetail> columngenerate(long vehiclerepairid, string[] test)
        {
            //return this.vehiclerepairdetails.GroupBy(item => item.supplierno)
            //    .SelectMany(grouping => grouping.OrderBy(item => item.supplierno).Take(1))
            //    .Where(v => v.vehiclerepairid == vehiclerepairid)
            //    .OrderBy(item => item.supplierno)
            //    .ToList();

            var resultparticulars = this.vehiclerepairdetails.Where(vrd => test.Contains(vrd.particulars)).
               Select(vrd => vrd.particulars).ToList();

            if (test.Length > 1)
            {
                return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid && !resultparticulars.Contains(vrd.particulars)).ToList();
            }
            else
            {
                return this.vehiclerepairdetails.Where(vrd => vrd.vehiclerepairid == vehiclerepairid).ToList();

            }

        }

    }
}