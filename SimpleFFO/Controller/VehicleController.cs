using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class VehicleController : SimpleDB
    {
        public companyvehicle GetCompanyVehicle(long id)
        {
            if (id == 0)
                return new companyvehicle {
                    vehicleid= this.companyvehicles.Select(e => e.vehicleid).DefaultIfEmpty(0).Max() + 1,
                };
            return this.companyvehicles.Find(id);
        }
        public List<companyvehicle> GetCompanyVehicles()
        {
            return this.companyvehicles.OrderBy(c => c.vehiclename).ToList();
        }

        public List<object> getActiveWarehouses()
        {
            List<object> lst= (from e in employees
                               join w in warehouses on e.warehouseid equals w.warehouseid
                               where (w.isactive ?? true) && (e.isactive ?? true)
                               select new { warehouseid = e.warehouseid, fullname = e.firstname + " " + e.lastname }).ToList<object>();
            return lst;
        }
        public warehouse getWarehouse(long id)
        {
            return this.warehouses.Where(w=> w.warehouseid==id).FirstOrDefault();
        }
        public long GetVehicleID(long? warehouseid)
        {
            companyvehicle vehicle = warehouses.Where(w => w.warehouseid == warehouseid).FirstOrDefault().companyvehicle;
            if (vehicle == null)
                return 0;
            else
                return vehicle.vehicleid;
        }

        public long GetCurrentOdo(long? warehouseid)
        {
            companyvehicle vehicle = warehouses.Where(w => w.warehouseid == warehouseid).FirstOrDefault().companyvehicle;
            if (vehicle == null)
                return 0;
            else
                return (long)vehicle.currentodo;
        }
    }
}