using System.Runtime.InteropServices;
using System;
using System.Reflection;
using System.Collections.Generic;
using CHPN.APDU;
using CHPN.IPDU;
using COMMON;

namespace CHPN
{
	[ComVisible(false)]
	public static class RegexCHPN
	{
		/// <summary>
		/// Protocol formats
		/// </summary>
		public const string REGEX_TLV = @"[0-9A-Za-z]{2}";
		public const string REGEX_N = @"[0-9]";
		public const string REGEX_CMC7 = @"[0-9A-Za-z\s]";
		public const string REGEX_AN = @"[0-9A-Za-z\s]";
		public const string REGEX_ANS = @"[0-9A-Za-z\s<>:%?!=*$£&""'+-.)]";
		public const string REGEX_MMJJ = "0([0-9]?|1[0-2]?)?([0-2]?[0-9]?|3[0-1]?)?";
		public const string REGEX_MMAA = "(0[0-9]?|1[0-2]?)?[0-9]{2,2}";
		public const string REGEX_HHMMSS = "([0-1]?[0-9]?|2[0-3]?)?([0-5]?[0-9]?){2,2}";
	}

	[ComVisible(true)]
	public enum Service
	{
		Inconnu = 0,
		GarantieCheque = APDUs.APDU_9100,
		InterrogationFNCI = APDUs.APDU_9300
	}

	[ComVisible(true)]
	public enum CapaciteDeLecture
	{
		F22_MANUAL_ENTRY_KEY_NOT_VERIFIED = 1,
		F22_MANUAL_ENTRY_KEY_VERIFIED = 2,
		F22_AUTOMATIC_ENTRY = 4
	}

	[ComVisible(true)]
	public enum CodeReponseFNCI
	{
		F39_PAS_DINTERROGATION_FNCI = 0x6262,
		F39_VERT = 0,
		F39_ORANGE = 1,
		F39_ROUGE = 2,
		F39_BLANC_CHEQUE_NON_REFERENCE = 3,
		F39_BLANC_IDCF_INVALIDE = 4,
		F39_BLANC_ERREUR_SERVEUR_FNCI = 5,
		F39_BLANC_CMC7_INVALIDE = 6,
		F39_BLANC_IDC_INVALIDE = 7,
		F39_BLANC_MATERIEL_INCONNU = 8,
		F39_BLANC_SERVICE_NON_DISPONIBLE = 10,
		F39_BLANC_REFUSEE = 99,
		F39_ERREUR_INCONNUE = -1
	}

	[ComVisible(true)]
	public enum CodeReponseGarantisseur
	{
		F40_ACCEPTEE = 0,
		F40_REFUSEE = 1,
		F40_NON_TRAITEE_AUTOMATIQUEMENT = 2,
		F40_IDCF_INVALIDE = 3,
		F40_TRANSACTION_INTERDITE = 4,
		F40_SERVICE_INTERDIT = 5,
		F40_ACCEPTEE_SOUS_RESERVE = 6,
		F40_IDC_INVALIDE = 7,
		F40_SERVICE_INDISPONIBLE = 10,
		F40_ERREUR_INCONNUE = -1
	}

	[ComVisible(true)]
	public enum TypeDePieceIdentite
	{
		F43_POS5_NONE = -1,
		F43_POS5_BEGIN = F43_POS5_CARTE_IDENTITE_NATIONALE,
		F43_POS5_CARTE_IDENTITE_NATIONALE = 1,
		F43_POS5_PERMIS_CONDUIRE_NATIONAL = 2,
		F43_POS5_PASSEPORT_NATIONAL = 3,
		F43_POS5_CARTE_RESIDENT = 4,
		F43_POS5_CARTE_SEJOUR = 5,
		F43_POS5_CARTE_IDENTITE_EUROPEENE = 6,
		F43_POS5_PERMIS_CONDUIRE_EUROPEEN = 7,
		F43_POS5_PASSEPORT_EUROPEEN = 8,
		F43_POS5_AUTRE = 9,
		F43_POS5_END = F43_POS5_AUTRE,
	}

	[ComVisible(true)]
	public enum TypeDeCheque
	{
		F43_POS10_NONE = -1,
		F43_POS10_BEGIN = F43_POS10_CHEQUE_PERSONNEL,
		F43_POS10_CHEQUE_PERSONNEL = 0,
		F43_POS10_CHEQUE_SOCIETE = 1,
		F43_POS10_END = F43_POS10_CHEQUE_SOCIETE,
	}

