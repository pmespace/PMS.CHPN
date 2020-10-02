using System.Runtime.InteropServices;
using CHPN.Encoder;
using CHPN;

namespace CHPN.APDU
	{
	[ComVisible(true)]
	public enum APDUs
		{
		APDU_9100 = 9100,
		APDU_9110 = 9110,
		APDU_9300 = 9300,
		APDU_9310 = 9310
		}

	[Guid("BE67E1B6-66A9-4470-BB6F-5B1863D8FBAB")]
	[ComVisible(true)]
	public interface IAPDUID
		{
		[DispId(1)]
		short Value { get; set; }
		[DispId(2)]
		byte[] Rawdata { get; }
		[DispId(3)]
		int RawdataLen { get; }
		[DispId(100)]
		string ToString();
		}

	[Guid("68E15B1D-221D-40F3-A600-A8ACD686F1BF")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class CAPDUID: CRawdata, IAPDUID
		{
		#region constants
		public const int MESSAGE_ID_SIZE = 4;
		#endregion

		#region public properties
		/// <summary>
		/// Message ID
		/// </summary>
		public short Value { get; set; } = 0;
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		public override byte[] Rawdata
			{
			get
				{
				string s = Value.ToString();
				return new CEncoderDCB().Encode(s, MESSAGE_ID_SIZE, MESSAGE_ID_SIZE, out int length);
				}
			}
		#endregion

		#region constructors
		public CAPDUID() { }
		/// <summary>
		/// Construct an ID from an already known ID
		/// </summary>
		/// <param name="id"></param>
		public CAPDUID(short id) { Value = id; }
		/// <summary>
		/// Construct an ID from a buffer
		/// </summary>
		/// <param name="buffer">A buffer inside which the message ID should be located</param>
		/// <param name="start">The position inside the buffer at which the message ID is supposed to be</param>
		/// <param name="offset">The offset inside the buffer after the message ID has been read</param>
		public CAPDUID(byte[] buffer, int start, ref int offset)
			{
			if (sizeof(short) <= buffer.Length - start)
				{
				byte[] rawdata;
				CEncoderDCB enc = new CEncoderDCB();
				Value = (short)enc.ToLong(enc.Decode(buffer, start, MESSAGE_ID_SIZE, MESSAGE_ID_SIZE, out offset, out rawdata));
				}
			else
				Value = 0;
			}
		#endregion

		#region methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString() { return Value.ToString("0000"); }
		#endregion
		}
	}
