using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Markup;
using System.Windows.Threading;
using System.Xaml;

namespace KsWare.Presentation.Lite {

	public abstract class BootstrapperBase : ResourceDictionary {

		private Dispatcher Dispatcher => Application.Current.Dispatcher; // TODO avoid Application.Current usage

		protected BootstrapperBase() {
			if(DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

			Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(OnStartup));
			Application.Current.Exit += (s, args) => OnExit(args);
		}

		protected virtual void OnExit(ExitEventArgs args) {
			
		}

		protected abstract void OnStartup();

		public void TestNoCodeBehind(Uri startupUri) {
			object view;
			var streamResourceInfo = Application.GetResourceStream(startupUri);

			// var xamlReader = new XamlReader();
			// view = xamlReader.LoadAsync(streamResourceInfo.Stream);

			using (var bamlReader = new Baml2006Reader(streamResourceInfo.Stream)) {
				using (var writer = new XamlObjectWriter(bamlReader.SchemaContext)) {
					while (bamlReader.Read()) writer.WriteNode(bamlReader);
					view = writer.Result;
				}
			}


			var mi = view.GetType().GetMethod("InitializeComponent", BindingFlags.Instance | BindingFlags.Public);
			// only if x:Class is specified
			mi?.Invoke(view,new object[0]);
			((Window)view).Show();
		}

		protected bool? Show(object viewModel, bool asDialog=false) {
			var viewLocator = new ViewLocator();
			var si = viewLocator.GetViewResource(viewModel.GetType());
			if(si==null) throw new InvalidOperationException("View not found!");
			object view = ViewLocatorHelper.Read(si);
			if(view==null) throw new InvalidOperationException("View not loaded!");
			if (view is FrameworkElement fe) fe.DataContext = viewModel;
			if(view is Window w) {
				if(asDialog) return w.ShowDialog(); 
				w.Show(); return null;
			}
			throw new InvalidOperationException("View can not be shown.");
		}

	}

}
