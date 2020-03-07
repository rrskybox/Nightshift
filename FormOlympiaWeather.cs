using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace NightShift
{
    public partial class FormOlympiaWeather : Form
    {
        public FormOlympiaWeather()
        {
            InitializeComponent();
            string imageUrl = "http://www.cleardarksky.com/c/OlympiaWAcsk.gif?c=2134853";
            WebRequest requestPic = WebRequest.Create(imageUrl);
            WebResponse responsePic = requestPic.GetResponse();
            Image webImage = Image.FromStream(responsePic.GetResponseStream()); // Error
            OlyWeatherPictureBox.Image = webImage;
            return;
        }

        private void FormOlympiaWeather_Load(object sender, EventArgs e)
        {

        }
    }
}
