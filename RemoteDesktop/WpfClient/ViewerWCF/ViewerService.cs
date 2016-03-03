using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ServiceModel;
using System.Threading;

namespace RLC.RemoteDesktop
{
	public class ViewerService : IViewerService
	{
		private static ServiceHost _viewerService = null;
		private static Thread _svcThread = null;
		private static bool _endThread = false;

		private static void ServiceThread()
		{
			string myHost = System.Net.Dns.GetHostName();
			string myIp = System.Net.Dns.GetHostEntry(myHost).AddressList[1].ToString();
			Uri baseAddress = new Uri("http://" + myIp + ":8080/Rlc/Viewer");
			_viewerService = new ServiceHost(typeof(ViewerService), baseAddress);
			_viewerService.Open();
			while (!_endThread)
			{
				Thread.Sleep(5000);
			}
			_viewerService.Close();
		}

		public static void StopService()
		{
			_endThread = true;
			_svcThread.Join();
		}

		public static void StartService()
		{
			_svcThread = new Thread(new ThreadStart(ServiceThread));
			_svcThread.Start();
		}

		private Image _screen = null;
		private Image _cursor = null;
		private int _cursorX = 0;
		private int _cursorY = 0;

		private Image _display = null;
		public Image Display
		{ 
			get 
			{ 
				return _display; 
			} 
		}

		public delegate void ImageChangeHandler(Image display);
		public event ImageChangeHandler OnImageChange;

		private ViewerService()
		{

		}

		#region IViewerService Members

		public void PushScreenUpdate(byte[] data)
		{
			if (data != null)
			{
				// Update the current screen
				//
				Utils.UpdateScreen(ref _screen, data);
			}
			else
			{
				// screen has not changed
			}
		}

		public void PushCursorUpdate(byte[] data)
		{
			if (data != null)
			{
				// Unpack the data.
				//
				Utils.UnpackCursorCaptureData(data, out _cursor, out _cursorX, out _cursorY);
			}
			else
			{
				_cursor = null;
			}
		}

		#endregion

		private void UpdateScreenImage()
		{
			if (_screen != null)
			{
				if (_cursor != null)
				{
					_display = Utils.MergeScreenAndCursor(_screen, _cursor, _cursorX, _cursorY);
				}
				else
				{
					_display = _screen;
				}

				if (OnImageChange != null)
				{
					OnImageChange(_display);
				}
			}
		}
	}
}
