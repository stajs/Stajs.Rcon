namespace Stajs.Rcon.Core.Commands
{
	internal class EndCommand : RconCommand
	{
		public EndCommand() : base(ServerCommandType.Execute)
		{
		}

		internal override string ToCommandString()
		{
			return "echo END";
		}
	}
}