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
            OlympiaWeatherBrowserBox.Url = new System.Uri("https://clearoutside.com/forecast/47.02/-122.88", System.UriKind.Absolute);
            return;
        }

        private void FormOlympiaWeather_Load(object sender, EventArgs e)
        {

        }
    }
}
