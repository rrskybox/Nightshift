﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Deployment.Application;
using System.IO;

namespace NightShift
{
    public partial class FormNightShift : Form
    {
        private Color RunningButtonColor = Color.LightSalmon;
        private Color ReadyButtonColor = Color.LightGreen;

        public FormNightShift()
        {
            InitializeComponent();
            // Acquire the version information and put it in the form header
            try { this.Text = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(); }
            catch { this.Text = " in Debug"; } //probably in debug, no version info available
            this.Text = "NightShift V" + this.Text;

            TargetSpecs ts = new TargetSpecs("Sun");
            SunRiseTextBox.Text = ts.RiseTime.ToString(@"hh\:mm");
            SunSetTextBox.Text = ts.SetTime.ToString(@"hh\:mm");
            TwilightTextBox.Text = ts.TwilightEODTime.ToString(@"hh\:mm");
            DawnTextBox.Text = ts.TwilightSODTime.ToString(@"hh\:mm");

            TargetSpecs tm = new TargetSpecs("Moon");
            MoonRiseTextBox.Text = tm.RiseTime.ToString(@"hh\:mm");
            MoonSetTextBox.Text = tm.SetTime.ToString(@"hh\:mm");
            MoonTransitTextBox.Text = tm.TransitTime.ToString(@"hh\:mm");
            MoonPhaseTextBox.Text = (tm.PhasePercent.ToString("0")) + "%";

            //Test to see which buttons have installed applications
            QualifyToolKitButtons();

            //Load the Target Plan datagridview
            WriteTargetList();

            //Load the task list datagridview
            LogXDataBase tldb = new LogXDataBase();
            List<string> taskList = tldb.GetTasks();

            TaskDataGrid.Rows.Clear();
            int tidx = 0;
            if (taskList != null)
            {
                foreach (string task in taskList)
                {
                    TaskDataGrid.Rows.Add();
                    TaskDataGrid.Rows[tidx].Cells[0].Value = task;
                    TaskDataGrid.Rows[tidx].Cells[1].Value = false;
                    tidx++;
                }
            }
            Show();
            TaskDataGrid.ClearSelection();
            //Set the display date the the current date
            LogDateTimePicker.Value = DateTime.Now;
            HumasonLogChoice.Checked = false;
            HumasonLogChoice.Checked = true;
            Show();
            return;
        }

        private string CheckToolKitApp(string toolName)
        {
            //builds file path to toolName.  returns empty if tool isn't installed
            string ttdir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\TSXToolkit\\TSXToolkit";
            string ifbPath = ttdir + "\\" + toolName + ".appref-ms";
            if (System.IO.File.Exists(ifbPath)) return ifbPath;
            else return null;
        }

        private void QualifyToolKitButtons()
        {
            //Checks out each app start button, disables the ones that don't have tools installed
            if (CheckToolKitApp("Humason") == null) DeactivateButton(HumasonButton);
            if (CheckToolKitApp("SuperScan") == null) DeactivateButton(SuperScanButton);
            if (CheckToolKitApp("Seeing Analyzer") == null) DeactivateButton(SeeingAnalyzerButton);
            if (CheckToolKitApp("Image Planner") == null) DeactivateButton(PlannerButton);
            if (CheckToolKitApp("Subframe Calculator") == null) DeactivateButton(SubFrameButton);
            if (CheckToolKitApp("AtGuider2") == null) DeactivateButton(AtGuider2Button);
            if (CheckToolKitApp("PEC Collect") == null) DeactivateButton(PECCollectButton);
            if (CheckToolKitApp("PEC Merge") == null) DeactivateButton(PECMergeButton);
            if (CheckToolKitApp("Auto Focus Profiler") == null) DeactivateButton(AFProfilerButton);
            if (CheckToolKitApp("Autoguide Profiler 3") == null) DeactivateButton(AGProfilerButton);
            if (CheckToolKitApp("Darks Knight") == null) DeactivateButton(DarksKnightButton);
            if (CheckToolKitApp("Kilolani Power Control") == null) DeactivateButton(KilolaniPowerButton);
            if (CheckToolKitApp("Kilolani Startup x64") == null) DeactivateButton(KilolaniStartUpButton);
            if (CheckToolKitApp("Kilolani Shutdown x64") == null) DeactivateButton(KilolaniShutDownButton);
            if (CheckToolKitApp("PreStack Kilolani") == null) DeactivateButton(PreStackButton);
            if (CheckToolKitApp("Mobilizer") == null) DeactivateButton(MobilizerButton);
            if (CheckToolKitApp("PreStackPI") == null) DeactivateButton(PreStackPIButton);
            if (CheckToolKitApp("Hot Pursuit") == null) DeactivateButton(HotPursuitButton);
            if (CheckToolKitApp("Kilolani Standard Configuration") == null) DeactivateButton(InitializeButton);
            if (CheckToolKitApp("Transient Search") == null) DeactivateButton(TransientSearchButton);

            return;
        }

