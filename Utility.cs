using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NightShift
{
    internal class Utility
    {
        public static bool IsNumber(string str)
        {
            if (str.All(char.IsDigit))
                return true;
            else
                return false;
        }

        public static DateTime? ParseDate(string sDate, char delim)
        {
            //Converts yyyy mm dd string to datetime split by delim char
            string[] fnSplit = sDate.Split(delim);
            if (fnSplit.Length < 3)
                return null;
            string lYear = fnSplit[0].Trim();
            string lMonth = fnSplit[1].Trim();
            string lDay = fnSplit[2].Trim();
            if (Utility.IsNumber(lYear) && Utility.IsNumber(lMonth) && Utility.IsNumber(lDay))
                return new DateTime(Convert.ToInt32(lYear), Convert.ToInt32(lMonth), Convert.ToInt32(lDay));
            else
                return null;
        }

        public static TimeSpan? ParseHour(string sHour, char delim)
        {
            //Converts yyyy mm dd string to datetime split by delim char
            string[] fnSplit = sHour.Split(delim);
            if (fnSplit.Length < 3)
                return null;
            string lHour = fnSplit[0].Trim();
            string lMinute = fnSplit[1].Trim();
            string lSecond = fnSplit[2].Trim();
            if (Utility.IsNumber(lHour) && Utility.IsNumber(lMinute) && Utility.IsNumber(lSecond))
                return TimeSpan.FromHours(Convert.ToDouble(lHour) + Convert.ToDouble(lMinute) / 60 + Convert.ToDouble(lSecond) / 3600);
            else
                return null;
        }

    }
}
