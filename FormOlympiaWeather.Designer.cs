namespace NightShift
{
    partial class FormOlympiaWeather
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
            this.OlympiaWeatherBrowserBox = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // OlympiaWeatherBrowserBox
            // 
            this.OlympiaWeatherBrowserBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OlympiaWeatherBrowserBox.Location = new System.Drawing.Point(0, 0);
            this.OlympiaWeatherBrowserBox.MinimumSize = new System.Drawing.Size(20, 20);
            this.OlympiaWeatherBrowserBox.Name = "OlympiaWeatherBrowserBox";
            this.OlympiaWeatherBrowserBox.ScriptErrorsSuppressed = true;
            this.OlympiaWeatherBrowserBox.Size = new System.Drawing.Size(1222, 894);
            this.OlympiaWeatherBrowserBox.TabIndex = 0;
            // 
            // FormOlympiaWeather
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1222, 894);
            this.Controls.Add(this.OlympiaWeatherBrowserBox);
            this.Name = "FormOlympiaWeather";
            this.Text = "Olympia Weather";
            this.Load += new System.EventHandler(this.FormOlympiaWeather_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser OlympiaWeatherBrowserBox;
    }
}