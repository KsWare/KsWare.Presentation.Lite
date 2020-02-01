using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace KsWare.Presentation.Lite {

	public class InitializeComponentWrapper<TViewType> : ContentControl where TViewType : new() {

		public InitializeComponentWrapper() {
			Content=new TViewType();
			ViewLocatorHelper.InitializeComponent(Content);
		}
	}
}
