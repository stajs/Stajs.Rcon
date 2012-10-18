using System.Linq;
using System.Text;
using Stajs.Rcon.Core.Exceptions;
using Stajs.Rcon.Core.Extensions;

namespace Stajs.Rcon.Core
{
	public class RconPacket
	{
		public static readonly int PacketSizeLength = 4;
		public static readonly int RequestIdLength = 4;
		public static readonly int CommandTypeLength = 4;
		public static readonly int ResponseTypeLength = 4;
		public static readonly int TerminatorLength = 1;
		public static readonly int DoubleTerminatorLength = 2;

		public byte[] Bytes { get; private set; }

		public int Size { get; private set; }
		public int RequestId { get; private set; }
		public ServerResponseType ResponseType { get; private set; }
		public string Response { get; private set; }
		public string String2 { get; private set; }

		public bool IsEndResponsePacket
		{
			get { return Response.Trim() == "END"; }
		}

		public RconPacket(byte[] bytes)
		{
			Bytes = bytes;

			Parse(bytes);
		}

		private void Parse(byte[] bytes)
		{
			/* Note: My interpretation of the packet structure is slightly simplified when compared to
			 * the official (but community written) wiki. Based on the limited number of packets I've
			 * seen so far, I have not seen String2 being used at all and it seems to me that the
			 * Response is just double-null terminated. I'm willing to re-instate String2 should it
			 * turn out to actually be used.
			 * 
			 * https://developer.valvesoftware.com/wiki/Source_RCON_Protocol
			 * 
			 *             +----------- Packet size -----------+
			 *             v                                   v
			 * +-----------+----------+----+--------+----------+
			 * |Packet size|Request Id|Type|Response|Terminator|
			 * +-----------+----------+----+--------+----------+
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
			 * Response type (int = 4 bytes)
			 *		Either SERVERDATA_RESPONSE_VALUE (0) or SERVERDATA_AUTH_RESPONSE (2).
			 *		SERVERDATA_AUTH_RESPONSE is sent in response to a SERVERDATA_AUTH command, or to a SERVERDATA_EXECCOMMAND
			 *		command when the connection is not successfully authenticated.
			 *		
			 * Response (variable length, up to 4096 characters)
			 *		The Reponse to the Command. Known as "string1" in the docs.
			 *		At most 4096 characters, so a single Command may result in multiple packets for the response. Packets seem to be
			 *		cut at the last newline that will fit inside the 4096 character limit.
			 *		If the Response type is SERVERDATA_AUTH_RESPONSE the Response will be null.
			 *		
			 * Terminator (2 x null = 2 bytes)
			 *		Used to terminate the packet
			 */

			if (!bytes.IsDoubleNullTerminated())
				throw new HolyShitException("String2 is used after all!");

			Size = bytes.ToInt();
			bytes = bytes.RemoveFromStart(PacketSizeLength);

			RequestId = bytes.ToInt();
			bytes = bytes.RemoveFromStart(RequestIdLength);

			ResponseType = bytes.ToInt().ToResponseType();
			bytes = bytes.RemoveFromStart(ResponseTypeLength);

			Response = Encoding.UTF8.GetString(bytes.RemoveFromEnd(DoubleTerminatorLength));
		}
	}
}