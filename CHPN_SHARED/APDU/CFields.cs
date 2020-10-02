using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace CHPN.APDU
	{
	[ComVisible(false)]
	public class FieldsDictionary: SortedDictionary<int, CField> { }

	[Guid("200135E4-B61A-4C26-B30B-2B61AB7871E5")]
	[ComVisible(true)]
	public interface IFields
		{
		[DispId(100)]
		CField GetField(int id);
		}

	[Guid("B305F9D6-4A7B-4F38-8C41-ED21E59089F3")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class CFields: IFields
		{
		#region properties
		private FieldsDictionary Fields = new FieldsDictionary();
		#endregion

		#region constructors
		/// <summary>
		/// Create the list of fields defined in the CHPN protocol
		/// </summary>
		public CFields()
			{
			CField f = new CField(2, FieldType.NUMERIC, 0, 19);
			Fields.Add(f.ID, f);

			f = new CField(3, FieldType.NUMERIC, 6, 6);
			Fields.Add(f.ID, f);

			f = new CField(4, FieldType.NUMERIC, 12, 12);
			Fields.Add(f.ID, f);

			f = new CField(7, FieldType.NUMERIC, 10, 10);
			Fields.Add(f.ID, f);

			f = new CField(11, FieldType.NUMERIC, 6, 6);
			Fields.Add(f.ID, f);

			f = new CField(12, FieldType.NUMERIC, 6, 6);
			Fields.Add(f.ID, f);

			f = new CField(13, FieldType.NUMERIC, 4, 4);
			Fields.Add(f.ID, f);

			f = new CField(18, FieldType.NUMERIC, 4, 4);
			Fields.Add(f.ID, f);

			f = new CField(22, FieldType.NUMERIC, 3, 3);
			Fields.Add(f.ID, f);

			f = new CField(25, FieldType.NUMERIC, 2, 2);
			Fields.Add(f.ID, f);

			f = new CField(32, FieldType.NUMERIC, 1, 11);
			Fields.Add(f.ID, f);

			f = new CField(35, FieldType.CMC7, 0, 35);
			Fields.Add(f.ID, f);

			f = new CField(37, FieldType.ALPHA_NUMERIC, 12, 12);
			Fields.Add(f.ID, f);

			f = new CField(38, FieldType.ALPHA_NUMERIC, 6, 6);
			Fields.Add(f.ID, f);

			f = new CField(39, FieldType.ALPHA_NUMERIC, 2, 2);
			Fields.Add(f.ID, f);

			f = new CField(40, FieldType.ALPHA_NUMERIC, 3, 3);
			Fields.Add(f.ID, f);

			f = new CField(41, FieldType.EXTENDED_ALPHA_NUMERIC, 8, 8);
			Fields.Add(f.ID, f);

			f = new CField(42, FieldType.EXTENDED_ALPHA_NUMERIC, 15, 15);
			Fields.Add(f.ID, f);

			f = new CField(43, FieldType.EXTENDED_ALPHA_NUMERIC, 40, 40);
			Fields.Add(f.ID, f);

			f = new CField(44, FieldType.EXTENDED_ALPHA_NUMERIC, 20, 25);
			Fields.Add(f.ID, f);

			f = new CField(45, FieldType.NUMERIC, 15, 15);
			Fields.Add(f.ID, f);

			f = new CField(46, FieldType.NUMERIC, 4, 4);
			Fields.Add(f.ID, f);

			f = new CField(47, FieldType.BINARY, 1, 65535, true);
			Fields.Add(f.ID, f);

			f = new CField(48, FieldType.BINARY, 1, 65535, true);
			Fields.Add(f.ID, f);

			f = new CField(49, FieldType.NUMERIC, 4, 4);
			Fields.Add(f.ID, f);
			}
		#endregion

		#region methods
		/// <summary>
		/// Get a copy of an existing CHPN field
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public CField GetField(int id)
			{
			try
				{
				CField f = new CField(Fields[id]);
				return f;
				}
			catch (System.Exception) { return null; }
			}
		#endregion
		}
	}
