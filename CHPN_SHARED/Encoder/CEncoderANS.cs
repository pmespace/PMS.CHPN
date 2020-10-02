using System.Runtime.InteropServices;
using System.Reflection;
using System;
using COMMON;

namespace CHPN.Encoder
	{
	[ComVisible(false)]
	class CEncoderANS: CEncoder
		{
		#region properties
		/// <summary>
		/// 
		/// </summary>
		public override string CharacterSet
			{
			get => RegexCHPN.REGEX_ANS;
			}
		#endregion

		#region encoders/decodes
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public override byte[] Encode(string buffer, int minlen, int maxlen, out int length)
			{
			byte[] data = null;
			// encode buffer
			buffer = CompleteValue(buffer, minlen, maxlen, out length);
			if (!string.IsNullOrEmpty(buffer))
				try
					{
					if (IsValidFormat(buffer, minlen, maxlen))
						{
						bool Fixed = (minlen == maxlen);
						// determine len of field to create
						int lengthinbytes = buffer.Length;
						// arrived here everything's OK, we can create the ANX
						data = new byte[Fixed ? lengthinbytes : lengthinbytes + 1];
						int start = Fixed ? 0 : 1;
						int counter = 0;
						foreach (char c in buffer)
							{
							data[start + counter] = (byte)c;
							counter++;
							}
						// add field length if not fixed
						if (!Fixed)
							data[0] = (byte)counter;
						}
					}
				catch (Exception ex)
					{
					CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
					data = null;
					}
			return data;
			}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="offset"></param>
		/// <param name="rawdata"></param>
		/// <returns></returns>
		public override byte[] Decode(byte[] buffer, int start, int minlen, int maxlen, out int offset, out byte[] rawdata)
			{
			rawdata = null;
			bool fOK;
			offset = start;
			int len = minlen;
			byte[] ret = null;
			bool Fixed = (minlen == maxlen);
			if (null != buffer)
				{
				// do we still have enough space to get the DCB variable length
				if (!Fixed && (fOK = buffer.Length - offset >= 1))
					{
					// length of data to decode
					len = (int)buffer[offset];
					offset++;
					}
				else
					// there must be no error if the field is fixed
					fOK = Fixed;
				if (fOK = (fOK && buffer.Length - offset >= len))
					{
					ret = new byte[len];
					// start decoding the field
					for (int i = 0; len > i; i++)
						ret[i] += buffer[i + offset];
					offset += len;
					rawdata = new byte[offset - start];
					Buffer.BlockCopy(buffer, start, rawdata, 0, rawdata.Length);
					}
				}
			return ret;
			}
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public override string CompleteValue(string value, int minlen, int maxlen, out int length)
			{
			if (minlen > value.Length)
				value = value + new string(DefaultChar, minlen - value.Length);
			length = value.Length;
			return value;
			}
		/// <summary>
		/// <see cref="IEncoder.ToString(byte[])"/>
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public override string ToString(byte[] buffer)
			{
			string res = string.Empty;
			try
				{
				foreach (byte b in buffer)
					{
					char c = (char)b;
					res += c;//					b.ToString("G");
					}
				}
			catch (Exception ex)
				{
				res = string.Empty;
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
				}
			return res;
			}
		#endregion
		}
	}
