using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NightShift
{
    public partial class FormWaimeaWeather : Form
    {
        public FormWaimeaWeather()
        {
            InitializeComponent();

            string isource = "http://mkwc.ifa.hawaii.edu/models/modelstill.cgi?model=wrf-hi&domain=900m&param=cloud&orient=horiz";
            string iframe = "<IFRAME SRC=" +
                                isource + "&" +
                                "fhr=030" + "&" +
                                "itime=latest" + "&" +
                                "level=mid" + "&" +
                                "collage=none" + "&" +
                                "imgsize=Large" + "&" +
                                "imginfo=Off" + "&" +
                                " WIDTH=1200 HEIGHT=900></IFRAME";

            MaunaWeatherWebBrowser.DocumentText = iframe;
            return;
            //WeatherPictureBox.Load("http://mkwc.ifa.hawaii.edu/models/modelstill.cgi?model=wrf-hi&domain=900m&param=cloud&orient=horiz&fhr=030&itime=latest&level=mid&collage=none&imgsize=Large&imginfo=On");
            //WeatherPictureBox.Load("C:\\Users\\rick-\\OneDrive\\Pictures\\Saved Pictures\\4278.jpeg");
            //var request = WebRequest.Create("http://mkwc.ifa.hawaii.edu/models/modelstill.cgi?model=wrf-hi&domain=900m&param=cloud&orient=horiz&fhr=030&itime=latest&level=mid&collage=none&imgsize=Large&imginfo=On");
            //var request = WebRequest.Create("http://mkwc.ifa.hawaii.edu");

            //using (var response = request.GetResponse())
            //using (var stream = response.GetResponseStream())

            //    WeatherPictureBox.Image = Bitmap.FromStream(stream);
        }
    }
}

