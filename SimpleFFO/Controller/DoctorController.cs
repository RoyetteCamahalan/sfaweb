using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class DoctorController : SimpleDB
    {
        public institutiondoctormap GetInstitutiondoctormap(long id)
        {
            if (id == 0)
                return new institutiondoctormap();
            return this.institutiondoctormaps.Find(id);
        }
        public doctor GetDoctor(int doc_id)
        {
            if (doc_id == 0)
                return new doctor();
            return this.doctors.FirstOrDefault(i => i.doc_id == doc_id);
        }
        public int GenerateDocID(long warehouseid)
        {
            int finalid = (from d in this.doctors
                          join idm in this.institutiondoctormaps on d.doc_id equals idm.doc_id
                          join i in this.institutions on idm.inst_id equals i.inst_id
                          where i.warehouseid == warehouseid && d.doc_id.ToString().StartsWith(warehouseid.ToString())
                           select d.doc_id).DefaultIfEmpty(0).Max();
            if (finalid == 0)
                finalid = Convert.ToInt32(warehouseid.ToString() + "0000");
            return (finalid+1);
        }
        public  bool isNameExists(string lastname, string firstname)
        {
            return this.doctors.Where(x => x.doc_lastname.ToLower() == lastname.ToLower() && x.doc_firstname.ToLower() == firstname.ToLower()).Count() > 0;
        }
        public List<specialization> GetSpecializations()
        {
            int[] ids = { 2, 18, 37 };
            return this.specializations.Where(x=> !ids.Contains(x.specialization_id)).OrderBy(x=> x.name).ToList();
        }
        public List<doctorclass> GetDoctorclasses()
        {
            int[] ids = { 1, 4 };
            return this.doctorclasses.Where(x => ids.Contains(x.doctor_class_id)).OrderBy(x => x.name).ToList();
        }

        public List<object> GetDoctorsDroplst(long warehouseid,string search="")
        {
            List<object> doctors = ((List<object>)(from d in this.doctors
                                    join idm in this.institutiondoctormaps on d.doc_id equals idm.doc_id
                                    join i in this.institutions on idm.inst_id equals i.inst_id
                                    join sp in this.specializations on d.specialization_id equals sp.specialization_id
                                    where ((i.warehouseid ?? 0) == warehouseid && idm.class_id !=3 && idm.class_id != 6 && idm.class_id != 7 && (d.isactive ?? true))
                                            && (d.doc_lastname.StartsWith(search) || d.doc_firstname.StartsWith(search))
                                                   select new {
                                       fullname = d.doc_lastname + " " + d.doc_firstname + " | " + i.inst_name,
                                       name = d.doc_lastname + " " + d.doc_firstname,
                                       doc_id = d.doc_id,
                                       specialization = sp.name 
                                   }).ToList<object>());
            return doctors;
        }

        public List<institutiondoctormap> GetForApproval(long warehouseid)
        {
            return (from i in institutions
                    join idm in institutiondoctormaps on i.inst_id equals idm.inst_id
                    join d in doctors on idm.doc_id equals d.doc_id
                    where i.warehouseid == warehouseid &&
                        ((d.isactive == false && d.updatedbyid == 0) || (d.isactive == true && d.deleted_at != null))
                    select idm).ToList();
        }
        public List<GenericObject> GetSpecializationPerWarehouse(long warehouseid)
        {
            int[] ids = { 2, 18, 37 };

            List<GenericObject> result = new List<GenericObject>();

            result = (from idm in this.institutiondoctormaps
                      join d in this.doctors on idm.doc_id equals d.doc_id
                      join i in this.institutions on idm.inst_id equals i.inst_id
                      join sp in this.specializations on d.specialization_id equals sp.specialization_id
                      where i.warehouseid == warehouseid && !ids.Contains(sp.specialization_id)
                      select new
                      {
                          sp.specialization_id,
                          sp.name
                      } into x
                      group x by new { x.specialization_id, x.name } into g
                      select new GenericObject
                      {
                          specialization_id = g.Key.specialization_id,
                          name = g.Key.name
                      }
                      ).OrderBy(x => x.name).ToList();

            return result;
        }
    }
}