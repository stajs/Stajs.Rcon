using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stajs.Rcon.Core
{
	internal class RconPacket
	{
		internal static readonly int PacketSizeLength = 4;
		internal static readonly int RequestIdLength = 4;
		internal static readonly int CommandTypeLength = 4;
		internal static readonly int ResponseTypeLength = 4;
		internal static readonly int TerminatorLength = 1;
	}
}