        private void DeactivateButton(Button button)
        {
            //Changes the back color of the specified button to gray.
            button.Enabled = false;
            button.BackColor = Color.Gray;
        }

        private bool LaunchToolKitApp(string toolName)
        {
            //Launches the specified toolName 
            //  returns true if successful, false otherwise
            string toolPath = CheckToolKitApp(toolName);
            if (toolPath != null)
            {
                Process pSystemExe = new Process();
                pSystemExe.StartInfo.FileName = toolPath;
                pSystemExe.Start();
                return true;
            }
            else return false;
        }

        private void WriteTargetList()
        {
            //Load the Target Plan datagridview
            TargetPlans tPlans = new TargetPlans();
            List<string> tpList = tPlans.PlanList;
            TargetDataGrid.Rows.Clear();
            int ridx = 0;
            foreach (string tp in tpList)
            {
                string targetName = tp.Split('.')[0];
                TargetSpecs tt = new TargetSpecs(targetName);
                TargetDataGrid.Rows.Add();
                TargetDataGrid.Rows[ridx].Cells[0].Value = tt.TargetName;
                TargetDataGrid.Rows[ridx].Cells[1].Value = tt.TargetType;
                TargetDataGrid.Rows[ridx].Cells[2].Value = tt.RiseTime.ToString(@"hh\:mm");
                TargetDataGrid.Rows[ridx].Cells[3].Value = tt.TransitTime.ToString(@"hh\:mm");
                TargetDataGrid.Rows[ridx].Cells[4].Value = tt.SetTime.ToString(@"hh\:mm");
                TargetDataGrid.Rows[ridx].Cells[5].Value = tt.Constellation;
                ridx++;
            }
            TargetDataGrid.ClearSelection();
            TargetDataGrid.Update();
            Show();
            return;
        }

        public void WriteTextLog(string logString)
        {
            //Writes the specified logString to the log text box, prepending the current time
            NotesTextBox.AppendText("* " + DateTime.Now.ToShortTimeString() + ": " + logString + "\r\n");
        }

        private void PlannerButton_Click(object sender, EventArgs e)
        {
            PlannerButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Image Planner");
            if (islaunched) PlannerButton.BackColor = ReadyButtonColor;
            else PlannerButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Planner launched");
            return;
        }

        private void HumasonButton_Click(object sender, EventArgs e)
        {
            HumasonButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Humason");
            if (islaunched) HumasonButton.BackColor = ReadyButtonColor;
            else HumasonButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Humason launched");
            return;
        }

        private void SuperScanButton_Click(object sender, EventArgs e)
        {
            SuperScanButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("SuperScan");
            if (islaunched) SuperScanButton.BackColor = ReadyButtonColor;
            else SuperScanButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("SuperScan launched");
            return;
        }

        private void VariScanButton_Click(object sender, EventArgs e)
        {
            VariScanButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("VariScan");
            if (islaunched) VariScanButton.BackColor = ReadyButtonColor;
            else VariScanButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("VariScan launched");
            return;
        }

        private void ExoScanButton_Click(object sender, EventArgs e)
        {
            ExoScanButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("ExoScan");
            if (islaunched) ExoScanButton.BackColor = ReadyButtonColor;
            else ExoScanButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("ExoScan launched");
            return;
        }

