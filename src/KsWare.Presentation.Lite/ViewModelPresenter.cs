// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-28
//
// Last Modified By : SchreinerK
// Last Modified On : 2020-01-28
// ***********************************************************************
// <copyright file="ViewModelPresenter.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Windows.Controls;
using System.Windows.Data;

namespace KsWare.Presentation.Lite {

	/// <summary>
	/// The ViewModelPresenter presents the matching view for the current data context.
	/// </summary>
	/// <seealso cref="System.Windows.Controls.ContentControl" />
	public class ViewModelPresenter : ContentControl {

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewModelPresenter"/> class.
		/// </summary>
		public ViewModelPresenter() {
			BindingOperations.SetBinding(this, ContentProperty, new Binding { Converter = ViewModelViewConverter.Default });
		}
	}
}
