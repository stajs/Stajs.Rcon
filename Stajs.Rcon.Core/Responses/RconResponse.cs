using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Stajs.Rcon.Core.Responses
{
	public class RconResponse
	{
		public int RequestId { get; private set; }
		public List<RconPacket> Packets { get; private set; }
		public string Content { get; private set; }

		public RconResponse(List<RconPacket> packets)
		{
			// This will throw an exception if more than one request id is found, which saves an explicit check
			RequestId = packets
				.Select(p => p.RequestId)
				.Distinct()
				.Single();

			Packets = packets;
			Content = GetContent();
			
			Debug.Print("  +  RequestId: " + RequestId);
			Debug.Print("  +  packets.Count: " + packets.Count);
			Debug.Print("  +  Content:\n" + Content);
			Debug.Print("----------------------------------------------");
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