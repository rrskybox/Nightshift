namespace NightShift
{
    partial class FormWaimeaWeather
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.WaimeaWeatherBrowserBox = new System.Windows.Forms.WebBrowser();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // WaimeaWeatherBrowserBox
            // 
            this.WaimeaWeatherBrowserBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WaimeaWeatherBrowserBox.Location = new System.Drawing.Point(0, 0);
            this.WaimeaWeatherBrowserBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.WaimeaWeatherBrowserBox.Name = "WaimeaWeatherBrowserBox";
            this.WaimeaWeatherBrowserBox.ScriptErrorsSuppressed = true;
            this.WaimeaWeatherBrowserBox.Size = new System.Drawing.Size(1238, 933);
            this.WaimeaWeatherBrowserBox.TabIndex = 0;
            // 
            // FormWaimeaWeather
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(1238, 933);
            this.Controls.Add(this.WaimeaWeatherBrowserBox);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "FormWaimeaWeather";
            this.Text = "Waimea Weather";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser WaimeaWeatherBrowserBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}