        private void HotPursuitButton_Click(object sender, EventArgs e)
        {
            HotPursuitButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Hot Pursuit");
            if (islaunched) HotPursuitButton.BackColor = ReadyButtonColor;
            else HotPursuitButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("HotPursuit launched");
            return;
        }

        private void KilolaniStartUpButton_Click(object sender, EventArgs e)
        {
            KilolaniStartUpButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Kilolani Startup x64");
            if (islaunched) KilolaniStartUpButton.BackColor = ReadyButtonColor;
            else KilolaniStartUpButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Kilolani Startup launched");
            return;
        }

        private void KilolaniShutDownButton_Click(object sender, EventArgs e)
        {
            KilolaniShutDownButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Kilolani Shutdown x64");
            if (islaunched) KilolaniShutDownButton.BackColor = ReadyButtonColor;
            else KilolaniShutDownButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Kilolani Shutdown launched");
            return;
        }

        private void InitializeButton_Click(object sender, EventArgs e)
        {
            InitializeButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Kilolani Standard Configuration");
            if (islaunched) InitializeButton.BackColor = ReadyButtonColor;
            else InitializeButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Kilolani Standard Configuration launched");
            return;

        }
        private void KilolaniPowerButton_Click(object sender, EventArgs e)
        {
            KilolaniPowerButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Kilolani Power Control");
            if (islaunched) KilolaniPowerButton.BackColor = ReadyButtonColor;
            else KilolaniPowerButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Kilolani Power Control launched");
            return;
        }

        private void DarksKnightButton_Click(object sender, EventArgs e)
        {
            DarksKnightButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Darks Knight");
            if (islaunched) DarksKnightButton.BackColor = ReadyButtonColor;
            else DarksKnightButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Darks Knight launched");
            return;
        }

        private void SeeingAnalyzerButton_Click(object sender, EventArgs e)
        {
            SeeingAnalyzerButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Seeing Analyzer");
            if (islaunched) SeeingAnalyzerButton.BackColor = ReadyButtonColor;
            else SeeingAnalyzerButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Seeing Analyzer launched");
            return;
        }

        private void SubFrameButton_Click(object sender, EventArgs e)
        {
            SubFrameButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Subframe Calculator");
            if (islaunched) SubFrameButton.BackColor = ReadyButtonColor;
            else SubFrameButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Subframe Calculator launched");
            return;
        }

        private void AtGuider2Button_Click(object sender, EventArgs e)
        {
            AtGuider2Button.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("AtGuider2");
            if (islaunched) AtGuider2Button.BackColor = ReadyButtonColor;
            else AtGuider2Button.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("AtGuider2 launched");
            return;
        }

        private void PECCollectButton_Click(object sender, EventArgs e)
        {
            PECCollectButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("PEC Collect");
            if (islaunched) PECCollectButton.BackColor = ReadyButtonColor;
            else PECCollectButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("PEC Collect launched");
            return;
        }

        private void PECMergeButton_Click(object sender, EventArgs e)
        {
            PECMergeButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("PEC Merge");
            if (islaunched) PECMergeButton.BackColor = ReadyButtonColor;
            else PECMergeButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("PEC Merge launched");
            return;
        }

        private void AFProfilerButton_Click(object sender, EventArgs e)
        {
            AFProfilerButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Auto Focus Profiler");
            if (islaunched) AFProfilerButton.BackColor = ReadyButtonColor;
            else AFProfilerButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("AFProfiler launched");
            return;
        }

        private void AGProfilerButton_Click(object sender, EventArgs e)
        {
            AGProfilerButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Autoguide Profiler 3");
            if (islaunched) AGProfilerButton.BackColor = ReadyButtonColor;
            else AGProfilerButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Autoguide Profiler 3 launched");
            return;
        }

        private void GuideLogButton_Click(object sender, EventArgs e)
        {
            GuideLogButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Guide Log Analyzer");
            if (islaunched) GuideLogButton.BackColor = ReadyButtonColor;
            else GuideLogButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Guide Log Analyzer launched");
            return;
        }


