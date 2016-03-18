using System.Net.Sockets;
using System.Net;
using System;


namespace multiCastRecv 
{

	class recv 
	{

		recv(string mcastGroup, string port) 
		{
			Socket s=new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			
			IPEndPoint ipep=new IPEndPoint(IPAddress.Any,int.Parse(port));
			s.Bind(ipep);
			
			IPAddress ip=IPAddress.Parse(mcastGroup);

			s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip,IPAddress.Any));

			while(true) 
			{
				byte[] b=new byte[10];
                Console.WriteLine("Waiting for data..");
				s.Receive(b);
				string str = System.Text.Encoding.ASCII.GetString(b,0,b.Length);
				Console.WriteLine("RX: " + str.Trim());
			}
			//s.Close();
		}


		public static void Main(string[] args) 
		{
			new recv("224.5.6.7","5000");
		}
	}







}