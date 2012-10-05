using System;
using System.Linq;

namespace Stajs.Rcon.Core.Extensions
{
	internal static class ArrayExtensions
	{
		internal static int ToInt(this byte[] bytes)
		{
			return BitConverter.ToInt32(bytes, 0);
		}

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
