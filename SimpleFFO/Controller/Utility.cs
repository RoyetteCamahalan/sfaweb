using SimpleFFO.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SimpleFFO.Controller
{
    public class Utility
    {

        public static DateTime getServerDate()
        {
            return DateTime.Now;
        }
        public static DateTime getFirstDayofCurrentWeek()
        {
            DateTime now = DateTime.Now;
            DateTime startOfWeek = now.AddDays(0 - (int)now.DayOfWeek);
            return startOfWeek;
        }
        private static readonly Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static long ToInt64(object obj)
        {
            if (obj == DBNull.Value)
                return 0;
            return Convert.ToInt64(obj);
        }
        public static int ToInt32(object obj)
        {
            if (obj == DBNull.Value)
                return 0;
            return Convert.ToInt32(obj);
        }
        public static decimal ToDecimal(object obj)
        {
            if (obj == DBNull.Value)
                return 0;
            return Convert.ToDecimal(obj);
        }

        public static string getImage(long warehouseid, string filename, enfiletype ft, bool isurl = false)
        {
            string foldername = "";
            if (ft == enfiletype.odo)
                foldername = "/" + AppModels.odofolder + "/";
            else if(ft == enfiletype.disbursementreceipt)
                foldername = "/" + AppModels.receiptfolder + "/";

            if (isurl)
                return "images/" + warehouseid + foldername + filename;
            return "~/images/" + warehouseid + foldername + filename;
        }
        public static string getFolder(long warehouseid, enfiletype ft)
        {
            if (ft == enfiletype.odo)
                return "/images/" + warehouseid + "/" + AppModels.odofolder;
            else if (ft == enfiletype.disbursementreceipt)
                return "/images/" + warehouseid + "/" + AppModels.receiptfolder;
            else
                return "";
        }
        public enum enfiletype
        {
            odo=0,
            disbursementreceipt=1
        }
        public static string saveImage(Page page, long warehouseid, FileUpload fu, enfiletype ft)
        {
            string imgName = Utility.RandomString(15) + "_" + fu.FileName;
            string imgPath = "/images/" + warehouseid + "/";
            if (ft == enfiletype.odo)
                imgPath += AppModels.odofolder;
            else if (ft == enfiletype.disbursementreceipt)
                imgPath += AppModels.receiptfolder;
            Directory.CreateDirectory(page.Server.MapPath(imgPath));
            imgPath = imgPath + "/" + imgName;
            fu.SaveAs(page.Server.MapPath(imgPath));
            return imgName;
        }

        public static string getMapEmbeddedURL(string longitude, string latitude)
        {
            return "https://maps.google.com/maps?q=" + latitude + ", " + longitude + "&z=15&output=embed";
        }

        private static string ConvertWholeNumber(string number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;
                bool isDone = false;
                double dblAmt = (Convert.ToDouble(number));
                if (dblAmt > 0)
                {
                    beginsZero = number.StartsWith("0");
                    int numDigits = number.Length;
                    int pos = 0;
                    string place = "";
                    switch (numDigits)
                    {
                        case 1:
                            word = ones(number);
                            isDone = true;
                            break;
                        case 2:
                            word = tens(number);
                            isDone = true;
                            break;
                        case 3:
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4:
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7:
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10:
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {
                        if (number.Substring(0, pos) != "0" && number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(number.Substring(0, pos)) + place + ConvertWholeNumber(number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(number.Substring(0, pos)) + ConvertWholeNumber(number.Substring(pos));
                        }
                    }
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }

        public static String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points, andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";
                        endStr = "" + endStr;
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return val;
        }

        private static String ConvertDecimals(String number)
        {
            String cd = "", digit, engOne;
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = " ";
                }
                else if (i == 0)
                {
                    digit += "0";
                    engOne = tens(digit);
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }

        private static string ones(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            string name = "";
            switch (_Number)
            {
                case 1:
                    name = "One ";
                    break;
                case 2:
                    name = "Two ";
                    break;
                case 3:
                    name = "Three ";
                    break;
                case 4:
                    name = "Four ";
                    break;
                case 5:
                    name = "Five ";
                    break;
                case 6:
                    name = "Six ";
                    break;
                case 7:
                    name = "Seven ";
                    break;
                case 8:
                    name = "Eight ";
                    break;
                case 9:
                    name = "Nine ";
                    break;
            }
            return name;
        }

        private static string tens(string Number)
        {
            int _Number = Convert.ToInt32(Number);
            string name = "";
            switch (_Number)
            {
                case 10:
                    name = "Ten ";
                    break;
                case 11:
                    name = "Eleven ";
                    break;
                case 12:
                    name = "Twelve ";
                    break;
                case 13:
                    name = "Thirteen ";
                    break;
                case 14:
                    name = "Fourteen ";
                    break;
                case 15:
                    name = "Fifteen ";
                    break;
                case 16:
                    name = "Sixteen ";
                    break;
                case 17:
                    name = "Seventeen ";
                    break;
                case 18:
                    name = "Eighteen ";
                    break;
                case 19:
                    name = "Nineteen ";
                    break;
                case 20:
                    name = "Twenty ";
                    break;
                case 30:
                    name = "Thirty ";
                    break;
                case 40:
                    name = "Fourty ";
                    break;
                case 50:
                    name = "Fifty ";
                    break;
                case 60:
                    name = "Sixty ";
                    break;
                case 70:
                    name = "Seventy ";
                    break;
                case 80:
                    name = "Eighty ";
                    break;
                case 90:
                    name = "Ninety ";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
    }
}