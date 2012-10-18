using System;
using System.Text;

namespace Stajs.Rcon.Core.Commands
{
	public abstract class RconCommand
	{
		public int? RequestId { get; private set; }
		public ServerCommandType CommandType { get; private set; }
		public string Command { get { return ToCommandString(); } }

		protected RconCommand(ServerCommandType commandType)
		{
			CommandType = commandType;
		}

		internal abstract string ToCommandString();

		internal byte[] GetBytes(int requestId)
		{
			/* Note: My interpretation of the packet structure is slightly simplified when compared to
			 * the official (but community written) wiki. Based on the limited number of packets I've
			 * seen so far, I have not seen String2 being used at all and it seems to me that the
			 * Response is just double-null terminated. I'm willing to re-instate String2 should it
			 * turn out to actually be used.
			 * 
			 * https://developer.valvesoftware.com/wiki/Source_RCON_Protocol
			 * 
			 *             +---------- Packet size -----------+
			 *             v                                  v
			 * +-----------+----------+----+-------+----------+
			 * |Packet size|Request Id|Type|Command|Terminator|
			 * +-----------+----------+----+-------+----------+
			 * 
			 * Packet size (int = 4 bytes)
			 *		Size of packet from the start of the Request Id to the end of the Terminator.
			 *	
			 * Request Id (int = 4 bytes)
			 *		If the Request Id is mirrored from the Request, then the response is successful.
			 *		If the Request Id is -1, then authentication failed with a bad password.
			 *		Any other Request Id is an an error and an auth Command should be sent before retrying.
			 *		It should be incremented for each request.
			 *		
			 * Command type (int = 4 bytes)
			 *		Either SERVERDATA_AUTH (3) or SERVERDATA_EXECCOMMAND (2)
			 *		
			 * Command (variable length, up to 4096 characters)
			 *		Known as "string1" in the docs. For SERVERDATA_AUTH, this will be the password to authenticate
			 *		with. For SERVERDATA_EXECCOMMAND, this will be the command to execute (e.g. "status" or "say hello world").
			 *		
			 * Command terminator (null = 1 byte)
			 *		Used to terminate the Command
			 *		
			 * Terminator (2 x null = 2 bytes)
			 *		Used to terminate the packet
			 */

			RequestId = requestId;

			var utf = new UTF8Encoding();

			var id = BitConverter.GetBytes(RequestId.Value);
			var commandType = BitConverter.GetBytes((int)CommandType);
			var command = utf.GetBytes(ToCommandString());

			var totalLength = RconPacket.PacketSizeLength
				+ RconPacket.RequestIdLength
				+ RconPacket.CommandTypeLength
				+ command.Length
				+ RconPacket.DoubleTerminatorLength;

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
			i++;

			bytes[i] = 0;

			return bytes;
		}
	}
}