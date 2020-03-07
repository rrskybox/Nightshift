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
            this.OlyWeatherPictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.OlyWeatherPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // OlyWeatherPictureBox
            // 
            this.OlyWeatherPictureBox.BackColor = System.Drawing.SystemColors.HotTrack;
            this.OlyWeatherPictureBox.Location = new System.Drawing.Point(12, 2);
            this.OlyWeatherPictureBox.Name = "OlyWeatherPictureBox";
            this.OlyWeatherPictureBox.Size = new System.Drawing.Size(1096, 294);
            this.OlyWeatherPictureBox.TabIndex = 0;
            this.OlyWeatherPictureBox.TabStop = false;
            // 
            // FormOlympiaWeather
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1120, 298);
            this.Controls.Add(this.OlyWeatherPictureBox);
            this.Name = "FormOlympiaWeather";
            this.Text = "Olympia Weather";
            this.Load += new System.EventHandler(this.FormOlympiaWeather_Load);
            ((System.ComponentModel.ISupportInitialize)(this.OlyWeatherPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox OlyWeatherPictureBox;
    }
}