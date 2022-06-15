using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class JointCallController : SimpleDB
    {
        public jointcall GetjointCall(long id)
        {
            return this.jointcalls.Find(id);
        }

        public service GetServicecall(int id)
        {
            return this.services.Find(id);
        }

        public List<Object> GetCallMaterials(long? call_id)
        {
            List<object> request = new List<object>();

            request = (from cm in this.callmaterials
                       join m in this.materials on cm.material_id equals m.material_id
                       where cm.call_id == call_id
                       select new { cm.call_id, name = m.material_name }).ToList<object>();
            return request;
        }

        public List<Object> GetCallProducts(long? call_id)
        {
            List<object> request = new List<object>();

            request = (from cm in this.callmaterials
                       join p in this.products on cm.product_id equals p.product_id
                       where cm.call_id == call_id
                       select new { cm.call_id, p.name }).ToList<object>();
            return request;
        }


        public List<callevaluation> GetJointRate(long call_id, int evaltypeid)
        {
            return this.callevaluations.Where(x => x.call_id == call_id && x.evaluation.evaltypeid == evaltypeid).OrderBy(x => x.evaluation.evalpriority).ToList();
        }
    }
}
