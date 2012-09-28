using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Stajs.Rcon.Core;

namespace Stajs.Rcon.CommandLine
{
	class Program
	{
		static void Main(string[] args)
		{
			var ipAddress = ConfigurationManager.AppSettings["IpAddress"];
			var port = int.Parse(ConfigurationManager.AppSettings["Port"]);
			var password = ConfigurationManager.AppSettings["Password"];
			
			var rcon = new RconClient(ipAddress, port, password);
			rcon.Test("hai");
		}
	}
}