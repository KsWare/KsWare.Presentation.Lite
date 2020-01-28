// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// Last Modified By : SchreinerK
// Last Modified On : 2020-01-27
// ***********************************************************************
// <copyright file="ViewLocatorHelper.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
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

		public static object ReadStreamResource(StreamResourceInfo streamResourceInfo) {
			switch (streamResourceInfo.ContentType) {
				case "application/baml+xml": return ReadPage(streamResourceInfo.Stream);
				case "application/xaml+xml": return ReadResource(streamResourceInfo.Stream);
				default: return null;
			}
		}

		public static IList<string> GetNamespaces(Assembly assembly) {
			return assembly.GetTypes().Select(t => t.Namespace).OrderBy(s => s).Distinct().ToList();
		}

		public static string BestResourceMatch(string possibleViewName, IEnumerable<string> resourceNames) {
			string possibleMatch = null;
			foreach (var resourceName in resourceNames) {
				var normalizedResourceName = resourceName.Replace("/", ".");
				if (!(possibleViewName + ".xaml").EndsWith(normalizedResourceName,StringComparison.InvariantCultureIgnoreCase) &&
				    !(possibleViewName + ".baml").EndsWith(normalizedResourceName,StringComparison.InvariantCultureIgnoreCase)) continue;
				if (possibleMatch == null || possibleMatch.Length < resourceName.Length) possibleMatch = resourceName;
			}

			return possibleMatch;
		}

		public static bool MatchParentFolderName(Type type, string parentFolderName) {
			if (String.IsNullOrEmpty(parentFolderName)) return true;
			var fn = type.FullName;
			var n = type.Name;
			var p = fn.Substring(0, fn.Length - n.Length);
			return p == parentFolderName || p.EndsWith("." + parentFolderName);
		}

		public static string BuildViewName(string viewModelFullName, string viewModelName, string parentFolder, string viewmodelSuffix, string viewFolder, string viewSuffix) {
			
			var sb=new StringBuilder(viewModelFullName);
			var n = viewModelName.Substring(0, viewModelName.Length - viewmodelSuffix.Length) + viewSuffix;
			sb.Remove(sb.Length - viewModelName.Length, viewModelName.Length); // remove name
			if (sb[sb.Length - 1] == '.') sb.Remove(sb.Length - 1, 1); // remove dot if any
			if(!string.IsNullOrEmpty(parentFolder))
				sb.Remove(sb.Length - parentFolder.Length, parentFolder.Length); // remove parent Folder
			if (sb[sb.Length - 1] == '.') sb.Remove(sb.Length - 1, 1); // remove dot if any

			if (!string.IsNullOrEmpty(viewFolder) && sb.Length > 0) sb.Append("." + viewFolder);
			if (sb.Length > 0) sb.Append(".");
			sb.Append(n);
			return sb.ToString();
		}

	}
}
// https://stackoverflow.com/questions/96317/how-do-you-get-the-root-namespace-of-an-assembly
// https://stackoverflow.com/questions/4885888/how-get-the-default-namespace-of-project-csproj-vs-2008