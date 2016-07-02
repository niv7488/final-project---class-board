using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoardCast
{
    /// <summary>
    /// Class that managed all opened processes
    /// </summary>
    class ProcessManager
    {
        
        public ProcessManager()
        {}

        /// <summary>
        /// Generate process to open file
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="editMode">Edit file mode (true in case of image)</param>
        public void GenerateProcess(string filePath,bool editMode = false)
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

    }
}
