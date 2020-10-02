using System.Runtime.InteropServices;
using System.Reflection;
using System;
using COMMON;

namespace CHPN.IPDU
	{
	[ComVisible(false)]
	static class CPIStream
		{
		#region methods
		/// <summary>
		/// Extract a PI from the given position in a buffer and returns the position of next PI (eventually) inside this buffer.
		/// Throws exceptions if the PI to extract is badly formated (size doesn't match PI,...)
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <param name="offset"></param>
		/// <returns>A PI object if found, null otherwise</returns>
		public static CPI ExtractPI(byte[] buffer, int start, out int offset)
			{
			// offset is at least the starting position
			offset = start;
			CPI cpi = null;
			// do we have anough space for a PGI+LGI
			if (CPI.PI_HEADER_SIZE <= buffer.Length - offset)
				{
				// we do... try to create a PI from the buffer
				try
					{
					cpi = new CPI(buffer[offset], buffer[offset + CPI.PI_PI_SIZE]);
					offset += CPI.PI_HEADER_SIZE;
					cpi.SetDataBytes(buffer, offset);
					// arrived here the PI has been created
					offset += cpi.PILen;
					}
				catch (Exception ex)
					{
					CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
					cpi = null;
					}
				}
			return cpi;
			}
		/// <summary>
		/// Search a specific PI and fetch it if found.
		/// Throws exceptions if the PI to fetch is badly formated (size doesn't match PI,...)
		/// </summary>
		/// <param name="pi"></param>
		/// <param name="buffer"></param>
		/// <returns>A PI object if found, null otherwise</returns>
		public static CPI SearchPI(byte pi, byte[] buffer)
			{
			CPI cpi = null;
			int offset = 0;
			bool fFound = false;
			do
				{
				int start = offset;
				// extract the current PI
				cpi = ExtractPI(buffer, start, out offset);
				if (null != cpi)
					{
					// a PI was found, is it the one we're looking for ?
					fFound = cpi.PI == pi;
					if (!fFound)
						cpi = null;
					}
				}
			while (!fFound && buffer.Length > offset);
			return cpi;
			}
		#endregion
		}
	}
