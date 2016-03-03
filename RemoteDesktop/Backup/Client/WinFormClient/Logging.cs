using System;
using System.Windows.Forms;

namespace WinFormClient
{
	public partial class Logging : Form
	{
		public Logging()
		{
			InitializeComponent();
		}

		private void Logging_Load(object sender, EventArgs e)
		{

		}

		public delegate void AddEntryDelegate(string message);
		public void AddEntry(string message)
		{
			if (listBox1.InvokeRequired)
			{
				Invoke(new AddEntryDelegate(AddEntry), new object[] { message }); ;
			}
			else
			{
				listBox1.Items.Add(message);
				listBox1.Refresh();
			}
		}
	}
}
