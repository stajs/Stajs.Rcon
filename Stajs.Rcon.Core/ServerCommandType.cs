namespace Stajs.Rcon.Core
{
	public enum ServerCommandType
	{
		/// <summary>
		/// SERVERDATA_AUTH
		/// Generally the first command sent to authenticate a connection.
		/// </summary>
		Authenticate = 3,

		/// <summary>
		/// SERVERDATA_EXECCOMMAND
		/// The RCON command to execute.
		/// </summary>
		Execute = 2
	}
}