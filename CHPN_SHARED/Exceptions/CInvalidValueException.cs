using System.Runtime.InteropServices;
using System;

namespace CHPN.Exceptions
	{/// <summary>
	 /// Details a value error
	 /// </summary>
	[ComVisible(false)]
	class CInvalidValueException: Exception
		{
		/// <summary>
		/// Creates a value error exception
		/// </summary>
		/// <param name="data">Message to display</param>
		/// <param name="value">Invalid value which caused the exception</param>
		public CInvalidValueException(string data, string value)
			: base("Invalid value for data: " + data + " - Value: " + value)
			{
			}
		}
	}
