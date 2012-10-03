using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Stajs.Rcon.Core.Commands;

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

		public void Test()
		{
			_socket.Connect(_server);

			var authCommand = new AuthenticateCommand(_password);
			Send(authCommand);
			var response = Receive();
			Debug.Print(response);

			var usersCommand = new UsersCommand();
			Send(usersCommand);
			response = Receive();
			Debug.Print(response);

			var sayCommand = new SayCommand("Oh hai!");
			Send(sayCommand);
			response = Receive();
			Debug.Print(response);

			var statusCommand = new StatusCommand();
			Send(statusCommand);
			response = Receive();
			Debug.Print(response);
		}

		private void Send(RconCommand command)
		{
			var bytes = command.GetBytes();
			var i = _socket.Send(bytes);
			Debug.Print("Sent {0} bytes.", i);
		}

		private string Receive()
		{
			//TODO: timeout

			// Packet size

			var buffer = new byte[4];
			var position = 0;

			while (position < buffer.Length)
				position += _socket.Receive(buffer, position, buffer.Length - position, SocketFlags.None);

			var packetSize = BitConverter.ToInt32(buffer, 0);

			// Request Id
			
			buffer = new byte[4];
			position = 0;

			while (position < buffer.Length)
				position += _socket.Receive(buffer, position, buffer.Length - position, SocketFlags.None);

			var requestId = BitConverter.ToInt32(buffer, 0);
			
			// Rest

			buffer = new byte[packetSize - 4]; // minus request id
			position = 0;

			while (position < buffer.Length)
				position += _socket.Receive(buffer, position, buffer.Length - position, SocketFlags.None);

			Debug.Print("Request Id: " + requestId);
			return requestId.ToString();

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