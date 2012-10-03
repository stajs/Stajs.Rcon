using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stajs.Rcon.Core.Commands
{
	internal abstract class RconCommand
	{
		private readonly ServerCommandType _commandType;

		// TODO: figure out what this is for
		private const string String2 = "";

		// TODO: increment
		private const int RequestId = 69;

		protected RconCommand(ServerCommandType commandType)
		{
			_commandType = commandType;
		}

		internal abstract string ToCommandString();

		internal byte[] GetBytes()
		{
			/* https://developer.valvesoftware.com/wiki/Source_RCON_Protocol
			 * 
			 *             +------------------------------ Packet size ----------------------------------+
			 *             v                                                                             v
			 * +-----------+----------+------------+-------+------------------+-------+------------------+
			 * |Packet size|Request Id|Command type|Command|Command terminator|String2|String2 terminator|
			 * +-----------+----------+------------+-------+------------------+-------+------------------+
			 * 
			 * Packet size (int = 4 bytes)
			 *		Size of packet from the start of the Request Id to the end of the String2 terminator.
			 *	
			 * Request Id (int = 4 bytes)
			 *		The Request Id is mirrored back in the response, so you can marry it up with the command.
			 *		It should be incremented for each request.
			 *		
			 * Command type (int = 4 bytes)
			 *		Either SERVERDATA_AUTH (3) or SERVERDATA_EXECCOMMAND (2)
			 *		
			 * Command (variable length)
			 *		Known as "string1" in the docs. For SERVERDATA_AUTH, this will be the password to authenticate
			 *		with. For SERVERDATA_EXECCOMMAND, this will be the command to execute (e.g. "status" or "say hello world").
			 *		
			 * Command terminator (null = 1 byte)
			 *		Used to terminate the Command
			 *		
			 * String2 (variable length)
			 *		NFI WTH this is for ATM. It's empty from what I can see so far. Guess I'll find out more as I go...
			 *		
			 * String2 terminator (null = 1 byte)
			 *		Used to terminate String2
			 */

			const int packetSizeLength = 4;
			const int requestIdLength = 4;
			const int commandTypeLength = 4;
			const int terminatorLength = 1;

			var utf = new UTF8Encoding();

			var requestId = BitConverter.GetBytes(RequestId);
			var commandType = BitConverter.GetBytes((int)_commandType);
			var command = utf.GetBytes(ToCommandString());
			var string2 = utf.GetBytes(String2);

			var totalLength = packetSizeLength
				+ requestIdLength
				+ commandTypeLength
				+ command.Length
				+ terminatorLength
				+ string2.Length
				+ terminatorLength;

			var packetSize = BitConverter.GetBytes(totalLength - packetSizeLength);
			var bytes = new byte[totalLength];
			var i = 0;

			packetSize.CopyTo(bytes, i);
			i += packetSizeLength;

			requestId.CopyTo(bytes, i);
			i += requestIdLength;

			commandType.CopyTo(bytes, i);
			i += commandTypeLength;

			command.CopyTo(bytes, i);
			i += command.Length;

			bytes[i] = 0;
			i += terminatorLength;

			string2.CopyTo(bytes, i);
			i += string2.Length;

			bytes[i] = 0;

			return bytes;
		}
	}
}