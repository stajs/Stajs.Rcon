namespace Stajs.Rcon.Core.Commands
{
	public class SayCommand : RconCommand, ISuggestion
	{
		internal override string CommandBase
		{
			get { return "say"; }
		}

		private readonly string _message;

		public SayCommand() : this(string.Empty)
		{
			
		}

		public SayCommand(string message) : base(ServerCommandType.Execute)
		{
			_message = message;
		}

		internal override string ToCommandString()
		{
			return string.Format("{0} {1}", CommandBase, _message);
		}

		public string ToSuggestionString()
		{
			return CommandBase;
		}
	}
}