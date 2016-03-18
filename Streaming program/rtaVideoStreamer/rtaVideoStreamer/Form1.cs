using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using rtaNetworking.Streaming;
using System.Net;
using System.Net.Sockets;

namespace rtaVideoStreamer
{
    public partial class Form1 : Form
    {

        private ImageStreamingServer _Server;

        public Form1()
        {

            InitializeComponent();
            this.linkLabel1.Text = string.Format("http://30.30.42.61:8080", Environment.MachineName);
            UdpClient udpclient = new UdpClient();

 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _Server = new ImageStreamingServer();
            _Server.Start(8080);
        }

        private DateTime time = DateTime.MinValue;

        private void timer1_Tick(object sender, EventArgs e)
        {
            int count = (_Server.Clients != null) ? _Server.Clients.Count() : 0;

            this.sts.Text = "Clients: " + count.ToString();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("chrome", this.linkLabel1.Text);

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e) {

        }

    }


}
