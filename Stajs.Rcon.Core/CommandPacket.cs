using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stajs.Rcon.Core
{
	internal abstract class CommandPacket
	{
		private readonly CommandType _commandType;
		private readonly string _command;

		protected CommandPacket(CommandType commandType, string command)
		{
			_commandType = commandType;
			_command = command;
		}

		// TODO: figure out what these are for
		private const string String2 = "";
		private const int RequestId = 69;

		internal byte[] GetBytes()
		{
			const int bytesForPacketSize = 4;
			const int bytesForRequestId = 4;
			const int bytesForCommand = 4;
			const int bytesForTerminator = 1;

			var requestId = BitConverter.GetBytes(RequestId);
			var command = BitConverter.GetBytes((int)_commandType);
			var utf = new UTF8Encoding();
			var string1 = utf.GetBytes(_command);
			var string2 = utf.GetBytes(String2);

			var length = bytesForPacketSize
				+ bytesForRequestId
				+ bytesForCommand
				+ string1.Length
				+ bytesForTerminator
				+ string2.Length
				+ bytesForTerminator;

			var bytes = new byte[length];
			var packetSize = BitConverter.GetBytes(length - bytesForPacketSize);

			var pointer = 0;
			packetSize.CopyTo(bytes, pointer);
			pointer += bytesForPacketSize;

			requestId.CopyTo(bytes, pointer);
			pointer += bytesForRequestId;

			command.CopyTo(bytes, pointer);
			pointer += bytesForCommand;

			string1.CopyTo(bytes, pointer);
			pointer += string1.Length;

			bytes[pointer] = 0;
			pointer += bytesForTerminator;

			string2.CopyTo(bytes, pointer);
			pointer += string2.Length;

			bytes[pointer] = 0;

			return bytes;
		}
	}
}