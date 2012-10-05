using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stajs.Rcon.Core.Extensions;

namespace Stajs.Rcon.Core
{
	internal class RconPacket
	{
		public static readonly int PacketSizeLength = 4;
		public static readonly int RequestIdLength = 4;
		public static readonly int CommandTypeLength = 4;
		public static readonly int ResponseTypeLength = 4;
		public static readonly int TerminatorLength = 1;

		public byte[] Bytes { get; private set; }

		public int Size { get; private set; }
		public int RequestId { get; private set; }
		public ServerResponseType ResponseType { get; private set; }
		public string Response { get; private set; }
		public string String2 { get; private set; }

		public RconPacket(byte[] bytes)
		{
			Bytes = bytes;

			Parse(bytes);
		}

		private void Parse(byte[] bytes)
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
			 *		The Reponse to the Command. Known as "string1" in the docs.
			 *		At most 4096 characters, so a single Command may result in Response packets. Not sure how to tie them together yet.
			 *		If the Response type is SERVERDATA_AUTH_RESPONSE the Response will be null.
			 *		
			 * Response terminator (null = 1 byte)
			 *		Used to terminate the Command
			 *		
			 * String2 (variable length)
			 *		NFI WTH this is for ATM. It's empty from what I can see so far. Guess I'll find out more as I go...
			 *		I'm not convinced this is actually used yet. From what I have seen it looks like "string1" is double-null terminated...
			 *		
			 * String2 terminator (null = 1 byte)
			 *		Used to terminate String2
			 */

			const int intSize = 4; // 4 bytes

			Size = bytes.ToInt();
			bytes = bytes.RemoveFromStart(intSize);

			RequestId = bytes.ToInt();
			bytes = bytes.RemoveFromStart(intSize);

			ResponseType = bytes.ToInt().ToResponseType();
			bytes = bytes.RemoveFromStart(intSize);

			var strings = Encoding.UTF8.GetString(bytes)
				.Substring(0, bytes.Length - 1) // Remove trailing '\0'
				.Split('\0');

			Response = strings.First();
			String2 = strings.Last();
		}
	}
}