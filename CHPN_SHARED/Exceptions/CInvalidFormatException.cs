using System.Runtime.InteropServices;
using System;

namespace CHPN.Exceptions
	{/// <summary>
	 /// Exception detailing a value error
	 /// </summary>
	[ComVisible(false)]
	class CInvalidFormatException: Exception
		{
		/// <summary>
		/// Creates a format error exception
		/// </summary>
		/// <param name="data">Name of the data in error</param>
		/// <param name="value">Value which is in error</param>
		/// <param name="format">Format which should apply. If missing or empty it won't be used</param>
		public CInvalidFormatException(string data, string value, string format)
				: base("Invalid format for data: " + data + " - Value: " + value + " - Format: " + format)
			{
			}
		}
	}
