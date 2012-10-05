using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Stajs.Rcon.Core.Commands;
using Stajs.Rcon.Core.Responses;

namespace Stajs.Rcon.Core
{
	public class RconClient : IDisposable
	{
		private readonly IPEndPoint _server;
		private readonly Socket _socket;
		private readonly string _password;

		private List<int> _openResponses = new List<int>();

		public int RequestId { get; private set; }

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

			RequestId = 0;
		}

		public void Test()
		{
			_socket.Connect(_server);

			Send(new AuthenticateCommand(_password));
			Receive();
			Receive();

			//var rawCommand = new RawCommand("cvarlist");
			//Send(rawCommand);
			//var response = Receive();

			//// TODO: extract to method
			//while (!(response.Response.Trim() == "END" && response.RequestId == rawCommand.RequestId + 1))
			//	response = Receive();

			Send(new UsersCommand());
			Receive();

			//Send(new SayCommand("Oh hai!"));
			//Receive();

			Send(new StatusCommand());
			Receive();

			Debug.Print("_openResponses.Count: " + _openResponses.Count);
			Debug.Print("_openResponses: " + string.Join(",", _openResponses));
		}

		private void Send(RconCommand command)
		{
			SendWithTerminator(command);
		}

		private void SendWithTerminator(RconCommand command)
		{
			var commands = new List<RconCommand> { command, new EndCommand() };

			foreach (var c in commands)
			{
				var bytes = c.GetBytes(++RequestId);
				var bytesSent = _socket.Send(bytes);

				if (!(c is EndCommand))
					_openResponses.Add(RequestId);

				Debug.Print("   > Bytes sent: " + bytesSent);
				Debug.Print("   > command.RequestId: " + c.RequestId);
				Debug.Print("   > command.CommandType: " + c.CommandType);
				Debug.Print("   > command.Command: " + c.Command);
				Debug.Print("----------------------------------------------");
			}
		}

		private RconPacket Receive()
		{
			// TODO: Timeout
			// TODO: Exceptions
			
			//while (!(response.Response.Trim() == "END" && response.RequestId == rawCommand.RequestId + 1))
			//	response = Receive();

			var sizeBuffer = ReadFromSocket(RconPacket.PacketSizeLength);
			var packetSize = BitConverter.ToInt32(sizeBuffer, 0);
			var packetBuffer = ReadFromSocket(packetSize);
			var totalBytes = sizeBuffer.Concat(packetBuffer).ToArray();
			var packet = new RconPacket(totalBytes);

			Debug.Print("<    Bytes received: " + totalBytes.Length);
			Debug.Print("<    packet.Size: " + packet.Size);
			Debug.Print("<    packet.RequestId: " + packet.RequestId);
			Debug.Print("<    packet.ResponseType: " + packet.ResponseType);
			Debug.Print("<    packet.Response: " + packet.Response);
			Debug.Print("==============================================");

			return packet;
		}

		private byte[] ReadFromSocket(int length)
		{
			var buffer = new byte[length];
			var position = 0;

			while (position < buffer.Length)
				position += _socket.Receive(buffer, position, buffer.Length - position, SocketFlags.None);

			return buffer;
		}

		void IDisposable.Dispose()
		{
			if (_socket.Connected)
				_socket.Close();
		}
	}
}