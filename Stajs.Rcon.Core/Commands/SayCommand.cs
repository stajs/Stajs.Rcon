﻿namespace Stajs.Rcon.Core.Commands
{
	public class SayCommand : RconCommand
	{
		private readonly string _message;

		public SayCommand(string message) : base(ServerCommandType.Execute)
		{
			_message = message;
		}

		internal override string ToCommandString()
		{
			return "say " + _message;
		}
	}
}