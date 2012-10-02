namespace Stajs.Rcon.Core.Commands
{
	internal class SayCommand : Command
	{
		private readonly string _message;

		public SayCommand(string message) : base(ServerCommand.Execute)
		{
			_message = message;
		}

		internal override string ToCommandString()
		{
			return "say " + _message;
		}
	}
}