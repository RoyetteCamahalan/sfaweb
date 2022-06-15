using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class SupplierController : SimpleDB
    {
        public supplier getSupplier(long supplierno)
        {
            if (supplierno == 0)
                return new supplier{ supplierno = this.suppliers.Select(s => s.supplierno).DefaultIfEmpty(0).Max() + 1 };
            return this.suppliers.FirstOrDefault(i => i.supplierno == supplierno);
        }
        public List<supplier> GetRepairShops(long branchid, bool strict)
        { 
            if(strict)
                return this.suppliers.Where(i => (i.branchid == branchid || branchid == 0) && (i.isrepairshop ?? false) == true).ToList();
            return this.suppliers.Where(i => (i.branchid == branchid || (i.branchid ?? 0) == 0) && (i.isrepairshop ?? false) == true).ToList();
        }
        public bool isExist(long id, string name)
        {
            return this.suppliers.Where(i => i.suppliername == name && i.supplierno != id).Count() > 0;
        }
    }
}