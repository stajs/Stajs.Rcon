namespace Stajs.Rcon.Core.Commands
{
	public class StatusCommand : RconCommand
	{
		public StatusCommand() : base(ServerCommandType.Execute)
		{
		}

		internal override string ToCommandString()
		{
			return "status";
		}
	}
}