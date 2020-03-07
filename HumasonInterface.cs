using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightShift
{
    public static class HumasonInterface
    {
        //This class provides static functions to interface to Humason files
        public static void ChangeTargetDefault(string newTargetName)
        {
            const string CurrentTargetXName = "CurrentTargetName";

            string xhPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Humason\\SessionControl.xml";
            if (File.Exists(xhPath))
            {
                XElement xHsc = XElement.Load(xhPath);
                XElement tgtname = xHsc.Element(CurrentTargetXName);
                tgtname.Value = newTargetName;
                xHsc.Save(xhPath);
            }
            return;
        }
    }
}
