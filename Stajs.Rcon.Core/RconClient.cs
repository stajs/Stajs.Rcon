using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Stajs.Rcon.Core.Commands;
using Stajs.Rcon.Core.Responses;

namespace Stajs.Rcon.Core
{
	public class RconClient : IDisposable
	{
		private readonly IPEndPoint _server;
		private readonly Socket _socket;
		private readonly string _password;
		private int _requestId;

		private readonly List<int> _openResponses = new List<int>();
		private readonly List<RconPacket> _openPackets = new List<RconPacket>();
		private readonly Queue<RconResponse> _responses = new Queue<RconResponse>();

		public RconClient(string ipAddress, int port) : this(IPAddress.Parse(ipAddress), port) { }

		public RconClient(IPAddress ipAddress, int port) : this(new IPEndPoint(ipAddress, port)) { }

		public RconClient(IPEndPoint server)
		{
			_requestId = 0;
			_server = server;
			_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
			{
				SendTimeout = 200,
				ReceiveTimeout = 200
			};

			_socket.Connect(_server);
		}

		[Obsolete]
		public void Test()
		{
			Send(new AuthenticateCommand(_password));
			Send(new RawCommand("cvarlist"));
			Send(new UsersCommand());
			Send(new SayCommand("Oh hai!"));
			Send(new StatusCommand());

			Receive();

			Debug.Print("_openResponses.Count: " + _openResponses.Count);
			Debug.Print("_openResponses: " + string.Join(",", _openResponses));
		}

		public int Send(RconCommand command)
		{
			return SendWithTerminator(command);
		}

		private int SendWithTerminator(RconCommand command)
		{
			var commands = new List<RconCommand> { command, new EndCommand() };

			foreach (var c in commands)
			{
				var bytes = c.GetBytes(++_requestId);
				var bytesSent = _socket.Send(bytes);

				if (!(c is EndCommand))
					_openResponses.Add(_requestId);

				Debug.Print("   > command.RequestId: " + c.RequestId);
				Debug.Print("   > command.CommandType: " + c.CommandType);
				Debug.Print("   > command.Command: " + c.Command);
				Debug.Print("   > Bytes sent: " + bytesSent);
				Debug.Print("----------------------------------------------");
			}

			return command.RequestId.Value;
		}

		private void Receive()
		{
			// TODO: Timeout
			// TODO: Exceptions

			while (_openResponses.Any())
			{
				ReadFromSocket();
			}
		}

		private void ReadFromSocket()
		{
			var sizeBuffer = ReadFromSocket(RconPacket.PacketSizeLength);
			var packetSize = BitConverter.ToInt32(sizeBuffer, 0);
			var packetBuffer = ReadFromSocket(packetSize);
			var totalBytes = sizeBuffer.Concat(packetBuffer).ToArray();
			var packet = new RconPacket(totalBytes);

			_openPackets.Add(packet);

			if (packet.IsEndResponsePacket)
				EndResponse(packet.RequestId);

			// TODO: verbose logging level
			//Debug.Print("<    Bytes received: " + totalBytes.Length);
			//Debug.Print("<    packet.Size: " + packet.Size);
			//Debug.Print("<    packet.RequestId: " + packet.RequestId);
			//Debug.Print("<    packet.ResponseType: " + packet.ResponseType);
			//Debug.Print("<    packet.Response: " + packet.Response);
			//Debug.Print("----------------------------------------------");
		}

		private void EndResponse(int endResponseId)
		{
			var responseId = endResponseId - 1;

			_openResponses.Remove(endResponseId);
			_openResponses.Remove(responseId);

			var packets = _openPackets.Where(p => p.RequestId == responseId).ToList();
			_responses.Enqueue(new RconResponse(packets));
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