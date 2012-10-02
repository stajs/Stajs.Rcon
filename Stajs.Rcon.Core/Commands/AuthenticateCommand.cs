namespace Stajs.Rcon.Core.Commands
{
	internal class AuthenticateCommand : ICommand
	{
		private readonly string _password;

		public AuthenticateCommand(string password)
		{
			_password = password;
		}

		public string ToCommandString()
		{
			return _password;
		}
	}
}