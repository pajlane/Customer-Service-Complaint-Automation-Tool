using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerServiceComplaintAutomationTool
{
    class WriteTo
    {
        public void WriteTextFile(string magentouserName, string magentoPass, string workEmail)
        {


            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))
            {
                file.WriteLine(magentouserName);
                file.WriteLine(magentoPass);
                file.WriteLine(workEmail);
               
            }


        }


    }
}