        private void MobilizerButton_Click(object sender, EventArgs e)
        {
            MobilizerButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Mobilizer");
            if (islaunched) MobilizerButton.BackColor = ReadyButtonColor;
            else MobilizerButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Mobilizer launched");
            return;

        }

        private void PreStackPIButton_Click(object sender, EventArgs e)
        {
            PreStackPIButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("PreStackPI");
            if (islaunched) PreStackPIButton.BackColor = ReadyButtonColor;
            else PreStackPIButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("PreStackPI launched");
            return;
        }

        private void PreStackButton_Click(object sender, EventArgs e)
        {
            PreStackButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("PreStack Kilolani");
            if (islaunched) PreStackButton.BackColor = ReadyButtonColor;
            else PreStackButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("PreStack Kilani Launched");
            return;
        }

        private void TransientSearchButton_Click(object sender, EventArgs e)
        {
            TransientSearchButton.BackColor = RunningButtonColor;
            bool islaunched = LaunchToolKitApp("Transient Search");
            if (islaunched) TransientSearchButton.BackColor = ReadyButtonColor;
            else TransientSearchButton.BackColor = RunningButtonColor;
            if (islaunched) WriteTextLog("Transient Search Launched");
            return;
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            LogXDataBase lgdb = new LogXDataBase();
            if (NotesTextBox.Text != null)
            {
                lgdb.AddNote(LogDateTimePicker.Value.ToShortDateString(), NotesTextBox.Text);
            }

            List<string> taskList = new List<string>();
            foreach (DataGridViewRow task in TaskDataGrid.Rows)
            {
                var taskName = task.Cells[0].Value;
                if (taskName != null)
                {
                    bool taskDone = Convert.ToBoolean(task.Cells[1].Value);
                    if (!taskDone && (taskName != null))
                    {
                        taskList.Add(taskName.ToString());
                    }
                }
            }
            lgdb.AddTasks(taskList);

            Close();
            return;
        }

        private void LogDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            //fill in notes
            string[] lglines;
            LogXDataBase lgdb = new LogXDataBase();
            char[] splt = { '*' };
            string lgstr = lgdb.GetNote(LogDateTimePicker.Value.ToShortDateString());
            if (lgstr != null)
            {
                lglines = lgstr.Split(splt, StringSplitOptions.RemoveEmptyEntries);
                foreach (string lg in lglines) NotesTextBox.AppendText("*" + lg + "\r\n");
                return;
            }
            return;
        }

