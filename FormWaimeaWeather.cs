using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace NightShift
{
    public partial class FormWaimeaWeather : Form
    {
        public FormWaimeaWeather()
        {
            InitializeComponent();
            WaimeaWeatherBrowserBox.Url = new System.Uri("https://clearoutside.com/forecast/20.04/-155.83", System.UriKind.Absolute);
            return;        }
    }
}

