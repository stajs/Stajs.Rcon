using System;

namespace Stajs.Rcon.Core
{
	internal class RconPacket
	{
		public ServerDataCommand ServerDataCommand { get; set; }
		public string String1 { get; set; }
		public string String2 { get; set; }

		public byte[] GetBytes()
		{
			throw new NotImplementedException();
		}
	}
}