        private void TargetDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Click on an entry in the Target list will cause:
            //the target name to be sent to web to show target visual.
            string tgtName;
            try
            { tgtName = TargetDataGrid.SelectedRows[0].Cells[0].Value.ToString(); }
            catch (Exception ex)
            {
                TargetDataGrid.ClearSelection();
                Show();
                return;
            }
            FormPreview formPV = new FormPreview(tgtName);
            WriteTextLog("Humason current target changed to " + tgtName);
            return;
        }

        private void TargetDataGrid_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //Click on an entry in the Target list will cause:
            //the target name to be changed to the selected target name
            //in the Humason Session Control xml file.
            string tgtName;
            try
            { tgtName = TargetDataGrid.SelectedRows[0].Cells[0].Value.ToString(); }
            catch (Exception ex) { return; }
            HumasonInterface.ChangeTargetDefault(tgtName);
            WriteTextLog("Humason current target changed to " + tgtName);
            return;
        }

        private void TargetListRefreshButton_Click(object sender, EventArgs e)
        {
            WriteTargetList();
            return;
        }

        private void ParkButton_Click(object sender, EventArgs e)
        {
            //Do everything the same as "Close" but minimize the form instead
            LogXDataBase lgdb = new LogXDataBase();
            if (NotesTextBox.Text != null)
            {
                lgdb.AddNote(LogDateTimePicker.Value.ToShortDateString(), NotesTextBox.Text);
            }

            List<string> taskList = new List<string>();
            foreach (DataGridViewRow task in TaskDataGrid.Rows)
            {
                var taskName = task.Cells[0].Value;
                if (taskName != null)
                {
                    bool taskDone = Convert.ToBoolean(task.Cells[1].Value);
                    if (!taskDone && (taskName != null))
                    {
                        taskList.Add(taskName.ToString());
                    }
                }
            }
            lgdb.AddTasks(taskList);

            this.WindowState = FormWindowState.Minimized;
            return;

        }

        private void ClearLogButton_Click(object sender, EventArgs e)
        {
            //Clears out the Log text 
            NotesTextBox.Text = "";
            Show();
            return;
        }

        private void LogDateListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //New date, new log
            string writeLog = "";
            List<string> parsedLog = new List<string>();
            LogTextBox.Clear();
            //figure out the new datetime from the selected log file name
            if (LogDateListBox.SelectedItem == null)
                return;
            DateTime sdate = Convert.ToDateTime(LogDateListBox.SelectedItem.ToString());
            if (HumasonLogChoice.Checked)
            {
                HumasonLogReader hReader = new HumasonLogReader(sdate);
                if (hReader.JoinedLog != null)
                {
                    foreach (string line in hReader.JoinedLog)
                        writeLog += line + "\r\n";
                    List<string> plog = hReader.ParseLog();
                    if (plog != null)
                    {
                        foreach (string line in plog) parsedLog.Add(line);
                    }
                    TargetSetBox.Items.Clear();
                    TargetSetBox.Items.AddRange(parsedLog.ToArray());
                    if (parsedLog.Count > 0)
                        TargetSetBox.SelectedIndex = 0;
                }
            }
            else if (SuperScanLogChoice.Checked)
            {
                SuperScanLogReader sReader = new SuperScanLogReader(sdate);
                if (sReader.JoinedLog != null)
                {
                    foreach (string line in sReader.JoinedLog)
                        writeLog += line + "\r\n";
                    List<string> plog = sReader.ParseLog();
                    if (plog != null)
                    {
                        foreach (string line in plog) parsedLog.Add(line);
                    }
                }
                TargetSetBox.Items.Clear();
                TargetSetBox.Items.AddRange(parsedLog.ToArray());
                if (parsedLog.Count > 0)
                    TargetSetBox.SelectedIndex = 0;
            }
            else if (VariScanLogChoice.Checked)
            {
                VariScanLogReader vReader = new VariScanLogReader(sdate);
                if (vReader.JoinedLog != null)
                {
                    foreach (string line in vReader.JoinedLog)
                        writeLog += line + "\r\n";
                    TargetSetBox.Items.Clear();
                    foreach (string col in vReader.CollectionList)
                        parsedLog.Add(Path.GetFileName(col));
                }
                TargetSetBox.Items.Clear();
                TargetSetBox.Items.AddRange(parsedLog.ToArray());
                if (parsedLog.Count > 0)
                    TargetSetBox.SelectedIndex = 0;
            }
            else if (ExoScanChoice.Checked)
            {
                if (TargetSetBox.Items.Count > 0)
                {
                    int tgt = 0;
                    if (TargetSetBox.SelectedItem != null)
                        tgt = TargetSetBox.SelectedIndex;
                    ExoScanLogReader exoReader = new ExoScanLogReader(sdate, TargetSetBox.Items[tgt].ToString());
                    if (exoReader.JoinedLog != null)
                    {
                        foreach (string line in exoReader.JoinedLog)
                            writeLog += line + "\r\n";
                        //TargetSetBox.Items.Clear();
                        foreach (string col in exoReader.CollectionList)
                            parsedLog.Add(Path.GetFileName(col));
                    }
                }
            }
            LogTextBox.Text = writeLog;
            LogTextBox.Select(0, 0);
            return;
        }


        private void HumasonLogChoice_CheckedChanged(object sender, EventArgs e)
        {
            if (HumasonLogChoice.Checked)
            {
                //Create Humason Log content for Log tab
                //Open log based on the most recent log
                HumasonLogReader hReader = new HumasonLogReader(DateTime.Now);
                //Get full list of logs in Log directory
                LogDateListBox.Items.Clear();
                TargetSetBox.Items.Clear();
                List<DateTime> hLogList = hReader.LogDates;
                foreach (DateTime ldate in hLogList)
                    LogDateListBox.Items.Add(ldate.ToShortDateString());
                if (LogDateListBox.Items.Count > 0)
                    LogDateListBox.SelectedIndex = LogDateListBox.Items.Count - 1;
            }
            return;

        }

        private void SuperScanLogChoice_CheckedChanged(object sender, EventArgs e)
        {
            if (SuperScanLogChoice.Checked)
            {
                //Create SuperScan Log content for Log tab
                //Open log based on the most recent log
                SuperScanLogReader sReader = new SuperScanLogReader(DateTime.Now);
                //Get full list of logs in Log directory
                LogDateListBox.Items.Clear();
                TargetSetBox.Items.Clear();
                List<DateTime> sLogList = sReader.LogDates;
                foreach (DateTime ldate in sLogList)
                    LogDateListBox.Items.Add(ldate.ToShortDateString());
                if (LogDateListBox.Items.Count > 0)
                    LogDateListBox.SelectedIndex = LogDateListBox.Items.Count - 1;
            }
            return;

        }

        private void VariScanLogChoice_CheckedChanged(object sender, EventArgs e)
        {
            if (VariScanLogChoice.Checked)
            {
                //Create SuperScan Log content for Log tab
                //Open log based on the most recent log
                VariScanLogReader vReader = new VariScanLogReader(DateTime.Now);
                //Get full list of logs in Log directory
                LogDateListBox.Items.Clear();
                TargetSetBox.Items.Clear();
                List<DateTime> vLogList = vReader.LogDates;
                foreach (DateTime ldate in vLogList)
                    LogDateListBox.Items.Add(ldate.ToShortDateString());
                if (LogDateListBox.Items.Count > 0)
                    LogDateListBox.SelectedIndex = LogDateListBox.Items.Count - 1;
            }
            return;
        }

        private void ExoScanChoice_CheckedChanged(object sender, EventArgs e)
        {
            if (ExoScanChoice.Checked)
            {
                //Create SuperScan Log content for Log tab
                //Open log based on the most recent log
                ExoScanLogReader exoReader = new ExoScanLogReader(DateTime.Now, null);
                //Get full list of logs in Log directory
                TargetSetBox.Items.Clear();
                foreach (string tgt in exoReader.CollectionList)
                    TargetSetBox.Items.Add(Path.GetFileNameWithoutExtension(tgt));
                LogDateListBox.Items.Clear();
                List<DateTime> exoLogList = exoReader.LogDates;
                foreach (DateTime ldate in exoLogList)
                    LogDateListBox.Items.Add(ldate.ToShortDateString());
                if (LogDateListBox.Items.Count > 0)
                    LogDateListBox.SelectedIndex = LogDateListBox.Items.Count - 1;
            }
            return;
        }

        private void TargetSetBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ExoScanChoice.Checked)
            {
                //Initialize Log file listing
                LogTextBox.Text = null;
                string target = TargetSetBox.SelectedItem.ToString();
                DateTime dateChoice;
                if (LogDateListBox.SelectedItem != null)
                    dateChoice = Convert.ToDateTime(LogDateListBox.SelectedItem.ToString());
                else
                    dateChoice = DateTime.Now;
                ExoScanLogReader exoReader = new ExoScanLogReader(dateChoice, target);
                //Fill in new log dates into list
                LogDateListBox.Items.Clear();
                List<DateTime> exoLogList = exoReader.LogDates;
                foreach (DateTime ldate in exoLogList)
                    LogDateListBox.Items.Add(ldate.ToShortDateString());
                //Select the last date in the list
                if (LogDateListBox.Items.Count > 0)
                    LogDateListBox.SelectedIndex = LogDateListBox.Items.Count - 1;
            }
            return;

        }

        private void WaimeaWeatherButton_Click(object sender, EventArgs e)
        {
            Form waimeaWeatherForm = new FormWaimeaWeather();
            waimeaWeatherForm.Show();
            return;
        }

        private void OlympiaWeatherButton_Click(object sender, EventArgs e)
        {
            Form olyWeatherForm = new FormOlympiaWeather();
            olyWeatherForm.Show();
            return;

        }



    }

}