using System;
using System.Collections.Generic;
using System.Drawing;
using System.ServiceModel;
using System.Threading;

namespace RLC.RemoteDesktop
{
	public class ViewerSession
	{
		public Guid Id;
		public Image Screen;
		public Image Cursor;
		public int CursorX;
		public int CursorY;
		public Image Display;
	}


	public class ViewerService : IViewerService
	{
		#region Static Members and Methods

		private static readonly Dictionary<Guid, ViewerSession> _sessions = new Dictionary<Guid, ViewerSession>();

		public delegate void ImageChangeHandler(Image display, string remoteIpAddress);
		public static event ImageChangeHandler OnImageChange;

		public static CommandInfoCollection Commands = new CommandInfoCollection();

		#endregion

		private ServiceHost _viewerService;
		private Thread _svcThread;
		private bool _endThread;

		private void ServiceThread()
		{
//			string myHost = System.Net.Dns.GetHostName();
//			string myIp = System.Net.Dns.GetHostEntry(myHost).AddressList[1].ToString();
//			Uri baseAddress = new Uri("http://" + myIp + ":8080/Rlc/Viewer");
			Uri baseAddress = new Uri("http://localhost:1003/Rlc/Viewer");
			_viewerService = new ServiceHost(typeof(ViewerService), baseAddress);
			_viewerService.Open();
			while (!_endThread)
			{
				Thread.Sleep(5000);
			}
			_viewerService.Close();
		}

		public void StopService()
		{
			_endThread = true;
			_svcThread.Join();
		}

		public void StartService()
		{
			_svcThread = new Thread(ServiceThread);
			_svcThread.Start();
		}


		#region IViewerService Members

		public void PushScreenUpdate(byte[] data)
		{
			if (data != null)
			{
				// Unpack the data.
				//
				Image partial;
				Rectangle bounds;
				Guid id;
				Utils.UnpackScreenCaptureData(data, out partial, out bounds, out id);

				// Update the current screen
				//
				ViewerSession viewSession;
				if (!_sessions.ContainsKey(id))
				{
					// Create a new session.
					//
					viewSession = new ViewerSession {Id = id};
					_sessions[id] = viewSession;
				}
				else
				{
					viewSession = _sessions[id];
				}
				Utils.UpdateScreen(ref viewSession.Screen, partial, bounds);

				UpdateScreenImage(id);
			}
		}

		public string PushCursorUpdate(byte[] data)
		{
			if (data != null)
			{
				// Unpack the data.
				//
				Image cursor;
				int cursorX, cursorY;
				Guid id;
				Utils.UnpackCursorCaptureData(data, out cursor, out cursorX, out cursorY, out id);

				// Update the current screen
				//
				ViewerSession viewSession;
				if (!_sessions.ContainsKey(id))
				{
					// Create a new session.
					//
					viewSession = new ViewerSession {Id = id};
					_sessions[id] = viewSession;
				}
				else
				{
					viewSession = _sessions[id];
				}
				viewSession.Cursor = cursor;
				viewSession.CursorX = cursorX;
				viewSession.CursorY = cursorY;
				UpdateScreenImage(id);
			}

			return Commands.SerializeCommandStack();
		}

		public string Ping()
		{
			return "Connected: ServerTime = " + DateTime.Now;
		}

		#endregion

		private static void UpdateScreenImage(Guid id)
		{
			ViewerSession viewSession = _sessions[id];
			if (viewSession == null)
			{
				return;
			}
			if (viewSession.Screen != null)
			{
				if (viewSession.Cursor != null)
				{
					viewSession.Display = Utils.MergeScreenAndCursor(viewSession.Screen,
						viewSession.Cursor, viewSession.CursorX, viewSession.CursorY);
				}
				else
				{
					viewSession.Display = viewSession.Screen;
				}

				if (OnImageChange != null)
				{
					OnImageChange(viewSession.Display, viewSession.Id.ToString());
				}
			}
		}
	}
}
