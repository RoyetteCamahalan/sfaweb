using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace SimpleFFO.Controller
{
    public class PageController
    {
        public enum EventType
        {
            click=0
        }
        public enum AlertType
        {
            success = 0,
            error=1,
            info=2,
            warning=3
        }
        public static void fnFireEvent(Page page, EventType e,string id,bool async=false)
        {
            string _event="";
            switch (e)
            {
                case EventType.click:
                    _event = "click";
                    break;
            }
            if(async)
                ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), "fnFireEvent('" + _event + "','" + id +"');", true);
            else
                page.Page.ClientScript.RegisterStartupScript(page.GetType(), "script", "fnFireEvent('" + _event + "','" + id + "');", true);
        }
        public static void fnScroll(Page page, string id, bool isclass, bool usejs)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), "fnScroll('" + id + "','" + (isclass ? "1" : "0") + "','" + (usejs ? "1" : "0") + "');", true);
            //page.Page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), "fnScroll('" + id + "','" + isclass + "');", true);
        }
        public static void fnShowLoader(Page page, string id)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), "fnShowLoader('" + id + "');", true);
        }
        public static void fnHideLoader(Page page, string id)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), "fnHideLoader('" + id + "');", true);
        }
        public static void fnShowAlert(Page page, AlertType t, string msg)
        {
            string _t = "";
            switch (t)
            {
                case AlertType.success:
                    _t = "success";
                    break;
                case AlertType.error:
                    _t = "error";
                    break;
                case AlertType.info:
                    _t = "info";
                    break;
                case AlertType.warning:
                    _t = "warning";
                    break;
            }
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), "fnshowalert('" + _t + "','" + msg + "');", true);
        }
        
        public static void fnOpenInNewTab(Page page, string url)
        {
            ScriptManager.RegisterStartupScript(page, page.GetType(), Guid.NewGuid().ToString(), "fnOpenInNewTab('" + url + "');", true);
        }
    }
}