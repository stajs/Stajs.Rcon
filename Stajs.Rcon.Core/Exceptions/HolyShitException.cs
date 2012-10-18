using System;

namespace Stajs.Rcon.Core.Exceptions
{
	public class HolyShitException : Exception
	{
		public new string Message { get; private set; }

		public HolyShitException(string message)
		{
			Message = message;
		}
	}
}