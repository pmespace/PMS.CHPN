using System.Runtime.InteropServices;
using System.Collections.Generic;
using CHPN;

namespace CHPN.IPDU
	{
	[ComVisible(true)]
	public enum PIs
		{
		PI01 = 0x01,
		PI03 = 0x03,
		PI04 = 0x04,
		PI05 = 0x05,
		PI06 = 0x06,
		PI08 = 0x08
		}

	[ComVisible(false)]
	public class CPIDictionary: Dictionary<byte, CPI> { }

	[Guid("8DF70BEA-5320-4AC6-AFBF-B4BE66042123")]
	[ComVisible(true)]
	public interface IPIs
		{
		[DispId(100)]
		CPI GetPI(PIs pi);
		}

	[Guid("C5DD893F-8011-4C6C-A5FB-71149A5F2EC7")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class CPIs: IPIs
		{
		#region properties
		private CPIDictionary pis = new CPIDictionary();
		#endregion

		#region constructors
		/// <summary>
		/// Creates a PI template with all available PI in the CHPN protocol
		/// </summary>
		public CPIs()
			{
			pis.Add((byte)PIs.PI01, new CPI((byte)PIs.PI01, 1));
			pis[(byte)PIs.PI01].SetDataByte(1, (byte)PI01ErrorCodes.PI01_NOERROR);

			pis.Add((byte)PIs.PI03, new CPI((byte)PIs.PI03, 1));
			pis[(byte)PIs.PI03].SetDataByte(1, 0x1E); // 30 seconds

			pis.Add((byte)PIs.PI04, new CPI((byte)PIs.PI04, 1));
			pis[(byte)PIs.PI04].SetDataByte(1, 0x13); // CBCOM 1.3

			pis.Add((byte)PIs.PI05, new CPI((byte)PIs.PI05, 2));
			pis[(byte)PIs.PI05].SetDataByte(1, 0x01);
			pis[(byte)PIs.PI05].SetDataByte(2, 0x01); // irrelevant

			pis.Add((byte)PIs.PI06, new CPI((byte)PIs.PI06, 1));
			pis[(byte)PIs.PI06].SetDataByte(1, 0x33); // CHPN 3.3

			pis.Add((byte)PIs.PI08, new CPI((byte)PIs.PI08, 2));
			pis[(byte)PIs.PI08].SetDataByte(1, 0x00);
			pis[(byte)PIs.PI08].SetDataByte(2, 0x32); // 50 seconds
			}
		#endregion

		#region methods
		/// <summary>
		/// Get a CPI by its PI value
		/// </summary>
		/// <param name="pi">PI value to look for</param>
		/// <returns>The CPI if it exists, NULL otherwise</returns>
		public CPI GetPI(PIs pi)
			{
			try
				{
				CPI cpi = new CPI(pis[(byte)pi]);
				return cpi;
				}
			catch (System.Exception) { return null; }
			}
		#endregion
		}
	}
