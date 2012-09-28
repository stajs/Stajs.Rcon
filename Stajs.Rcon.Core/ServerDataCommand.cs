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
	public enum ServerDataCommand
	{
		None = -1,
		Auth = 3,
		Exec = 2
	}
}