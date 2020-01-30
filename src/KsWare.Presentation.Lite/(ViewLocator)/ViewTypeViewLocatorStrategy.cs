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

		private object LoadViewWithActivator(object viewModel) {
			if (viewModel == null) return null;
			var viewModelType = viewModel is Type type ? type : viewModel.GetType();
			var viewType = GetViewType(viewModelType);
			if (viewType == null) return null;

			var view = Activator.CreateInstance(viewType);
			ViewLocatorHelper.InitializeComponent(view);
			if(view is FrameworkElement fe && !(viewModel is Type)) fe.DataContext=viewModel;
			return view;
		}

		public Type GetViewType(Type viewModelType) {
			if (_viewModelViewMap.TryGetValue(viewModelType, out var viewType)) return viewType;
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
