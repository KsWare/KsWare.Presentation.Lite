// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// ***********************************************************************
// <copyright file="IViewLocator.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace KsWare.Presentation.Lite {

	/// <summary>
	/// Create the views for view models.
	/// </summary>
	public interface IViewLocator {

		/// <summary>
		/// Creates the view for the specified view model.
		/// </summary>
		/// <param name="viewModelOrType">The view model.</param>
		/// <returns>The matching view or <c>null if no view was found.</c></returns>
		object CreateView(object viewModelOrType);

		/// <summary>
		/// Gets the view type for the specified view model.
		/// </summary>
		/// <param name="viewModelOrType">The view model.</param>
		/// <returns>The matching view type or <c>null if no view was found.</c></returns>
		Type GetViewType(object viewModelOrType);
	}

}
