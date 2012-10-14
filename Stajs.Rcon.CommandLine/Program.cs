﻿using System;
using System.Configuration;
using Stajs.Rcon.Core;
using Stajs.Rcon.Core.Commands;

namespace Stajs.Rcon.CommandLine
{
	class Program
	{
		static void Main(string[] args)
		{
			var ipAddress = ConfigurationManager.AppSettings["IpAddress"];
			var port = int.Parse(ConfigurationManager.AppSettings["Port"]);
			var password = ConfigurationManager.AppSettings["Password"];

			Console.WriteLine("Connecting to {0}:{1}", ipAddress, port);
			var rcon = new RconClient(ipAddress, port);

			Console.WriteLine("Connected, authenticating with password \"{0}\"...", password);
			var requestId = rcon.Send(new AuthenticateCommand(password));

			Console.WriteLine("requestId: {0}", requestId);

			Console.ReadKey();
			//rcon.Test();
		}
	}
}