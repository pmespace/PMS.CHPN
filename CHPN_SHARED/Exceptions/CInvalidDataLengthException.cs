using System.Runtime.InteropServices;
using System;

namespace CHPN.Exceptions
	{
	/// <summary>
	/// Exception detailing a length error
	/// </summary>
	[ComVisible(false)]
	class CInvalidDataLengthException: Exception
		{
		/// <summary>
		/// Creates a length error exception
		/// </summary>
		/// <param name="data">Name of the data in error</param>
		/// <param name="len">Actual length of data in error</param>
		/// <param name="minlen">Minimum length for that data. If missing or set to 0 it won't be used</param>
		/// <param name="maxlen">Minimum length for that data. If missing or set to 0 it won't be used</param>
		public CInvalidDataLengthException(string data, int len, int minlen = 0, int maxlen = 0)
				: base("Invalid data (data name: " + data + ") - Actual length: " + len + (0 != minlen || 0 != maxlen ? Environment.NewLine + (0 != minlen ? "Minimum length is: " + minlen + "; " : string.Empty) + (0 != maxlen ? " - Maximum length is: " + maxlen + "; " : string.Empty) : string.Empty))
			{
			}
		}
	}
