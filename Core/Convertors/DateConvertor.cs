using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Core.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" +
                   pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00");
        }
        public static string ToShamsiWithTime(this DateTime value)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(value) + "/" +
                   pc.GetMonth(value).ToString("00") + "/" +
                   pc.GetDayOfMonth(value).ToString("00") + " " + pc.GetHour(value).ToString("0#") + ":" + pc.GetMinute(value).ToString("0#");
        }
        public static string ToShamsiN(this DateTime? value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {

                PersianCalendar pc = new PersianCalendar();
                return pc.GetYear((DateTime)value) + "/" +
                       pc.GetMonth((DateTime)value).ToString("00") + "/" +
                       pc.GetDayOfMonth((DateTime)value).ToString("00");


            }
        }
        public static string ToShamsiN_WithTime(this DateTime? value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {

                PersianCalendar pc = new PersianCalendar();
                return pc.GetYear((DateTime)value) + "/" +
                       pc.GetMonth((DateTime)value).ToString("00") + "/" +
                       pc.GetDayOfMonth((DateTime)value).ToString("00")
                + " | " + pc.GetHour((DateTime)value).ToString("0#") + ":" + pc.GetMinute((DateTime)value).ToString("0#") + ":" + pc.GetSecond((DateTime)value).ToString("0#");
            }
        }
        public static DateTime ToMiladiWithoutTime(this string persianDate)
        {
            //PersianCalendar pc = new PersianCalendar();

            //var persianDateSplitedParts = persianDate.Split('/');
            //DateTime dateTime = new DateTime(int.Parse(persianDateSplitedParts[0]), int.Parse(persianDateSplitedParts[1]), int.Parse(persianDateSplitedParts[2]), pc);
            //return DateTime.Parse(dateTime.ToString(CultureInfo.CreateSpecificCulture("en-US")));
            string[] dte = persianDate.Split("/");
            int dy =int.Parse(dte[0]);
            int dm = int.Parse(dte[1]);
            int dd = int.Parse(dte[2]);
            return new DateTime(dy, dm, dd, new PersianCalendar());
        }
        public static DateTime ToMiladiWithTime(this string ShamsiDate, string Time)
        {
            PersianCalendar pc = new PersianCalendar();
            var persianDateSplitedParts = ShamsiDate.Split('/');
            int H = 0; int M = 0; int S = 0;
            if (!string.IsNullOrEmpty(Time))
            {
                if (Time.Contains(":"))
                {
                    string[] tt = Time.Split(":");
                    if (tt.Length != 0)
                    {
                        if (tt.Length == 1)
                        {
                            H = int.Parse(tt[0]);
                        }
                        if (tt.Length == 2)
                        {
                            H = int.Parse(tt[0]);
                            M = int.Parse(tt[1]);
                        }
                        if (tt.Length == 3)
                        {
                            H = int.Parse(tt[0]);
                            M = int.Parse(tt[1]);
                            S = int.Parse(tt[2]);
                        }
                    }
                }
            }
            DateTime dateTime = new DateTime(int.Parse(persianDateSplitedParts[0]), int.Parse(persianDateSplitedParts[1]), int.Parse(persianDateSplitedParts[2]), H, M, S, pc);
            return dateTime;
        }

    }
}