	[Guid("DA9B8651-8E97-4D52-8BC4-FE020CAF3B49")]
	[InterfaceType(ComInterfaceType.InterfaceIsDual)]
	[ComVisible(true)]
	public interface ICHPN
	{
		[DispId(1)]
		string F02_Input { get; set; }
		[DispId(2)]
		string F02 { get; }
		[DispId(5)]
		string F03_Input { get; set; }
		[DispId(6)]
		string F03 { get; }
		[DispId(10)]
		uint F04_InputAmount { get; set; }
		[DispId(11)]
		string F04_Input { get; }
		[DispId(12)]
		string F04 { get; }
		[DispId(15)]
		string F07_Output { get; }
		[DispId(16)]
		string F07 { get; }
		[DispId(20)]
		uint F11_InputTransactionID { get; set; }
		[DispId(21)]
		string F11_Input { get; }
		[DispId(22)]
		uint F11_OutputTransactionID { get; }
		[DispId(23)]
		string F11_Output { get; }
		[DispId(25)]
		string F12_OutputLocalTime { get; }
		[DispId(26)]
		string F12 { get; }
		[DispId(27)]
		string F13_OutputLocalDate { get; }
		[DispId(28)]
		string F13 { get; }
		[DispId(29)]
		string F13_F12_Input { get; }
		[DispId(30)]
		string F18_Input { get; }
		[DispId(31)]
		string F18 { get; }
		[DispId(35)]
		string F22_Input { get; set; }
		[DispId(36)]
		string F22 { get; }
		[DispId(40)]
		string F25_Input { get; set; }
		[DispId(41)]
		string F25 { get; }
		[DispId(45)]
		string F32_InputBankID { get; set; }
		[DispId(46)]
		string F32_Input { get; }
		[DispId(47)]
		string F32 { get; }
		[DispId(50)]
		string F35_InputCMC7 { get; set; }
		[DispId(51)]
		string F35_Input { get; }
		[DispId(52)]
		string F35 { get; }
		[DispId(55)]
		string F37_InputIDC { get; set; }
		[DispId(56)]
		string F37_Input { get; }
		[DispId(57)]
		string F37 { get; }
		[DispId(60)]
		string F38_OutputGuaranteeSignature { get; }
		[DispId(61)]
		string F38_Output { get; }
		[DispId(62)]
		string F38 { get; }
		[DispId(65)]
		string F39_OutputFnciResponseCode { get; }
		[DispId(66)]
		string F39_OutputFnciResponseCodeAsString { get; }
		[DispId(67)]
		CodeReponseFNCI F39_OutputFnciResponseCodeAsCode { get; }
		[DispId(68)]
		string F39_Output { get; }
		[DispId(69)]
		string F39 { get; }
		[DispId(70)]
		string F40_OutputGuaranteeResponseCode { get; }
		[DispId(71)]
		string F40_OutputGuaranteeResponseCodeAsString { get; }
		[DispId(72)]
		CodeReponseGarantisseur F40_OutputGuaranteeResponseCodeAsCode { get; }
		[DispId(73)]
		string F40_Output { get; }
		[DispId(74)]
		string F40 { get; }
		[DispId(75)]
		uint F41_InputTerminalID { get; set; }
		[DispId(76)]
		string F41_Input { get; }
		[DispId(77)]
		string F41 { get; }
		[DispId(80)]
		string F42_InputIDCF { get; set; }
		[DispId(81)]
		string F42_Input { get; }
		[DispId(82)]
		string F42 { get; }
		[DispId(85)]
		string F43_InputGuaranteeBirthDate { get; set; }
		[DispId(86)]
		TypeDePieceIdentite F43_InputGuaranteeIDType { get; set; }
		[DispId(87)]
		string F43_InputGuaranteeIDTypeDate { get; set; }
		[DispId(88)]
		TypeDeCheque F43_InputGuaranteeCheckType { get; set; }
		[DispId(89)]
		string F43_Input { get; }
		[DispId(90)]
		string F43_Output { get; }
		[DispId(91)]
		string F43 { get; }
		[DispId(95)]
		string F44_OutputFnciMessage { get; }
		[DispId(96)]
		uint F44_OutputFnciCounterIntraDay { get; }
		[DispId(97)]
		uint F44_OutputFnciCounterN { get; }
		[DispId(98)]
		string F44_OutputRLMC { get; }
		[DispId(99)]
		string F44_OutputFnciSignature { get; }
		[DispId(100)]
		uint F44_OutputFnciCounterX { get; }
		[DispId(101)]
		string F44_Output { get; }
		[DispId(102)]
		string F44 { get; }
		[DispId(105)]
		int F45_InputManufacturerID { get; }
		[DispId(106)]
		int F45_InputDeviceType { get; }
		[DispId(107)]
		int F45_InputSoftwareID { get; }
		[DispId(108)]
		string F45_Input { get; }
		[DispId(109)]
		string F45 { get; }
		[DispId(110)]
		string F46_Input { get; }
		[DispId(111)]
		string F46 { get; }
		[DispId(200)]
		string F47_Output { get; }
		[DispId(201)]
		string F47 { get; }
		[DispId(300)]
		string F48_Output { get; }
		[DispId(301)]
		string F48 { get; }
		[DispId(115)]
		string F49_Input { get; }
		[DispId(116)]
		string F49 { get; }
		[DispId(500)]
		string DescribeMessage(string title, CIPDU ipdu, bool underline = true);
		[DispId(501)]
		CIPDU PrepareMessage(Service service);
		[DispId(510)]
		CIPDU SendMessage(CIPDU request, string ip, uint port, string servername, int sendtimeout, int receivetimeout);
		[DispId(511)]
		CIPDU SendMessage(CIPDU request, CStreamClientSettings settings);
		[DispId(550)]
		string TypeDeChequeAsString(TypeDeCheque i);
		[DispId(551)]
		string TypeDePieceIdentiteAsString(TypeDePieceIdentite i);
		[DispId(552)]
		string CBCOMResultAsString(CPI pi);
		[DispId(553)]
		string CodeReponseGarantisseurAsString(string s, out CodeReponseGarantisseur code, string IDCF = "", string IDC = "");
		[DispId(554)]
		string CodeReponseFNCIAsString(string s, out CodeReponseFNCI code, string CMC7 = "", string IDCF = "", string IDC = "", string device = "");
	}

	[Guid("975A515A-6214-49D2-A715-6CDA73F216E2")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComVisible(true)]
	public class CCHPN : ICHPN
	{
		#region constructors
		/// <summary>
		/// Constructor loads merchant settings
		/// </summary>
		public CCHPN() { }
		#endregion

		#region constants
		private const string F02_DEFAULT = "";
		private const string F03_DEFAULT = "000000";
		private const string F18_DEFAULT = "9999";
		private const string F22_DEFAULT = "042";
		private const string F25_DEFAULT = "00";
		private const string F32_DEFAULT = "00000";
		private const string F45_DEFAULT = "999330000001001";
		private const string F46_DEFAULT = "0300";
		private const string F49_DEFAULT = "978";
		#endregion

		#region consts
		private const string REGEX_HHMMSS = @"^([0-1]{1}[0-9]{1}|2[0-3]{1})[0-5]{1}[0-9]{1}[0-5]{1}[0-9]{1}$";
		private const string REGEX_MMYY = @"^(0[0-9]{1}|1[0-2]{1})[0-9]{2}$";
		private const string REGEX_MMDD = @"^(((0[13578]{1}|1[02]{1})(0[1-9]{1}|[1-2]{1}[0-9]{1}|3[0-1]{1}))|((0[469]{1})|(11)(0[1-9]{1}|[1-2]{1}[0-9]{1}|30))|(02(0[1-9]{1}|[1-2]{1}[0-9]{1})))$";
		#endregion

