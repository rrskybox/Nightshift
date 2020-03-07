using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightShift
{
    class HumasonReader
    {
        //Class to read Humason log files, maybe session info, who knows?
        private const string HumasonDir = "Humason";

        private string HumasonLogTodayFilePath { get; set; } = null;
        private string HumasonLogTomorrowFilePath { get; set; } = null;

        private List<string> HumasonTodayLog = null;
        private List<string> HumasonTomorrowLog = null;
        public List<string> HumasonJoinedLog = new List<string>();
        public List<DateTime> HumasonLogDates = new List<DateTime>();

        public HumasonReader(DateTime logDate)
        {
            //Verifies file paths for Humason, Hamason/Logs, and Humason/Logs/date.log
            //sets HumasonLogfilePath to null if not found
            string hlDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + HumasonDir + "\\Logs";
            //Check to see if NS log file exists, if not create it
            if (!Directory.Exists(hlDir)) return;
            string lpToday = hlDir + "\\" + logDate.ToString("yyyy-MM-dd") + ".log";
            string lpTomorrow = hlDir + "\\" + logDate.AddDays(1).ToString("yyyy-MM-dd") + ".log";
            //Create list of file names in Log Directory
            // HumasonLogList = Directory.GetFiles(hlDir).Select(Path.GetFileName).ToList();
            HumasonLogDates = ParseDates(hlDir);

            if (File.Exists(lpToday))
            {
                HumasonLogTodayFilePath = lpToday;
                HumasonTodayLog = File.ReadLines(lpToday).ToList();
            }
            // Now do the same for the following day
            if (File.Exists(lpTomorrow))
            {
                HumasonLogTomorrowFilePath = lpTomorrow;
                HumasonTomorrowLog = File.ReadLines(lpTomorrow).ToList();
            }
            //Join the two (if two) logs
            //First, load the two logs into enumerable lists
            if (HumasonTodayLog != null)
                //Write all lines in today's Humason Log that are after 1PM into the output list
                foreach (string line in HumasonTodayLog)
                {
                    if (line != "")
                    {
                        string lTime = line.Split(' ')[0];
                        string[] lSplit = lTime.Split(':');
                        TimeSpan oClock = new TimeSpan(Convert.ToInt32(lSplit[0]), Convert.ToInt32(lSplit[1]), Convert.ToInt32(lSplit[2]));
                        if (oClock > new TimeSpan(13, 0, 0)) HumasonJoinedLog.Add(line);
                    }
                }
            //Write all the lines in tomorrows Humason log that are before 11AM
            if (HumasonTomorrowLog != null)
            {
                foreach (string line in HumasonTomorrowLog)
                {
                    if (line != "")
                    {
                        string lTime = line.Split(' ')[0];
                        string[] lSplit = lTime.Split(':');
                        TimeSpan oClock = new TimeSpan(Convert.ToInt32(lSplit[0]), Convert.ToInt32(lSplit[1]), Convert.ToInt32(lSplit[2]));
                        if (oClock < new TimeSpan(11, 0, 0)) HumasonJoinedLog.Add(line);
                    }
                }
            }
            return;
        }

        private List<DateTime> ParseDates(string logDirectory)
        {
            //searches all the log files for dates which have logs based on am/pm data
            //  a date is selected if there exists data between 1PM on that date and 11AM the next day
            //
            //for each log file in the list, if the log file contains dates in the above range, 
            //  then the date is added to the output list
            //  if the log contains only times between midnight and 11AM, then the date for the day before is 
            //    listed, unless it's already there.
            //so, we work through the list from most recent to least recent
            //
            List<DateTime> dtList = new List<DateTime>();

            List<string> fileList = Directory.GetFiles(logDirectory).ToList();
            foreach (string fName in fileList)
            {
                //Convert file path to date
                string logName = Path.GetFileName(fName).Split('.')[0];
                DateTime logDate = new DateTime();
                try
                {
                    logDate = new DateTime(Convert.ToInt32(logName.Split('-')[0]), Convert.ToInt32(logName.Split('-')[1]), Convert.ToInt32(logName.Split('-')[2]));
                }
                catch (Exception ex)
                {
                    //Just move on to the next one
                }
                //Read the log, looking for its range
                bool isToday = false;
                bool isYesterday = false;
                if (logDate != null)
                {
                    List<string> logLines = File.ReadLines(fName).ToList();
                    foreach (string line in logLines)
                        if (line != "")
                        {
                            string lTime = line.Split(' ')[0];
                            string[] lSplit = lTime.Split(':');
                            try
                            {
                                TimeSpan oClock = new TimeSpan(Convert.ToInt32(lSplit[0]), Convert.ToInt32(lSplit[1]), Convert.ToInt32(lSplit[2]));
                                if (oClock < new TimeSpan(11, 0, 0))
                                    isYesterday = true;
                                else isToday = true;
                            }
                            catch (Exception ex)
                            {
                                //Just drop through
                            }
                        }
                    //if today is true, then add date to date list (if not there already)
                    //if yesterday is true, then add date-1day to datelist (if not already there)
                    if (isToday)
                        if (dtList != null && !dtList.Contains(logDate)) dtList.Add(logDate);
                    if (isYesterday)
                        if (dtList != null && !dtList.Contains(logDate.AddDays(-1))) dtList.Add(logDate.AddDays(-1));
                }
            }
            return dtList.OrderBy(x => x.Date).ToList();
        }
    }
}
