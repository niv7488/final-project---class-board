using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RLC.RemoteDesktop
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class RdViewer : Window, IViewerService
	{
		private System.Drawing.Image _screen = null;
		private System.Drawing.Image _cursor = null;
		private int _cursorX = 0;
		private int _cursorY = 0;

		public RdViewer()
		{
			//InitializeComponent();
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
	}
}
