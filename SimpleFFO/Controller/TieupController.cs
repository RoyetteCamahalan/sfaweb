using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class TieupController : SimpleDB
    {
        public tieup GetTieup(long tieupid)
        {
            if (tieupid == 0)
                return new tieup
                {
                    tieupid = this.tieups.Select(s => s.tieupid).DefaultIfEmpty(0).Max() + 1
                };
            return this.tieups.FirstOrDefault(i => i.tieupid == tieupid);
        }

        public List<object> GetTieupProducts(long tieupid,int typeid, bool hidenotselected)
        {
            List<object> lst = new List<object>();
            /*if (tieupid > 0)
                lst = this.tieupproducts.Where(t => t.tieupid == tieupid && t.product.itemtypeid==typeid).ToList<object>();
*/
            if (hidenotselected)
            {
                lst = (from p in this.products
                       join t in this.tieupproducts on p.product_id equals t.product_id
                       where t.tieupid == tieupid
                       select new
                       {
                           t.tieupproductid,
                           p.name,
                           p.product_id,
                           price = t.price ?? p.price,
                           pinmoney = (t.pinmoney ?? 0) == 0 ? p.pinmoney : t.pinmoney,
                           rebate = t.rebate ?? 0,
                           monthlyqty = t.monthlyqty ?? 0,
                           discount = t.discount ?? 0,
                       }).ToList<object>();
            }
            else
            {
                lst = (from p in this.products
                       join t in this.tieupproducts on p.product_id equals t.product_id into fg
                       from fgi in fg.Where(f => f.tieupid == tieupid).DefaultIfEmpty()
                       where p.itemtypeid == typeid
                       select new
                       {
                           tieupproductid = (long?)fgi.tieupproductid ?? 0,
                           p.name,
                           p.product_id,
                           price = fgi.price ?? p.price,
                           pinmoney = (fgi.pinmoney ?? 0) == 0 ? p.pinmoney : fgi.pinmoney,
                           rebate = fgi.rebate ?? 0,
                           monthlyqty = fgi.monthlyqty ?? 0,
                           discount = fgi.discount ?? 0,
                       }).ToList<object>();
            }

            return lst;
        }

        public void SaveTieUp(bool isnew, tieup tieup, List<tieupproduct> tieupproducts)
        {
            //remove items from list
            foreach (var existingChild in tieup.tieupproducts.ToList())
            {
                if (!tieupproducts.Any(c => c.tieupproductid == existingChild.tieupproductid))
                    this.tieupproducts.Remove(existingChild);
            }
            //add/update list
            foreach (tieupproduct a in tieupproducts)
            {

                if (a.tieupproductid == 0)
                    tieup.tieupproducts.Add(a);
                else
                {
                    a.tieupid = tieup.tieupid;
                    this.Entry(tieup.tieupproducts.Where(p => p.tieupproductid == a.tieupproductid).First()).CurrentValues.SetValues(a);
                }
            }
            if (isnew)
            {
                tieup.datecreated = Utility.getServerDate();
                this.tieups.Add(tieup);
            }

            if (tieup.status == AppModels.Status.submitted)
            {
                this.Entry(tieup).Reference(c => c.warehouse).Load();
                ApprovalController approvalController = new ApprovalController();
                tieup.endorsedto = approvalController.SaveTrailOnSubmit(tieup.tieupclass == AppModels.Tieup.TieupClass.STOP ? AppModels.Modules.stop : AppModels.Modules.tup, tieup.tieupid, tieup.status ?? 0, tieup.warehouse.employees.First(), "");
            }
            this.SaveChanges();
        }
    }
}