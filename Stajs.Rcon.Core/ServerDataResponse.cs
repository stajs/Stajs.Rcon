using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Stajs.Rcon.Core
{
	public enum ServerDataResponse
	{
		None = -1,
		Auth = 2,
		Value = 0
	}
}