using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stajs.Rcon.Core.Extensions
{
	internal static class ArrayExtensions
	{
		internal static byte[] RemoveFromStart(this byte[] bytes, int count)
		{
			if (count > bytes.Length)
				return bytes;

			return bytes
				.Skip(count)
				.ToArray();
		}
	}
}
