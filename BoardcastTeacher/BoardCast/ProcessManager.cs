using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoardCast
{
    class ProcessManager
    {
        
        public ProcessManager()
        {
            
        }

        public void GenerateProcess(string filePath,bool editMode)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = filePath;
            startInfo.WindowStyle = ProcessWindowStyle.Maximized;
            if (editMode)
            {
                startInfo.Verb = "edit";
            }
            Process.Start(startInfo);

        }

        public void CloseAllProcess()
        {
           
        }

        private void DeleteUnusedProcesses()
        {
            
        }
    }
}
