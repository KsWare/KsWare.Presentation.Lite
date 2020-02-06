// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// ***********************************************************************
// <copyright file="ViewTypeViewLocatorStrategy.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace KsWare.Presentation.Lite {

	internal class ViewTypeViewLocatorStrategy : ViewLocatorStrategyBase {
		private readonly Dictionary<Type, Type> _viewModelViewMap = new Dictionary<Type, Type>();
		private readonly Dictionary<Type, Type> _viewViewModelMap = new Dictionary<Type, Type>();

		public override object GetView(object viewModelOrType) => LoadViewWithActivator(viewModelOrType);

		public override Type GetViewType(object viewModelOrType) {
			if (viewModelOrType == null) return null;
			var viewModelType = viewModelOrType is Type type ? type : viewModelOrType.GetType();
			var viewType = FindViewType(viewModelType);
			return viewType;
		}

		private object LoadViewWithActivator(object viewModelOrType) {
			var viewType = GetViewType(viewModelOrType);
			if (viewType == null) return null;
			var view = Activator.CreateInstance(viewType);
			ViewLocatorHelper.InitializeComponent(view);
			if(view is FrameworkElement fe && !(viewModelOrType is Type)) fe.DataContext=viewModelOrType;
			return view;
		}

		public Type FindViewType(Type viewModelType) {
			if (_viewModelViewMap.TryGetValue(viewModelType, out var viewType)) return viewType;
			if (viewModelType == typeof(object)) { return typeof(ViewModelPresenter); }
			if (viewModelType == typeof(ViewModelPresenter)) { return typeof(ViewModelPresenter); }
			var triedTypes = new List<string>();
			try {
				var viewmodelName = viewModelType.FullName??"";

				foreach (var viewmodelSuffix in ViewModelSuffixes) {
					var viewSuffix = ViewSuffixes.First();
					if (!viewmodelName.EndsWith(viewmodelSuffix)) continue;
					foreach (var possibleParentFolderName in ViewModelFolderNames.OrderBy(x => x.Length)) {
						if (!ViewLocatorHelper.MatchParentFolderName(viewModelType, possibleParentFolderName)) continue;
						var pfni = Array.IndexOf(ViewModelFolderNames, possibleParentFolderName);
						var possibleParentViewFolderName = ViewFolderNames[pfni];
						var possibleViewName = ViewLocatorHelper.BuildViewName(viewmodelName, viewModelType.Name, possibleParentFolderName, viewmodelSuffix, possibleParentViewFolderName, viewSuffix);
						viewType = viewModelType.Assembly.GetType(possibleViewName, false);
						if (viewType != null) return viewType;
						triedTypes.Add(possibleViewName);
					}

				}
				throw new TypeLoadException($"Matching view not found for \"{viewModelType.FullName}\".\nPossible view types:\n\t{string.Join("\n\t", triedTypes)}");
				return null;
			}
			// catch (Exception ee) {
			// 	throw;
			// }
			finally {
				if (viewType != null) {
					_viewModelViewMap.Add(viewModelType, viewType);
					_viewViewModelMap.Add(viewType, viewModelType);
				}
			}

		}
	}

}
