using System;
using System.Collections.Generic;
using System.Text;

namespace KsWare.Presentation.Lite.TestApp {

	internal sealed class Bootstrapper : BootstrapperBase {
		protected override void OnStartup() {
			
			//new ShellView { DataContext = new ShellViewModel()}.Show(); // quick and dirty
			//TestNoCodeBehind(new Uri("/KsWare.Presentation.Lite.TestApp;component/ShellView.xaml",UriKind.Relative));
			Show(new ShellViewModel());
		}

	}
}