		#region properties
		private CPIs _pis = new CPIs();
		private CFields _fields = new CFields();
		/// <summary>
		/// Field 2
		/// </summary>
		public string F02_Input
		{
			get => _f02_input;
			set => _f02_input = CheckValue(value, @"^[0-9]{1,19}$", F02_DEFAULT);
		}
		private string _f02_input = F02_DEFAULT;
		public string F02 { get; private set; }
		/// <summary>
		/// Field 3
		/// </summary>
		public string F03_Input
		{
			get => _f03_input;
			set => _f03_input = CheckValue(value, @"^0[01]{1}0000$", F03_DEFAULT);
		}
		private string _f03_input = F03_DEFAULT;
		public string F03 { get; private set; }
		/// <summary>
		/// Field 4
		/// </summary>
		public uint F04_InputAmount { get; set; }
		public string F04_Input { get => F04_InputAmount.ToString("000000000000"); }
		public string F04 { get; private set; }
		/// <summary>
		/// Field 7
		/// </summary>
		public string F07_Output { get; private set; }
		public string F07 { get; private set; }
		/// <summary>
		/// Field 11
		/// </summary>
		public uint F11_InputTransactionID
		{
			get => _f11_inputtransactionid;
			set => _f11_inputtransactionid = (9999 < value ? 1 : (1 > value ? 1 : value));
		}
		private uint _f11_inputtransactionid = 1;
		public string F11_Input { get => F11_InputTransactionID.ToString("000000"); }
		public uint F11_OutputTransactionID { get; private set; }
		public string F11_Output { get; private set; }
		public string F11 { get; private set; }
		/// <summary>
		/// Last transaction date and time as set by the system
		/// Format is MMDDhhmmss or empty if no transaction - F13+12
		/// </summary>
		public string F12_OutputLocalTime { get => F12; }
		public string F12 { get; private set; }
		public string F13_OutputLocalDate { get => F13; }
		public string F13 { get; private set; }
		public string F13_F12_Input
		{
			get => F13_OutputLocalDate + F12_OutputLocalTime;
		}
		/// <summary>
		/// Field 18
		/// </summary>
		public string F18_Input { get => F18_DEFAULT; }
		public string F18 { get; private set; }
		/// <summary>
		/// Field 22
		/// </summary>
		public string F22_Input
		{
			get => _f22_input;
			set => _f22_input = CheckValue(value, @"^0[124]{1}2$", F22_DEFAULT);
		}
		private string _f22_input = F22_DEFAULT;
		public string F22 { get; private set; }
		/// <summary>
		/// Field 25
		/// </summary>
		public string F25_Input
		{
			get => _f25_input;
			set => _f25_input = CheckValue(value, @"^0[01]{1}$", F25_DEFAULT);
		}
		private string _f25_input = F25_DEFAULT;
		public string F25 { get; private set; }
		/// <summary>
		/// Field F32
		/// </summary>
		public string F32_InputBankID
		{
			get => _f32_inputbankid;
			set => _f32_inputbankid = CheckValue(value, @"^000000[0-9]{5}$", F32_DEFAULT);
		}
		private string _f32_inputbankid = F32_DEFAULT;
		public string F32_Input { get => F32_InputBankID; }
		public string F32 { get; private set; }
		/// <summary>
		/// Field 35
		/// </summary>
		public string F35_InputCMC7
		{
			get => _f35_cmc7;
			set
			{
				// replace S1
				value = value.Replace((char)0x2F, 'B');
				// replace S2
				//value = value.Replace((char)0x2B, )
				// replace S3
				value = value.Replace((char)0x3D, 'D');
				// replace S4
				//value = value.Replace((char)0x23, )
				// replace S5
				value = value.Replace((char)0x5E, 'F');
				// replace invalid char
				value = value.Replace((char)0x3F, 'A');
				_f35_cmc7 = CheckValue(value, @"^[0-9ABDF]{1,35}$", string.Empty);
			}
		}
		private string _f35_cmc7 = string.Empty;
		public string F35_Input { get => F35_InputCMC7; }
		public string F35 { get; private set; }
		/// <summary>
		/// Field 37
		/// </summary>
		public string F37_InputIDC
		{
			get => _f37_inputidc;
			set
			{
				_f37_inputidc = CheckValue(value.ToUpper(), @"^[0-9A-Z]{10}[\s]{0,2}$", string.Empty);
				_f37_inputidc = PadRight(_f37_inputidc, 12, ' ');
			}
		}
		private string _f37_inputidc = string.Empty;
		public string F37_Input { get => F37_InputIDC; }
		public string F37 { get; private set; }
		/// <summary>
		/// Field 38
		/// </summary>
		public string F38_OutputGuaranteeSignature { get => F38_Output; }
		public string F38_Output { get; private set; }
		public string F38 { get; private set; }
		/// <summary>
		/// Field 39
		/// </summary>
		public string F39_OutputFnciResponseCode { get; private set; } = string.Empty;
		public string F39_OutputFnciResponseCodeAsString { get; private set; } = string.Empty;
		public CodeReponseFNCI F39_OutputFnciResponseCodeAsCode { get; private set; } = CodeReponseFNCI.F39_BLANC_REFUSEE;
		public string F39_Output
		{
			get => _f39_output;
			private set
			{
				_f39_output = value;
				F39_OutputFnciResponseCode = F39_Output;
				CodeReponseFNCI c;
				F39_OutputFnciResponseCodeAsString = CodeReponseFNCIAsString(F39_OutputFnciResponseCode, out c, F35_InputCMC7, F42_InputIDCF, F37_InputIDC, F45_Input);
				F39_OutputFnciResponseCodeAsCode = c;
			}
		}
		private string _f39_output = string.Empty;
		public string F39 { get; private set; }
		/// <summary>
		/// Field 40
		/// </summary>
		public string F40_OutputGuaranteeResponseCode { get; private set; } = string.Empty;
		public string F40_OutputGuaranteeResponseCodeAsString { get; private set; } = string.Empty;
		public CodeReponseGarantisseur F40_OutputGuaranteeResponseCodeAsCode { get; private set; } = CodeReponseGarantisseur.F40_REFUSEE;
		public string F40_Output
		{
			get => _f40_output;
			private set
			{
				_f40_output = value;
				CodeReponseGarantisseur c;
				F40_OutputGuaranteeResponseCode = F40_Output;
				F40_OutputGuaranteeResponseCodeAsString = CodeReponseGarantisseurAsString(F40_OutputGuaranteeResponseCode, out c, F42_InputIDCF, F37_InputIDC);
				F40_OutputGuaranteeResponseCodeAsCode = c;
			}
		}
		private string _f40_output = string.Empty;
		public string F40 { get; private set; }
		/// <summary>
		/// Field 41
		/// </summary>
		public uint F41_InputTerminalID
		{
			get => _f41_input;
			set => _f41_input = (999 < value ? 0 : value);
		}
		private uint _f41_input = 0;
		public string F41_Input { get => PadRight(F41_InputTerminalID.ToString("000"), 8, ' '); }
		public string F41 { get; private set; }
		/// <summary>
		/// Field 42
		/// </summary>
		public string F42_InputIDCF
		{
			get => _f42_inputidcf;
			set
			{
				_f42_inputidcf = CheckValue(value, @"^[01]{1}([0-9]{14}|[0-9a-zA-Z]{10})$", string.Empty);
				_f42_inputidcf = PadRight(_f42_inputidcf, 15, ' ');
			}
		}
		private string _f42_inputidcf = string.Empty;
		public string F42_Input { get => F42_InputIDCF; }
		public string F42 { get; private set; }
		/// <summary>
		/// Field43
		/// </summary>
		public string F43_InputGuaranteeBirthDate
		{
			get => _f43_input_pos1to4;
			set => _f43_input_pos1to4 = CheckValue(value, REGEX_MMYY, string.Empty);
		}
		private string _f43_input_pos1to4 = string.Empty;
		public TypeDePieceIdentite F43_InputGuaranteeIDType
		{
			get => _f43_input_pos5;
			set => _f43_input_pos5 = ((TypeDePieceIdentite.F43_POS5_BEGIN <= value && TypeDePieceIdentite.F43_POS5_END >= value) || TypeDePieceIdentite.F43_POS5_NONE == value ? value : TypeDePieceIdentite.F43_POS5_NONE);
		}
		private TypeDePieceIdentite _f43_input_pos5 = TypeDePieceIdentite.F43_POS5_NONE;
		public string F43_InputGuaranteeIDTypeDate
		{
			get => _f43_input_pos6to9;
			set => _f43_input_pos6to9 = CheckValue(value, REGEX_MMYY, string.Empty);
		}
		private string _f43_input_pos6to9 = string.Empty;
		public TypeDeCheque F43_InputGuaranteeCheckType
		{
			get => _f43_input_pos10;
			set => _f43_input_pos10 = ((TypeDeCheque.F43_POS10_BEGIN <= value && TypeDeCheque.F43_POS10_END >= value) || TypeDeCheque.F43_POS10_NONE == value ? value : TypeDeCheque.F43_POS10_NONE);
		}
		private TypeDeCheque _f43_input_pos10 = TypeDeCheque.F43_POS10_NONE;
		public string F43_Input
		{
			get => PadLeft(F43_InputGuaranteeBirthDate, 4, ' ', true) + (TypeDePieceIdentite.F43_POS5_NONE == F43_InputGuaranteeIDType ? " " : ((int)F43_InputGuaranteeIDType).ToString("0")) + PadLeft(F43_InputGuaranteeIDTypeDate, 4, ' ', true) + (TypeDeCheque.F43_POS10_NONE == F43_InputGuaranteeCheckType ? " " : ((int)F43_InputGuaranteeCheckType).ToString()) + PadLeft(string.Empty, 30, ' ', true);
		}
		public string F43_OutputGuaranteeMessage { get; private set; } = string.Empty;
		public string F43_Output
		{
			get => _f43_output;
			private set
			{
				_f43_output = value;
				try
				{
					F43_OutputGuaranteeMessage = F43_Output.Substring(0, 15);
				}
				catch (Exception)
				{
					F43_OutputGuaranteeMessage = string.Empty;
				}
			}
		}
		private string _f43_output = string.Empty;
		public string F43 { get; private set; }
		/// <summary>
		/// Field 44
		/// </summary>
		public string F44_OutputFnciMessage
		{
			get
			{
				try
				{
					return F44_Output.Substring(0, 10);
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}
		}
		public uint F44_OutputFnciCounterIntraDay
		{
			get
			{
				try
				{
					return (uint)CMisc.StrToLong(F44_Output.Substring(10, 2));
				}
				catch (Exception)
				{
					return 0;
				}
			}
		}
		public uint F44_OutputFnciCounterN
		{
			get
			{
				try
				{
					return (uint)CMisc.StrToLong(F44_Output.Substring(12, 2));
				}
				catch (Exception)
				{
					return 0;
				}
			}
		}
		public string F44_OutputRLMC
		{
			get
			{
				try
				{
					return F44_Output.Substring(14, 2);
				}
				catch (Exception)
				{
					return String.Empty;
				}
			}
		}
		public string F44_OutputFnciSignature
		{
			get
			{
				try
				{
					return F44_Output.Substring(16, 4);
				}
				catch (Exception)
				{
					return String.Empty;
				}
			}
		}
		public uint F44_OutputFnciCounterX
		{
			get
			{
				try
				{
					return (uint)CMisc.StrToLong(F44_Output.Substring(20, 2));
				}
				catch (Exception)
				{
					return 0;
				}
			}
		}
		public string F44_Output { get; private set; } = string.Empty;
		public string F44 { get; private set; }
		/// <summary>
		/// Field 45
		/// </summary>
		public int F45_InputManufacturerID
		{
			get => _f45_input_pos1to3;
			set => _f45_input_pos1to3 = (0 <= value && 999 >= value ? value : 0);
		}
		private int _f45_input_pos1to3 = 999;
		public int F45_InputDeviceType
		{
			get => _f45_input_pos7to12;
			set => _f45_input_pos7to12 = (0 <= value && 999999 >= value ? value : 0);
		}
		private int _f45_input_pos7to12 = 1;
		public int F45_InputSoftwareID
		{
			get => _f45_input_pos13to15;
			set => _f45_input_pos13to15 = (0 <= value && 999 >= value ? value : 0);
		}
		private int _f45_input_pos13to15 = 1;
		public string F45_Input
		{
			get => F45_InputManufacturerID.ToString("000") + "330" + F45_InputDeviceType.ToString("000000") + F45_InputSoftwareID.ToString("000");
		}
		public string F45 { get; private set; }
		/// <summary>
		/// Field 46
		/// </summary>
		public string F46_Input { get => F46_DEFAULT; }
		public string F46 { get; private set; }
		/// <summary>
		/// Field 47
		/// </summary>
		public string F47_Output { get => F47; }
		public string F47 { get; private set; }
		/// <summary>
		/// Field 48
		/// </summary>
		public string F48_Output { get => F48; }
		public string F48 { get; private set; }
		/// <summary>
		/// Field 49
		/// </summary>
		public string F49_InputCurrency { get => F49_Input; }
		public string F49_Input { get => F49_DEFAULT; }
		public string F49 { get; private set; }
		#endregion

		#region methods
		/// <summary>
		/// Complete and return a field property's value, completed according to the field definition of the protocol
		/// This is for a get statement
		/// </summary>
		/// <param name="value">The value to complete</param>
		/// <param name="field">The field to consider to complete the value</param>
		/// <returns>The completed value</returns>
		private string CompletedValue(string value, CField field)
		{
			return field.DefaultEncoder().CompleteValue(value, field.Minlen, field.Maxlen, out int length);
		}
		/// <summary>
		/// Set a field value according to a protocol defined field
		/// </summary>
		/// <param name="value">The value which is to be assigned and completed to form a valid value for the property</param>
		/// <param name="field">The reference field to use to complete the value</param>
		/// <param name="head">A potential header to the value</param>
		/// <param name="trail">A potential trailer to the value</param>
		/// <returns>The string containing the final value of the property</returns>
		private string SetValue(string value, CField field, string head = "", string trail = "")
		{
			// an empty string is accepted to reset any data
			if (string.IsNullOrEmpty(value))
				return string.Empty;
			// otherwise complete the string according to the field
			value = CompletedValue(head + value + trail, field);
			// now test the value, it has to comply with the field requirements (character set, length)
			if (CMisc.IsValidFormat(value, head + field.DefaultEncoder().CharacterSet + trail, field.Minlen - head.Length - trail.Length, field.Maxlen - head.Length - trail.Length, true))
			{
				// the value is compliant, return it
				return value;
			}
			else
			{
				// the value does not comply with the field's requirements, return an empty string
				return string.Empty;
			}
		}
		/// <summary>
		/// Create a time compatible with protocol
		/// </summary>
		/// <returns>Current time in format hhmmss</returns>
		private string CreateTime(DateTime dt)
		{
			return dt.Hour.ToString("00") + dt.Minute.ToString("00") + dt.Second.ToString("00");
		}
		/// <summary>
		/// Create a date compatible with protocol
		/// </summary>
		/// <returns>Current date in format MMDD</returns>
		private string CreateDate(DateTime dt)
		{
			return dt.Month.ToString("00") + dt.Day.ToString("00");
		}
		/// <summary>
		/// Reset all fields produced by the library
		/// </summary>
		private void ResetAllFields()
		{
			F02 = F03 = F04 = F07 = F11 = F12 = F13 = F18 = F22 = F25 = F32 = F35 = F37 = F38 = F39 = F40 = F41 = F42 = F43 = F44 = F45 = F46 = F47 = F48 = F49 = string.Empty;
		}
		/// <summary>
		/// Reset all output fields
		/// </summary>
		private void ResetOutputFields()
		{
			ResetAllFields();
			F07_Output = string.Empty;
			F11_Output = string.Empty;
			F38_Output = string.Empty;
			F39_Output = string.Empty;
			F40_Output = string.Empty;
			F43_Output = string.Empty;
			F44_Output = string.Empty;
		}
		/// <summary>
		/// Create a field from its template and set its value from a string
		/// </summary>
		/// <param name="f"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private CField CreateField(int f, string value)
		{
			CField field = _fields.GetField(f);
			field.SetData(value);
			return field;
		}
		/// <summary>
		/// Create a field from its template and set its value from a numeric value
		/// </summary>
		/// <param name="f"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		private CField CreateField(int f, uint value)
		{
			CField field = _fields.GetField(f);
			field.SetData(value);
			return field;
		}
		/// <summary>
		/// Retrieve the raw value of a field
		/// </summary>
		/// <param name="ipdu">IPDU to search for</param>
		/// <param name="i">Field ID to look for</param>
		/// <returns>A string representing the raw representation of the field or an empty string if that later one doesn' exist</returns>
		private string FieldToString(CIPDU ipdu, int i)
		{
			CField field;
			if (null != (field = ipdu.APDU.GetField(i)))
				return field.ToString();
			return string.Empty;
		}
		/// <summary>
		/// Retrieve a string representation of a field's data (not including the length)
		/// </summary>
		/// <param name="ipdu">IPDU to search for</param>
		/// <param name="i">Field ID to look for</param>
		/// <returns>A string representing the string representation of the field's data or an empty string if that later one doesn' exist</returns>
		private string FieldToData(CIPDU ipdu, int i)
		{
			CField field;
			return (null != (field = ipdu.APDU.GetField(i)) ? field.Data : string.Empty);
		}
		/// <summary>
		/// Retrieve a string representation of a field's data (not including the length) from 2 IPDU.
		/// If the field is not found in the first IPDU (aka te response), it is searched for in a second one (aka the request)
		/// </summary>
		/// <param name="response">First IPDU to search for</param>
		/// <param name="request">Second IPDU to search for</param>
		/// <param name="i">Field ID to look for</param>
		/// <returns>A string representing the string representation of the field's data or an empty string if that later one doesn' exist</returns>
		private string FieldToDataEx(CIPDU response, CIPDU request, int i)
		{
			string s;
			if (string.IsNullOrEmpty((s = FieldToData(response, i))))
				return FieldToData(request, i);
			return s;
		}
		/// <summary>
		/// Provides a text description of a CHPN message
		/// </summary>
		/// <param name="title">A title to apply to the description</param>
		/// <param name="ipdu">The IPDU to describe</param>
		/// <param name="underline">Indicates whether to add borders around the title or not</param>
		/// <returns></returns>
		public string DescribeMessage(string title, CIPDU ipdu, bool underline = true)
		{
			string msg = string.Empty;
			if (null != ipdu)
			{
				// create a title
				if (!string.IsNullOrEmpty(title) && underline)
				{
					string header = new string('+', title.Length + 4);
					msg += header + Chars.CRLF + "+ " + title.ToUpper() + " +" + Chars.CRLF + header + Chars.CRLF;
				}
				else if (underline)
					msg += "++++++++++" + Chars.CRLF;
				// IPDU details
				msg += "Longueur totale: " + ipdu.TotalLength.ToString() + " octets" + Chars.CRLF;
				msg += "IPDU => PGI: " + ipdu.PGI.ToString("X2") + " - LGI: " + ipdu.LGI.ToString("X2") + Chars.CRLF;
				// IPDU PI
				foreach (KeyValuePair<byte, CPI> pi in ipdu.PI)
					msg += " + PI" + pi.Value.PI.ToString("X2") + " - Longueur: " + pi.Value.PILen.ToString() + " - Valeur: " + CMisc.BytesToHexStr(pi.Value.Data) + Chars.CRLF;

				//APDU details
				msg += "APDU => ID: " + ipdu.APDU.ID + " - Bitmap: " + ipdu.APDU.Bitmap.ToString() + Chars.CRLF;

				//// display bitmap in bits
				//int v = CBitmap.BITMAP_NB_BITS;
				//for (int i = 1; i <= v; i++)
				//	{
				//	int b = i / 10;
				//	msg += (0 == b ? " " : b.ToString("0"));
				//	}
				//msg += CRLF;
				//for (int i = 1; i <= v; i++)
				//	msg += i % 10;
				//msg += CRLF;
				//for (int i = 0; i < v; i++)
				//	msg += (ipdu.APDU.Bitmap.IsBitSet(i + 1) ? "X" : " ");
				//msg += CRLF;

				// APDU fields
				foreach (KeyValuePair<int, CField> fi in ipdu.APDU.Fields)
					msg += " + CHAMP " + fi.Key.ToString("00") + " - Valeur: " + CMisc.BytesToHexStr(fi.Value.Rawdata) + " (" + fi.Value.Data + ")" + Chars.CRLF;

				msg += "IPDU complète: " + ipdu.ToString() + Chars.CRLF;
				msg += "APDU complète: " + ipdu.APDU.ToString() + Chars.CRLF;
			}
			return msg;
		}
		/// <summary>
		/// Create an IPDU with the requested APDU.
		/// All values must be set before creating the IPDU
		/// </summary>
		/// <param name="service">The request to create</param>
		/// <returns>A buffer containing the full IPDU or NULL if an error occured</returns>
		public CIPDU PrepareMessage(Service service)
		{
			CLog.Add(">>> Service demandé: " + service.ToString());
			if (Service.GarantieCheque != (Service)service && Service.InterrogationFNCI != (Service)service)
				return null;
			ResetOutputFields();
			CIPDU request = new CIPDU((byte)IPDUs.IPDU_DE);
			try
			{
				// Add the CHPN standard PI
				request.AddPI(_pis.GetPI(PIs.PI04));
				request.AddPI(_pis.GetPI(PIs.PI05));
				request.AddPI(_pis.GetPI(PIs.PI06));
				// Create the APDU and set fields inside
				CAPDU apdu = new CAPDU((short)service);
				F02 = apdu.AddField(CreateField(2, F02_Input));
				F03 = apdu.AddField(CreateField(3, F03_Input));
				F04 = apdu.AddField(CreateField(4, F04_Input));
				F11 = apdu.AddField(CreateField(11, F11_Input));
				DateTime dt = DateTime.Now;
				F12 = apdu.AddField(CreateField(12, CreateTime(dt)));
				F13 = apdu.AddField(CreateField(13, CreateDate(dt)));
				F18 = apdu.AddField(CreateField(18, F18_Input));
				F22 = apdu.AddField(CreateField(22, F22_Input));
				F25 = apdu.AddField(CreateField(25, F25_Input));
				F32 = apdu.AddField(CreateField(32, F32_Input));
				F35 = apdu.AddField(CreateField(35, F35_Input));
				F37 = apdu.AddField(CreateField(37, F37_Input));
				F41 = apdu.AddField(CreateField(41, F41_Input));
				F42 = apdu.AddField(CreateField(42, F42_Input));
				F45 = apdu.AddField(CreateField(45, F45_Input));
				F46 = apdu.AddField(CreateField(46, F46_Input));
				F49 = apdu.AddField(CreateField(49, F49_Input));
				// Guarantee only fields
				if (Service.GarantieCheque == (Service)service)
				{
					F43 = apdu.AddField(CreateField(43, (F43_Input ?? string.Empty)));
				}
				request.APDU = apdu;
				CLog.Add("APDU à envoyer - Message: " + apdu.ID.ToString() + " - Bitmap: " + apdu.Bitmap.ToString() + " - Rawdata: " + apdu.ToString());
				CLog.Add("IPDU à envoyer: " + request.ToString());
				return request;
			}
			catch (Exception ex)
			{
				CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
				return null;
			}
		}
		/// <summary>
		/// Create the IP address and try to send data
		/// </summary>
		/// <param name="request">IPDU to send</param>
		/// <param name="ip">IP address or URL to reach</param>
		/// <param name="port">Port to reach</param>
		/// <param name="servername">Server name to authenticate against</param>
		/// <param name="sendtimeout">Timer to use to send data</param>
		/// <param name="receivetimeout">Timer to use to receive data</param>
		/// <returns>A buffer containing the response or NULL if an error has occured</returns>
		public CIPDU SendMessage(CIPDU request, string ip, uint port, string servername, int sendtimeout, int receivetimeout)
		{
			CStreamClientSettings streamSettings = new CStreamClientSettings(ip, port);
			streamSettings.ServerName = servername;
			streamSettings.CheckCertificate = !string.IsNullOrEmpty(servername);
			streamSettings.SendTimeout = sendtimeout;
			streamSettings.ReceiveTimeout = receivetimeout;
			return SendMessage(request, streamSettings);
		}
		/// <summary>
		/// Send a message using the specified settings
		/// </summary>
		/// <param name="request">IPDU to send</param>
		/// <param name="settings">Settings to use</param>
		/// <returns>A buffer containing the response or NULL if an error has occured</returns>
		public CIPDU SendMessage(CIPDU request, CStreamClientSettings settings)
		{
			byte[] buffer;
			CIPDU response = null;
			if (null != (buffer = CStream.ConnectSendReceive(settings, request.Rawdata, false, out int replySize, out bool timeout)))
				response = RetrieveMessage(request, buffer);
			return response;
		}
		/// <summary>
		/// Retrieve all data from a received message
		/// </summary>
		/// <param name="request">IPDU sent to the server</param>
		/// <param name="buffer">The buffer received from the server, containing the response IPDU</param>
		/// <returns>The IPDU received in return, NULL if an error occurred</returns>
		private CIPDU RetrieveMessage(CIPDU request, byte[] buffer)
		{
			ResetOutputFields();
			// try to convert the buffer to an IPDU
			CIPDU response = new CIPDU(buffer);
			if (null != response && null != request)
			{
				try
				{
					CLog.Add("IPDU reçue: " + response.ToString());
					if ((byte)IPDUs.IPDU_DE == response.PGI)
					{
						CLog.Add("APDU reçue - Message: " + response.APDU.ID.ToString() + " - Bitmap: " + response.APDU.Bitmap.ToString() + " - Rawdata: " + response.APDU.ToString());
						F02 = FieldToString(response, 2);
						F03 = FieldToString(response, 3);
						F04 = FieldToString(response, 4);
						F07 = FieldToString(response, 7);
						F07_Output = FieldToData(response, 7);
						F11 = FieldToString(response, 11);
						F11_Output = FieldToData(response, 11);
						F11_OutputTransactionID = (uint)CMisc.StrToLong(F11_Output);
						F12 = FieldToString(response, 12);
						F13 = FieldToString(response, 13);
						F32 = FieldToString(response, 32);
						F35 = FieldToString(response, 35);
						if ((short)APDUs.APDU_9110 == response.APDU.ID.Value)
						{
							F38 = FieldToString(response, 38);
							F38_Output = FieldToData(response, 38);
						}
						F39 = FieldToString(response, 39);
						F39_Output = FieldToData(response, 39);
						F40 = FieldToString(response, 40);
						F40_Output = FieldToData(response, 40);
						F41 = FieldToString(response, 41);
						F42 = FieldToString(response, 42);
						if ((short)APDUs.APDU_9110 == response.APDU.ID.Value)
						{
							F43 = FieldToString(response, 43);
							F43_Output = FieldToData(response, 43);
						}
						F44 = FieldToString(response, 44);
						F44_Output = FieldToData(response, 44);
						F45 = FieldToString(response, 45);
						F47 = FieldToString(response, 47);
						F48 = FieldToString(response, 48);
						F49 = FieldToString(response, 49);
					}
					else
					{
						CLog.Add("+++ Erreur CBCOM +++ ", TLog.ERROR);
					}
				}
				catch (Exception ex)
				{
					CLog.AddException(MethodBase.GetCurrentMethod().Name, ex);
				}
			}
			else
			{
				CLog.Add("+++ Aucune donnée reçue +++");
			}
			return response;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public string TypeDeChequeAsString(TypeDeCheque i)
		{
			switch (i)
			{
				case TypeDeCheque.F43_POS10_CHEQUE_PERSONNEL:
					return "Chèque personnel";
				case TypeDeCheque.F43_POS10_CHEQUE_SOCIETE:
					return "Chèque de société";
				default:
					return "ERREUR, TYPE DE CHÈQUE INCONNU";
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		public string TypeDePieceIdentiteAsString(TypeDePieceIdentite i)
		{
			switch (i)
			{
				case TypeDePieceIdentite.F43_POS5_CARTE_IDENTITE_NATIONALE:
					return "Carte d'identité nationale";
				case TypeDePieceIdentite.F43_POS5_PERMIS_CONDUIRE_NATIONAL:
					return "Permis de conduire national";
				case TypeDePieceIdentite.F43_POS5_PASSEPORT_NATIONAL:
					return "Passeport national";
				case TypeDePieceIdentite.F43_POS5_CARTE_RESIDENT:
					return "Carte de résident";
				case TypeDePieceIdentite.F43_POS5_CARTE_SEJOUR:
					return "Carte de séjour";
				case TypeDePieceIdentite.F43_POS5_CARTE_IDENTITE_EUROPEENE:
					return "Carte d'identité européenne";
				case TypeDePieceIdentite.F43_POS5_PERMIS_CONDUIRE_EUROPEEN:
					return "Permis de conduire européen";
				case TypeDePieceIdentite.F43_POS5_PASSEPORT_EUROPEEN:
					return "Passeport européen";
				case TypeDePieceIdentite.F43_POS5_AUTRE:
					return "Autre";
				default:
					return "ERREUR, TYPE DE PIÈCE INCONNU";
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pi"></param>
		/// <returns></returns>
		public string CBCOMResultAsString(CPI pi)
		{
			if ((byte)PIs.PI01 == pi.PI)
			{
				switch (pi.Data[0])
				{
					case (byte)PI01ErrorCodes.PI01_NOERROR:
						return "Succes";
					case (byte)PI01ErrorCodes.PI01_INVALID_PGI:
						return "PGI invalide";
					case (byte)PI01ErrorCodes.PI01_MANDATORY_PARAMETER_IS_MISSING:
						return "Paramètre manquant";
					case (byte)PI01ErrorCodes.PI01_ACCESS_POINT_FULL:
						return "Point d'accès inatteignable";
					case (byte)PI01ErrorCodes.PI01_SYNCH_ERROR:
						return "Erreur de synchronisation";
					case (byte)PI01ErrorCodes.PI01_SERVICE_HAS_STOPPED:
						return "Le service est arrêté";
					case (byte)PI01ErrorCodes.PI01_ACCESS_POINT_HAS_STOPPED:
						return "Le point d'accès est arrêté";
					case (byte)PI01ErrorCodes.PI01_UNKNOWN_APDU:
						return "APDU inconnue";
					case (byte)PI01ErrorCodes.PI01_INVALID_APDU:
						return "IPDU invalide";
					case (byte)PI01ErrorCodes.PI01_INVALID_APDU_LENGTH:
						return "Longueur d'IPDU invalide";
					case (byte)PI01ErrorCodes.PI01_PROTOCOL_ERROR:
						return "Erreur de protocole";
					case (byte)PI01ErrorCodes.PI01_INACTIVITY_TIMER_HAS_EXPIRED:
						return "Le timer d'inactivité a expiré";
					case (byte)PI01ErrorCodes.PI01_INVALID_TRANSACTION:
						return "Transaction invalide";
					case (byte)PI01ErrorCodes.PI01_INVALID_IPDU_FORMAT:
						return "Format d'IPDU invalide";
					case (byte)PI01ErrorCodes.PI01_INVALID_IPDU_EXCHANGE:
						return "Echange d'IPDU invalide";
					case (byte)PI01ErrorCodes.PI01_INVALID_PARAMETER:
						return "Paramètre invalide";
					default:
						return "Erreur inconnue";
				}
			}
			return string.Empty;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s">Received message</param>
		/// <param name="code">Translated</param>
		/// <param name="IDC">IDC inside the requets</param>
		/// <param name="IDCF">IDCF inside the request</param>
		/// <returns></returns>
		public string CodeReponseGarantisseurAsString(string s, out CodeReponseGarantisseur code, string IDCF = "", string IDC = "")
		{
			string desc = string.Empty;
			switch (s)
			{
				case "000":
					desc = "Transaction accordée";
					code = CodeReponseGarantisseur.F40_ACCEPTEE;
					break;
				case "001":
					desc = "Transaction refusée";
					code = CodeReponseGarantisseur.F40_REFUSEE;
					break;
				case "002":
					desc = "Demande non traitée de façon automatique (contre appel à réaliser)";
					code = CodeReponseGarantisseur.F40_NON_TRAITEE_AUTOMATIQUEMENT;
					break;
				case "003":
					desc = "Numéro d'abonné incorrect" + (!string.IsNullOrEmpty(IDCF) ? " (postionné à " + IDCF + ")" : string.Empty);
					code = CodeReponseGarantisseur.F40_IDCF_INVALIDE;
					break;
				case "004":
					desc = "Transaction interdite";
					code = CodeReponseGarantisseur.F40_TRANSACTION_INTERDITE;
					break;
				case "005":
					desc = "Service interdit à cet abonné" + (!string.IsNullOrEmpty(IDCF) ? " (postionné à " + IDCF + ")" : string.Empty);
					code = CodeReponseGarantisseur.F40_SERVICE_INTERDIT;
					break;
				case "006":
					desc = "Accord avec réserve";
					code = CodeReponseGarantisseur.F40_ACCEPTEE_SOUS_RESERVE;
					break;
				case "007":
					desc = "Identifiant du centre informatique (IDC) incorrect" + (!string.IsNullOrEmpty(IDC) ? " (postionné à " + IDC + ")" : string.Empty);
					code = CodeReponseGarantisseur.F40_IDC_INVALIDE;
					break;
				case "010":
					desc = "Service non disponible";
					code = CodeReponseGarantisseur.F40_SERVICE_INDISPONIBLE;
					break;
				default:
					desc = "Erreur inconnue";
					code = CodeReponseGarantisseur.F40_ERREUR_INCONNUE;
					break;
			}
			return desc;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		/// <param name="code"></param>
		/// <param name="IDCF">IDCF inside the request</param>
		/// <param name="IDC">IDC inside the request</param>
		/// <param name="CMC7">CMC7 inside the request</param>
		/// <param name="device">Device (ITP) inside the request</param>
		/// <returns></returns>
		public string CodeReponseFNCIAsString(string s, out CodeReponseFNCI code, string CMC7 = "", string IDCF = "", string IDC = "", string device = "")
		{
			string desc = String.Empty;
			switch (s)
			{
				case "  ":
					desc = "Pas d'interrogation FNCI";
					code = CodeReponseFNCI.F39_PAS_DINTERROGATION_FNCI;
					break;
				case "00":
					desc = "VERT - le chèque ne fait l'objet d'aucune interdiction";
					code = CodeReponseFNCI.F39_VERT;
					break;
				case "01":
					desc = "ORANGE - Alerte sur le compte";
					code = CodeReponseFNCI.F39_ORANGE;
					break;
				case "02":
					desc = "ROUGE - Chèque irrégulier";
					code = CodeReponseFNCI.F39_ROUGE;
					break;
				case "03":
					desc = "BLANC - Chèque non référencé";
					code = CodeReponseFNCI.F39_BLANC_CHEQUE_NON_REFERENCE;
					break;
				case "04":
					desc = "BLANC - Numéro d'abonné incorrect" + (!string.IsNullOrEmpty(IDCF) ? "(postionné à " + IDCF + ")" : string.Empty);
					code = CodeReponseFNCI.F39_BLANC_IDCF_INVALIDE;
					break;
				case "05":
					desc = "BLANC - Erreur du serveur FNCI";
					code = CodeReponseFNCI.F39_BLANC_ERREUR_SERVEUR_FNCI;
					break;
				case "06":
					desc = "BLANC - Piste CMC7 incorrecte" + (!string.IsNullOrEmpty(IDCF) ? "(postionné à " + CMC7 + ")" : string.Empty);
					code = CodeReponseFNCI.F39_BLANC_CMC7_INVALIDE;
					break;
				case "07":
					desc = "BLANC - Identifiant du centre informatique (IDC) incorrect" + (!string.IsNullOrEmpty(IDCF) ? "(postionné à " + IDC + ")" : string.Empty);
					code = CodeReponseFNCI.F39_BLANC_IDC_INVALIDE;
					break;
				case "08":
					desc = "BLANC - Matériel non référencé" + (!string.IsNullOrEmpty(IDCF) ? "(postionné à " + device + ")" : string.Empty);
					code = CodeReponseFNCI.F39_BLANC_MATERIEL_INCONNU;
					break;
				case "10":
					desc = "BLANC - Service non disponible";
					code = CodeReponseFNCI.F39_BLANC_SERVICE_NON_DISPONIBLE;
					break;
				case "99":
					desc = "BLANC - Transaction refusée";
					code = CodeReponseFNCI.F39_BLANC_REFUSEE;
					break;
				default:
					desc = "Erreur inconnue";
					code = CodeReponseFNCI.F39_ERREUR_INCONNUE;
					break;
			}
			return desc;
		}
		/// <summary>
		/// Tests the value entered for a FIELD property and returns the appropriate value
		/// </summary>
		/// <param name="value">The value to test</param>
		/// <param name="regex">The regular expression applying for the test</param>
		/// <param name="defvalue">Default value to use if the value is invalid</param>
		/// <returns>The value to use</returns>
		private string CheckValue(string value, string regex, string defvalue)
		{
			value = value.Trim();
			if (CMisc.IsValidFormat(value, regex, true))
				return value;
			return defvalue;
		}
		/// <summary>
		/// Pad left the value
		/// </summary>
		/// <param name="value">Value to eventually pad</param>
		/// <param name="min">Minimum characters inside the string</param>
		/// <param name="c">Padding character</param>
		/// <param name="pasIfEmpty">Indicates whether to pad the value even if the string is empty</param>
		/// <returns>The padded string or the original string</returns>
		private string PadLeft(string value, int min, char c, bool pasIfEmpty = false)
		{
			if (!string.IsNullOrEmpty(value) || pasIfEmpty)
				if (min > value.Length)
					value = value.PadLeft(min, c);
			return value;
		}
		/// <summary>
		/// Pad right the value
		/// </summary>
		/// <param name="value">Value to eventually pad</param>
		/// <param name="min">Minimum characters inside the string</param>
		/// <param name="c">Padding character</param>
		/// <param name="pasIfEmpty">Indicates whether to pad the value even if the string is empty</param>
		/// <returns>The padded string or the original string</returns>
		private string PadRight(string value, int min, char c, bool pasIfEmpty = false)
		{
			if (!string.IsNullOrEmpty(value) || pasIfEmpty)
				if (min > value.Length)
					value = value.PadRight(min, c);
			return value;
		}
		#endregion
	}
}
