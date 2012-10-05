using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stajs.Rcon.Core.Extensions
{
	internal static class IntExtensions
	{
		internal static ServerResponseType ToResponseType(this int i)
		{
			if (!Enum.IsDefined(typeof(ServerResponseType), i))
				throw new InvalidCastException("Value is not defined in enum");

			return (ServerResponseType) i;
		}
	}
}