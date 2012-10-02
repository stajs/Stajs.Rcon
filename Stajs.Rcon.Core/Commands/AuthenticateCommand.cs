namespace Stajs.Rcon.Core.Commands
{
	internal class AuthenticateCommand : Command
	{
		private readonly string _password;

		public AuthenticateCommand(string password) : base(ServerCommand.Authenticate)
		{
			_password = password;
		}

		internal override string ToCommandString()
		{
			return _password;
		}
	}
}