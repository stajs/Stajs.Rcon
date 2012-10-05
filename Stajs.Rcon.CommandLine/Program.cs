using System.Configuration;
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
			rcon.Test();
		}
	}
}