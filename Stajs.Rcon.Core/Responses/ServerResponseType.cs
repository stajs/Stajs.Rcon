using Stajs.Rcon.Core.Commands;

namespace Stajs.Rcon.Core.Responses
{
	public enum ServerResponseType
	{
		/// <summary>
		/// SERVERDATA_AUTH_RESPONSE
		/// Response to a <see cref="ServerCommandType.Authenticate"/> command, or to a <see cref="ServerCommandType.Execute"/> command if the connection is not authenticated.
		/// </summary>
		Auth = 2,

		/// <summary>
		/// SERVERDATA_RESPONSE_VALUE
		/// Response to a <see cref="ServerCommandType.Execute"/> if the connection is authenticated.
		/// </summary>
		Value = 0
	}
}