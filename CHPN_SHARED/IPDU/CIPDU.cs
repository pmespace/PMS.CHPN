using System.Runtime.InteropServices;
using System;
using System.Reflection;
using System.Collections.Generic;
using COMMON;
using CHPN.APDU;
using CHPN;

namespace CHPN.IPDU
{
	[ComVisible(true)]
	public enum PI01ErrorCodes
	{
		PI01_NOERROR = 0,
		PI01_INVALID_PGI = 2,
		PI01_MANDATORY_PARAMETER_IS_MISSING = 4,
		PI01_ACCESS_POINT_FULL = 7,
		PI01_SYNCH_ERROR = 9,
		PI01_SERVICE_HAS_STOPPED = 14,
		PI01_ACCESS_POINT_HAS_STOPPED = 15,
		PI01_UNKNOWN_APDU = 17,
		PI01_INVALID_APDU = 18,
		PI01_INVALID_APDU_LENGTH = 19,
		PI01_PROTOCOL_ERROR = 24,
		PI01_INACTIVITY_TIMER_HAS_EXPIRED = 25,
		PI01_INVALID_TRANSACTION = 34,
		PI01_INVALID_IPDU_FORMAT = 35,
		PI01_INVALID_IPDU_EXCHANGE = 36,
		PI01_INVALID_PARAMETER = 39
	}

	[ComVisible(true)]
	public enum IPDUs
	{
		IPDU_DE = 0xC1,
		IPDU_AB = 0xC9
	}

	[Guid("32D6E57B-44B9-49CC-B96C-0C471C1572D7")]
	[ComVisible(true)]
	public interface IIPDU
	{
		#region properties
		/// <summary>
		/// The CBCOM total length, at the veru beginning of every message
		/// </summary>
		[DispId(1)]
		int TotalLength { get; }
		/// <summary>
		/// The CBCOM PGI
		/// </summary>
		[DispId(2)]
		byte PGI { get; set; }
		/// <summary>
		/// The CBCOM LGI
		/// </summary>
		[DispId(3)]
		byte LGI { get; }
		/// <summary>
		/// The list of PI in raw format
		/// </summary>
		[DispId(4)]
		byte[] PIData { get; }
		/// <summary>
		/// The APDU inside the IPDU
		/// </summary>
		[DispId(5)]
		CAPDU APDU { get; set; }
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		[DispId(6)]
		byte[] Rawdata { get; }
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		[DispId(7)]
		int RawdataLen { get; }
		#endregion

		#region methods
		[DispId(100)]
		bool AddPI(CPI pi);
		[DispId(101)]
		bool RemovePI(CPI pi);
		[DispId(102)]
		CPI GetPI(byte pi);
		[DispId(103)]
		bool UpdatePI(byte pi, byte[] value);
		#endregion
	}

