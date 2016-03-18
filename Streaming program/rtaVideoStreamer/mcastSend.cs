using System;
using System.Net;
using System.Net.Sockets;

namespace multiCastSend
{
	class send
	{
		send(string mcastGroup, string port, string ttl, string rep) 
		{
			IPAddress ip;
			try 
			{
				Console.WriteLine("MCAST Send on Group: {0} Port: {1} TTL: {2}",mcastGroup,port,ttl);
				ip=IPAddress.Parse(mcastGroup);
				
				Socket s=new Socket(AddressFamily.InterNetwork, 
								SocketType.Dgram, ProtocolType.Udp);
				
				s.SetSocketOption(SocketOptionLevel.IP, 
					SocketOptionName.AddMembership, new MulticastOption(ip));

				s.SetSocketOption(SocketOptionLevel.IP, 
					SocketOptionName.MulticastTimeToLive, int.Parse(ttl));
			
				byte[] b=new byte[10];
				for(int x=0;x<b.Length;x++) b[x]=(byte)(x+65);

				IPEndPoint ipep=new IPEndPoint(IPAddress.Parse(mcastGroup),int.Parse(port));
				
				Console.WriteLine("Connecting...");

				s.Connect(ipep);

                for(int x=0;x<int.Parse(rep);x++) {
					Console.WriteLine("Sending ABCDEFGHIJ...");
					s.Send(b,b.Length,SocketFlags.None);
                }

				Console.WriteLine("Closing Connection...");
				s.Close();
			} 
			catch(System.Exception e) { Console.Error.WriteLine(e.Message); }
		}

		static void Main(string[] args)
		{

            new send("224.5.6.7", "5000", "1", "2");
		}
	}
}
