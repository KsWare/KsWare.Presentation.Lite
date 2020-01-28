// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// Last Modified By : SchreinerK
// Last Modified On : 2020-01-27
// ***********************************************************************
// <copyright file="ViewTypeViewLocatorStrategy.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;

namespace KsWare.Presentation.Lite {

	internal class ViewTypeViewLocatorStrategy : ViewLocatorStrategyBase {
		private readonly Dictionary<Type, Type> _viewModelViewMap = new Dictionary<Type, Type>();
		private readonly Dictionary<Type, Type> _viewViewModelMap = new Dictionary<Type, Type>();

		public override object GetView(object viewModel) => LoadViewWithActivator(viewModel);

		private object LoadViewWithActivator(object viewModel) {
			var viewModelType = viewModel.GetType();
			var viewType = GetViewType(viewModelType);
			if (viewType == null) return null;

			var view = Activator.CreateInstance(viewType);
			return view;
		}

		public Type GetViewType(Type viewModelType) {
			if (_viewModelViewMap.TryGetValue(viewModelType, out var viewType)) return viewType;

			try {
				var viewmodelName = viewModelType.FullName;

				foreach (var viewmodelSuffix in ViewModelSuffixes) {
					var viewSuffix = ViewSuffixes.First();
					if (viewmodelName.EndsWith(viewmodelSuffix)) {
						var possibleViewName = viewmodelName.Substring(0, viewmodelName.Length - viewmodelSuffix.Length) + viewSuffix;
						viewType = viewModelType.Assembly.GetType(possibleViewName, false);
						if (viewType != null) return viewType;
					}
				}

				return null;
			}
			catch (Exception e) {
				Console.WriteLine(e);
				throw;
			}
			finally {
				if (viewType != null) {
					_viewModelViewMap.Add(viewModelType, viewType);
					_viewViewModelMap.Add(viewType, viewModelType);
				}
			}

		}
	}

}
