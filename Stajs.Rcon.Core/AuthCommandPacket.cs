using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stajs.Rcon.Core
{
	internal class AuthCommandPacket : CommandPacket
	{
		public AuthCommandPacket(string password) : base(CommandType.Auth, password)
		{
			
		}
	}
}
