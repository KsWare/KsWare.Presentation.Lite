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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Resources;
using System.Xaml;
using System.Xml;
using XamlReader = System.Windows.Markup.XamlReader;

namespace KsWare.Presentation.Lite {
	internal class ViewLocatorHelper {
		// TODO same code as in TemplateConverterHelper/ResourceConverterHelper

		public static object ReadResource(StreamResourceInfo streamResourceInfo, Func<string> exceptionCallback) {
			switch (streamResourceInfo.ContentType) {
				case "application/baml+xml": return ReadPage(streamResourceInfo.Stream, exceptionCallback);
				case "application/xaml+xml": return ReadResource(streamResourceInfo.Stream, exceptionCallback);
				default: return null;
			}
		}

		// public static object ReadStreamResource(StreamResourceInfo streamResourceInfo, Func<string> exceptionCallback) {
		// 	switch (streamResourceInfo.ContentType) {
		// 		case "application/baml+xml": return ReadPage(streamResourceInfo.Stream);
		// 		case "application/xaml+xml": return ReadResource(streamResourceInfo.Stream);
		// 		default: return null;
		// 	}
		// }

		public static object ReadResource(Stream stream, Func<string> exceptionCallback) {
			// read the stream for build action "Resource"
			var xamlReader = new XamlReader();
			return xamlReader.LoadAsync(stream);
		}

		public static object ReadPage(Stream stream, Func<string> exceptionCallback) {
			// read the stream for build action "Page"

			using (var bamlReader = new Baml2006Reader(stream)) {
				using (var writer = new XamlObjectWriter(bamlReader.SchemaContext)) {
					while (bamlReader.Read()) {
						try { writer.WriteNode(bamlReader); }
						catch /*1*/ (XamlObjectWriterException ex) {
							var lineInfo = ((IXamlLineInfo) bamlReader);
							var errorMessage = ex.Message+"\n\n"+$"Line:{lineInfo.LineNumber}, Position:{lineInfo.LinePosition}";
							if (exceptionCallback != null) errorMessage += "\n" + exceptionCallback?.Invoke();
							if (stream.TryGetDebugInformation(out var s)) errorMessage += "\n" + s;
							//errorMessage += "\nPossible reason: multiple call of InitializeComponent. Check also inline code!";
							stream.DeleteDebugInformation();
							try { writer.Close(); } catch(XamlObjectWriterException) { /*WORKAROUND A */} 
							throw new InvalidDataException(errorMessage, ex);
						}
					}
					return writer.Result;
				}
			}
			// *1* catch XamlObjectWriterException
			// 'Beim Festlegen der Eigenschaft "System.Windows.ResourceDictionary.DeferrableContent" wurde eine Ausnahme ausgelöst.'
			// HResult: -2146233088
			// Inner Exception: { InvalidOperationException}
			// InvalidOperationException: Die ResourceDictionary-Instanz kann nicht erneut initialisiert werden.
			// Possible reason: multiple call of InitializeComponent
			// <x:Code><![CDATA[public ShellView(){InitializeComponent();}]]></x:Code>


			// WORKAROUND A)
			// using IDisposable.Dispose() is calling writer.Close() => throws another exception BEFORE our exception is effective! 
			// System.Xaml.XamlObjectWriterException: 'XAML-Knotenstream: CurrentObject fehlt vor EndObject.'
		}


