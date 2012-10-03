using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Stajs.Rcon.Core.Extensions;

namespace Stajs.Rcon.Core.Responses
{
	internal class RconResponse
	{
		public int RequestId { get; private set; }
		public ServerResponseType ServerResponseType { get; private set; }
		public string Response { get; private set; }
		public string String2 { get; private set; }

		public RconResponse(byte[] packet)
		{
			Parse(packet);
		}

		private void Parse(byte[] packet)
		{
			/* https://developer.valvesoftware.com/wiki/Source_RCON_Protocol
			 * 
			 *             +-------------------------------- Packet size -----------------------------------+
			 *             v                                                                                v
			 * +-----------+----------+-------------+--------+-------------------+-------+------------------+
			 * |Packet size|Request Id|Response type|Response|Response terminator|String2|String2 terminator|
			 * +-----------+----------+-------------+--------+-------------------+-------+------------------+
			 * 
			 * Packet size (int = 4 bytes)
			 *		Size of packet from the start of the Request Id to the end of the String2 terminator.
			 *		Apparently the response can span multiple packets (will wait and see)
			 *	
			 * Request Id (int = 4 bytes)
			 *		If the Request Id is -1 (0xffffffff), then authentication failed with a bad password.
			 *		If the Request Id is mirrored from the Request, then authentication is successful.
			 *		Any other Request Id is an an error and an auth Command should be sent before retrying.
			 *		
			 * Response type (int = 4 bytes)
			 *		Either SERVERDATA_RESPONSE_VALUE (0) or SERVERDATA_AUTH_RESPONSE (2).
			 *		SERVERDATA_AUTH_RESPONSE is sent in response to a SERVERDATA_AUTH command, or to a SERVERDATA_EXECCOMMAND
			 *		command when the connection is not successfully authenticated.
			 *		
			 * Response (variable length, up to 4096 characters)
			 *		The Reponse to the Command.
			 *		At most 4096 characters, so a single Command may result in Response packets. Not sure how to tie them together yet.
			 *		If the Response type is SERVERDATA_AUTH_RESPONSE the Response will be null.
			 *		
			 * Response terminator (null = 1 byte)
			 *		Used to terminate the Command
			 *		
			 * String2 (variable length)
			 *		NFI WTH this is for ATM. It's empty from what I can see so far. Guess I'll find out more as I go...
			 *		If the Response type is SERVERDATA_AUTH_RESPONSE the Response will be null.
			 *		
			 * String2 terminator (null = 1 byte)
			 *		Used to terminate String2
			 */

			// TODO: Exceptions
			// TODO: store packet?

			RequestId = ParseInt(packet);
			packet = packet.RemoveFromStart(RconPacket.RequestIdLength);

			ServerResponseType = (ServerResponseType) ParseInt(packet);
			packet = packet.RemoveFromStart(RconPacket.ResponseTypeLength);

			var strings = Encoding.UTF8.GetString(packet)
				.Substring(0, packet.Length - 1) // Remove trailing '\0'
				.Split('\0');

			Response = strings.First();
			String2 = strings.Last();
		}

		private int ParseInt(byte[] bytes)
		{
			return BitConverter.ToInt32(bytes, 0);
		}
	}
}