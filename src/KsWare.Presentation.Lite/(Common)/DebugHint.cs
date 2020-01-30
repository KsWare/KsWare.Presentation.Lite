using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Data;
using System.Windows.Markup;
using System.Xaml;

namespace KsWare.Presentation.Lite {

	[MarkupExtensionReturnType(typeof(object))]
	public class DebugHint : MarkupExtension {

		public DebugHint() {}

		/// <summary>
		/// Gets or sets the type of the view. use {x:Type} Markup syntax, the name as string does not work for Navigation
		/// </summary>
		/// <value>The type of the view.</value>
		public object ViewType { get; set; }

		// The only syntax I got to work is:
		//   Hint="{ksl:DebugHint ViewType={x:Type toolbar:Toolbar2View}}"
		// Type in Constructor=> XamlParseException
		// Type in Property=> XamlParseException
		// This I would me wish:
		//   Hint="{ksl:DebugHint toolbar:Toolbar2View}"

		public override object ProvideValue(IServiceProvider serviceProvider) {
			// ITypeDescriptorContext, IServiceProvider, IXamlTypeResolver, IUriContext, IAmbientProvider, IXamlSchemaContextProvider, IRootObjectProvider, IXamlNamespaceResolver, IProvideValueTarget, IXamlNameResolver, IDestinationTypeProvider
			// 
			var uriContext =(IUriContext)serviceProvider.GetService(typeof(IUriContext));
			var xamlLIneInfo=(IXamlLineInfo)serviceProvider.GetService(typeof(IXamlLineInfo));
			var xamlLineInfoText= xamlLIneInfo?.HasLineInfo??false ? $"Line: {xamlLIneInfo.LineNumber}, Position: {xamlLIneInfo.LinePosition}" :null;
			var baseUri = uriContext?.BaseUri;
			var rootObjectProvider = (IRootObjectProvider)serviceProvider.GetService(typeof(IRootObjectProvider));
			var rootObject = rootObjectProvider?.RootObject;
			var rootObjectName = rootObject is Type rootObjectType ? rootObjectType.FullName : rootObject?.GetType().FullName;
			var provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
			var targetProperty = provideValueTarget.TargetProperty;
			var targetPropertyName = (targetProperty is DependencyProperty targetDependencyProperty
				? targetDependencyProperty.Name : (targetProperty is PropertyInfo targetPropertyInfo ? targetPropertyInfo.Name : null));
			var targetObject = provideValueTarget.TargetObject;
			var targetDependencyObject = provideValueTarget.TargetObject as DependencyObject;
			var targetFrameworkElement = provideValueTarget.TargetObject as FrameworkElement;
			// var typeDescriptorContext = (ITypeDescriptorContext)serviceProvider.GetService(typeof(ITypeDescriptorContext));
			// var destinationTypeProvider = (IDestinationTypeProvider)serviceProvider.GetService(typeof(IDestinationTypeProvider));
			// var destinationType = destinationTypeProvider?.GetDestinationType();
			var automationId = targetDependencyObject != null ? AutomationProperties.GetAutomationId(targetDependencyObject) : null;
			var name = targetFrameworkElement?.Name;
			targetObject.SetDebugInformation(
				F("\tBaseUri: {0}\n", baseUri?.AbsoluteUri) +
				F("\tRootObject: {0}\n", rootObjectName) +
				$"\tTarget: {targetObject?.GetType().Name}{F(" Name={0}", name)}{F(" AutomationId={0}", automationId)}" + "\n" +
				F("\tTargetProperty: {0}\n", targetPropertyName) +
				F("\tLineInfo: {0}\n", xamlLineInfoText) +
				$"\tView: {(targetObject as ViewModelPresenter)?.HintType?.FullName ?? (ViewType as Type)?.FullName ?? ViewType}"
			);

			var r = targetObject.GetDebugInformation();
			return r;
		}

		// conditional Format
		private static string F(string format, string value) {
			if (string.IsNullOrEmpty(value)) return "";
			if (format?.Contains("{0}")??false) return string.Format(format, value);
			return $"{format}={value}";
		}

	}
}
