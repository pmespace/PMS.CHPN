using System.Runtime.InteropServices;
using System.Reflection;
using System;
using CHPN.Exceptions;
using CHPN.Transcoder;
using CHPN.Encoder;
using COMMON;

namespace CHPN.APDU
	{
	[ComVisible(false)]
	class CTLV: CFieldGeneric
		{
		#region constants
		public const int TLV_SIZE_T = 2;
		public const int TLV_SIZE_L = 2;
		#endregion

		#region private data
		string _tag;
		#endregion

		#region properties
		/// <summary>
		/// TLV tag value
		/// </summary>
		public string T
			{
			get => _tag;
			private set
				{
				value = value.Trim().ToUpper();
				if (CMisc.IsValidFormat(value, RegexCHPN.REGEX_TLV))
					_tag = value;
				else
					_tag = string.Empty;
				}
			}
		/// <summary>
		/// TLV length value
		/// </summary>
		public short L
			{
			get;
			private set;
			}
		/// <summary>
		/// TLV value
		/// </summary>
		public byte[] V
			{
			get;
			private set;
			}
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		public override byte[] Rawdata
			{
			get
				{
				int length;
				IEncoder ans = new CEncoderANS();
				IEncoder ansb = new CEncoderANSB();
				// size of Rawdata
				byte[] ab = new byte[TLV_SIZE_T + TLV_SIZE_L + L];
				int offset = 0;
				// add tag
				byte[] b = ans.Encode(T, TLV_SIZE_T, TLV_SIZE_T, out length);
				Buffer.BlockCopy(b, 0, ab, offset, TLV_SIZE_T);
				offset += TLV_SIZE_T;
				// len
				b = ans.Encode(L.ToString(), TLV_SIZE_L, TLV_SIZE_L, out length);
				Buffer.BlockCopy(b, 0, ab, offset, TLV_SIZE_L);
				offset += TLV_SIZE_L;
				// value
				Buffer.BlockCopy(V, 0, ab, offset, L);
				return ab;
				}
			}
		#endregion

		#region constructors
		/// <summary>
		/// Creates a TLV
		/// </summary>
		/// <param name="tag">T</param>
		/// <param name="len">L</param>
		/// <param name="type">The generic field to use to create the TLV</param>
		public CTLV(string tag, short len, FieldType type)
				: base(type)
			{
			T = tag;
			L = len;
			V = new byte[L];
			}
		#endregion

		#region methods
		/// <summary>
		/// Get the default alphabet
		/// </summary>
		/// <returns>The default alphabet</returns>
		public override SAlphabet DefaultAlphabet()
			{
			return new SAlphabet(SAlphabet.ASCII);
			}
		/// <summary>
		/// Set the TLV value part from a string
		/// Throws exceptions if the value length doesn't match the created TLV
		/// </summary>
		/// <param name="value">The string to use to set the value part</param>
		public void SetV(string value)
			{
			if (value.Length != L)
				throw new CInvalidDataLengthException("tag " + T, value.Length, L, L);
			byte[] bb = DefaultEncoder().Encode(value, L, L, out int length);
			Buffer.BlockCopy(Transcoder.Transcoder.Transcode(bb, new SAlphabet(SAlphabet.ASCII), Alphabet), 0, V, 0, L);
			}
		/// <summary>
		/// Set the TLV value part from an array of bytes
		/// Throws exceptions if buffer's length doesn't match the created TLV
		/// </summary>
		/// <param name="buffer">The array of bytes to set the TLV value from</param>
		/// <returns>TRUE if the value has been set, FALSE otherwise</returns>
		public void SetV(byte[] buffer)
			{
			if (buffer.Length != L)
				throw new CInvalidDataLengthException("tag " + T, buffer.Length, L, L);
			byte[] bb = DefaultEncoder().Encode(buffer, L, L, out int length);
			Buffer.BlockCopy(Transcoder.Transcoder.Transcode(bb, new SAlphabet(SAlphabet.ASCII), Alphabet), 0, V, 0, L);
			}
		/// <summary>
		/// Set a TLV value part specific byte from a byte
		/// Throws exceptions if the index is out of boundaries of TLV
		/// </summary>
		/// <param name="b">Byte to set</param>
		/// <param name="index">Index of the byte to set</param>
		public void SetV(byte b, int index)
			{
			if (L < index)
				throw new CInvalidDataLengthException("tag " + T, index, L, L);
			V[index - 1] = Transcoder.Transcoder.Transcode(b, new SAlphabet(SAlphabet.ASCII), Alphabet);
			}
		/// <summary>
		/// Set the TLV value part from a short (2 bytes) value
		/// Throws exceptions if the TLV size doesn't match the value size
		/// </summary>
		/// <param name="value">The value to set</param>
		public void SetV(short value)
			{
			SetV(value, sizeof(short));
			}
		/// <summary>
		/// Set the TLV value part from a integer (4 bytes) value
		/// Throws exceptions if the TLV size doesn't match the value size
		/// </summary>
		/// <param name="value">The value to set</param>
		public void SetV(int value)
			{
			SetV(value, sizeof(int));
			}
		/// <summary>
		/// Set the TLV value part from a long (8 bytes) value
		/// Throws exceptions if the TLV size doesn't match the value size
		/// </summary>
		/// <param name="value">The value to set</param>
		public void SetV(long value)
			{
			SetV(value, sizeof(long));
			}
		/// <summary>
		/// Set the TLV value part from an integral type value
		/// Throws exceptions if the TLV size doesn't match the value size indicated
		/// </summary>
		/// <param name="value">The value to set</param>
		/// <param name="size">The size of the integral type (sizeof...)</param>
		private void SetV(long value, int size)
			{
			if (L != size)
				throw new CInvalidDataLengthException("tag " + T, size, L, L);
			byte[] bb = DefaultEncoder().Encode(CMisc.SetBytesFromIntegralTypeValue((sizeof(short) == size ? (short)value : sizeof(int) == size ? (int)value : value)), L, L, out int length);
			Buffer.BlockCopy(Transcoder.Transcoder.Transcode(bb, new SAlphabet(SAlphabet.ASCII), Alphabet), 0, V, 0, L);
			}
		/// <summary>
		/// Get the content of a TLV value part
		/// </summary>
		/// <param name="buffer">Buffer to extract the TLV from</param>
		/// <param name="start">Start position inside the buffer to retrieve the TLV</param>
		/// <param name="offset">Offset after the TLV</param>
		public void GetTLV(byte[] buffer, int start, out int offset)
			{
			//offset = start;
			//if (buffer.Length - offset > CTLV.TLV_SIZE_T + CTLV.TLV_SIZE_L + 1)
			//	{
			//	byte[] rawdata;
			//	int mystart = start;
			//	EncoderAN encoderAN = new EncoderAN();
			//	// enough room for T+L+V (1 byte at least)

			//	//get T
			//	T = (encoderAN.Decode(buffer, offset, CTLV.TLV_SIZE_T, CTLV.TLV_SIZE_T, out offset, out rawdata)).ToString();
			//	// get L
			//	L = (short)encoderAN.DecodeToLong(buffer, offset, TLV_SIZE_L, TLV_SIZE_L, out offset, out rawdata);
			//	if (0 != L)
			//		if (buffer.Length - offset >= L)
			//			{
			//			// get V
			//			Buffer.BlockCopy(buffer, offset, CTranscoder.Transcode(V, Alphabet, new SAlphabet(SAlphabet.ASCII)), 0, L);
			//			offset += L;
			//			}
			//		else
			//			throw new CInvalidDataSizeException("TAG " + T, L, buffer.Length - offset);
			//	else
			//		throw new CInvalidValueException("TAG " + T, L.ToString());
			//	}
			//else
			//	throw new CInvalidDataSizeException("TLV", TLV_SIZE_T + TLV_SIZE_L + 1, buffer.Length - offset);

			CTLV tlv = CTLVStream.ExtractTLV(buffer, start, out offset);
			if (null != tlv)
				{
				T = tlv.T;
				L = tlv.L;
				V = tlv.V;
				}
			}
		#endregion
		}
	}
