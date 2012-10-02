using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stajs.Rcon.Core.Commands;

namespace Stajs.Rcon.Core
{
	internal class AuthenticateCommandPacket : CommandPacket
	{
		public AuthenticateCommandPacket(string password) : base(CommandType.Authenticate, new AuthenticateCommand(password))
		{
			
		}
	}
}