	[Guid("1AF86EA1-CF19-4119-AE19-4D46329CB5C5")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class CIPDU : CRawdata, IIPDU
	{
		#region constants
		public const int IPDU_TOTAL_LENGTH_SIZE = 4;
		public const int IPDU_PGI_SIZE = 1;
		public const int IPDU_LGI_SIZE = 1;
		#endregion

		#region properties
		/// <summary>
		/// All PI inside the IPDU
		/// </summary>
		internal CPIDictionary PI { get; set; } = new CPIDictionary();
		/// <summary>
		/// The CBCOM total length, at the veru beginning of every message
		/// </summary>
		public int TotalLength { get => RawdataLen - IPDU_TOTAL_LENGTH_SIZE; }
		/// <summary>
		/// The CBCOM PGI
		/// </summary>
		public byte PGI { get; set; }
		/// <summary>
		/// The CBCOM LGI
		/// </summary>
		public byte LGI { get; private set; }
		/// <summary>
		/// The list of PI in raw format
		/// </summary>
		public byte[] PIData { get; private set; }
		/// <summary>
		/// The APDU inside the IPDU
		/// </summary>
		public CAPDU APDU { get; set; }
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		public override byte[] Rawdata
		{
			get
			{
				// get the total size of the IPDU
				int sizeipdu = MinimumLength() + LGI; // DO NOT CHANGE THE TYPE OF THIS DATA, it MUST remain "int"
				int sizeapdu = (null != APDU ? APDU.RawdataLen : 0);
				int size = sizeipdu + sizeapdu;
				// build Rawdata
				byte[] ab = new byte[size];
				// set total length
				byte[] bb = CMisc.SetBytesFromIntegralTypeValue(size - IPDU_TOTAL_LENGTH_SIZE);
				Buffer.BlockCopy(bb, 0, ab, 0, IPDU_TOTAL_LENGTH_SIZE);
				// set PGI LGI + PIs
				ab[IPDU_TOTAL_LENGTH_SIZE] = (byte)PGI;
				ab[IPDU_TOTAL_LENGTH_SIZE + IPDU_PGI_SIZE] = (byte)LGI;
				if (0 != LGI)
					Buffer.BlockCopy(PIData, 0, ab, MinimumLength(), LGI);
				// add the APDU is any
				if (0 != sizeapdu)
					Buffer.BlockCopy(APDU.Rawdata, 0, ab, MinimumLength() + LGI, APDU.RawdataLen);
				return ab;
			}
		}
		#endregion

		#region constructors
		public CIPDU() { }
		/// <summary>
		/// Create an empty IPDU
		/// </summary>
		public CIPDU(byte ipdu) { PGI = ipdu; }
		/// <summary>
		/// Create an IPDU from a buffer (incoming IPDU from the server).
		/// Throws exceptions if the buffer isn't big enough or is badly formatted
		/// </summary>
		/// <param name="buffer">Buffer to use to create the IPDU</param>
		public CIPDU(byte[] buffer)
		{
			bool fOK;
			int offset = 0;
			if (fOK = HasMinimumLength(buffer))
			{
				// check total length
				byte[] totallength = new byte[IPDU_TOTAL_LENGTH_SIZE];
				Buffer.BlockCopy(buffer, offset, totallength, 0, totallength.Length);
				if (fOK = CMisc.GetIntegralTypeValueFromBytes(totallength, IPDU_TOTAL_LENGTH_SIZE) == buffer.Length - IPDU_TOTAL_LENGTH_SIZE)
				{
					// total length matches
					PGI = buffer[IPDU_TOTAL_LENGTH_SIZE];
					int lgi = buffer[IPDU_TOTAL_LENGTH_SIZE + IPDU_PGI_SIZE];
					// start offsetting the buffer
					offset += IPDU_TOTAL_LENGTH_SIZE + IPDU_PGI_SIZE + IPDU_LGI_SIZE;
					// extract all PIs
					int startlgi = offset;
					while (buffer.Length - offset > 0 && offset - startlgi < lgi)
					{
						// start position is at offset
						CPI pi = CPIStream.ExtractPI(buffer, offset, out offset);
						if (null != pi)
							// a PI has been found, we add it to the list of PIs
							AddPI(pi);
					}
					// arrived here offset points to the APDU
					byte[] apdu = new byte[buffer.Length - offset];
					Buffer.BlockCopy(buffer, offset, apdu, 0, buffer.Length - offset);
					APDU = new CAPDU(apdu);
				}
			}
		}
		#endregion

		#region methods
		/// <summary>
		/// Add a PI to the IPDU
		/// </summary>
		/// <param name="pi">The PI to add</param>
		/// <returns>TRUE if the PI was added, FALSE otherwise</returns>
		public bool AddPI(CPI pi)
		{
			try
			{
				PI.Add(pi.PI, pi);
				PIData = CreatePIData();
				return true;
			}
			catch (Exception ex)
			{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
				return false;
			}
		}
		/// <summary>
		/// Remove a PI from the IPDU
		/// </summary>
		/// <param name="pi">The PI to remove</param>
		/// <returns>TRUE if the PI was removed, FALSE otherwise</returns>
		public bool RemovePI(CPI pi)
		{
			try
			{
				PI.Remove(pi.PI);
				PIData = CreatePIData();
				return true;
			}
			catch (Exception ex)
			{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
				return false;
			}
		}
		/// <summary>
		/// Get a PI from the list of PIs.
		/// Returns null if the PI doesn't exist inside the list of PIs
		/// </summary>
		/// <param name="pi">Pi to look for</param>
		/// <returns>The requested PI if it exists, NULL otherwise</returns>
		public CPI GetPI(byte pi)
		{
			try
			{
				return PI[pi];
			}
			catch (Exception ex)
			{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
				return null;
			}
		}
		/// <summary>
		/// Update a PI inside the IPDU
		/// </summary>
		/// <param name="pi">The PI to update</param>
		/// <param name="value">The new value of the PI</param>
		/// <returns>TRUE if the PI was removed, FALSE otherwise</returns>
		public bool UpdatePI(byte pi, byte[] value)
		{
			try
			{
				CPI cpi = PI[pi];
				// arrived here the PI exists and is specified
				cpi.SetDataBytes(value, 0);
				PIData = CreatePIData();
				return true;
			}
			catch (Exception ex)
			{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
				return false;
			}
		}
		/// <summary>
		/// Set the LGI value
		/// </summary>
		private byte[] CreatePIData()
		{
			byte[] buffer = null;
			// set LGI
			LGI = 0;
			foreach (KeyValuePair<byte, CPI> pi in PI)
				LGI += (byte)pi.Value.RawdataLen;
			if (0 != LGI)
			{
				// Set PIData
				int offset = 0;
				buffer = new byte[LGI];
				foreach (KeyValuePair<byte, CPI> pi in PI)
				{
					Buffer.BlockCopy(pi.Value.Rawdata, 0, buffer, offset, pi.Value.RawdataLen);
					offset += pi.Value.RawdataLen;
				}
			}
			return buffer;
		}
		/// <summary>
		/// The very minimum length for an IPDU
		/// </summary>
		/// <returns>The minimum size for an almost valid IPDU</returns>
		private int MinimumLength()
		{
			return IPDU_TOTAL_LENGTH_SIZE + IPDU_PGI_SIZE + IPDU_LGI_SIZE;
		}
		/// <summary>
		/// Test a buffer for the IPDU minimum size
		/// </summary>
		/// <param name="buffer">The buffer to use to determine the length of data</param>
		/// <param name="start">The starting position to get buffer size</param>
		/// <returns>TRUE if the buffer is at least MinimumSize long, FALSE otherwise</returns>
		private bool HasMinimumLength(byte[] buffer, int start = 0)
		{
			// test minimum _totallength
			return (MinimumLength() <= buffer.Length - start);
		}
		#endregion
	}
}
