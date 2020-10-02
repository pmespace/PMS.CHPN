using System.Runtime.InteropServices;
using COMMON;

namespace CHPN.Encoder
	{
	[ComVisible(false)]
	class CEncoderCMC7: CEncoderDCB
		{
		#region properties
		/// <summary>
		/// 
		/// </summary>
		public override string CharacterSet
			{
			get => RegexCHPN.REGEX_CMC7;
			}
		#endregion

		#region methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public override long ToLong(byte[] buffer)
			{
			return 0;
			}
		#endregion
		}
	}
