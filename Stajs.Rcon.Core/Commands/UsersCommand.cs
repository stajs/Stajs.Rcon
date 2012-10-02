namespace Stajs.Rcon.Core.Commands
{
	internal class UsersCommand : Command
	{
		public UsersCommand() : base(ServerCommand.Execute)
		{
		}

		internal override string ToCommandString()
		{
			return "users";
		}
	}
}