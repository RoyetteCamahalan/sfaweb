using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class ProductController : SimpleDB
    {
        public product getProduct(long product_id)
        {
            if (product_id == 0)
                return new product { product_id = this.products.Select(s => s.product_id).DefaultIfEmpty(0).Max() + 1 };
            return this.products.FirstOrDefault(i => i.product_id == product_id);
        }
        public itemcategory getProductCategory(string itemcatcode)
        {
            if (itemcatcode == "0")
                return new itemcategory { itemcatcode = (Convert.ToInt32(this.itemcategories.Select(s => s.itemcatcode).DefaultIfEmpty("0").Max()) + 1).ToString() };
            return this.itemcategories.FirstOrDefault(i => i.itemcatcode == itemcatcode);
        }
        public List<product> GetProductList()
        {
            return this.products.OrderBy(s => s.name).ToList();
        }
        public List<itemcategory> GetItemCategories()
        {
            return this.itemcategories.OrderBy(s => s.itemcatdescription).ToList();
        }
        public List<packaging> GetPackagings()
        {
            return this.packagings.Where(p => (p.isactive ?? false)).ToList();
        }
        public bool isExist(int id, string name)
        {
            return this.products.Where(i => i.name == name && i.product_id != id).Count() > 0;
        }
        public bool isCategoryExist(string id, string name)
        {
            return this.itemcategories.Where(i => i.itemcatdescription == name && i.itemcatcode != id).Count() > 0;
        }
    }
}