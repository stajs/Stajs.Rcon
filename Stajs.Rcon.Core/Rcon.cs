using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Stajs.Rcon.Core
{
	public class Rcon
	{
		private readonly IPAddress _ipAddress;
		private readonly int _port;

		public Rcon(string ipAddress, int port, string password) : this(IPAddress.Parse(ipAddress), port, password)
		{
			
		}

		public Rcon(IPAddress ipAddress, int port, string password)
		{
			_ipAddress = ipAddress;
			_port = port;
		}

		public void Connect()
		{
			var server = new IPEndPoint(_ipAddress, _port);
			var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.Connect(server);
			
			socket.Disconnect(true);
		}
	}
}