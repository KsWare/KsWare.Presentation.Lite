// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// Last Modified By : SchreinerK
// Last Modified On : 2020-01-28
// ***********************************************************************
// <copyright file="IViewLocatorStrategy.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace KsWare.Presentation.Lite {

	/// <summary>
	/// Presents a mapping strategy for ViewLocator
	/// </summary>
	internal interface IViewLocatorStrategy {

		/// <summary>
		/// Gets the view for the specified view model.
		/// </summary>
		/// <param name="viewModelOrType">The view model.</param>
		/// <returns>The matching view or <c>null if no view was found.</c></returns>
		object GetView(object viewModelOrType);

		/// <summary>
		/// Gets the view type for the specified view model.
		/// </summary>
		/// <param name="viewModelOrType">The view model.</param>
		/// <returns>The matching view type or <c>null if no view was found.</c></returns>
		Type GetViewType(object viewModelOrType);

	}

}
