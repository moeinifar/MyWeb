using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace MyWeb.Models.Utility
{
    public class ConvertDateToPersianCalendar
    {
        public DateTime ConvertToPersian(DateTime dt)
        {
            try
            {
                PersianCalendar pr = new PersianCalendar();
                int Day = pr.GetDayOfMonth(dt);
                int Month = pr.GetMonth(dt);
                int Year = pr.GetYear(dt);
                return Convert.ToDateTime(Year.ToString() + "/" + Month.ToString() + "/" + Day.ToString());
            }
            catch
            {
                return Convert.ToDateTime("1300/1/1");
            }
        }
    }
}