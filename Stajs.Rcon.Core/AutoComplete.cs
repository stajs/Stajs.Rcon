using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Stajs.Rcon.Core.Commands;

namespace Stajs.Rcon.Core
{
	public static class AutoComplete
	{
		private static IEnumerable<string> _suggestions; 
		internal static IEnumerable<string> Suggestions
		{
			get
			{
				if (_suggestions == null)
				{
					var types = Assembly.GetExecutingAssembly()
						.GetTypes()
						.Where(t => typeof (ISuggestion).IsAssignableFrom(t) && !t.IsInterface)
						.Select(Activator.CreateInstance);

					_suggestions = types
						.Select(t => ((ISuggestion) t).ToSuggestionString())
						.OrderBy(s => s);
				}

				return _suggestions;
			}
		}

		public static IEnumerable<string> GetSuggestion(string s)
		{
			return Suggestions.Where(x => x.StartsWith(s));
		}
	}
}
