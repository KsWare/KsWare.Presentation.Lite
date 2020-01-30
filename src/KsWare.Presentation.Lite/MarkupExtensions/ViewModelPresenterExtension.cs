// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-30
//
// ***********************************************************************
// <copyright file="ViewModelPresenterExtension.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Markup;

namespace KsWare.Presentation.Lite.MarkupExtensions {


	/// <summary>
	/// Class ViewModelPresenterExtension.
	/// Implements the <see cref="System.Windows.Markup.MarkupExtension" />
	/// </summary>
	/// <seealso cref="System.Windows.Markup.MarkupExtension" />
	/// <example>
	/// <code>
	/// &lt;ListBox 
	/// ItemsSource="{Items}"
	/// ItemTemplate="{ViewModelPresenter ViewTypeHint={x:Type MyView}}" /&gt;
	/// </code>
	/// </example>
	public class ViewModelPresenterExtension : MarkupExtension {


		/// <summary>
		/// Gets or sets the type for which view model type the view shall be created.
		/// </summary>
		/// <value>The type of the view model.</value>
		/// <remarks>If not specified the markup extension throws a <see cref="ArgumentNullException"/>.</remarks>
		public object DataType { get; set; }

		/// <summary>
		/// Gets or sets the view type hint.
		/// </summary>
		/// <value>The view type hint.</value>
		public object ViewTypeHint { get; set; }

		/// <inheritdoc />
		public override object ProvideValue(IServiceProvider serviceProvider) {
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (serviceProvider == null) return this; // can be null it design time!

			var debugInfo = new DebugInformation(serviceProvider) {ViewType = ViewTypeHint as Type};

			if(DataType==null) throw  new ArgumentNullException(nameof(DataType), $"DataType not specified for {nameof(ViewModelPresenterExtension)}\n\nExtended Information:\n{debugInfo}");

			var provideValueTarget = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
			var targetObject = provideValueTarget.TargetObject as DependencyObject;
			var targetProperty = provideValueTarget.TargetProperty as DependencyProperty;

			if (targetObject == null || targetProperty == null) {
				throw new NotSupportedException($"{nameof(ViewModelPresenterExtension)} is only applicable to dependency properties.\n\nExtended Information:\n{debugInfo}");
			}

			DataType.SetDebugInformation(debugInfo.ToString());
			var value = ViewModelViewConverter.Default.Convert(DataType, targetProperty.PropertyType, null, CultureInfo.CurrentCulture);
			return value;
		}

	}
}
