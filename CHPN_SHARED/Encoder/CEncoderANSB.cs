using System.Runtime.InteropServices;
using System;
using COMMON;

namespace CHPN.Encoder
	{
	[ComVisible(false)]
	class CEncoderANSB: CEncoder
		{
		#region constants
		private const int FIELD_LENGTH_SIZE = 2;
		#endregion

		#region properties
		/// <summary>
		/// 
		/// </summary>
		public override string CharacterSet
			{
			get => string.Empty;
			}
		#endregion

		#region encoders/decodes
		/// <summary>
		/// Encoding from a string is done from an hexadecimal representation of the buffer
		/// </summary>
		/// <param name="buffer">The buffer to encode, it must be in hexadecimal, each byte on 2 characters</param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public override byte[] Encode(string buffer, int minlen, int maxlen, out int length)
			{
			length = 0;
			byte[] raw = null;
			if (!string.IsNullOrEmpty(buffer))
				{
				// The string must have an unodd length (as every byte is represented by 2 characters)
				if (0 == (buffer.Length % 2))
					{
					// each pair of characters must be analysed to create an hexadecimal to binary representation
					raw = new byte[buffer.Length / 2];
					for (int i = 0; i < buffer.Length; i += 2)
						{
						string s = buffer.Substring(i, 2);
						raw[i / 2] = CMisc.TwoHexToBin(s);
						length = raw.Length;
						}
					}
				}
			return raw;
			}
		/// <summary>
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public override byte[] Encode(byte[] buffer, int minlen, int maxlen, out int length)
			{
			length = 0;
			byte[] raw = null;
			if (null != buffer)
				{
				int size = CMisc.LenToUse(buffer, ref minlen, ref maxlen);
				if (0 != size)
					{
					raw = new byte[size + FIELD_LENGTH_SIZE];
					Buffer.BlockCopy(buffer, 0, raw, FIELD_LENGTH_SIZE, size);
					Buffer.BlockCopy(CMisc.SetBytesFromIntegralTypeValue((short)buffer.Length), 0, raw, 0, FIELD_LENGTH_SIZE);
					length = raw.Length;
					}
				}
			return raw;
			}
		/// <summary>
		/// Decoding will be made in hexadecimal representation, 1 byte being giving way to 2 characters
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="offset"></param>
		/// <param name="rawdata"></param>
		/// <returns>An string in hexadecimal representation</returns>
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
				if (!Fixed && (fOK = buffer.Length - offset >= 2))
					{
					// length of data to decode
					len = (int)CMisc.GetIntegralTypeValueFromBytes(buffer, start, FIELD_LENGTH_SIZE);
					offset += 2;
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
		/// <see cref="IEncoder.ToString(byte[])"/>
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public override string ToString(byte[] buffer)
			{
			return CMisc.BytesToHexStr(buffer);
			}
		/// <summary>
		/// <see cref="IEncoder.ToLong(byte[])"/>
		/// </summary>
		/// <param name="buffer">The raw representation to decode</param>
		/// <returns>A string representing the content of the raw part</returns>	
		public override long ToLong(byte[] buffer)
			{
			return 0;
			}
		#endregion
		}
	}
