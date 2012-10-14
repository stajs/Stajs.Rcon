using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Stajs.Rcon.Core.Exceptions;

namespace Stajs.Rcon.Core.Responses
{
	public class RconResponse
	{
		public int RequestId { get; private set; }
		public ServerResponseType ResponseType { get; private set; }
		public List<RconPacket> Packets { get; private set; }
		public string Content { get; private set; }

		public RconResponse(List<RconPacket> packets)
		{
			// Ensure packets are all the same RequestId
			RequestId = packets
				.Select(p => p.RequestId)
				.Distinct()
				.Single();

			Packets = packets;
			ResponseType = GetResponseType();
			Content = GetContent();
			
			Debug.Print("  +  RequestId: " + RequestId);
			Debug.Print("  +  ResponseType: " + ResponseType);
			Debug.Print("  +  Content:\n" + Content);
			Debug.Print("  +  packets.Count: " + packets.Count);
			Debug.Print("----------------------------------------------");
		}

		private ServerResponseType GetResponseType()
		{
			if (Packets.All(p => p.ResponseType == ServerResponseType.Value))
				return ServerResponseType.Value;

			// A successful authentication response is a Value packet, followed by an Auth packet
			if (Packets.Count == 2)
				if (Packets[0].ResponseType == ServerResponseType.Value && Packets[1].ResponseType == ServerResponseType.Auth)
					return ServerResponseType.Auth;
			
			throw new HolyShitException("I'm not sure what these reponse types are...");
		}

		private string GetContent()
		{
			var sb = new StringBuilder();

			foreach (var packet in Packets)
				sb.Append(packet.Response);

			return sb.ToString();
		}
	}
}