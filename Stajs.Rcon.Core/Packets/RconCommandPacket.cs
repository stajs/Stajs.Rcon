using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Stajs.Rcon.Core.Commands;

namespace Stajs.Rcon.Core.Packets
{
	public class RconCommandPacket : RconPacket
	{
		public RconCommandPacket(RconCommand command)
		{
			Bytes = GetBytes(command);
		}

		private byte[] GetBytes(RconCommand command)
		{
			Debug.Assert(command.RequestId.HasValue);

			var utf = new UTF8Encoding();

			var id = BitConverter.GetBytes(command.RequestId.Value);
			var commandType = BitConverter.GetBytes((int)command.CommandType);
			var commandString = utf.GetBytes(command.ToCommandString());

			var totalLength = PacketSizeLength
				+ RequestIdLength
				+ TypeLength
				+ commandString.Length
				+ TerminatorLength;

			var packetSize = BitConverter.GetBytes(totalLength - PacketSizeLength);
			var bytes = new byte[totalLength];
			var i = 0;

			packetSize.CopyTo(bytes, i);
			i += PacketSizeLength;

			id.CopyTo(bytes, i);
			i += RequestIdLength;

			commandType.CopyTo(bytes, i);
			i += TypeLength;

			commandString.CopyTo(bytes, i);
			i += commandString.Length;

			bytes[i] = 0;
			i++;

			bytes[i] = 0;

			return bytes;
		}
	}
}