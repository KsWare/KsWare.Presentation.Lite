// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-28
//
// ***********************************************************************
// <copyright file="ViewModelPresenter.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;

namespace KsWare.Presentation.Lite {

	/// <summary>
	/// The ViewModelPresenter presents the matching view for the current data context.
	/// </summary>
	public class ViewModelPresenter : ContentControl {

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelPresenter"/> class.
		/// </summary>
		public ViewModelPresenter() {
			// I don't use binding for better exception handling
			// var binding = new Binding {Converter = ViewModelViewConverter.Default,Mode = BindingMode.OneWay };
			// BindingOperations.SetBinding(this, ContentProperty, binding);

			DependencyPropertyDescriptor
				.FromProperty(DataContextProperty, typeof(ViewModelPresenter))
				.AddValueChanged(this, OnDataContextChanged);
		}

		private void OnDataContextChanged(object sender, EventArgs e) {
			try {
				Trace($"loading view for {DataContext?.GetType().Name??"null"}...");
				Content = ViewModelViewConverter.Default.Convert(DataContext, typeof(object), null, CultureInfo.CurrentCulture);
				Trace($"view for {DataContext?.GetType().Name??"null"} loaded: {Content?.GetType().Name??"null"}");
			}
			catch (Exception ex) {
				Trace($"loading view for {DataContext?.GetType().Name??"null"} failed. {ex.Message}");
				ex.Data.Add("DebugInfo",this.GetDebugInformation());
				throw new Exception(ex.Message+"\n\nDebugInfo:\n"+ this.GetDebugInformation());
			}
			
		}


		/// <summary>
		/// Gets or sets the debug hint.
		/// </summary>
		/// <value>The debug hint.</value>
		/// <remarks><para>The DebugHint is used to show in exception.</para>
		/// <para>You can use DebugHint markup extension to set the property.</para>
		/// </remarks>
		/// <example>
		/// <code languange="XAML">
		/// &lt;ksl:ViewModelPresenter
		///	    DataContext="{Binding MenuControls}"
		///     DebugHint="{ksl:DebugHint ViewType={x:Type menuControls:MenuControlsView}}"/&gt;
		/// </code>
		/// </example>
		public string DebugHint { get; set; }

		/// <summary>
		/// Set this property the expected view type.
		/// </summary>
		/// <value>The hint.</value>
		/// <remarks>The hint is useful to navigate directly to the view and/or to mark it as 'in use'.</remarks>
		public Type ViewTypeHint { get; set; }

		// private object OnCoerceDataContext(object baseValue) {
		// 	var binding = BindingOperations.GetBinding(this, DataContextProperty);
		// 	var elementName = binding.ElementName;
		// 	var path = binding.Path.Path;
		// 	var source = binding.Source;
		//
		// 	var bindingExpression = BindingOperations.GetBindingExpression(this, DataContextProperty);
		// 	var target = bindingExpression.Target;
		// 	var targetProperty = bindingExpression.TargetProperty;
		// 	var dataItem = bindingExpression.DataItem;
		// 	var resolvedSource = bindingExpression.ResolvedSource;
		// 	var resolvedSourcePropertyName = bindingExpression.ResolvedSourcePropertyName;
		//
		//
		// 	return baseValue;
		// }

		[Conditional("DEBUG")]
		private void Trace(string message) {
			Debug.WriteLine($"{nameof(ViewModelPresenter)}: {message}");
		}

	}
}
