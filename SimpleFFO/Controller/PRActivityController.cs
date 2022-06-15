using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SimpleFFO.Controller
{
    public class PRActivityController : SimpleDB
    {
        public practivity GetPRActivity(long practivityid)
        {
            if (practivityid == 0)
                return new practivity { 
                    practivityid = this.practivities.Select(s => s.practivityid).DefaultIfEmpty(0).Max() + 1, 
                    status=AppModels.Status.draft };
            return this.practivities.FirstOrDefault(i => i.practivityid == practivityid);
        }

        public List<activity> GetActivities()
        {
            return this.activities.Where(a => a.isactive ?? true).ToList();
        }

        public List<budgettype> GetBudgettypes()
        {
            return this.budgettypes.Where(b => b.isactive ?? true).ToList();
        }
        public void SavePRActivity(bool isnew, practivity practivity,List<attendee> attendees, List<practivityoutcome> practivityoutcomes, List<practivitybudget> practivitybudgets)
        {
            //remove items from list
            foreach (var existingChild in practivity.attendees.ToList())
            {
                if (!attendees.Any(c => c.attendeeid == existingChild.attendeeid))
                    this.attendees.Remove(existingChild);
            }
            foreach (var existingChild in practivity.practivityoutcomes.ToList())
            {
                if (!practivityoutcomes.Any(c => c.practivityoutcomeid == existingChild.practivityoutcomeid))
                    this.practivityoutcomes.Remove(existingChild);
            }
            foreach (var existingChild in practivity.practivitybudgets.ToList())
            {
                if (!practivitybudgets.Any(c => c.practivitybudgetid == existingChild.practivitybudgetid))
                    this.practivitybudgets.Remove(existingChild);
            }

            //add/update list
            foreach (attendee a in attendees)
            {

                if (a.attendeeid == 0)
                    practivity.attendees.Add(a);
                else
                {
                    this.Entry(practivity.attendees.Where(p => p.attendeeid == a.attendeeid).First()).CurrentValues.SetValues(a);
                }
            }
            foreach (practivityoutcome a in practivityoutcomes)
            {

                if (a.practivityoutcomeid == 0)
                    practivity.practivityoutcomes.Add(a);
                else
                {
                    this.Entry(practivity.practivityoutcomes.Where(p => p.practivityoutcomeid == a.practivityoutcomeid).First()).CurrentValues.SetValues(a);
                }
            }
            foreach (practivitybudget a in practivitybudgets)
            {

                if (a.practivitybudgetid == 0)
                    practivity.practivitybudgets.Add(a);
                else
                {
                    this.Entry(practivity.practivitybudgets.Where(p => p.practivitybudgetid == a.practivitybudgetid).First()).CurrentValues.SetValues(a);
                }
            }
            if (isnew)
            {
                practivity.datecreated = DateTime.Now;
                this.practivities.Add(practivity);
            }

            if (practivity.status == AppModels.Status.submitted)
            {
                this.Entry(practivity).Reference(c => c.warehouse).Load();
                ApprovalController approvalController = new ApprovalController();
                practivity.endorsedto = approvalController.SaveTrailOnSubmit(AppModels.Modules.practivity, practivity.practivityid,practivity.status ?? 0, practivity.warehouse.employees.First(),"");
            }
            this.SaveChanges();
        }
    }
}