namespace Stajs.Rcon.Core.Commands
{
	internal class RawCommand : RconCommand
	{
		private readonly string _command;

		public RawCommand(string command) : base(ServerCommandType.Execute)
		{
			_command = command;
		}

		internal override string ToCommandString()
		{
			return _command;
		}
	}
}