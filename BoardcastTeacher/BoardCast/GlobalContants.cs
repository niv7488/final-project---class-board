using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoardCast
{
    public static class GlobalContants
    {
        public static string screenshotFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Screenshots");
        public static string canvasFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "CanvasLayouts");
        public static string backgroundFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Backgrounds");
        public static string loginServerAddress = "http://52.34.153.216:3000/teacherLogin";
    }
}
