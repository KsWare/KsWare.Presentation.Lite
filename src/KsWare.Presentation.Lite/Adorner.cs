using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Documents;

namespace KsWare.Presentation.Lite {
	public class Adorner : System.Windows.Documents.Adorner {

		private AdornerLayer _adornerLayer;

		public Adorner(UIElement adornedElement) : base(adornedElement) {
			Dispatcher.BeginInvoke(new Action(OnInitialize));
		}

		protected virtual void OnInitialize() {
			_adornerLayer = AdornerLayer.GetAdornerLayer(AdornedElement);
			_adornerLayer.Add(this);
		}

		public void Close() {
			_adornerLayer.Remove(this);
		}

		

	}
}
