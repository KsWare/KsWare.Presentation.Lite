// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// Last Modified By : SchreinerK
// Last Modified On : 2020-01-28
// ***********************************************************************
// <copyright file="ViewLocator.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.ComponentModel.Composition;

namespace KsWare.Presentation.Lite {

	/// <summary>
	/// Create the views for view models.
	/// Implements the <see cref="KsWare.Presentation.Lite.IViewLocator" />
	/// </summary>
	/// <seealso cref="KsWare.Presentation.Lite.IViewLocator" />
	[Export(typeof(IViewLocator))]
	public class ViewLocator : IViewLocator {

		/// <summary>
		/// The default ViewLocator.
		/// </summary>
		public static IViewLocator Default = new ViewLocator();

		/// <summary>
		/// Gets or sets the strategy.
		/// </summary>
		/// <value>The strategy.</value>
		private IViewLocatorStrategy Strategy { get; set; } = new ViewTypeViewLocatorStrategy();

		/// <summary>
		/// Creates the view for the specified view model.
		/// </summary>
		/// <param name="viewModelOrType">The view model.</param>
		/// <returns>The matching view or <c>null if no view was found.</c></returns>
		public object CreateView(object viewModelOrType) => Strategy.GetView(viewModelOrType);

		/// <summary>
		/// Gets the view type for the specified view model.
		/// </summary>
		/// <param name="viewModelOrType">The view model.</param>
		/// <returns>The matching view type or <c>null if no view was found.</c></returns>
		public Type GetViewType(object viewModelOrType) => Strategy.GetViewType(viewModelOrType);

	}

}
