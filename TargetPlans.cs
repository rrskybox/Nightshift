using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace NightShift
{
    class TargetPlans
    {

        public List <string> PlanList;

        public TargetPlans()
        {
            //Searches the Humason folder for all xml files with TargetPlan in the name
            string humDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Humason";
            PlanList = Directory.EnumerateFiles(humDir, "*TargetPlan*.xml").ToList();
            for (int i = 0; i < PlanList.Count; i++)
            {
                PlanList[i] = Path.GetFileNameWithoutExtension(PlanList[i]);
            }
            for (int i = 0; i < PlanList.Count; i++)
            {
                if (PlanList[i].Contains("Default")) PlanList.Remove(PlanList[i]);
            }
            return;
        }

       
    }
}
