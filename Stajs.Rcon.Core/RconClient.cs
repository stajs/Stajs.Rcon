using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Stajs.Rcon.Core
{
	public class RconClient : IDisposable
	{
		private readonly IPEndPoint _server;
		private readonly Socket _socket;
		private readonly string _password;

		public RconClient(string ipAddress, int port, string password) : this(IPAddress.Parse(ipAddress), port, password) { }

		public RconClient(IPAddress ipAddress, int port, string password) : this(new IPEndPoint(ipAddress, port), password) { }

		public RconClient(IPEndPoint server, string password)
		{
			Debug.Indent();
			_server = server;
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
			{
				SendTimeout = 5000,
				ReceiveTimeout = 5000
			};
			_password = password;	
		}

		public void Test(string message)
		{
			Debug.Print("Connecting...");
			_socket.Connect(_server);
			Debug.Print("Connected: " + _socket.Connected);
			Debug.Print("Say {0}", message);

			var packet = new RconPacket
			{
				ServerDataCommand = ServerDataCommand.Auth,
				String1 = _password 
			};

			Send(packet);
			var response = Receive();
			Debug.Print(response);

			packet = new RconPacket
			{
				ServerDataCommand = ServerDataCommand.Exec,
				String1 = "say " + message
			};

			Send(packet);
			response = Receive();
			Debug.Print(response);

			packet = new RconPacket
			{
				ServerDataCommand = ServerDataCommand.Exec,
				String1 = "status"
			};

			Send(packet);
			response = Receive();
			Debug.Print(response);
		}

		private void Send(RconPacket packet)
		{
			var bytes = packet.GetBytes();
			var i = _socket.Send(bytes);
			Debug.Print("Sent {0} bytes.", i);
		}

		private string Receive()
		{
			//var position = 0;
			//var bytes = new byte[4];
			//while (position < bytes.Length)
			//{
			//	position += _socket.Receive(bytes, position, bytes.Length - position, SocketFlags.None);
			//}
			//Debug.Print("Received {0} bytes.", position);

			//var i =_socket.Receive(bytes);
			//Debug.Print("Received {0} bytes.", i);

			//var packetSize = BitConverter.ToInt32(bytes, 0);
			//Debug.Print("packetSize: " + packetSize);

			//bytes = new byte[packetSize + 400];

			//position = 0;
			//while (position < bytes.Length)
			//{
			//	position += _socket.Receive(bytes, position, bytes.Length - position, SocketFlags.None);
			//}

			//Debug.Print("Received {0} bytes.", position);
			
			var bytes = new byte[311];

			Thread.Sleep(150);

			var i = _socket.Receive(bytes);
			Debug.Print("Received {0} bytes.", i);

			var response = Encoding.UTF8.GetString(bytes);
			Debug.Print(response);

			return response;
		}

		void IDisposable.Dispose()
		{
			if (_socket.Connected)
				_socket.Close();
		}
	}
}