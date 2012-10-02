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
	public enum ResponseType
	{
		None = -1,

		/// <summary>
		/// SERVERDATA_AUTH_RESPONSE
		/// Response to a <see cref="CommandType.Authenticate"/> command, or to a <see cref="CommandType.Execute"/> command if the connection is not authenticated.
		/// </summary>
		Auth = 2,

		/// <summary>
		/// SERVERDATA_RESPONSE_VALUE
		/// Response to a <see cref="CommandType.Execute"/> if the connection is authenticated.
		/// </summary>
		Value = 0
	}
}