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
            int[] ids ={ 1, 2 };
            if (strict)
                //return this.suppliers.Where(i => (i.branchid == branchid || branchid == 0) && (i.isrepairshop ?? false) == true).ToList();
                return this.suppliers.Where(i => (i.branchid == branchid || branchid == 0) && (ids.Contains((int) i.isactive)) && i.vendorcategoryid == 2).ToList();
            //return this.suppliers.Where(i => (i.branchid == branchid || (i.branchid ?? 0) == 0) && (i.isrepairshop ?? false) == true).ToList();
            return this.suppliers.Where(i => (i.branchid == branchid || (i.branchid ?? 0) == 0) && (ids.Contains((int) i.isactive)) && i.vendorcategoryid == 2).ToList();
        }
        public bool isExist(long id, string name)
        {
            return this.suppliers.Where(i => i.suppliername == name && i.supplierno != id).Count() > 0;
        }

        public List<supplier> GetLiquidationSupplier()
        {
            int[] ids = { 1 ,3 };
            return this.suppliers.Where(x => ids.Contains((int)x.vendorcategoryid)).OrderBy(x => x.suppliername).ToList();
        }

        public List<specialization> GetSpecializations()
        {
            int[] ids = { 2, 18, 37 };
            return this.specializations.Where(x => !ids.Contains(x.specialization_id)).OrderBy(x => x.name).ToList();
        }


        public void SaveVendors(List<Vendor> list)
        {
            bool validation;

            foreach (var data in list)
            {
                if (validation = confimation(data.Vendorid))
                {
                    supplier result = new supplier
                    {
                        supplierno = data.Vendorid,
                        suppliername = data.Vendorname,
                        isactive = data.Status,
                        branchid = data.Branchid,
                        created_at = DateTime.Now,
                        vendorcategoryid = data.VendorCategoryId,
                        createdbyid = 0,
                    };
                    this.suppliers.Add(result);
                }
                this.SaveChanges();
            }
        }

        public void SaveVendorCategory(List<vendorcategory> list)
        {
            bool validation;

            foreach(var data in list)
            {
                if(validation = vendorconfirmation(data.vendorcategoryid))    
                {
                    vendorcategory result = new vendorcategory
                    {
                        vendorcategoryid = data.vendorcategoryid,
                        vendorcategoryname = data.vendorcategoryname,
                        isactive = data.isactive,
                        created_at = DateTime.Now
                    };
                    this.vendorcategories.Add(result);
                }
                this.SaveChanges();
            }
        }

        private bool confimation(long id)
        {
            var query = this.suppliers.Where(s => s.supplierno == id).Select(s => s.supplierno).ToList();
            if (query.Count > 0)
                return false;
            return true;
        }

        private bool vendorconfirmation(int id)
        {
            var query = this.vendorcategories.Where(v => v.vendorcategoryid == id).Select(s => s.vendorcategoryid).ToList();
            if (query.Count > 0)
                return false;
            return true;
        }
    }
}