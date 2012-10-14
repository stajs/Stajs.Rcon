namespace Stajs.Rcon.Core.Commands
{
	public class UsersCommand : RconCommand
	{
		public UsersCommand() : base(ServerCommandType.Execute)
		{
		}

		internal override string ToCommandString()
		{
			return "users";
		}
	}
}