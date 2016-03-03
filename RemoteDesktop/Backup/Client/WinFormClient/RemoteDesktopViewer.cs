using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WinFormClient;

namespace RLC.RemoteDesktop
{
	public partial class RemoteDesktopViewer : Form
	{
		private readonly Logging _logger = new Logging();
		private ViewerService _svc;

		private readonly Dictionary<string, Image> _remoteViews = new Dictionary<string, Image>();

		public RemoteDesktopViewer()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.DoubleBuffer, true); 
			
			_logger.Show();
			_svc = new ViewerService();
			ViewerService.OnImageChange += SvcOnImageChange;
			_svc.StartService();
			_logger.AddEntry("Service started...");
		}

		void SvcOnImageChange(Image display, string remoteIpAddress)
		{
			if(display!=null)
			{
				lock (display)
				{
					UpdateTabs(display, remoteIpAddress);
				}
			}
		}

		private delegate void UpdateTabsDelegate(Image display, string remoteIpAddress);
		private void UpdateTabs(Image display, string remoteIpAddress)
		{
			if (tabControl1.InvokeRequired)
			{
				Invoke(new UpdateTabsDelegate(UpdateTabs), new object[] { display, remoteIpAddress });
			}
			else
			{
				if (!_remoteViews.ContainsKey(remoteIpAddress))
				{
					// Add a new tab
					//
					TabPage page = new TabPage(remoteIpAddress);
					tabControl1.TabPages.Add(page);
				}
				
				// Add this to or update the dictionary
				//
				_remoteViews[remoteIpAddress] = display;

				// Update the viewer
				//
				pictureBox1.BackgroundImage = _remoteViews[tabControl1.SelectedTab.Text];
			}
		}

		private void RemoteDesktopViewer_FormClosing(object sender, FormClosingEventArgs e)
		{
			_svc.StopService();
		}

		public delegate void LogDelegate(string message);
		private void Log(string message)
		{
			if (InvokeRequired)
			{
				// Not on the UI thread.
				Invoke(new LogDelegate(Log), new object[] { message });
			}
			else
			{
				// On the UI thread.
				_logger.AddEntry(message);
			}
		}

		private void PictureBox1MouseMove(object sender, MouseEventArgs e)
		{
			if (pictureBox1.BackgroundImage != null)
			{
				int cursorX = e.X * pictureBox1.BackgroundImage.Width / pictureBox1.Width;
				int cursorY = e.Y * pictureBox1.BackgroundImage.Height / pictureBox1.Height;
				string data = cursorX + "," + cursorY;
				CommandInfo cmd = new CommandInfo(CommandInfo.CommandTypeOption.MouseMove, data);
				ViewerService.Commands.Add(cmd);
			}
		}
	}
}
