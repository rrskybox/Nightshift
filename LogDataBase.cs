using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace NightShift
{
    public class LogXDataBase
    {
        //the log database is an xml file of log entries
        //the maintenance database is an xml file of maintenance entries
        //
        //a log entry is opened each time that Nightshift is opened.  If no
        //  activity is performed, then no record is left  (all empty records will be cleaned at
        //   the end of session.
        //there is just one maintenance entry and it is updated as needed
        //
        //  

        //Null record:
        //  <DailyLog>
        //  <LogDate>Monday, MMM dd, yyyy</LogDate>
        //  <Seeing></Seeing>
        //  <InitTask />
        //  <Notes></Notes>
        //</DailyLog>

        public string XNightShiftLog = "NightShiftLog";

        public string XLogEntry = "LogEntry";

        public string XLogDate = "LogDate";
        public string XLogTime = "LogTime";
        public string XLogNotes = "Notes";
        public string XLogTool = "LaunchedTool";


        public string XNightShiftTask = "NightShiftTask";

        public string XTaskEntry = "TaskEntry";

        public string XTaskItem = "TaskItem";

        private NSFileManagement LogXfiles { get; set; }

        public LogXDataBase()
        {
            //Get the folder name from the Night Shift configuration file
            //  the configureform class will create both the NightShift directory and configuration XML file, if (necessary
            //if the log file doesnt exist, create it
            LogXfiles = new NSFileManagement();
            return;
        }

        public string GetNote(string date)
        {
            //Retrieves all the note contents from the date node
            XElement opdata = LogXfiles.GetXLog();
            XElement xN = ReadXRecord(opdata, date);
            if (xN == null) return null;
            else return xN.Element(XLogNotes).Value;
        }

        public void AddNote(string date, string note)
        {
            //Creates and adds a notes text block to the Notes xml file under the date node
            XElement opdata = LogXfiles.GetXLog();
            XElement xE = new XElement(XLogEntry);
            xE.Add(new XElement(XLogDate, date));
            xE.Add(new XElement(XLogNotes, note));
            opdata = WriteXRecords(opdata, date, xE);
            LogXfiles.SetXLog(opdata);
            return;
        }

        public List<string> GetTasks()
        {
            //Retrieves all the task contents from task file
            List<string> tList = new List<string>();
            XElement opdata = LogXfiles.GetXTasks();
            var pastops = from XElement op in opdata.Elements(XTaskEntry) select op;
            if (pastops.Count() > 0)
            {
                foreach (XElement op in pastops) { tList.Add(op.Value); }
                return tList;
            }
            return null;
        }

        public void AddTasks(List<string> tasks)
        {
            //Creates and adds a notes text block to the Notes xml file under the date node
            XElement opdata = new XElement(XNightShiftTask);
            foreach (string task in tasks) opdata.Add(new XElement(XTaskEntry, task));
            LogXfiles.SetXTasks(opdata);
            return;
        }

        public XElement ReadXRecord(XElement opdata, string logDate)
        {
            //Find an operations record for the given date logDate and return; that record
            //  as an Xelement.  if (none for that date, { return; an empty logDate element
            //Dim xlmFileName = "C:\Users\" + System.Environment.UserName + "\" + logFields.NightShiftCfgFilePath + "\" + logFields.NightShiftXLogFileName

            //Get the path to the current log file and read in the contents

            var pastops = from XElement op in opdata.Elements(XLogEntry) select op;
            if (pastops.Count() > 0)
            {
                foreach (XElement op in pastops)
                {
                    if (op.Element(XLogDate).Value == logDate) return op;
                }
            }
            return null;
        }

        public XElement WriteXRecords(XElement opdata, string logDate, XElement daterecord)
        {
            //Writes the logdata to the log file
            //
            //For each daily log element in the log file, check to see if (it matches the requested log date
            //  if (so, { replace that daily log element with the new daily log element
            //  if (no daily log is found for that date, { add the new daily log element
            //{ save the whole log tree back to the log file
            var pastops = from XElement op in opdata.Elements(XLogEntry) select op;
            if (pastops.Count() > 0)
            {
                bool replaceflag = false;
                foreach (XElement op in pastops)
                {
                    if (op.Element(XLogDate).Value == logDate)
                    {
                        op.ReplaceWith(daterecord);
                        replaceflag = true;
                    }
                }
                if (!replaceflag) opdata.Add(daterecord);
            }
            else opdata.Add(daterecord);
            return opdata;

        }

        public void CleanXAllRecords()
        {
            //Clears out empty records
            //  Reads in all log daily records, checks for any content whatsoever, removes records that are empty { re-saves all the remaining records

            //Get the path to the current log file and read in the contents
            //Read in all the records
            XElement opdata = LogXfiles.GetXLog();
            //run through the records and remove empty records
            LogXfiles.SetXLog(opdata);
            return;
        }

        public XElement ReadXAllRecords()
        {
            //Reads in all log daily records
            //Get the path to the current log file and read in the contents
            XElement opdata = LogXfiles.GetXLog();
            return opdata;
        }

        public void WriteXAllRecords(XElement opdata)
        {
            //Get the path to the current log file and read in the contents
            LogXfiles.SetXLog(opdata);
            return;

        }

        public string ReadXField(XElement logRecord, string logField)
        {
            //return;s the field string associated with the log record integer
            if (logRecord == null)
            {
                return null;
            }
            XElement fieldrecord = logRecord.Element(logField);
            if (fieldrecord == null) return null;
            else return fieldrecord.Value;
        }

        public string ReadXContent(XElement logRecord, string logField)
        {
            //return;s the field string associated with the log record integer
            if (logRecord == null) return null;
            string fieldrecord = logRecord.Value;
            if (fieldrecord == null) return null;
            else return logRecord.Value;
        }

        public XElement DateXLog(string logDate)
        {
            //Used to create new, empty log entry for a specific date in the XML database tree
            XElement OLoperentry = new XElement(XLogDate, new XElement(XLogDate, logDate));
            return OLoperentry;
        }

        public XElement AddXMaintenance(XElement opsXentry, string operationfield, string operationtype)
        {
            //Used to add Seeing to XML database tree
            opsXentry.Add(new XElement(operationfield, operationtype));
            return opsXentry;
        }

        public XElement AddXNote(XElement opsXentry, string notes)
        {
            //Used to add Notes to XML database tree
            opsXentry.Add(new XElement(XLogNotes, notes));
            return opsXentry;
        }





    }
}
