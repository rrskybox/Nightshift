using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightShift
{
    class ExoScanLogReader
    {
        //Class to read ExoScan log files, maybe session info, who knows?
        private const string LogFolder = "ExoScan";

        private string LogTodayFilePath { get; set; } = null;
        private string LogTomorrowFilePath { get; set; } = null;

        private List<string> TodayLog = null;
        private List<string> TomorrowLog = null;

        public List<string> CollectionList = new List<string>();
        public List<string> JoinedLog = new List<string>();
        public List<DateTime> LogDates = new List<DateTime>();

        public ExoScanLogReader(DateTime logDate, string exoTarget)
        {

            //Get list of directories inside of "ExoScan" root
            string exoDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + LogFolder;
            CollectionList = Directory.GetDirectories(exoDir).ToList();
            //Set the collecion list selection to the first item
            if (CollectionList.Count > 0)
            {
                string tgtDir = null;
                if (exoTarget == null)
                    tgtDir = CollectionList[0];
                else
                    tgtDir = CollectionList.Single(x => Path.GetFileNameWithoutExtension(x) == exoTarget);
                //Open the Log directory for this collection (if any, but should be)
                string logDir = tgtDir + "\\Logs";
                if (!Directory.Exists(logDir))
                    return;
                string lpToday = logDir + "\\" + logDate.ToString("yyyy_MM_dd") + ".log";
                string lpTomorrow = logDir + "\\" + logDate.AddDays(1).ToString("yyyy_MM_dd") + ".log";
                //Create list of file names in Log Directory
                LogDates = ParseDates(logDir);

                if (File.Exists(lpToday))
                {
                    LogTodayFilePath = lpToday;
                    TodayLog = File.ReadLines(lpToday).ToList();
                }
                // Now do the same for the following day
                if (File.Exists(lpTomorrow))
                {
                    LogTomorrowFilePath = lpTomorrow;
                    TomorrowLog = File.ReadLines(lpTomorrow).ToList();
                }
                //Join the two (if two) logs
                //First, load the two logs into enumerable lists
                string sline = "";
                if (TodayLog != null)
                    //Write all lines in today's ExoScan Log that are after 1PM into the output list
                    foreach (string line in TodayLog)
                    {
                        TimeSpan? oClock = Utility.ParseHour(line, ':');
                        if (oClock == null)
                           JoinedLog.Add( "  >> " + line);
                        else if (oClock > new TimeSpan(12, 0, 0))
                            JoinedLog.Add(line);
                    }
                //Write all the lines in tomorrows Humason log that are before 11AM
                if (TomorrowLog != null)
                {
                    foreach (string line in TomorrowLog)
                    {
                        TimeSpan? oClock = Utility.ParseHour(line, ':');
                        if (oClock == null)
                            JoinedLog.Add("  >> " + line);
                        else if (oClock <= new TimeSpan(12, 0, 0))
                            JoinedLog.Add(line);
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
                string logName = Path.GetFileNameWithoutExtension(fName);
                DateTime? logDate = Utility.ParseDate(logName, '_');
                if (logDate != null)
                {
                    //Read the log, looking for its range
                    bool isToday = false;
                    bool isYesterday = false;
                    List<string> logLines = File.ReadLines(fName).ToList();
                    foreach (string line in logLines)
                    {
                        TimeSpan? oClock = Utility.ParseHour(line, ':');
                        if (oClock != null)
                        {
                            if (oClock < new TimeSpan(11, 0, 0))
                                isYesterday = true;
                            else isToday = true;
                        }

                        //if today is true, then add date to date list (if not there already)
                        //if yesterday is true, then add date-1day to datelist (if not already there)
                        if (isToday)
                            if (dtList != null && !dtList.Contains((DateTime)logDate)) dtList.Add((DateTime)logDate);
                        if (isYesterday)
                            if (dtList != null && !dtList.Contains(((DateTime)logDate).AddDays(-1))) dtList.Add(((DateTime)logDate).AddDays(-1));
                    }
                }
            }
            return dtList.OrderBy(x => x.Date).ToList();
        }



    }
}
