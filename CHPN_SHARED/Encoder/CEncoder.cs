using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using System;
using CHPN.Exceptions;
using COMMON;

namespace CHPN.Encoder
	{
[ComVisible(false)]
	public interface IEncoder
		{
		#region properties
		/// <summary>
		/// The character set with which the field must comply on entry or exit
		/// </summary>
		string CharacterSet { get; }
		/// <summary>
		/// The default char to use to fill up a field attached to this encoder
		/// </summary>
		char DefaultChar { get; }
		#endregion

		#region methods
		/// <summary>
		/// Encode a field from a string to a raw reprensentation
		/// </summary>
		/// <param name="buffer">The string to encode</param>
		/// <param name="minlen">The minimum length of the field</param>
		/// <param name="maxlen">The maximum length of the field</param>
		/// <param name="length">The length of the data which has been encoded</param>
		/// <returns>An raw representation of the field after encoding</returns>
		byte[] Encode(string buffer, int minlen, int maxlen, out int length);
		/// <summary>
		/// Encode an array of bytes to a raw representation
		/// </summary>
		/// <param name="buffer">The array of bytes to encode</param>
		/// <param name="minlen">The minimum length of the field</param>
		/// <param name="maxlen">The maximum length of the field</param>
		/// <param name="length">The length of the data which has been encoded</param>
		/// <returns>An raw representation of the field after encoding</returns>
		byte[] Encode(byte[] buffer, int minlen, int maxlen, out int length);
		/// <summary>
		/// Decode a raw representation to an array of bytes
		/// </summary>
		/// <param name="buffer">The raw representation to decode</param>
		/// <param name="start">The starting position to decode</param>
		/// <param name="minlen">The minimum length of the field</param>
		/// <param name="maxlen">The maximum length of the field</param>
		/// <param name="offset">The offset to the next field after the one decoded</param>
		/// <param name="rawdata">The rawdata which has been decoded</param>
		/// <returns>An array of byte containing the decoded value</returns>
		byte[] Decode(byte[] buffer, int start, int minlen, int maxlen, out int offset, out byte[] rawdata);
		/// <summary>
		/// Test whether the string on entry or exit contains valid characters accordig to the character set declared
		/// Length (min and max) is also tested
		/// </summary>
		/// <param name="value">The string to test</param>
		/// <param name="minlen">The minimum length of the field</param>
		/// <param name="maxlen">The maximum length of the field</param>
		/// <returns>TRUE if the string complies with the character set and length, FALSE otherwise</returns>
		bool IsValidFormat(string value, int minlen, int maxlen);
		/// <summary>
		/// Completes a value not fitting with the minlen and maxlen values.
		/// Completion is according to the type of the field.
		/// </summary>
		/// <param name="value">The value to eventually complete</param>
		/// <param name="minlen">The minimum length of the field</param>
		/// <param name="maxlen">The maximum length of the field</param>
		/// <param name="length">The length of the data which is to be encoded</param>
		/// <returns>The completed value</returns>
		string CompleteValue(string value, int minlen, int maxlen, out int length);
		/// <summary>
		/// Decode a raw representation to an string
		/// </summary>
		/// <param name="buffer">The raw representation to decode</param>
		/// <returns>A string representing the content of the raw part</returns>	
		string ToString(byte[] buffer);
		/// <summary>
		/// Decode a raw representation to a numeric value
		/// </summary>
		/// <param name="buffer">The raw representation to decode</param>
		/// <returns>A string representing the content of the raw part</returns>	
		long ToLong(byte[] buffer);
		#endregion
		}

	[ComVisible(false)]
	abstract class CEncoder: IEncoder
		{
		//#region constants
		//public const int NONE = 0;
		//public const int NUMERIC = 1;
		//public const int ALPHA_NUMERIC = 2;
		//public const int EXTENDED_ALPHA_NUMERIC = 3;
		//public const int BINARY = 4;
		//public const int CMC7 = 5;
		//#endregion

		#region properties
		/// <summary>
		/// 
		/// </summary>
		public virtual string CharacterSet { get => string.Empty; }
		/// <summary>
		/// 
		/// </summary>
		public virtual char DefaultChar { get => ' '; }
		#endregion

		#region encoders/decodes
		/// <summary>
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public virtual byte[] Encode(string buffer, int minlen, int maxlen, out int length)
			{
			throw new ENotImplemented(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.ToString());
			}
		/// <summary>
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public virtual byte[] Encode(byte[] buffer, int minlen, int maxlen, out int length)
			{
			throw new ENotImplemented(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.ToString());
			}
		/// <summary>
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="start"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="offset"></param>
		/// <param name="rawdata"></param>
		/// <returns></returns>
		public virtual byte[] Decode(byte[] buffer, int start, int minlen, int maxlen, out int offset, out byte[] rawdata)
			{
			throw new ENotImplemented(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.ToString());
			}
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <returns></returns>
		public bool IsValidFormat(string value, int minlen, int maxlen)
			{
			return CMisc.IsValidFormat(value, CharacterSet, minlen, maxlen);
			}
		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <param name="minlen"></param>
		/// <param name="maxlen"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public virtual string CompleteValue(string value, int minlen, int maxlen, out int length)
			{
			throw new ENotImplemented(MethodBase.GetCurrentMethod().Name, MethodBase.GetCurrentMethod().DeclaringType.ToString());
			}
		/// <summary>
		/// <see cref="IEncoder.ToString(byte[])"/>
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public virtual string ToString(byte[] buffer)
			{
			string s = null;
			try
			{
				s = Encoding.UTF8.GetString(buffer);
			}
			catch (Exception) { }
			return s;
			}
		/// <summary>
		/// <see cref="IEncoder.ToLong(byte[])"/>
		/// </summary>
		/// <param name="buffer">The raw representation to decode</param>
		/// <returns>A string representing the content of the raw part</returns>	
		public virtual long ToLong(byte[] buffer)
			{
			return CMisc.StrToLong(ToString(buffer));
			}
		/// <summary>
		/// Specific verification functions according to fields types, character sets are already defined
		/// An empty value is never accepted as valid
		/// </summary>
		/// <param name="value">The value to check</param>
		/// <param name="minlen">The minimum length the value must comply with</param>
		/// <param name="maxlen">The maximum length the value must comply with</param>
		/// <returns>TRUE if the value complies with the regular expression, FALSE otherwise</returns>
		protected static bool IsCMC7Compliant(string value, int minlen, int maxlen)
			{
			return CMisc.IsValidFormat(value, (new CEncoderCMC7()).CharacterSet, minlen, maxlen);
			}
		protected static bool IsDCBCompliant(string value, int minlen, int maxlen)
			{
			return CMisc.IsValidFormat(value, (new CEncoderDCB()).CharacterSet, minlen, maxlen);
			}
		protected static bool IsANCompliant(string value, int minlen, int maxlen)
			{
			return CMisc.IsValidFormat(value, (new CEncoderAN()).CharacterSet, minlen, maxlen);
			}
		protected static bool IsANSCompliant(string value, int minlen, int maxlen)
			{
			return CMisc.IsValidFormat(value, (new CEncoderANS()).CharacterSet, minlen, maxlen);
			}

		#endregion
		}
	}
