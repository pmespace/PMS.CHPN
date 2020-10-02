using System.Runtime.InteropServices;
using CHPN;
using CHPN.Encoder;
using CHPN.Transcoder;

namespace CHPN.APDU
{
	[ComVisible(true)]
	public enum FieldType
	{
		NONE = 0,
		NUMERIC,
		ALPHA_NUMERIC,
		EXTENDED_ALPHA_NUMERIC,
		BINARY,
		CMC7
	}

	[ComVisible(false)]
	public abstract class CFieldGeneric: CRawdata
	{
		#region constructors
		/// <summary>
		/// Create a generic field able to carry a data or a TLV
		/// </summary>
		/// <param name="type">Type of the underlying field</param>
		public CFieldGeneric(FieldType type) { Type = type; }
		#endregion

		#region constants
		public const int VARIABLE_FIELD_NON_BINARY_LENGTH_SIZE = 1;
		public const int VARIABLE_FIELD_BINARY_LENGTH_SIZE = 2;
		#endregion

		#region properties
		/// <summary>
		/// Field type (numeric, alpha, binary...)
		/// </summary>
		public FieldType Type { get; protected set; }
		/// <summary>
		/// The encoding/decoding alphabet, set to NOTRANSCODING if invalid, meaning no transcoding anytime
		/// </summary>
		public SAlphabet Alphabet { get => DefaultAlphabet(); }
		/// <summary>
		/// The character set applying to the field
		/// </summary>
		public string CharacterSet { get => DefaultEncoder().CharacterSet; }
		#endregion

		#region methods
		/// <summary>
		/// Default alphabet to use according to the field type
		/// </summary>
		/// <returns>The alphabet to use</returns>
		public virtual SAlphabet DefaultAlphabet()
		{
			switch (Type)
			{
				case FieldType.CMC7:
				case FieldType.NUMERIC:
					return new SAlphabet(SAlphabet.NONE);
				case FieldType.ALPHA_NUMERIC:
				case FieldType.EXTENDED_ALPHA_NUMERIC:
					return new SAlphabet(SAlphabet.EBCDIC);
				case FieldType.BINARY:
					return new SAlphabet(SAlphabet.ASCII);
				default:
					return new SAlphabet(SAlphabet.NONE);
			}
		}
		/// <summary>
		/// Get the encoder to use according to the data type carried inside the field
		/// </summary>
		/// <returns>The encoder to use (DCB, AN, ANS or ANSB)</returns>
		public virtual IEncoder DefaultEncoder()
		{
			switch (Type)
			{
				case FieldType.CMC7:
					return new CEncoderCMC7();
				case FieldType.NUMERIC:
					return new CEncoderDCB();
				case FieldType.ALPHA_NUMERIC:
					return new CEncoderAN();
				case FieldType.EXTENDED_ALPHA_NUMERIC:
					return new CEncoderANS();
				case FieldType.BINARY:
					return new CEncoderANSB();
				default:
					return null;
			}
		}
		/// <summary>
		/// Default size length according to the data type
		/// </summary>
		/// <returns></returns>
		protected int DefaultSizeLength()
		{
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
		#endregion
	}
}
