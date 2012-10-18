using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stajs.Rcon.Core.Exceptions;
using Stajs.Rcon.Core.Extensions;

namespace Stajs.Rcon.Core.Packets
{
	public class RconResponsePacket : RconPacket
	{
		public ServerResponseType ResponseType { get; private set; }

		public bool IsEndResponsePacket
		{
			get { return Content.Trim() == "END"; }
		}

		public RconResponsePacket(byte[] bytes)
		{
			Bytes = bytes;

			Parse(bytes);
		}

		private void Parse(byte[] bytes)
		{
			if (!bytes.IsDoubleNullTerminated())
				throw new HolyShitException("String2 is used after all!");

			Size = bytes.ToInt();
			bytes = bytes.RemoveFromStart(PacketSizeLength);

			RequestId = bytes.ToInt();
			bytes = bytes.RemoveFromStart(RequestIdLength);

			ResponseType = bytes.ToInt().ToResponseType();
			bytes = bytes.RemoveFromStart(TypeLength);

			Content = Encoding.UTF8.GetString(bytes.RemoveFromEnd(TerminatorLength));
		}
	}
}
