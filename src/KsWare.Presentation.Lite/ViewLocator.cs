using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Resources;

namespace KsWare.Presentation.Lite {

	[Export(typeof(IViewLocator))]
	public class ViewLocator : IViewLocator {

		private readonly Dictionary<Type, Type> _viewModelViewMap = new Dictionary<Type, Type>();
		private readonly Dictionary<Type, Type> _viewViewModelMap = new Dictionary<Type, Type>();
		private readonly Dictionary<Type, Uri> _viewModelViewResourceUriMap = new Dictionary<Type, Uri>();

		public string[] ViewSuffixes { get; set; }= { "View" };
		public string[] ViewModelSuffixes { get; set; } = { "ViewModel","VM" };

		public object GetView(object viewModel) {
			var viewModelType = viewModel.GetType();
			var viewType = GetViewType(viewModelType);
			if (viewType == null) return null;

			var view = Activator.CreateInstance(viewType);

			if (view is System.Windows.Markup.IComponentConnector componentConnector)
				componentConnector.InitializeComponent();
			if (view is FrameworkElement frameworkElement)
				frameworkElement.DataContext = viewModel;

			return view;
		}

		public StreamResourceInfo GetViewResource(Type viewModelType) {
			if (_viewModelViewResourceUriMap.TryGetValue(viewModelType, out var viewResourceUri)) return Application.GetResourceStream(viewResourceUri); ;

			var assemblyName = viewModelType.Assembly.GetName(false).Name;
			var viewmodelName = viewModelType.FullName;
			var resourceNames = ViewLocatorHelper.GetResourceNames(viewModelType.Assembly); //lowercase with slashes

			foreach (var viewmodelSuffix in ViewModelSuffixes) {
				var viewSuffix = ViewSuffixes.First();
				if (viewmodelName.EndsWith(viewmodelSuffix)) {
					var possibleViewName = viewmodelName.Substring(0, viewmodelName.Length - viewmodelSuffix.Length) + viewSuffix;
					var possibleViewNameLowerCase= possibleViewName.ToLower(CultureInfo.InvariantCulture); // TODO umlaute??
																							 //        new.folder/usercontrol1.baml
																							 // RootNS.New.Folder.UserControl1			<== match
																							 // RootNS.Other.New.Folder.UserControl1		<== no match

					string possibleMatch = null;
					foreach (var resourceName in resourceNames) {
						var normalizedResourceName = resourceName.Replace("/", ".");
						if (!(possibleViewNameLowerCase + ".xaml").EndsWith(normalizedResourceName) &&
						    !(possibleViewNameLowerCase + ".baml").EndsWith(normalizedResourceName)) continue;
						if (possibleMatch == null || possibleMatch.Length > resourceName.Length) possibleMatch = resourceName;
					}

					if (possibleMatch == null) continue;
					if (possibleMatch.EndsWith(".baml")) possibleMatch = possibleMatch.Substring(0, possibleMatch.Length - 4) + "xaml";

					var possibleViewResourceUri=new Uri($"/{assemblyName};component/{possibleMatch}",UriKind.Relative);
					try { 
						var streamResourceInfo = Application.GetResourceStream(possibleViewResourceUri);
						if (streamResourceInfo != null) {
							_viewModelViewResourceUriMap.Add(viewModelType, possibleViewResourceUri);
							return streamResourceInfo;
						}
					}
					catch (Exception ex) {
						continue;
					}
				}
			}

			return null;
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

	public interface IViewLocator {

		string[] ViewSuffixes { get; set; }
		string[] ViewModelSuffixes { get; set; }


		Type GetViewType(Type viewModelType);

		object GetView(object viewModel); //TODO DRAFT
	}

}
