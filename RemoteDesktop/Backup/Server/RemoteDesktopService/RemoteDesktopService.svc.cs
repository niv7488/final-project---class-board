using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Drawing;
using System.IO;
using RLC.RemoteDesktop;

namespace RLC.RemoteDesktop
{
	public class RemoteDesktopService : IRemoteDesktop
	{
		// An instance of the screen capture class.
		//
		private ScreenCapture capture = new ScreenCapture();

		/// <summary>
		/// Capture the screen image and return bytes.
		/// </summary>
		/// <returns>4 ints [top,bot,left,right] (16 bytes) + image data bytes</returns>
		public byte[] UpdateScreenImage()
		{
			// Capture minimally sized image that encompasses
			//	all the changed pixels.
			//
			Rectangle bounds = new Rectangle();
			Bitmap img = capture.Screen(ref bounds);
			if (img != null)
			{
				// Something changed.
				//
				byte[] result = Utils.PackScreenCaptureData(img, bounds);

				// Log to the console.
				//
				Console.WriteLine(DateTime.Now.ToString() + " Screen Capture - {0} bytes, {1} percent", result.Length, capture.PercentOfImage);
				return result;
			}
			else
			{
				// Nothing changed.
				//

				// Log to the console.
				Console.WriteLine(DateTime.Now.ToString() + " Screen Capture - {0} bytes, {1} percent", 0, 0.0);
				return null;
			}
		}

		/// <summary>
		/// Capture the cursor data.
		/// </summary>
		/// <returns>2 ints [x,y] (8 bytes) + image bytes</returns>
		public byte[] UpdateCursorImage()
		{
			// Get the cursor bitmap.
			//
			int cursorX = 0;
			int cursorY = 0;
			Image img = capture.Cursor(ref cursorX, ref cursorY);
			if (img != null)
			{
				// Something changed.
				//
				byte[] result = Utils.PackCursorCaptureData(img, cursorX, cursorY);

				// Log to the console.
				//
				Console.WriteLine(DateTime.Now.ToString() + " Cursor Capture - {0} bytes", result.Length);
				return result;
			}
			else
			{
				// Nothing changed.
				//

				// Log to the console.
				//
				Console.WriteLine(DateTime.Now.ToString() + " Cursor Capture - {0} bytes", 0);
				return null;
			}
		}
	}
}
