using System;
using System.Text;
using Stajs.Rcon.Core.Packets;

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

		public RconCommandPacket ToPacket(int requestId)
		{
			RequestId = requestId;
			return new RconCommandPacket(this);
		}
	}
}