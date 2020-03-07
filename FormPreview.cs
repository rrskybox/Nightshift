﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TheSkyXLib;

namespace NightShift
{
    public partial class FormPreview : Form
    {

        public FormPreview(string targetName)
        {
            InitializeComponent();
            //
            //private string isource1 = "http://server1.sky-map.org/skywindow?img_source=DSS2&object=ic2177&zoom=8";
            string isource1 = "http://server1.sky-map.org/skywindow?img_source=DSS2";
            string izoom = "zoom=";
            string ira = "ra=";
            string idec = "de=";
            string ishowbox = "show_box=1";
            string ishowboxwidth = "box_width=";
            string ishowboxheight = "box_height=";
            string ishowgrids = "show_grid=0";
            string ishowconstellationlines = "show_constellation_lines=0";
            string ishowconstellationboundaries = "show_constellation_boundaries=0";
            int angularFrameWidth = 0;
            double iWidthD = 30;  //default width of 30 minutes
            double iHeightD = 20;  //default height of 20 minutes


            //get image fov center, convert to degreess (sky6MyFOV returns arc minutes)
            //get overall image width at 4 times FOV, convert to degrees
            angularFrameWidth = (int)(4 * iWidthD / 60);
            //Get RA/Dec coordinates for target in box
            //string targetName = parentForm.TargetNameBox.Text;
            this.Text = targetName;

            angularFrameWidth = (int)(4 * iWidthD / 60);

            sky6StarChart tsxs = new sky6StarChart();
            sky6ObjectInformation tsxo = new sky6ObjectInformation();
            //if the object is not found, just return
            try
            {
                tsxs.Find(targetName);
            }
            catch
            {
                tsxs = null;
                tsxo = null;
                return;
            }

            int cnt = tsxo.Count;
            tsxo.Index = 0;
            tsxo.Property(TheSkyXLib.Sk6ObjectInformationProperty.sk6ObjInfoProp_RA_2000);
            double dRA = tsxo.ObjInfoPropOut;
            tsxo.Property(TheSkyXLib.Sk6ObjectInformationProperty.sk6ObjInfoProp_DEC_2000);
            double dDec = tsxo.ObjInfoPropOut;
            tsxs.RightAscension = dRA;
            tsxs.Declination = dDec;
            tsxs.FieldOfView = angularFrameWidth;

            double fullpixDeg0 = (2000.0 / 360.0);                   //pixels per degree at zoom = 0 (maximum pixel width = 2000, at frame width = 360 degrees)
            double fullpixDegN = (2000.0 / angularFrameWidth);         //pixels per degree where the frame width == maximum width in pixels, scaled
            double fullzoomX = Math.Log((fullpixDegN / fullpixDeg0), 2);     //zoom level N that produces a pixel per degree of pixDegN
            int fullzoom = Convert.ToInt32(fullzoomX) - 1;
            //convert zoom to integer
            int fullpixDegAtZoomN = (int)(Math.Pow(2, fullzoom) * fullpixDeg0);         //pixels per degree at integer zoom N (integerized)  

            int showboxWidth = Convert.ToInt32((iWidthD / 60) * fullpixDegAtZoomN);   //width of showbox in frame which is zoomed to N
            int showboxHeight = Convert.ToInt32((iHeightD / 60) * fullpixDegAtZoomN);   //width of showbox in frame which is zoomed to N

            string iframer = "<IFRAME SRC=" +
                              isource1 + "&" +
                              izoom + fullzoom.ToString() + "&" +
                              ira + dRA.ToString("0.0000") + "&" +
                              idec + dDec.ToString("0.0000") + "&" +
                              ishowbox + "&" +
                              ishowboxwidth + showboxWidth.ToString("00") + "&" +
                              ishowboxheight + showboxHeight.ToString("00") + "&" +
                              ishowgrids + "&" +
                              ishowconstellationlines + "&" +
                              ishowconstellationboundaries + "&" +
                              " WIDTH=400 HEIGHT=400></IFRAME";

            //set the preview window so that it always shows in front of imageforecast window, if active
            //this.Owner = FormImagePlanner;
            //We had some fatal errors here with loading the iframe, i think, so try to catch and ignor
            try
            {
                WebBrowserFrame.DocumentText = iframer;
            }
            catch
            {
                // Do nothing
            }

            System.Threading.Thread.Sleep(1000);
            this.Show();
            tsxo = null;
            tsxs = null;
            return;
        }

        private void WebBrowserFrame_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) // Handles WebBrowserFrame.DocumentCompleted
        {
            return;
        }
    }
}
