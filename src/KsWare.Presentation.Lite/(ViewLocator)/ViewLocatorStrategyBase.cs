// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// Last Modified By : SchreinerK
// Last Modified On : 2020-01-27
// ***********************************************************************
// <copyright file="ViewLocatorStrategyBase.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
namespace KsWare.Presentation.Lite {

	internal abstract class ViewLocatorStrategyBase : IViewLocatorStrategy {
		public string[] ViewSuffixes { get; set; } = { "View" };
		public string[] ViewModelSuffixes { get; set; } = { "ViewModel", "VM" };

		public string[] ViewFolderNames { get; set; } = { "", "View" };
		public string[] ViewModelFolderNames { get; set; } = { "", "ViewModel" };

		public abstract object GetView(object viewModelOrType);
	}

}
