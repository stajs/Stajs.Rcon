namespace Stajs.Rcon.Core.Commands
{
	internal class UsersCommand : RconCommand
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