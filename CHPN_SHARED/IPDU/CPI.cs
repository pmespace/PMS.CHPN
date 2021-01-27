using System.Runtime.InteropServices;
using System.Collections.Generic;
using System;
using CHPN.Exceptions;

namespace CHPN.IPDU
{
	[Guid("233E8CC2-C035-40C8-9231-BC2E846BD0E1")]
	[ComVisible(true)]
	public interface IPI
	{
		#region properties
		/// <summary>
		/// PI indicates the PI name
		/// </summary>
		[DispId(1)]
		byte PI { get; set; }
		/// <summary>
		/// PI length in bytes
		/// </summary>
		[DispId(2)]
		int PILen { get; }
		/// <summary>
		/// Specific PI data, without PI and PILen
		/// </summary>
		[DispId(3)]
		byte[] Data { get; }
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		[DispId(4)]
		byte[] Rawdata { get; }
		/// <summary>
		/// Describes length of the raw representation of a type
		/// </summary>
		[DispId(5)]
		int RawdataLen { get; }
		#endregion
		#region methods
		/// <summary>
		///  Assign the current PI from an existing one
		/// </summary>
		/// <param name="cpi">The existing PI to assign from</param>
		[DispId(100)]
		void AssignFromPI(CPI cpi);
		[DispId(101)]
		void SetDataBytes(byte[] buffer, int start);
		[DispId(102)]
		void SetDataByte(int index, byte b);
		[DispId(103)]
		byte GetDataByte(int index);
		#endregion
	}

	[Guid("72A7E9F5-8805-44CE-8CA0-9B0C6543DB30")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class CPI : CRawdata, IPI
	{
		#region constants
		public const int PI_PI_SIZE = 1;
		public const int PI_LI_SIZE = 1;
		public const int PI_HEADER_SIZE = PI_PI_SIZE + PI_LI_SIZE;
		#endregion

		#region properties
		/// <summary>
		/// PI indicates the PI name
		/// </summary>
		public byte PI
		{
			get => _pi;
			set
			{
				if (value != PI)
					PILen = 0;
				_pi = value;
			}
		}
		private byte _pi = 0;
		/// <summary>
		/// PI length in bytes
		/// </summary>
		public int PILen
		{
			get => _pilen;
			private set
			{
				_pilen = value;
				Data = new byte[PILen];
			}
		}
		private int _pilen = 0;
		/// <summary>
		/// Specific PI data, without PI and PILen
		/// </summary>
		public byte[] Data
		{
			get => _data;
			private set
			{
				if (PILen == value.Length)
					_data = value;
			}
		}
		private byte[] _data = new byte[0];
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		public override byte[] Rawdata
		{
			get
			{
				byte[] ab = new byte[PI_HEADER_SIZE + PILen];
				ab[0] = (byte)PI;
				ab[1] = (byte)PILen;
				Buffer.BlockCopy(Data, 0, ab, PI_HEADER_SIZE, PILen);
				return ab;
			}
		}
		#endregion

		#region constructors
		public CPI() { }
		/// <summary>
		/// Create a PI from an incoming buffer
		/// </summary>
		/// <param name="pi">Name of the PI</param>
		/// <param name="size">Size of the PI</param>
		public CPI(byte pi, byte size)
		{
			PI = pi;
			PILen = size;
		}
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cpi">PI to copy from</param>
		public CPI(CPI cpi) { AssignFromPI(cpi); }
		#endregion

		#region public methods
		/// <summary>
		///  Assign the current PI from an existing one
		/// </summary>
		/// <param name="cpi">The existing PI to assign from</param>
		public void AssignFromPI(CPI cpi)
		{
			PI = cpi.PI;
			PILen = cpi.PILen;
			Buffer.BlockCopy(cpi.Data, 0, Data, 0, PILen);
		}
		/// <summary>
		/// Set the Data part (nothing else) of a PI from a buffer.
		/// Throws Exceptions if buffer to set is bigger that actual PI size.
		/// </summary>
		/// <param name="buffer">Buffer to copy to the Data part</param>
		/// <param name="start">Position inside the buffer to use as the start of the data to copy to the Data part</param>
		public void SetDataBytes(byte[] buffer, int start)
		{
			// if the buffer is big enough, from the starting position, we copy it
			if (PILen <= buffer.Length - start)
			{
				// we copy ONLY PILen bytes
				Buffer.BlockCopy(buffer, start, Data, 0, PILen);
			}
			else
				throw new CInvalidDataLengthException("PI" + PI.ToString("X2"), buffer.Length - start, 0, PILen);
		}
		/// <summary>
		/// Set 1 byte inside the Data part of a PI.
		/// Throws Exceptions if the index doesn't match PI size.
		/// </summary>
		/// <param name="index">Index of byte to modify (NOT 0 based)</param>
		/// <param name="b">Value to set</param>
		public void SetDataByte(int index, byte b)
		{
			if (PILen >= index)
			{
				Data[index - 1] = b;
			}
			else
				throw new CInvalidDataLengthException("PI" + PI.ToString("X2"), index, 0, PILen);
		}
		/// <summary>
		/// Get the value of a byte.
		/// Throws Exceptions if the index doesn't match PI size.
		/// </summary>
		/// <param name="index">Index of the byte to retrieve (NOT 0 based)</param>
		/// <returns>The content of the requested byte</returns>
		public byte GetDataByte(int index)
		{
			if (PILen >= index)
				return Data[index - 1];
			else
				throw new CInvalidDataLengthException("PI" + PI.ToString("X2"), index, 0, PILen);
		}
		#endregion
	}
}
