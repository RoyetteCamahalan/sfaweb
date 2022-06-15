using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;

namespace SimpleFFO.Controller
{
    public class Validator
    {
        public static void SetError(WebControl ctrl, bool haserror = false)
        {
            if (haserror)
                ctrl.CssClass += " is-invalid";
            else
                ctrl.CssClass = ctrl.CssClass.Replace(" is-invalid", "");
        }
        public static bool RequiredField(TextBox txt)
        {
            if (txt.Text == "")
            {
                SetError(txt, true);
                return false;
            }
            SetError(txt, false);
            return true;
        }
        public static bool DecZeroAbove(WebControl ctrl, bool showerror=true)
        {
            string input = "";
            if (ctrl.GetType() == typeof(TextBox))
                input = ((TextBox)ctrl).Text;
            else if (ctrl.GetType() == typeof(DropDownList))
                input = ((DropDownList)ctrl).SelectedValue;
            if (Decimal.TryParse(input, out _))
            {
                if (Convert.ToDecimal(input) >= 0)
                {
                    SetError(ctrl);
                    return true;
                }
            }
            SetError(ctrl, showerror);
            return false;
        }

        public static bool DecOneAbove(WebControl ctrl)
        {
            string input = "";
            if (ctrl.GetType() == typeof(TextBox))
                input = ((TextBox)ctrl).Text;
            else if (ctrl.GetType() == typeof(DropDownList))
                input = ((DropDownList)ctrl).SelectedValue;
            if (Decimal.TryParse(input, out _))
            {
                if (Convert.ToDecimal(input) >= 1)
                {
                    SetError(ctrl);
                    return true;
                }
            }
            SetError(ctrl, true);
            return false;
        }
        public static bool CharRequired(WebControl ctrl)
        {
            string input = "";
            if (ctrl.GetType() == typeof(TextBox))
                input = ((TextBox)ctrl).Text;
            else if (ctrl.GetType() == typeof(DropDownList))
                input = ((DropDownList)ctrl).SelectedValue;
            if (input != "")
            {
                SetError(ctrl);
                return true;
            }
            SetError(ctrl, true);
            return false;
        }
        public static bool DateValid(WebControl ctrl)
        {
            string input = "";
            if (ctrl.GetType() == typeof(TextBox))
                input = ((TextBox)ctrl).Text;
            else if (ctrl.GetType() == typeof(DropDownList))
                input = ((DropDownList)ctrl).SelectedValue;

            if (DateTime.TryParse(input, out _))
            {
                SetError(ctrl);
                return true;
            }
            SetError(ctrl, true);
            return false;
        }
        public static int ExtractInt(string input)
        {
            input = Regex.Match(input, @"\d+").Value;
            if (int.TryParse(input, out _))
                return int.Parse(input);
            return 0;
        }
    }
}