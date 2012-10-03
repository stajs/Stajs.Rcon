namespace Stajs.Rcon.Core.Commands
{
	internal class StatusCommand : RconCommand
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