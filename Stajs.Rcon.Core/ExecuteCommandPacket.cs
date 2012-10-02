using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stajs.Rcon.Core.Commands;

namespace Stajs.Rcon.Core
{
	internal class ExecuteCommandPacket : CommandPacket
	{
		public ExecuteCommandPacket(ICommand command) : base(CommandType.Execute, command)
		{
			
		}
	}
}