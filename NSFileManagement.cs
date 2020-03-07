using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightShift
{
    public class NSFileManagement
    {
        //Handles the  file interfaces for Night Shift
        //Configuration XML declarations
        private static string XLOG_NightShiftRoot = "NightShift";

        //Configuration XML default file path and name declarations
        private static string NightShiftDirectoryName = "NightShift";
        private static string NightShiftLogFileName = "NightShiftLog.xml";
        private static string NightShiftTaskFileName = "NightShiftTask.xml";
        private string NSLogPath { get; set; }
        private string NSTaskPath { get; set; }

        public NSFileManagement()
        {
            //Check set up.  If no base directory, create it (documents\night shift), and 
            // if no config file, then create it
            // then load the file to an xdocument.
            //
            //Put together the foldername for the log folder
            string nsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\" + NightShiftDirectoryName;
            //Check to see if NS log file exists, if not create it
            if (!Directory.Exists(nsDir)) Directory.CreateDirectory(nsDir);
            NSLogPath = nsDir + "\\" + NightShiftLogFileName;
            if (!File.Exists(NSLogPath))
            {
                //Create and initialize log file
                //File.Create(NSFilePath);
                //NSXDocument = XDocument.Load(NSFilePath);
                XElement xcfg = new XElement(XLOG_NightShiftRoot);
                //Then, write the config file//s folder name into the config file, first line
                xcfg.Save(NSLogPath);
            }
            //Put together the foldername for the log folder
            NSTaskPath = nsDir + "\\" + NightShiftTaskFileName;
            if (!File.Exists(NSTaskPath))
            {
                //Create and initialize log file
                //File.Create(NSFilePath);
                //NSXDocument = XDocument.Load(NSFilePath);
                XElement xcfg = new XElement(XLOG_NightShiftRoot);
                //Then, write the config file//s folder name into the config file, first line
                xcfg.Save(NSTaskPath);
            }
            return;
        }

        public XElement GetXLog()
        {
            //returns NS XML file document object
            return XElement.Load(NSLogPath, LoadOptions.PreserveWhitespace);
            ;
        }

        public void SetXLog(XElement nsX)
        {
            nsX.Save(NSLogPath);
            return;
        }

        public void ClearXLog()
        {
            XElement nsX = new XElement(XLOG_NightShiftRoot);
            //Then, write the config file//s folder name into the config file, first line
            nsX.Save(NSLogPath);
            return;
        }
        public XElement GetXTasks()
        {
            //returns NS XML file document object
            return XElement.Load(NSTaskPath, LoadOptions.PreserveWhitespace);
            ;
        }

        public void SetXTasks(XElement nsX)
        {
            nsX.Save(NSTaskPath);
            return;
        }

        public void ClearXTask()
        {
            XElement nsX = new XElement(XLOG_NightShiftRoot);
            //Then, write the config file//s folder name into the config file, first line
            nsX.Save(NSTaskPath);
            return;
        }
    }
}
