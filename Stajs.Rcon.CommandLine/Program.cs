using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stajs.Rcon.CommandLine
{
	class Program
	{
		static void Main(string[] args)
		{
			const string ipAddress = "0.0.0.0";
			const int port = 27015;
			const string password = "blah";

			var rcon = new Core.Rcon(ipAddress, port, password);
			rcon.Connect();
		}
	}
}