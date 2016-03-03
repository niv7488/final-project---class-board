using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Drawing;
using RLC.RemoteDesktop;
using System.Threading;
using System.Diagnostics;

namespace RLC.RemoteDesktop
{
	class Program
	{
		private static ScreenCapture capture = new ScreenCapture();
		private static RemoteDesktopServer.ViewerProxy.ViewerServiceClient viewerProxy = new RemoteDesktopServer.ViewerProxy.ViewerServiceClient();

		private static Thread _threadScreen = null;
		private static Thread _threadCursor = null;
		private static bool _stopping = false;
		private static int _numByteFullScreen = 1;

		[STAThread()]
		static void Main(string[] args)
		{
			_threadScreen = new Thread(new ThreadStart(ScreenThread));
			_threadScreen.Start();

			_threadCursor = new Thread(new ThreadStart(CursorThread));
			_threadCursor.Start();

			Console.ReadLine();

			_stopping = true;
			_threadCursor.Join();
			_threadScreen.Join();
		}

		/// <summary>
		/// Refreshes the connection.
		/// </summary>
		private static void RefreshConnection()
		{
			// Get a new proxy to the WCF service.
			//
			viewerProxy = new RemoteDesktopServer.ViewerProxy.ViewerServiceClient();

			// Force a full screen capture.
			//
			capture.Reset();
		}

		/// <summary>
		/// Screens the thread.
		/// </summary>
		private static void ScreenThread()
		{
			Rectangle bounds = Rectangle.Empty;
			// Run until we are asked to stop.
			//
			while (!_stopping)
			{
				try
				{
					// Capture a bitmap of the changed pixels.
					//
					Bitmap image = capture.Screen(ref bounds);
					if (_numByteFullScreen == 1)
					{
						// Initialize the screen size (used for performance metrics)
						//
						_numByteFullScreen = bounds.Width * bounds.Height * 4;
					}
					if (bounds != Rectangle.Empty && image != null)
					{
						// We have data...pack it and send it.
						//
						byte[] data = Utils.PackScreenCaptureData(image, bounds);
						if (data != null)
						{
							// Thread safety on the proxy.
							//
							lock (viewerProxy)
							{
								try
								{
									// Push the data.
									//
									viewerProxy.PushScreenUpdate(data);

									// Show performance metrics
									//
									double perc1 = 100.0 * 4.0 * image.Width * image.Height / _numByteFullScreen;
									double perc2 = 100.0 * data.Length / _numByteFullScreen;
									Console.WriteLine(DateTime.Now.ToString() + ": Screen - {0:0.0} percent, {1:0.0} percent with compression", perc1, perc2);
								}
								catch (Exception ex)
								{
									// Push exception...log it
									//
									Console.WriteLine("*******************");
									Console.WriteLine(ex.ToString());
									Console.WriteLine("No connection...trying again in 5 seconds");
									RefreshConnection();
									Thread.Sleep(5000);
								}
							}
						}
						else
						{
							// Show performance metrics.
							//
							Console.WriteLine(DateTime.Now.ToString() + ": Screen - no data bytes");
						}
					}
					else
					{
						// Show performance metrics.
						//
						Console.WriteLine(DateTime.Now.ToString() + ": Screen - no new image data");
					}
				}
				catch (Exception ex)
				{
					// Unhandled exception...log it.
					//
					Console.WriteLine("Unhandled: ************");
					Console.WriteLine(ex.ToString());
				}
			}
		}

		/// <summary>
		/// Cursors the thread.
		/// </summary>
		private static void CursorThread()
		{
			// Run until we are asked to stop.
			//
			while (!_stopping)
			{
				try
				{
					// Get an update for the cursor.
					//
					int cursorX = 0;
					int cursorY = 0;
					Bitmap image = capture.Cursor(ref cursorX, ref cursorY);
					if (image != null)
					{
						// We have valid data...pack and push it.
						//
						byte[] data = Utils.PackCursorCaptureData(image, cursorX, cursorY);
						if (data != null)
						{
							try
							{
								// Push the data.
								//
								string commandStack = viewerProxy.PushCursorUpdate(data);

								// Show performance metrics.
								//
								double perc1 = 100.0 * 4.0 * image.Width * image.Height / _numByteFullScreen;
								double perc2 = 100.0 * data.Length / _numByteFullScreen;
								Console.WriteLine(DateTime.Now.ToString() + ": Cursor - {0:0.0} percent, {1:0.0} percent with compression", perc1, perc2);

								// Process command stack
								//
								ProcessCommands(commandStack);
							}
							catch (Exception ex)
							{
								// Push exception...log it.
								//
								Thread.Sleep(1000);
							}
						}
					}
				}
				catch(Exception ex)
				{
					// Unhandled exception...log it.
					//
					Console.WriteLine("Unhandled: ************");
					Console.WriteLine(ex.ToString());
				}

				// Throttle this thread a bit.
				//
				Thread.Sleep(10);
			}
		}

		private static void ProcessCommands(string commandStack)
		{
			Console.WriteLine(commandStack);

			// Parse the commands
			//
			CommandInfoCollection cmds = new CommandInfoCollection();
			cmds.DeserializeCommandStack(commandStack);

			CommandInfo cmd = null;
			while( (cmd = cmds.GetNextCommand()) != null)
			{
				Command.Execute(cmd);
				Console.WriteLine(cmd);
			}
		}
	}
}
