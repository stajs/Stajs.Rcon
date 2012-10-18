using System.Text;
using Stajs.Rcon.Core.Exceptions;
using Stajs.Rcon.Core.Extensions;

namespace Stajs.Rcon.Core.Packets
{
	/* Note: My interpretation of the packet structure is slightly simplified when compared to
	 * the official (but community written) wiki. Based on the limited number of packets I've
	 * seen so far, I have not seen String2 being used at all and it seems to me that the
	 * Content is just double-null terminated. I'm willing to re-instate String2 should it
	 * turn out to actually be used.
	 * 
	 * https://developer.valvesoftware.com/wiki/Source_RCON_Protocol
	 * 
	 *             +---------- Packet size -----------+
	 *             v                                  v
	 * +-----------+----------+----+-------+----------+
	 * |Packet size|Request Id|Type|Content|Terminator|
	 * +-----------+----------+----+-------+----------+
	 * 
	 * Packet size (int = 4 bytes)
	 *		Size of packet from the start of the Request Id to the end of the Terminator.
	 *	
	 * Request Id (int = 4 bytes)
	 *		The Request Id is used to match commands and responses.
	 *		It should be incremented for each request.
	 *		If the Request Id is mirrored from the Request, then the Response is successful.
	 *		If the Request Id is -1, then authentication failed with a bad password.
	 *		Any other Request Id is an an error and an auth command should be sent before re-trying.
	 *		
	 * Type (int = 4 bytes)
	 *		Requests
	 *			Either SERVERDATA_AUTH (3) or SERVERDATA_EXECCOMMAND (2)
	 *		Responses
	 *			Either SERVERDATA_RESPONSE_VALUE (0) or SERVERDATA_AUTH_RESPONSE (2).
	 *			SERVERDATA_AUTH_RESPONSE is sent in response to a SERVERDATA_AUTH command, or to a SERVERDATA_EXECCOMMAND
	 *			command when the connection is not successfully authenticated.
	 *		
	 * Content (variable length, up to 4096 characters)
	 *		Known as "string1" in the docs.
	 *		Requests
	 *			For SERVERDATA_AUTH, this will be the password to authenticate with.
	 *			For SERVERDATA_EXECCOMMAND, this will be the command to execute (e.g. "status" or "say hello world").
	 *		Responses
	 *			The reponse to the command.
	 *			At most 4096 characters, so a single Command may result in multiple packets for the response. Packets seem to be
	 *			cut at the last newline that will fit inside the 4096 character limit.
	 *			If the Response type is SERVERDATA_AUTH_RESPONSE the Response will be null.
	 *		
	 * Terminator (2 x null = 2 bytes)
	 *		Used to terminate the packet
	 */

	public class RconPacket
	{
		public const int PacketSizeLength = 4;
		public const int RequestIdLength = 4;
		public const int TypeLength = 4;
		public const int TerminatorLength = 2;

		public byte[] Bytes { get; internal set; }

		public int Size { get; internal set; }
		public int RequestId { get; internal set; }
		


		public string Content { get; internal set; }
	}
}