		public static object ReadPageDebug(Stream stream, Func<string> exceptionCallback) {
			// read the stream for build action "Page"
			using (var bamlReader = new Baml2006Reader(stream)) {
				var lineInfo = ((IXamlLineInfo) bamlReader);

				using (var writer = new XamlObjectWriter(bamlReader.SchemaContext)) {
					var indentLevel = 0;
					while (true) {
						var ind = new String(' ', Math.Max(0,indentLevel) * 4);
						try {
							if (!bamlReader.Read()) break;
							switch (bamlReader.NodeType) {
								case XamlNodeType.NamespaceDeclaration:
									Debug.WriteLine($"{lineInfo.LineNumber}:{lineInfo.LinePosition}{ind} xmlns:{bamlReader.Namespace.Prefix}={bamlReader.Namespace.Namespace}"); break;
								case XamlNodeType.StartObject:
									Debug.WriteLine($"{lineInfo.LineNumber}:{lineInfo.LinePosition}{ind} <{bamlReader.Type?.Name}");
									indentLevel++;
									break;
								case XamlNodeType.EndObject: 
									indentLevel--;
									ind = new String(' ', Math.Max(0, indentLevel) * 4);
									Debug.WriteLine($"{lineInfo.LineNumber}:{lineInfo.LinePosition}{ind} >");
									break;
								case XamlNodeType.StartMember:
									Debug.WriteLine($"{lineInfo.LineNumber}:{lineInfo.LinePosition}{ind} {bamlReader.Member.Name}=");
									indentLevel++;
									break;
								case XamlNodeType.EndMember:
									indentLevel--;
									ind = new String(' ', Math.Max(0, indentLevel) * 4);
									Debug.WriteLine($"{lineInfo.LineNumber}:{lineInfo.LinePosition}{ind}");
									break;
								case XamlNodeType.Value:
									Debug.WriteLine($"{lineInfo.LineNumber}:{lineInfo.LinePosition}{ind} {bamlReader.Value}"); break;
								case XamlNodeType.GetObject: break;
								default:
									Debug.WriteLine($"{lineInfo.LineNumber}:{lineInfo.LinePosition}{ind} {bamlReader.NodeType} {bamlReader.Type?.Name}{bamlReader.Member?.Name}{bamlReader.Value}");
									break;

							}
						}
						catch (Exception ex) { throw; }

						try { writer.WriteNode(bamlReader); }
						catch (Exception ex) { throw; }

					}

					return writer.Result;
				}
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

		/// <summary>
		///	Call InitializeComponent for the specified element.
		/// </summary>
		/// <param name = "element">The element to initialize</param>
		/// <remarks>When a view does not contain a code-behind file, we need to call InitializeComponent.</remarks>
		public static void InitializeComponent(object element) {

			if (element is System.Windows.Markup.IComponentConnector componentConnector) {
				componentConnector.InitializeComponent();
				return;
			}

			// var method = element.GetType()
			// 	.GetMethod("InitializeComponent", BindingFlags.Public | BindingFlags.Instance);
			// method?.Invoke(element, null);

            var method = element.GetType().GetTypeInfo()
                .GetDeclaredMethods("InitializeComponent")
                .SingleOrDefault(m => m.GetParameters().Length == 0);
            method?.Invoke(element, null);

		}

		// source https://github.com/KsWare/KsWare.Presentation.Converters/blob/features/kux/src/KsWare.Presentation.Converters/ResourceConverterHelper.cs
		public static DataTemplate CreateDataTemplateFromUIElement(UIElement content) {
			var contentXaml = SerializeToXaml(content);
			var templateXaml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
{contentXaml}
</DataTemplate>";


			var sr = new StringReader(templateXaml);
			var xr = XmlReader.Create(sr);
			var dataTemplate = (DataTemplate)XamlReader.Load(xr);
			return dataTemplate;
		}

		// source https://github.com/KsWare/KsWare.Presentation.Converters/blob/features/kux/src/KsWare.Presentation.Converters/ResourceConverterHelper.cs
		public static ControlTemplate CreateControlTemplateFromUIElement(UIElement content) {
			var contentXaml = SerializeToXaml(content);
			var templateXaml = $@"<?xml version=""1.0"" encoding=""utf-8""?>
<ControlTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation"">
{contentXaml}
</ControlTemplate>";


			var sr = new StringReader(templateXaml);
			var xr = XmlReader.Create(sr);
			var controlTemplate = (ControlTemplate)XamlReader.Load(xr);
			return controlTemplate;
		}

		// source https://github.com/KsWare/KsWare.Presentation.Converters/blob/features/kux/src/KsWare.Presentation.Converters/ResourceConverterHelper.cs
		public static string SerializeToXaml(UIElement element) {
			var xaml = System.Windows.Markup.XamlWriter.Save(element);

			using (var stream = new MemoryStream()) {
				using (var streamWriter = new StreamWriter(stream, Encoding.UTF8, 64 * 1024, true)) {
					streamWriter.Write(xaml);
				}

				stream.Position = 0;
				return new StreamReader(stream).ReadToEnd();
			}
		}

	}
}
// https://stackoverflow.com/questions/96317/how-do-you-get-the-root-namespace-of-an-assembly
// https://stackoverflow.com/questions/4885888/how-get-the-default-namespace-of-project-csproj-vs-2008