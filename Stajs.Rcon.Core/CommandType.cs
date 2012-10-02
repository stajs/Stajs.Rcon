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
	public enum CommandType
	{
		None = -1,

		/// <summary>
		/// SERVERDATA_AUTH
		/// Generally the first command sent to authenticate a connection.
		/// </summary>
		Authenticate = 3,

		/// <summary>
		/// SERVERDATA_EXECCOMMAND
		/// The RCON command to execute.
		/// </summary>
		Execute = 2
	}
}