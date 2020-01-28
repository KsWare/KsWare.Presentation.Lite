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
using System.Globalization;
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
			if (value == null && !targetType.IsClass)
				throw new InvalidOperationException($"The conversion is not supported. Value: null, TargetType: {targetType?.GetType().FullName ?? "null"}");

			var view = ViewLocator.Default.CreateView(value);
			if (view == null) 
				throw new NullReferenceException($"Matching view not available. Value: {value?.GetType().FullName ?? "null"}");
			if(!targetType.IsInstanceOfType(view))
				throw new InvalidOperationException($"The conversion is not supported. Value: {value?.GetType().FullName??"null"}, View: {view?.GetType().FullName ?? "null"}, TargetType: {targetType?.GetType().FullName??"null"}");
			
			return view;
		}

		/// <inheritdoc />
		/// <exception cref="NotImplementedException"></exception>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}

	}

}
