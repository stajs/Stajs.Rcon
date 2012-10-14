using System;
using System.Configuration;
using Stajs.Rcon.Core;
using Stajs.Rcon.Core.Commands;
using Stajs.Rcon.Core.Responses;

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

			Console.WriteLine("Authenticate");
			var response = rcon.Send(new AuthenticateCommand(password));
			FormatResponse(response);

			Console.WriteLine("Users");
			response = rcon.Send(new UsersCommand());
			FormatResponse(response);

			Console.WriteLine("Say");
			response = rcon.Send(new SayCommand("Hai!"));
			FormatResponse(response);

			Console.WriteLine("Status");
			response = rcon.Send(new StatusCommand());
			FormatResponse(response);

			Console.ReadKey();
			//rcon.Test();
		}

		private static void FormatResponse(RconResponse response)
		{
			Console.ForegroundColor = ConsoleColor.Cyan;
			Console.WriteLine(response.Content);
			Console.ForegroundColor = ConsoleColor.Gray;
		}
	}
}