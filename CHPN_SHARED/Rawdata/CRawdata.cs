using System.Runtime.InteropServices;

namespace CHPN
	{
	[ComVisible(false)]
	public abstract class CRawdata
		{
		#region properties
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		public virtual byte[] Rawdata { get; }
		/// <summary>
		/// Describes the length of the raw representation of a type
		/// </summary>
		public int RawdataLen
			{
			get
				{
				byte[] ab = Rawdata;
				if (null != ab)
					return ab.Length;
				return 0;
				}
			}
		#endregion

		#region methods
		/// <summary>
		/// Converts the rawdata to a string
		/// </summary>
		/// <returns>The converted string</returns>
		public override string ToString()
			{
			string s = string.Empty;
			byte[] ab = Rawdata;
			if (null != ab)
				foreach (byte b in ab)
					s += b.ToString("X2");
			return s;
			}
		#endregion
		}
	}
