using System;
using System.Text;

namespace Stajs.Rcon.Core.Commands
{
	public abstract class RconCommand
	{
		public int? RequestId { get; private set; }
		public ServerCommandType CommandType { get; private set; }
		public string Command { get { return ToCommandString(); } }
		public string String2 { get; private set; }

		protected RconCommand(ServerCommandType commandType)
		{
			CommandType = commandType;
			String2 = string.Empty;
		}

		internal abstract string ToCommandString();

		internal byte[] GetBytes(int requestId)
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
			 *		I'm not convinced this is actually used yet. From what I have seen it looks like "string1" is double-null terminated...
			 *		
			 * String2 terminator (null = 1 byte)
			 *		Used to terminate String2
			 */

			RequestId = requestId;

			var utf = new UTF8Encoding();

			var id = BitConverter.GetBytes(RequestId.Value);
			var commandType = BitConverter.GetBytes((int)CommandType);
			var command = utf.GetBytes(ToCommandString());
			var string2 = utf.GetBytes(String2);

			var totalLength = RconPacket.PacketSizeLength
				+ RconPacket.RequestIdLength
				+ RconPacket.CommandTypeLength
				+ command.Length
				+ RconPacket.TerminatorLength
				+ string2.Length
				+ RconPacket.TerminatorLength;

			var packetSize = BitConverter.GetBytes(totalLength - RconPacket.PacketSizeLength);
			var bytes = new byte[totalLength];
			var i = 0;

			packetSize.CopyTo(bytes, i);
			i += RconPacket.PacketSizeLength;

			id.CopyTo(bytes, i);
			i += RconPacket.RequestIdLength;

			commandType.CopyTo(bytes, i);
			i += RconPacket.CommandTypeLength;

			command.CopyTo(bytes, i);
			i += command.Length;

			bytes[i] = 0;
			i += RconPacket.TerminatorLength;

			string2.CopyTo(bytes, i);
			i += string2.Length;

			bytes[i] = 0;

			return bytes;
		}
	}
}