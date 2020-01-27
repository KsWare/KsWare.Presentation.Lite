using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Baml2006;
using System.Windows.Resources;
using System.Xaml;
using XamlReader = System.Windows.Markup.XamlReader;

namespace KsWare.Presentation.Lite {
	internal class ViewLocatorHelper {
		// TODO same code as in TemplateConverterHelper/ResourceConverterHelper

		public static object ReadResource(StreamResourceInfo streamResourceInfo) {
			switch (streamResourceInfo.ContentType) {
				case "application/baml+xml": return ReadPage(streamResourceInfo.Stream);
				case "application/xaml+xml": return ReadResource(streamResourceInfo.Stream);
				default: return null;
			}
		}

		public static object ReadResource(Stream stream) {
			// read the stream for build action "Resource"
			var xamlReader = new XamlReader();
			return xamlReader.LoadAsync(stream);
		}

		public static object ReadPage(Stream stream) {
			// read the stream for build action "Page"
			using (var bamlReader = new Baml2006Reader(stream))
			using (var writer = new XamlObjectWriter(bamlReader.SchemaContext)) {
				while (bamlReader.Read()) writer.WriteNode(bamlReader);
				return writer.Result;
			}
		}

		public string GetRootNamespaceUsingResourceDraft(Assembly assembly) {
			// KsWare.Presentation.Lite.TestApp.g.resources
			// ns.Resource1.resources

			var wpfResourceName = assembly.GetName().Name + ".g.resources";

			var resourceNames = assembly.GetManifestResourceNames()
				.Where(n => n.EndsWith(".resources") && n != wpfResourceName);
				
			//TODO ???
			return null;
		}

		public static IList<string> GetResourceNames(Assembly assembly) {
			// var resourceManager = new System.Resources.ResourceManager($"{assembly.GetName(false).Name}.g.resources", assembly);
			// var resourceSet = resourceManager.GetResourceSet(CultureInfo.CreateSpecificCulture(""), false, true);
			// var names=new List<string>();
			// foreach (DictionaryEntry entry in resourceSet) {
			// 	switch (entry.Key) {
			// 		case string s: names.Add(s); break;
			// 	}
			// }
			// return names;

			var names = new List<string>();
			var stream = assembly.GetManifestResourceStream(assembly.GetName(false).Name + ".g.resources");
			using (var reader = new ResourceReader(stream)) {
				foreach (DictionaryEntry entry in reader) {
					switch (entry.Key) {
						case string s: names.Add(s); break;
					}
				}
			}

			return names;

			// new.folder/usercontrol1.baml			> ROOTNS.new.folder.usercontrol1
			// (nonamespace)/usercontrol1.baml		> ROOTNS.usercontrol1
			// new%20folder/usercontrol1.baml		> ROOTNS.new_folder.usercontrol1
		}

		public static object Read(StreamResourceInfo streamResourceInfo) {
			switch (streamResourceInfo.ContentType) {
				case "application/baml+xml": return ViewLocatorHelper.ReadPage(streamResourceInfo.Stream);
				case "application/xaml+xml": return ViewLocatorHelper.ReadResource(streamResourceInfo.Stream);
				default: return null;
			}
		}

	}
}
// https://stackoverflow.com/questions/96317/how-do-you-get-the-root-namespace-of-an-assembly
// https://stackoverflow.com/questions/4885888/how-get-the-default-namespace-of-project-csproj-vs-2008