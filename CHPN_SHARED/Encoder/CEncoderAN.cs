using System.Runtime.InteropServices;
using COMMON;

namespace CHPN.Encoder
	{
	[ComVisible(false)]
	class CEncoderAN: CEncoderANS
		{
		#region properties
		/// <summary>
		/// 
		/// </summary>
		public override string CharacterSet
			{
			get => RegexCHPN.REGEX_AN;
			}
		#endregion
		}
	}
