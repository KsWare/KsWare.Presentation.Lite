using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Markup;
using System.Xaml;

namespace KsWare.Presentation.Lite {
	internal class DebugInformation {

		private readonly IServiceProvider _serviceProvider;

		public DebugInformation(IServiceProvider serviceProvider) {
			_serviceProvider = serviceProvider;

			ReadAll();
		}

		private void ReadAll() {
			LineInfo = (IXamlLineInfo)_serviceProvider.GetService(typeof(IXamlLineInfo));
			var xamlLineInfoText = LineInfo?.HasLineInfo ?? false ? $"Line: {LineInfo.LineNumber}, Position: {LineInfo.LinePosition}" : null;
			var uriContext = (IUriContext)_serviceProvider.GetService(typeof(IUriContext));
			BaseUri = uriContext?.BaseUri;
			var rootObjectProvider = (IRootObjectProvider)_serviceProvider.GetService(typeof(IRootObjectProvider));
			var rootObject = rootObjectProvider?.RootObject;
			var rootObjectName = rootObject is Type rootObjectType ? rootObjectType.FullName : rootObject?.GetType().FullName;
			RootObject = rootObject is Type ? null : rootObject;
			RootObjectType = rootObject is Type type ? type: rootObject?.GetType();
			var provideValueTarget = (IProvideValueTarget)_serviceProvider.GetService(typeof(IProvideValueTarget));
			var targetProperty = provideValueTarget?.TargetProperty;
			TargetDependencyProperty = targetProperty as DependencyProperty;
			TargetPropertyInfo = targetProperty as PropertyInfo;
			TargetPropertyName = TargetDependencyProperty?.Name ?? TargetPropertyInfo?.Name;
			TargetObject = provideValueTarget?.TargetObject;
			TargetObjectType = TargetObject?.GetType();
			TargetDependencyObject = TargetObject as DependencyObject;
			TargetFrameworkElement = TargetObject as FrameworkElement;
			// var typeDescriptorContext = (ITypeDescriptorContext)_serviceProvider.GetService(typeof(ITypeDescriptorContext));
			// var destinationTypeProvider = (IDestinationTypeProvider)_serviceProvider.GetService(typeof(IDestinationTypeProvider));
			// var destinationType = destinationTypeProvider?.GetDestinationType();
			TargetAutomationId = TargetDependencyObject != null ? AutomationProperties.GetAutomationId(TargetDependencyObject) : null;
			TargetName = TargetFrameworkElement?.Name;
			ViewType = ViewType ?? (TargetObject as ViewModelPresenter)?.ViewTypeHint;
		}

		public Type ViewType { get; set; }

		/// <summary>
		/// Gets the target <see cref="FrameworkElement.Name"/>. 
		/// </summary>
		/// <value>The target name.</value>
		public string TargetName { get; private set; }

		/// <summary>
		/// Gets the target AutomationId.
		/// </summary>
		/// <value>The target AutomationId.</value>
		public string TargetAutomationId { get; private set; }

		public FrameworkElement TargetFrameworkElement { get; private set; }

		public DependencyObject TargetDependencyObject { get; private set; }

		public object TargetObject { get; private set; }

		public Type TargetObjectType { get; private set; }

		public PropertyInfo TargetPropertyInfo { get; private set; }

		public DependencyProperty TargetDependencyProperty { get; private set; }

		public string TargetPropertyName { get; set; }

		public Type RootObjectType { get; private set; }

		public object RootObject { get; private set; }

		public IXamlLineInfo LineInfo { get; private set; }

		public Uri BaseUri { get; private set; }

		public override string ToString() {
			return
				F("\tBaseUri: {0}\n", BaseUri?.AbsoluteUri) +
				F("\tRootObject: {0}\n", RootObjectType.FullName) +
				$"\tTarget: {TargetObjectType.Name}{F(" Name={0}", TargetName)}{F(" AutomationId={0}", TargetAutomationId)}" + "\n" +
				F("\tTargetProperty: {0}\n", TargetPropertyName) +
				F("\tLineInfo: Line: {0}, Position: {1}\n", LineInfo?.HasLineInfo ?? false ? null : LineInfo?.LineNumber, LineInfo?.LinePosition) +
				F("\tView: {0}", ViewType?.FullName);
		}

		private static string F(string format, params object[] args) {
			if ((args?.Length??0) == 0) return "";
			if (args.Any(x => string.IsNullOrEmpty(x as string))) return "";
			if (format?.Contains("{0}") ?? false) return string.Format(format, args);
			return $"{format}={string.Join(",", args)}";
		}

	}
}
