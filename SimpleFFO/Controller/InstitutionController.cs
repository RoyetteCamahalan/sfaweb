using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class InstitutionController : SimpleDB 
    {
        public InstitutionController()
        {
        }
        public institution getInstitution(int ins_id)
        {
            if (ins_id == 0)
                return new institution();
            return this.institutions.FirstOrDefault(i => i.inst_id==ins_id);
        }
        public int GenerateInstID(long warehouseid)
        {
            int attid = Convert.ToInt32(warehouseid.ToString() + "999");
            int finalid = this.institutions.Where(x => x.warehouseid == warehouseid && x.inst_id != attid).Select(x => x.inst_id).DefaultIfEmpty(0).Max();
            if (finalid == 0)
                finalid = Convert.ToInt32(warehouseid.ToString() + "000");
            return (finalid+1);
        }
        public List<institution> GetAll(long warehouseid)
        {
            return this.institutions.Where(i => (i.warehouseid ?? 0) == warehouseid && !(i.isattendance ?? false) && i.inst_name!= "GENERIC" && i.inst_name != "PH GENERIC").ToList();
        }
        public List<institutiontype> GetInstitutionTypes()
        {
            return this.institutiontypes.Where(i => (i.isactive ?? false)).ToList();
        }

        public bool isExist(int id,string name)
        {
            return this.institutions.Where(i => i.inst_name == name && i.inst_id!=id).Count() > 0;
        }
    }
}