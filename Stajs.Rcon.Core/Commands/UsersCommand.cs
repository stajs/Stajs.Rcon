namespace Stajs.Rcon.Core.Commands
{
	public class UsersCommand : RconCommand, ISuggestion
	{
		internal override string CommandBase
		{
			get { return "users"; }
		}

		public UsersCommand() : base(ServerCommandType.Execute)
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