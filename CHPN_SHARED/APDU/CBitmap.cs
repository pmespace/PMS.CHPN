using System.Runtime.InteropServices;
using System;
using COMMON;

namespace CHPN.APDU
	{
	[Guid("793FA9EA-C6E2-41A0-83F1-BEE3BFB83F43")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	public interface IBitmap
		{
		[DispId(1)]
		long Bitmap1 { get; }
		[DispId(2)]
		long Bitmap2 { get; }
		[DispId(3)]
		byte[] Rawdata { get; }
		[DispId(4)]
		int RawdataLen { get; }
		[DispId(5)]
		bool IsSet { get; }

		[DispId(100)]
		bool IsBitSet(int bit);
		}

	[Guid("98393A28-FF86-44A6-B7E9-D1E6A71D1C4F")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class CBitmap: CRawdata, IBitmap
		{
		#region constructors
		/// <summary>
		/// Creates an empty bitmap
		/// </summary>
		public CBitmap() { Reset(); }
		/// <summary>
		/// Construct an bitmap from a buffer
		/// </summary>
		/// <param name="buffer">A buffer inside which the bitmap should be located</param>
		/// <param name="start">The position inside the buffer at which the bitmap is supposed to be</param>
		/// <param name="offset">The offset inside the buffer after the bitmap has been read</param>
		public CBitmap(byte[] buffer, int start, ref int offset)
			{
			Reset();
			SetBitmap(buffer, start, out offset);
			}
		#endregion

		#region constants
		private const int BITMAP_BIT_MIN = 1;
		private const int BITMAP_BIT_MAX = BITMAP_LONG_SIZE * 8;
		private const int BITMAP_SHORT_SIZE = 8;
		private const int BITMAP_LONG_SIZE = BITMAP_SHORT_SIZE * 2;
		public const int BITMAP_NB_BITS = BITMAP_SHORT_SIZE * 8;
		#endregion

		#region data
		private long[] _bitmap = new long[2];
		#endregion

		#region properties
		/// <summary>
		/// Bitmap, bits 1 to 64
		/// </summary>
		public long Bitmap1
			{
			get => _bitmap[0];
			set
				{
				_bitmap[0] = value;
				// if first bit is not set clear second bitmap
				if (0x000000000000000 == (1 << 63 & Bitmap1)) // 0xE000000000000000 & Bitmap1))
					Bitmap2 = 0;
				}
			}
		/// <summary>
		/// Bitmap, bits 65 to 128
		/// </summary>
		public long Bitmap2
			{
			get => _bitmap[1];
			set
				{
				// if first bit is not set in Bitmap1 clear second bitmap
				if (0x000000000000000 == (1 << 63 & Bitmap2)) // 0xE000000000000000 & Bitmap1))
					_bitmap[1] = 0;
				else
					_bitmap[1] = value;
				}
			}
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		public override byte[] Rawdata
			{
			get
				{
				byte[] ab = new byte[(0 == Bitmap2 ? BITMAP_SHORT_SIZE : BITMAP_LONG_SIZE)];
				Buffer.BlockCopy(CMisc.SetBytesFromIntegralTypeValue((long)Bitmap1), 0, ab, 0, BITMAP_SHORT_SIZE);
				if (0 != Bitmap2)
					Buffer.BlockCopy(CMisc.SetBytesFromIntegralTypeValue((long)Bitmap2), 0, ab, BITMAP_SHORT_SIZE, BITMAP_SHORT_SIZE);
				return ab;
				}
			}
		/// <summary>
		/// Indicates if a bitmap is valid or not (it has been set to any value)
		/// An invalid bitmap is fully set to 0
		/// </summary>
		public bool IsSet { get => 0 != Bitmap1; }
		#endregion

		#region methods
		/// <summary>
		/// Clears the bitmap
		/// </summary>
		private void Reset()
			{
			Bitmap1 = 0;
			Bitmap2 = 0;
			}
		/// <summary>
		/// Set the bitmap from a bitmap contained inside a buffer
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		private int SetBitmap(byte[] buffer, int start, out int offset)
			{
			Reset();
			int nb = 0;
			offset = start;
			// first bitmap
			Bitmap1 = GetBitmap(buffer, start, out bool fSet);
			nb += (fSet ? 1 : 0);
			if (fSet)
				if (IsBitSet(1))
					{
					// second bitmap
					Bitmap2 = GetBitmap(buffer, start + BITMAP_SHORT_SIZE, out fSet);
					nb += (fSet ? 1 : 0);
					}
			offset += nb * BITMAP_SHORT_SIZE;
			return nb;
			}
		/// <summary>
		/// Indicates whether a specific bit is set or not inside a bitmap
		/// </summary>
		/// <param name="bit">Bit to check</param>
		/// <returns>TRUE is bit is set, FALSE otherwise</returns>
		public bool IsBitSet(int bit)
			{
			if (1 <= bit && 128 >= bit)
				{
				long i = (long)1 << (64 - (64 < bit ? bit - 64 : bit));
				if (1 <= bit && 64 >= bit)
					return 0 != (Bitmap1 & i);
				else
					return 0 != (Bitmap2 & i);
				}
			return false;
			}
		/// <summary>
		/// Set a bit inside a bitmap
		/// </summary>
		/// <param name="bit">Bit to set</param>
		internal void SetBit(int bit)
			{
			if (1 <= bit && 128 >= bit)
				{
				long i = (long)1 << (64 - (64 < bit ? bit - 64 : bit));
				if (64 >= bit)
					Bitmap1 |= i;
				else
					Bitmap2 |= i;
				}
			}
		/// <summary>
		/// Unset a bit inside a bitmap
		/// </summary>
		/// <param name="bit">Bit to unset</param>
		internal void UnsetBit(int bit)
			{
			if (1 <= bit && 128 >= bit)
				{
				long i = 0xFFFFFFFFFFFFFFF ^ (long)1 << (64 - (64 < bit ? bit - 64 : bit));
				if (64 >= bit)
					Bitmap1 &= i;
				else
					Bitmap2 &= i;
				}
			}
		/// <summary>
		/// Get a bitmap part value from a buffer
		/// </summary>
		/// <param name="buffer">The buffer to use</param>
		/// <param name="start">Position inside that buffer to search the bitmap value</param>
		/// <param name="secondBitmap">Indicates if a second bitmap is present (true) or not (false)</param>
		/// <returns>The value of the bitmap (if at least 8 bytes inside the buffer), 0 otherwise</returns>
		private long GetBitmap(byte[] buffer, int start, out bool secondBitmap)
			{
			if (BITMAP_SHORT_SIZE <= buffer.Length - start)
				{
				secondBitmap = true;
				return (long)CMisc.GetIntegralTypeValueFromBytes(buffer, start, BITMAP_SHORT_SIZE);
				}
			else
				{
				secondBitmap = false;
				return 0;
				}
			}
		#endregion
		}
	}
