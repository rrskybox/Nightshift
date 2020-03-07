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
            this.MaunaWeatherWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // MaunaWeatherWebBrowser
            // 
            this.MaunaWeatherWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MaunaWeatherWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.MaunaWeatherWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.MaunaWeatherWebBrowser.Name = "MaunaWeatherWebBrowser";
            this.MaunaWeatherWebBrowser.Size = new System.Drawing.Size(1238, 933);
            this.MaunaWeatherWebBrowser.TabIndex = 0;
            // 
            // FormMaunaWeather
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.HotTrack;
            this.ClientSize = new System.Drawing.Size(1238, 933);
            this.Controls.Add(this.MaunaWeatherWebBrowser);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Name = "FormMaunaWeather";
            this.Text = "MaunaWeather";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser MaunaWeatherWebBrowser;
    }
}