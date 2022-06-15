using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SimpleFFO.Controller;
using SimpleFFO.Model;

namespace SimpleFFO.views
{
    public partial class CallReportDetailInfo : System.Web.UI.Page
    {
        CallController _callsController;
        protected void Page_Load(object sender, EventArgs e)
        {
            _callsController = new CallController();

            if (!IsPostBack)
            {
                if (Page.RouteData.Values.ContainsKey("id"))
                {
                    long call_id = Convert.ToInt64(Auth.AppSecurity.URLDecrypt(Page.RouteData.Values["id"].ToString()));
                    call c = _callsController.GetCall(call_id);
                    if (c == null)
                        return;

                    txtbox_docode.Text = c.institutiondoctormap.doctor.doc_code;
                    txtbox_fullname.Text = c.institutiondoctormap.doctor.doc_firstname + " " + c.institutiondoctormap.doctor.doc_lastname;
                    txtbox_specialization.Text = c.institutiondoctormap.doctor.specialization.name;
                    txtbox_birthday.Text = (c.institutiondoctormap.doctor.birthdate ?? DateTime.MinValue).ToString("dd MMMM yyyy");
                    txtbox_contact.Text = c.institutiondoctormap.doctor.contact_number;
                    txtbox_license.Text = c.institutiondoctormap.doctor.prc_licensed.ToString();
                    txtbox_emailAddress.Text = c.institutiondoctormap.doctor.email_address.ToString();
                    txtbox_institution.Text = c.institutiondoctormap.institution.inst_name.ToString();

                    txtbox_callDate.Text = (c.start_datetime ?? DateTime.MinValue).ToString("dddd, dd MMMM yyyy");
                    txtbox_planned.Text = (c.planned ?? false) ? "Yes" : "No";
                    txtbox_makeup.Text = (c.makeup ?? false) ? "Yes" : "No";
                    txtbox_started.Text = (c.start_datetime ?? DateTime.MinValue).ToString("hh:mm tt");
                    txtbox_ended.Text = (c.end_datetime ?? DateTime.MinValue).ToString("hh:mm tt");


                    if (c.signatures.FirstOrDefault() != null)
                        imgSRC.ImageUrl = AppModels.imageurl + c.signatures.First().path;
                    else
                        imgSRC.ImageUrl = AppModels.noimageurl;

                    List<callnote> _callnote = c.callnotes.ToList();
                    lstnotes.DataSource = _callnote;
                    lstnotes.DataTextField = "notes";
                    lstnotes.DataValueField = "call_note_id";
                    lstnotes.DataBind();

                    urlframe.Attributes.Add("src", Utility.getMapEmbeddedURL(c.longitude,c.latitude));
                }
            }
            else
            {

            }
        }
    }
}