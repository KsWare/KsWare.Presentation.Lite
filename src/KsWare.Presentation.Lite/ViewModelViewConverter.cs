// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// Last Modified By : SchreinerK
// Last Modified On : 2020-01-28
// ***********************************************************************
// <copyright file="ViewModelViewConverter.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace KsWare.Presentation.Lite {

	// TODO ViewModelViewConverter basic implementation
	/// <summary>
	/// Converts ViewModel into matching views .
	/// Implements the <see cref="System.Windows.Data.IValueConverter" />
	/// </summary>
	public class ViewModelViewConverter : IValueConverter {

		/// <summary>
		/// The default ViewModelViewConverter.
		/// </summary>
		public static readonly ViewModelViewConverter Default=new ViewModelViewConverter();

		/// <inheritdoc />
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) {
				if (targetType.IsClass) return null;
				throw new InvalidOperationException($"The conversion is not supported. Value: null, TargetType: {targetType?.GetType().FullName ?? "null"}");
			}

			var view = ViewLocator.Default.CreateView(value);
			if (view == null) {
				// throw new NullReferenceException($"Matching view not available. Value: {value?.GetType().FullName ?? "null"}");
				Debug.WriteLine($"ViewModelViewConverter: No view for: {value?.GetType().FullName ?? "null"}");
				return null;
			}

			if (targetType == typeof(DataTemplate)) {
				return ViewLocatorHelper.CreateDataTemplateFromUIElement((UIElement) view);
			}
			if (targetType == typeof(HierarchicalDataTemplate)) {
				throw new NotImplementedException($"Conversion to HierarchicalDataTemplate is not implemented. Converter: {GetType().FullName}");
				// return ViewLocatorHelper.CreateHierarchicalDataTemplateFromUIElement((UIElement)view);
			}
			if (targetType == typeof(ControlTemplate)) {
				return ViewLocatorHelper.CreateControlTemplateFromUIElement((UIElement)view);
			}			
			if (targetType == typeof(ItemContainerTemplate)) {
				throw new NotImplementedException($"Conversion to ItemContainerTemplate is not implemented. Converter: {GetType().FullName}");
				// return ViewLocatorHelper.CreateItemContainerTemplateFromUIElement((UIElement)view);
			}
			if (targetType.IsInstanceOfType(view)) {
				return view;
			}

			throw new InvalidOperationException($"The conversion is not supported. Value: {value?.GetType().FullName??"null"}, View: {view?.GetType().FullName ?? "null"}, TargetType: {targetType?.GetType().FullName??"null"}, Converter: {nameof(ViewModelViewConverter)}");
			
			
		}

		/// <inheritdoc />
		/// <exception cref="NotSupportedException"></exception>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException($"ConvertBack is not supported. Converter: {GetType().FullName}");
		}

		private void Trace(string value) {

		}

	}

}
