using System.Runtime.InteropServices;
using System;
using System.Reflection;
using System.Collections.Generic;
using COMMON;

namespace CHPN.APDU
	{
	[ComVisible(true)]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	public interface IAPDU
		{
		#region properties
		/// <summary>
		/// APDU ID
		/// </summary>
		[DispId(1)]
		CAPDUID ID { get; set; }
		/// <summary>
		/// APDU bitmap
		/// </summary>
		[DispId(2)]
		CBitmap Bitmap { get; }
		/// <summary>
		/// APDU raw represenatation
		/// </summary>
		[DispId(3)]
		byte[] Rawdata { get; }
		#endregion

		#region methods
		/// <summary>
		/// Get a specific field from the list of fields inside the message
		/// </summary>
		/// <param name="field">Number of the field to get</param>
		/// <returns>The field if dound in the list (<see cref="CField"/>), null otherwise</returns>
		[DispId(100)]
		string AddField(CField field);
		//[DispId(101)]
		//bool AddFieldTLV(CFieldTLV field);
		[DispId(102)]
		bool RemoveField(int field);
		[DispId(103)]
		CField GetField(int field);
		//[DispId(104)]
		//CFieldTLV GetFieldTLV(int field);
		#endregion
		}

	[Guid("7D222689-7E82-4437-A60F-F377EAFECF79")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class CAPDU: CRawdata, IAPDU
		{
		#region constructors
		public CAPDU() { }
		/// <summary>
		/// Creates an empty APDU with an ID
		/// </summary>
		public CAPDU(short msgid) { ID = new CAPDUID(msgid); }
		/// <summary>
		/// Creates an APDU from an existing buffer building a database of fields as contained inside the APDU
		/// </summary>
		/// <param name="buffer">The buffer to scan to search the fields.</param>
		public CAPDU(byte[] buffer)
			{
			int offset = 0;
			// get message ID
			ID = new CAPDUID(buffer, offset, ref offset);
			if (0 != ID.Value)
				{
				// Get the bitmap
				Bitmap = new CBitmap(buffer, offset, ref offset);
				if (Bitmap.IsSet)
					{
					// arrived here we can process the fields described inside the bitmap
					CFields fields = new CFields();

					// offset now points to the beginning of the fields - WE ONLY CONSIDER BITMAP1 as bitmap2 fields are not described in the dictionary
					for (int field = 2; field <= 0xFF && 0 < buffer.Length - offset; field++)
						{
						if (Bitmap.IsBitSet(field))
							{
							// create and retrieve field
							//CField fg = fields.GetField(field);
							//CField f = new CField(fg.ID, fg.Type, fg.Minlen, fg.Maxlen);
							CField f = fields.GetField(field);
							if (f.SetData(buffer, offset, out offset))
								{
								// save the field inside the list of fields
								try
									{
									AddField(f);
									}
								catch (Exception ex)
									{
									CLog.AddException(MethodBase.GetCurrentMethod().Name, ex, "CHAMP: " + field);
									}
								}
							}
						}
					}
				}
			}
		#endregion

		#region properties
		/// <summary>
		/// APDU ID
		/// </summary>
		public CAPDUID ID { get; set; }
		/// <summary>
		/// Bitmap of fields
		/// </summary>
		public CBitmap Bitmap { get; private set; } = new CBitmap();
		/// <summary>
		/// Rawdata representation
		/// </summary>
		public override byte[] Rawdata
			{
			get
				{
				// get size of Rawdata
				int size = /*CAPDUID.MESSAGE_ID_SIZE*/ID.RawdataLen + Bitmap.RawdataLen; // APDU + bitmap length
				int sizefields = 0;
				foreach (KeyValuePair<int, CField> f in Fields)
					sizefields += f.Value.RawdataLen;
				// assign buffer
				byte[] ab = new byte[size + sizefields];
				int offset = 0;
				// add the message ID
				Buffer.BlockCopy(ID.Rawdata, 0, ab, offset, ID.RawdataLen);
				offset += ID.RawdataLen;
				// add the bitmap
				Buffer.BlockCopy(Bitmap.Rawdata, 0, ab, offset, Bitmap.RawdataLen);
				offset += Bitmap.RawdataLen;
				// add the fields
				if (0 != sizefields)
					foreach (KeyValuePair<int, CField> f in Fields)
						{
						if (0 != f.Value.RawdataLen)
							{
							Buffer.BlockCopy(f.Value.Rawdata, 0, ab, offset, f.Value.RawdataLen);
							offset += f.Value.RawdataLen;
							}
						}
				return ab;
				}
			}
		/// <summary>
		/// All fields contained inside the APDU
		/// </summary>
		internal FieldsDictionary Fields { get; set; } = new FieldsDictionary();
		#endregion

		#region methods
		/// <summary>
		/// Add a field to the message
		/// </summary>
		/// <param name="f">The field to add <see cref="CField"/></param>
		/// <returns>The string representation of the field</returns>
		public string AddField(CField f)
			{
			try
				{
				if (0 != f.RawdataLen)
					{
					Fields.Add(f.ID, f);
					Bitmap.SetBit(f.ID);
					return f.ToString();
					}
				else
					return string.Empty;
				}
			catch (Exception ex)
				{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
				return string.Empty;
				}
			}
		///// <summary>
		///// Add a TLV field to the message
		///// </summary>
		///// <param name="f">The field to add <see cref="CField"/></param>
		///// <returns>The string representation of the field</returns>
		//public string AddField(CFieldTLV f)
		//	{
		//	try
		//		{
		//		Fields.Add(f.ID, f);
		//		Bitmap.SetBit(f.ID);
		//		return f.ToString();
		//		}
		//	catch (Exception ex)
		//		{
		//		CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
		//		return string.Empty;
		//		}
		//	}
		/// <summary>
		/// Remove a field from the APDU
		/// </summary>
		/// <param name="field">Field id to remove</param>
		/// <returns>TRUE if removed, FALSE otherwise (it could not be there)</returns>
		public bool RemoveField(int field)
			{
			try
				{
				Fields.Remove(field);
				Bitmap.UnsetBit(field);
				return true;
				}
			catch (Exception ex)
				{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex, "CHAMP: " + field);
				return false;
				}
			}
		/// <summary>
		/// Get the requested field
		/// </summary>
		/// <param name="field">Field id to get</param>
		/// <returns>The field if found, <see langword="null"/>otherwise</returns>
		public CField GetField(int field)
			{
			try
				{
				return Fields[field];
				}
			catch (Exception ex)
				{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex, "CHAMP: " + field);
				return null;
				}
			}
		#endregion
		}
	}
