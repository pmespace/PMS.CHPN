using System.Runtime.InteropServices;
using System;

namespace CHPN.Exceptions
	{/// <summary>
	 /// Details a buffer size error (when copying,...)
	 /// </summary>
	[ComVisible(false)]
	class CInvalidDataSizeException: Exception
		{
		/// <summary>
		/// Create a buffer size exception
		/// </summary>
		/// <param name="data">Name of the data to which this exception applies</param>
		/// <param name="expectedSize">Expected data size</param>
		/// <param name="availableSize">Available data size</param>
		public CInvalidDataSizeException(string data, int expectedSize, int availableSize)
				: base("Invalid buffer size (data: " + data + ") - Expected: " + expectedSize + " - Usable: " + availableSize)
			{
			}
		}
	}
