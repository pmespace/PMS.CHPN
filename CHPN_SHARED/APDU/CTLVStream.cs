using System.Runtime.InteropServices;
using System.Reflection;
using System;
using CHPN.Exceptions;
using CHPN.Encoder;
using COMMON;

namespace CHPN.APDU
	{

	[ComVisible(false)]
	static class CTLVStream
		{
		#region methods
		/// <summary>
		/// Get the content of a TLV value part
		/// </summary>
		/// <param name="buffer">Buffer to extract the TLV from.
		/// This buffer must contain ONLY TLV fields as it will rely on the available siez to stop searching TLVs.
		/// If the buffer contains something else than TLV fields, the function won't notice it.</param>
		/// <param name="start">Start position inside the buffer to retrieve the TLV</param>
		/// <param name="offset">Offset after the TLV</param>
		public static CTLV ExtractTLV(byte[] buffer, int start, out int offset)
			{
			CTLV tlv = null;
			offset = start;
			if (buffer.Length - offset > CTLV.TLV_SIZE_T + CTLV.TLV_SIZE_L + 1)
				{
				byte[] rawdata;
				int mystart = start;
				CEncoderAN encoderAN = new CEncoderAN();
				// enough room for T+L+V (1 byte at least)

				//get T
				string T = (encoderAN.Decode(buffer, offset, CTLV.TLV_SIZE_T, CTLV.TLV_SIZE_T, out offset, out rawdata)).ToString();
				// get L
				short L = (short)CMisc.StrToLong(encoderAN.ToString(encoderAN.Decode(buffer, offset, CTLV.TLV_SIZE_L, CTLV.TLV_SIZE_L, out offset, out rawdata)));
				if (0 != tlv.L)
					if (buffer.Length - offset >= L)
						{
						byte[] V = new byte[L];
						// get V
						Buffer.BlockCopy(buffer, offset, V, 0, L);
						offset += tlv.L;
						tlv = new CTLV(T, L, FieldType.NONE);
						tlv.SetV(V);
						return tlv;
						}
					else
						throw new CInvalidDataSizeException("TAG " + T, L, buffer.Length - offset);
				else
					throw new CInvalidValueException("TAG " + T, L.ToString());
				}
			else
				throw new CInvalidDataSizeException("TLV", CTLV.TLV_SIZE_T + CTLV.TLV_SIZE_L + 1, buffer.Length - offset);
			}
		/// <summary>
		/// Search a specific TLC in a buffer
		/// </summary>
		/// <param name="tag">The TLV tag to find</param>
		/// <param name="buffer">The buffer to loolk the TLV inside</param>
		/// <returns>A CTLV object</returns>
		public static CTLV SearchTLV(string tag, byte[] buffer)
			{
			CTLV tlv = null;
			int offset = 0;
			bool fFound = false, fOK = true;
			do
				{
				int start = offset;
				try
					{
					// extract the current PI
					tlv = ExtractTLV(buffer, start, out offset);
					}
				catch (Exception ex)
					{
					CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
					fOK = false;
					tlv = null;
					}
				if (null != tlv)
					{
					// a PI was found, is it the one we're looking for ?
					fFound = tlv.T == tag;
					if (!fFound)
						tlv = null;
					}
				}
			while (!fFound && buffer.Length > offset && fOK);
			return tlv;
			}
		#endregion
		}
	}
