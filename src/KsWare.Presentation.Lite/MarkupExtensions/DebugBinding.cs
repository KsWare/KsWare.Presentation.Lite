// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-30
//
// ***********************************************************************
// <copyright file="DebugBinding.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Text;

namespace KsWare.Presentation.Lite.MarkupExtensions {

	/// <inheritdoc />
	public class DebugBinding : Binding {

		/// <inheritdoc />
		public DebugBinding() { }

		/// <inheritdoc />
		public DebugBinding(string path) : base(path) { }

		/// <inheritdoc />
		public override object ProvideValue(IServiceProvider serviceProvider) {
			//TODO add code for debugging
			return base.ProvideValue(serviceProvider);
		}

	}
}
