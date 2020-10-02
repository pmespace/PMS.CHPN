using System.Runtime.InteropServices;
using System;
using System.Reflection;
using System.Collections.Generic;
using COMMON;

namespace CHPN.APDU
	{
	[ComVisible(false)]
	class CFieldTLV: CFieldGeneric
		{
		#region privates
		private class TLVDictionary: SortedDictionary<string, CTLV> { }
		private TLVDictionary _tlv = new TLVDictionary();
		#endregion

		#region properties
		/// <summary>
		/// Describes the raw representation of a type
		/// </summary>
		public override byte[] Rawdata
			{
			get
				{
				byte[] ab = null;
				// get size of all TLV
				short size = 0;
				foreach (KeyValuePair<string, CTLV> t in _tlv)
					size += (short)t.Value.RawdataLen;
				if (0 != size)
					{
					ab = new byte[size + DefaultSizeLength()];
					int offset = VARIABLE_FIELD_BINARY_LENGTH_SIZE;
					// add all TLV to Rawdata
					foreach (KeyValuePair<string, CTLV> t in _tlv)
						{
						Buffer.BlockCopy(t.Value.Rawdata, 0, Rawdata, offset, t.Value.RawdataLen);
						offset += t.Value.RawdataLen;
						}
					// Add field size
					byte[] bb = CMisc.SetBytesFromIntegralTypeValue(size);
					Buffer.BlockCopy(bb, 0, Rawdata, 0, VARIABLE_FIELD_BINARY_LENGTH_SIZE);
					}
				return ab;
				}
			}
		#endregion

		#region constructors
		public CFieldTLV() : base(FieldType.BINARY) { }
		#endregion

		#region tlv specific methods
		/// <summary>
		/// Add a TLV record to the field
		/// </summary>
		/// <param name="tlv">The fully formatted TLV record</param>
		/// <returns></returns>
		public bool AddTLV(CTLV tlv)
			{
			try
				{
				_tlv.Add(tlv.T, tlv);
				return true;
				}
			catch (Exception ex)
				{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex, "TLV: " + tlv.ToString());
				return false;
				}
			}
		/// <summary>
		/// Remove a TLV record from the field
		/// </summary>
		/// <param name="tlv">The fully formatted TLV record</param>
		/// <returns></returns>
		public bool RemoveTLV(CTLV tlv)
			{
			try
				{
				_tlv.Remove(tlv.T);
				return true;
				}
			catch (Exception ex)
				{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex, "TLV: " + tlv.ToString());
				return false;
				}
			}
		/// <summary>
		/// Get a TLV from the list of TLV
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CTLV GetTLV(string id)
			{
			try
				{
				return _tlv[id];
				}
			catch (Exception ex)
				{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex, "T: " + id);
				return null;
				}
			}
		#endregion
		}
	}
