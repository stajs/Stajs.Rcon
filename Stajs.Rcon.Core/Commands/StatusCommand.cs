namespace Stajs.Rcon.Core.Commands
{
	public class StatusCommand : RconCommand, ISuggestion
	{
		internal override string CommandBase
		{
			get { return "status"; }
		}

		public StatusCommand() : base(ServerCommandType.Execute)
		{
		}

		internal override string ToCommandString()
		{
			return CommandBase;
		}

		public string ToSuggestionString()
		{
			return CommandBase;
		}
	}
}