// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// ***********************************************************************
// <copyright file="ViewModelViewConverter.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
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
		[SuppressMessage("ReSharper", "TooManyArguments", Justification = "IValueConverter implementation")]
		[SuppressMessage("ReSharper", "MethodTooLong", Justification = "it makes no sense to split this method")]
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			//TODO revise throwing exceptions in converter.
			if (value == null) {
				if (targetType.IsClass) return null;
				throw new InvalidOperationException($"The conversion is not supported. Value: null, TargetType: {targetType?.GetType().FullName ?? "null"}");
			}

			var viewType = ViewLocator.Default.GetViewType(value);

			if (targetType == typeof(DataTemplate)) {
				return ViewLocatorHelper.CreateDataTemplate(viewType);
			}
			if (targetType == typeof(HierarchicalDataTemplate)) {
				throw new NotImplementedException($"Conversion to HierarchicalDataTemplate is not implemented. Converter: {GetType().FullName}");
				// return ViewLocatorHelper.CreateHierarchicalDataTemplateFromUIElement((UIElement)view);
			}
			if (targetType == typeof(ControlTemplate)) {
				return ViewLocatorHelper.CreateControlTemplate(viewType);
			}			
			if (targetType == typeof(ItemContainerTemplate)) {
				throw new NotImplementedException($"Conversion to ItemContainerTemplate is not implemented. Converter: {GetType().FullName}");
				// return ViewLocatorHelper.CreateItemContainerTemplateFromUIElement((UIElement)view);
			}
			if (targetType.IsAssignableFrom(viewType)) {
				var view = ViewLocator.Default.CreateView(value);
				if (view == null) {
					// throw new NullReferenceException($"Matching view not available. Value: {value?.GetType().FullName ?? "null"}");
					Debug.WriteLine($"ViewModelViewConverter: No view for: {value?.GetType().FullName ?? "null"}");
					return null;
				}

				return view;
			}

			throw new InvalidOperationException($"The conversion is not supported. Value: {value?.GetType().FullName??"null"}, View: {viewType.FullName ?? "null"}, TargetType: {targetType?.GetType().FullName??"null"}, Converter: {nameof(ViewModelViewConverter)}");
		}

		/// <inheritdoc />
		/// <exception cref="NotSupportedException"></exception>
		[SuppressMessage("ReSharper", "TooManyArguments", Justification = "IValueConverter implementation")]
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotSupportedException($"ConvertBack is not supported. Converter: {GetType().FullName}");
		}

		private void Trace(string value) {

		}

	}

}
