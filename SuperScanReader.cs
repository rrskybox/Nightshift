using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightShift
{
    class SuperScanLogReader
    {
        //Class to read SuperScan log files, maybe session info, who knows?
        private const string LogFolder = "SuperScan";

        private string LogTodayFilePath { get; set; } = null;
        private string LogTomorrowFilePath { get; set; } = null;

        private List<string> TodayLog = null;
        private List<string> TomorrowLog = null;
        public List<string> JoinedLog = new List<string>();
        public List<DateTime> LogDates = new List<DateTime>();

        public SuperScanLogReader(DateTime logDate)
        {
            //Verifies file paths for Humason, Hamason/Logs, and Humason/Logs/date.log
            //sets HumasonLogfilePath to null if not found
            string slDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + LogFolder + "\\Logs";
            //Check to see if NS log file exists, if not create it
            if (!Directory.Exists(slDir)) return;
            string lpToday = slDir + "\\" + logDate.ToString("yyyy_MM_dd") + ".log";
            string lpTomorrow = slDir + "\\" + logDate.AddDays(1).ToString("yyyy_MM_dd") + ".log";
            //Create list of file names in Log Directory
            LogDates = ParseDates(slDir);

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
            TimeSpan oClock = TimeSpan.MinValue;
            string sline = "";
            if (TodayLog != null)
                //Write all lines in today's Humason Log that are after 1PM into the output list
                foreach (string line in TodayLog)
                {
                    if (line != "")
                    {
                        string lTime = line.Split(' ')[0];
                        string[] lSplit = lTime.Split(':');
                        sline = line;
                        try
                        {
                            oClock = new TimeSpan(Convert.ToInt32(lSplit[0]), Convert.ToInt32(lSplit[1]), Convert.ToInt32(lSplit[2]));
                        }
                        catch
                        {
                            sline = "  >> " + line;
                        }
                        if (oClock > new TimeSpan(13, 0, 0)) JoinedLog.Add(sline);
                    }
                }
            //Write all the lines in tomorrows Humason log that are before 11AM
            if (TomorrowLog != null)
            {
                foreach (string line in TomorrowLog)
                {
                    if (line != "")
                    {
                        string lTime = line.Split(' ')[0];
                        string[] lSplit = lTime.Split(':');
                        sline = line;
                        try
                        {
                            oClock = new TimeSpan(Convert.ToInt32(lSplit[0]), Convert.ToInt32(lSplit[1]), Convert.ToInt32(lSplit[2]));
                        }
                        catch
                        {
                            sline = "  >> " + line;
                        }
                        if (oClock < new TimeSpan(11, 0, 0)) JoinedLog.Add(sline);
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
                    logDate = new DateTime(Convert.ToInt32(logName.Split('_')[0]), Convert.ToInt32(logName.Split('_')[1]), Convert.ToInt32(logName.Split('_')[2]));
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

        public List<string> ParseLog()
        {
            //Routines goes through log looking for successful images count with filters
            //Look at each successive line in the log file
            //When an "Imaging Target is found, then save the last word as the targetname
            string targetName = null;
            int[] filterXcount = { 0, 0, 0, 0, 0, 0, 0, 0 };
            string exposureLen = null;
            List<string> imgDataOut = new List<string>();

            //List<string> logList = hReader.HumasonLog.ToList()) ;
            foreach (string line in JoinedLog)
            {
                // this is the per line loop
                // at the start, 
                //   targetName = null;
                //   filterXcount[] = null;
                //   exposureLen = null;
                if (line.Contains("Session Done"))
                {
                    //if the target name is not null, then serve up the previously parsed data
                    if (targetName != null)
                    {
                        //readout filters
                        string fstr = null;
                        for (int i = 0; i < filterXcount.Length; i++)
                            if (filterXcount[i] != 0) fstr += "\r\n\t" + "Filter " + i.ToString() + " - " + filterXcount[i].ToString() + " ";
                        imgDataOut.Add(targetName + "- " + "Exposure: " + exposureLen + " sec, " + fstr);
                    }
                    targetName = null;
                }
                if (line.Contains("Imaging Target:"))
                {
                    //start target loop 
                    //  then clear the counters
                    for (int i = 0; i < filterXcount.Length; i++) filterXcount[i] = 0;
                    exposureLen = "None";

                    //set target to last word in line
                    targetName = line.Split(' ')[5];
                }
                if (line.Contains("Imaging Filter"))
                {
                    //pickup image information
                    string[] ls = line.Split(' ');
                    //Time Imaging Filter 0 @ 600 sec (# 1 of 14) as example
                    exposureLen = ls[5];
                    ++filterXcount[Convert.ToInt32(ls[3])];
                }
            }
            //if the target name is not null, then serve up the previously parsed data
            if (targetName != null) //meaning that the session did not end before the log ended
            {
                //readout filters
                string fstr = null;
                for (int i = 0; i < filterXcount.Length; i++)
                    if (filterXcount[i] != 0) fstr += "Filter " + i.ToString() + " - " + filterXcount[i].ToString() + " ";
                imgDataOut.Add(targetName + "- " + "Exposure: " + exposureLen + " sec, " + fstr);
            }
            return imgDataOut;
        }

    }
}
