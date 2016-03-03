using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RLC.RemoteDesktop
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
            Console.WriteLine("client");
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new RemoteDesktopViewer());
		}
	}
}
