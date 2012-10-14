namespace Stajs.Rcon.Core.Commands
{
	public class AuthenticateCommand : RconCommand
	{
		private readonly string _password;

		public AuthenticateCommand(string password) : base(ServerCommandType.Authenticate)
		{
			_password = password;
		}

		internal override string ToCommandString()
		{
			return _password;
		}
	}
}