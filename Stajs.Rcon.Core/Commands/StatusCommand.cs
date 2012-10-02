namespace Stajs.Rcon.Core.Commands
{
	internal class StatusCommand : Command
	{
		public StatusCommand() : base(ServerCommand.Execute)
		{
		}

		internal override string ToCommandString()
		{
			return "status";
		}
	}
}