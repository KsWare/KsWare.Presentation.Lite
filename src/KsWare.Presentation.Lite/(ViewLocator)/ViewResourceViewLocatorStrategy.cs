// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-27
//
// Last Modified By : SchreinerK
// Last Modified On : 2020-01-27
// ***********************************************************************
// <copyright file="ViewResourceViewLocatorStrategy.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Resources;

namespace KsWare.Presentation.Lite {

	internal class ViewResourceViewLocatorStrategy : ViewLocatorStrategyBase {

		private readonly Dictionary<Type, Uri> _viewModelViewResourceUriMap = new Dictionary<Type, Uri>();
		private readonly AssemblyInfoCache _assemblyInfo = new AssemblyInfoCache();

		public override object GetView(object viewModelOrType) => LoadViewFromResource(viewModelOrType);
		public override Type GetViewType(object viewModelOrType) {
			throw new NotSupportedException($"GetViewType is not support in this strategy. Strategy: {this.GetType().AssemblyQualifiedName}");
		}

		private object LoadViewFromResource(object viewModel) {
			if (viewModel == null) return null;
			var viewModelType = viewModel is Type type ? type : viewModel.GetType();
			if (viewModelType == typeof(object)) { return new ViewModelPresenter(); }
			if (viewModelType == typeof(ViewModelPresenter)) { return new ViewModelPresenter(); }
			var sri = FindViewResource(viewModelType);
			if (sri == null) return null;
			var view = ViewLocatorHelper.ReadResource(sri,()=>$"ViewModel: {viewModelType.FullName}");
			sri.Stream.DeleteDebugInformation();

			ViewLocatorHelper.InitializeComponent(view);
			if (view is FrameworkElement frameworkElement && !(viewModel is Type))
				frameworkElement.DataContext = viewModel;

			return view;
		}

		public StreamResourceInfo FindViewResource(Type viewModelType) {
			if (_viewModelViewResourceUriMap.TryGetValue(viewModelType, out var viewResourceUri)) {
				var sri = Application.GetResourceStream(viewResourceUri);
				sri.Stream.SetDebugInformation($"ViewModelType: {viewModelType.FullName}");
				sri.Stream.SetDebugInformation($"ResourceUri: {viewResourceUri}");
			} ;

			var assemblyName = viewModelType.Assembly.GetName(false).Name;
			var viewmodelName = viewModelType.FullName;
			foreach (var viewmodelSuffix in ViewModelSuffixes) {
				var viewSuffix = ViewSuffixes.First(); //TODO
				if (!viewmodelName.EndsWith(viewmodelSuffix)) continue;
				foreach (var possibleParentFolderName in ViewModelFolderNames.OrderByDescending(x=>x.Length)) {
					if(!ViewLocatorHelper.MatchParentFolderName(viewModelType, possibleParentFolderName)) continue;
					var pfni = Array.IndexOf(ViewModelFolderNames, possibleParentFolderName);
					var possibleParentViewFolderName = ViewFolderNames[pfni];
					var possibleViewName = ViewLocatorHelper.BuildViewName(viewmodelName, viewModelType.Name, possibleParentFolderName, viewmodelSuffix, possibleParentViewFolderName, viewSuffix);
					
					//        new.folder/usercontrol1.baml
					//        new/folder.usercontrol1.baml      !!! Improbable but possible
					// RootNS.New.Folder.UserControl1			<== match
					// RootNS.Other.New.Folder.UserControl1		<== no match
					string possibleMatch = ViewLocatorHelper.BestResourceMatch(possibleViewName, _assemblyInfo[viewModelType.Assembly].ResourceKeys);
					

					if (possibleMatch == null) continue;
					if (possibleMatch.EndsWith(".baml")) possibleMatch = possibleMatch.Substring(0, possibleMatch.Length - 4) + "xaml";

					var possibleViewResourceUri = new Uri($"/{assemblyName};component/{possibleMatch}", UriKind.Relative);
					try {
						var sri = Application.GetResourceStream(possibleViewResourceUri);
						if (sri != null) {
							_viewModelViewResourceUriMap.Add(viewModelType, possibleViewResourceUri);
							sri.Stream.SetDebugInformation($"ViewModelType: {viewModelType.FullName}");
							sri.Stream.SetDebugInformation($"ResourceUri: {possibleViewResourceUri}");
							return sri;
						}
					}
					catch (Exception ex) {
						continue;
					}

				}

			}

			return null;
		}

		internal class AssemblyInfoCache {

			private readonly IDictionary<Assembly, AssemblyInfo> _dic=new Dictionary<Assembly, AssemblyInfo>();

			public AssemblyInfo this[Assembly assembly] {
				get {
					if (!_dic.TryGetValue(assembly, out var ai)) {
						ai = CreateAssemblyInfo(assembly);
						_dic.Add(assembly, ai);
					}
						
					return ai;
				}
			}

			private AssemblyInfo CreateAssemblyInfo(Assembly assembly) {
				return new AssemblyInfo {
					ResourceKeys = ViewLocatorHelper.GetResourceNames(assembly),
					Namespaces = ViewLocatorHelper.GetNamespaces(assembly)
				};

			}

		}

		internal class AssemblyInfo {

			public IList<string> Namespaces { get; set; }
			public IList<string> ResourceKeys { get; set; }

		}

	}

}
