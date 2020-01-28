// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// Last Modified By : SchreinerK
// Last Modified On : 2020-01-27
// ***********************************************************************
// <copyright file="IViewLocator.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace KsWare.Presentation.Lite {

	/// <summary>
	/// Create the views for view models.
	/// </summary>
	public interface IViewLocator {

		/// <summary>
		/// Creates the view for the specified view model.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <returns>The matching view or <c>null if no view was found.</c></returns>
		object CreateView(object viewModel);
	}

}
