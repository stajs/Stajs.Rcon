﻿using System;
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

		/// <summary>
		/// SERVERDATA_AUTH_RESPONSE
		/// Response to a <see cref="ServerDataCommand.Auth"/> command, or to a <see cref="ServerDataCommand.Exec"/> command if the connection is not authenticated.
		/// </summary>
		Auth = 2,

		/// <summary>
		/// SERVERDATA_RESPONSE_VALUE
		/// Response to a <see cref="ServerDataCommand.Exec"/> if the connection is authenticated.
		/// </summary>
		Value = 0
	}
}