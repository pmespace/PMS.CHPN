using System.Runtime.InteropServices;
using System;
using System.Reflection;
using COMMON;

namespace CHPN.Encoder
	{
	[ComVisible(false)]
	class CEncoderDCB: CEncoder
		{
		#region properties
		/// <summary>
		/// 
		/// </summary>
		public override string CharacterSet
			{
			get => RegexCHPN.REGEX_N;
			}
		/// <summary>
		/// 
		/// </summary>
		public override char DefaultChar
			{
			get => '0';
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
			byte[] raw = null;
			buffer = buffer.Trim();
			buffer = CompleteValue(buffer, minlen, maxlen, out length);
			if (!string.IsNullOrEmpty(buffer))
				try
					{
					if (IsValidFormat(buffer, minlen, maxlen))
						{
						bool fPrefixAdded;
						// align buffer on an unodd boundary
						if (fPrefixAdded = (0 != buffer.Length % 2))
							// add a "0" at the beginning if necessay (odd length)
							buffer = "0" + buffer;
						// is the field of fixed lengt ?
						bool Fixed = (minlen == maxlen);
						// determine len of returned buffer in bytes
						int lengthinbytes = buffer.Length / 2 + (Fixed ? 0 : 1);
						// now create the DCB
						raw = new byte[lengthinbytes];
						int offset = (Fixed ? 0 : 1);
						int counter = 0;
						foreach (char c in buffer)
							{
							int v;
							// store the binary representation of the digit
							if ('0' <= c && c <= '9')
								v = (int)c - (int)'0';
							else
								v = (int)c - (int)'A' + 10;
							if (0 == counter % 2)
								raw[offset + counter / 2] = (byte)((v << 4) & 0xFF);
							else
								raw[offset + counter / 2] = (byte)(v | (int)raw[offset + counter / 2]);
							counter++;
							}
						// add field length if not fixed
						if (!Fixed)
							raw[0] = (byte)(fPrefixAdded ? counter - 1 : counter);
						}
					}
				catch (Exception ex)
					{
					CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
					raw = null;
					}
			return raw;
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
					// length of raw data to decode
					len = (int)buffer[offset];
					offset++;
					}
				else
					// there must be no error if the field is fixed
					fOK = Fixed;
				// only if we could retrieve the length can we carry on
				if (fOK)
					{
					// determine buffer's length in bytes
					bool fPrefixAdded = 0 != len % 2;
					int lengthinbytes = (fPrefixAdded ? len / 2 + 1 : len / 2);
					// check whether there's enough space for the raw data
					if (fOK = (buffer.Length - offset >= lengthinbytes))
						{
						byte[] retex = new byte[len + (fPrefixAdded ? 1 : 0)];
						// arrived here everything's OK, we can decode the DCB
						int counter = 0;
						for (int i = offset; i - offset < lengthinbytes; i++, counter += 2)
							{
							// transform each half byte in a byte
							retex[counter] = (byte)(buffer[i] >> 4);
							retex[counter + 1] = (byte)(buffer[i] & 0x0F);
							}
						ret = new byte[len];
						// if an additional char begins the string remove it
						Buffer.BlockCopy(retex, (fPrefixAdded ? 1 : 0), ret, 0, len);
						// set the offset to the next position after the decodes raw
						offset += lengthinbytes;
						rawdata = new byte[offset - start];
						Buffer.BlockCopy(buffer, start, rawdata, 0, rawdata.Length);
						}
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
			value = value.Trim();
			if (minlen > value.Length)
				value = new string(DefaultChar, minlen - value.Length) + value;
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
					if (b <= 9)
						res += b.ToString("G");
					else
						res += b.ToString("X");
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
