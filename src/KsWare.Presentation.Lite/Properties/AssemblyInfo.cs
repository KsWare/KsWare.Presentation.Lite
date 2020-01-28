using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Markup;
using static KsWare.Presentation.Lite.AssemblyInfo;

[assembly: ThemeInfo(
	ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
									 //(used if a resource is not found in the page,
									 // or application resource dictionaries)
	ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
											  //(used if a resource is not found in the page,
											  // app, or any theme specific resource dictionaries)
)]

[assembly: XmlnsDefinition(XmlNamespace, RootNamespace)]
[assembly: XmlnsPrefix(XmlNamespace, "ksl")]

[assembly: InternalsVisibleTo("KsWare.Presentation.Lite.Tests, " +
                              "PublicKey=0024000004800000940000000602000000240" +
                              "000525341310004000001000100CD4B0B5EFEA32ACB3B87" +
                              "9FCBE885E899A4C10BA34B58DBB2443AE290A4475FFFFF7" +
                              "3D091FDED09B82CE40F85C705C08D616DCABEAD583DD169" +
                              "444F313E119409571283F68005F2DDE8F2E2B9C9E3DD658" +
                              "4C1D3A91A525DFC7556C6EA604F717878C6C900D551DC1E" +
                              "05FD6B694A9B7CCF2807E25D3093D3F659B14A51710652C5")]

// namespace must equal to assembly name
// ReSharper disable once CheckNamespace
namespace KsWare.Presentation.Lite {

	/// <summary>
	/// Provides assembly information
	/// </summary>
	public static class AssemblyInfo {

		/// <summary>
		/// Gets this assembly.
		/// </summary>
		/// <value>The assembly.</value>
		public static Assembly Assembly => Assembly.GetExecutingAssembly();

		/// <summary>
		/// The XML namespace for this assembly
		/// </summary>
		public const string XmlNamespace = "http://ksware.de/Presentation/Lite";

		/// <summary>
		/// The root namespace for this assembly
		/// </summary>
		public const string RootNamespace = "KsWare.Presentation.Lite";

	}
}