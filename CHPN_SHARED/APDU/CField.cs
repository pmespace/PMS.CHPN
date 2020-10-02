using System.Runtime.InteropServices;
using System;
using CHPN.Encoder;
using CHPN.Transcoder;
using COMMON;

namespace CHPN.APDU
	{
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IField
		{
		#region properties
		/// <summary>
		/// Field number
		/// </summary>
		[DispId(1)]
		int ID { get; }
		/// <summary>
		/// Indicates whether the field is of fixed size or not
		/// </summary>
		[DispId(2)]
		bool Fixed { get; }
		/// <summary>
		/// Field size len in bytes
		/// </summary>
		[DispId(3)]
		int LengthPartSizeInBytes { get; }
		/// <summary>
		/// Minimum length of field
		/// </summary>
		[DispId(4)]
		int Minlen { get; }
		/// <summary>
		/// Maximum length of field
		/// </summary>
		[DispId(5)]
		int Maxlen { get; }
		/// <summary>
		/// Minimum field length in bytes
		/// </summary>
		[DispId(6)]
		bool IsTLV { get; }
		/// <summary>
		/// Length of the data
		/// </summary>
		[DispId(7)]
		int Length { get; }
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		[DispId(8)]
		byte[] Rawdata { get; }
		/// <summary>
		/// Size of raw representation
		/// </summary>
		[DispId(9)]
		int RawdataLen { get; }
		/// <summary>
		/// The readeable data passed to create the field
		/// </summary>
		[DispId(10)]
		string Data { get; }
		#endregion
		}

	[Guid("7D8E249E-F998-4E39-A59F-9C61AA7AA227")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class CField: CFieldGeneric, IField
		{
		#region privates
		private int _minlen = 0, _maxlen = 0;
		#endregion

		#region properties
		/// <summary>
		/// Field number
		/// </summary>
		public int ID { get; private set; }
		/// <summary>
		/// Indicates whether the field is of fixed size or not
		/// </summary>
		public virtual bool Fixed { get => (Minlen == Maxlen); }
		/// <summary>
		/// Field size len in bytes
		/// </summary>
		public int LengthPartSizeInBytes
			{
			get
				{
				if (Fixed)
					return 0;
				else
					switch (Type)
						{
						case FieldType.CMC7:
						case FieldType.NUMERIC:
						case FieldType.ALPHA_NUMERIC:
						case FieldType.EXTENDED_ALPHA_NUMERIC:
							return VARIABLE_FIELD_NON_BINARY_LENGTH_SIZE;
						case FieldType.BINARY:
							return VARIABLE_FIELD_BINARY_LENGTH_SIZE;
						default:
							return 0;
						}
				}
			}
		/// <summary>
		/// Minimum length of field
		/// If equal to maximum length the field is of fixed size
		/// </summary>
		public int Minlen
			{
			//get => (_minlen < _maxlen ? (1 > _minlen ? 1 : _minlen) : (1 > _maxlen ? 1 : _maxlen));
			get => (_minlen < _maxlen ? (0 > _minlen ? 0 : _minlen) : (0 > _maxlen ? 0 : _maxlen));
			private set => _minlen = value;
			}
		/// <summary>
		/// Maximum length of field
		/// If equal to minimum length the field is of fixed size
		/// </summary>
		public int Maxlen
			{
			get => (_minlen > _maxlen ? _minlen : _maxlen);
			private set => _maxlen = value;
			}
		/// <summary>
		/// Minimum field length in bytes
		/// </summary>
		public int MinlenInBytes { get; protected set; }
		/// <summary>
		/// Maximum field length in bytes
		/// </summary>
		public int MaxlenInBytes { get; protected set; }
		/// <summary>
		/// Indicates whther the field has been encoded
		/// </summary>
		public bool Encoded { get; protected set; }
		/// <summary>
		/// Indicates whther the field has been decoded
		/// </summary>
		public bool Decoded { get; protected set; }
		public bool IsTLV { get; private set; }
		/// <summary>
		/// Length of the data
		/// </summary>
		public int Length
			{
			get => (Fixed ? Minlen : _length);
			protected set => _length = value;
			}
		private int _length = 0;
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		public override byte[] Rawdata { get => _rawdata; }
		private byte[] _rawdata = null;
		/// <summary>
		/// The readeable data passed to create the field
		/// </summary>
		public string Data { get; protected set; }
		#endregion

		#region constructors
		/// <summary>
		/// Creates a Field from nothing
		/// </summary>
		/// <param name="id">Field ID (2, 3, 22,...)</param>
		/// <param name="type">Field type (numeric, alphanumeric,...)</param>
		/// <param name="minlen">Minimum length of the Field, if equal to Maxlen the field is of fixed length</param>
		/// <param name="maxlen">Maximum length of the Field, if equal to Minlen the field is of fixed length</param>
		/// <param name="istlv">Indicates whether the field is TLV composed or not</param>
		internal CField(int id, FieldType type, int minlen = 1, int maxlen = 1, bool istlv = false)
			: base(type)
			{
			Initialise(id, minlen, maxlen, istlv);
			}
		/// <summary>
		/// Initialise the field itself
		/// </summary>
		/// <param name="id"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="istlv"></param>
		private void Initialise(int id, int minlen = 1, int maxlen = 1, bool istlv = false)
			{
			IsTLV = istlv;
			// set standard attributes
			ID = id;
			Minlen = minlen;
			Maxlen = maxlen;
			if (FieldType.NUMERIC == Type)
				{
				MinlenInBytes = (0 == Minlen % 2 ? Minlen / 2 : Minlen / 2 + 1);
				MaxlenInBytes = (0 == Maxlen % 2 ? Maxlen / 2 : Maxlen / 2 + 1);
				}
			else
				{
				MinlenInBytes = Minlen;
				MaxlenInBytes = Maxlen;
				}
			Encoded = false;
			Decoded = false;
			}
		/// <summary>
		/// Create a Field from another field
		/// </summary>
		/// <param name="f"></param>
		public CField(CField f)
			: base(f.Type)
			{
			Initialise(f.ID, f.Minlen, f.Maxlen, f.IsTLV);
			}
		#endregion

		#region methods
		/// <summary>
		/// Set field data, ENCODING it according to its type
		/// </summary>
		/// <param name="value">The value to encode, as a string</param>
		/// <returns>TRUE if encoding was successful, FALSE otherwise</returns>
		public bool SetData(string value)
			{
			Data = value;
			Encoded = (null != (_rawdata = Transcoder.Transcoder.Transcode(base.DefaultEncoder().Encode(value, Minlen, Maxlen, out _length), new SAlphabet(SAlphabet.ASCII), Alphabet)));
			return Encoded;
			}
		/// <summary>
		/// Set field data, ENCODING it according to its type
		/// </summary>
		/// <param name="value">The value to encode, as a string</param>
		/// <returns>TRUE if encoding was successful, FALSE otherwise</returns>
		public bool SetData(uint value)
			{
			Data = value.ToString();
			Encoded = (null != (_rawdata = Transcoder.Transcoder.Transcode(base.DefaultEncoder().Encode(value.ToString(), Minlen, Maxlen, out _length), new SAlphabet(SAlphabet.ASCII), Alphabet)));
			return Encoded;
			}
		/// <summary>
		/// Set field data, ENCODING it according to its type
		/// </summary>
		/// <param name="buffer">The value to encode, as an array of bytes</param>
		/// <returns>TRUE if encoding was successful, FALSE otherwise</returns>
		public bool SetData(byte[] buffer)
			{
			Data = CMisc.BytesToStr(buffer);
			Encoded = (null != (_rawdata = Transcoder.Transcoder.Transcode(base.DefaultEncoder().Encode(buffer, Minlen, Maxlen, out _length), new SAlphabet(SAlphabet.ASCII), Alphabet)));
			return Encoded;
			}
		/// <summary>
		/// Set field data from an incoming buffer, DECODING it 
		/// </summary>
		/// <param name="buffer">The buffer containing the field</param>
		/// <param name="start">The starting positio inside this buffer to discover teh field</param>
		/// <param name="offset">On exit, the offset from the start position to the next field</param>
		///<returns></returns>
		public bool SetData(byte[] buffer, int start, out int offset)
			{
			byte[] data;
			byte[] rawdata;
			IEncoder encoder = DefaultEncoder();
			if (Decoded = (null != (data = encoder.Decode(buffer, start, Minlen, Maxlen, out offset, out rawdata))))
				{
				Length = data.Length;
				Data = encoder.ToString(Transcoder.Transcoder.Transcode(data, Alphabet, new SAlphabet(SAlphabet.ASCII)));
				_rawdata = Transcoder.Transcoder.Transcode(rawdata, Alphabet, new SAlphabet(SAlphabet.ASCII));
				// eventual size part must not be transcoded, for simplicity we just copy it back
				Buffer.BlockCopy(rawdata, 0, _rawdata, 0, LengthPartSizeInBytes);
				}
			return Decoded;
			}
		#endregion
		}
	}