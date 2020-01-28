// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-01-26
//
// Last Modified By : SchreinerK
// Last Modified On : 2020-01-28
// ***********************************************************************
// <copyright file="BootstrapperBase.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Threading;

namespace KsWare.Presentation.Lite {

	/// <summary>
	/// The base class for an bootstrapper.
	/// Implements the <see cref="System.Windows.ResourceDictionary" />
	/// </summary>
	/// <seealso cref="System.Windows.ResourceDictionary" />
	public abstract class BootstrapperBase : ResourceDictionary {

		private Dispatcher Dispatcher => Application.Current.Dispatcher; // TODO avoid Application.Current usage

		/// <summary>
		/// Initializes a new instance of the <see cref="BootstrapperBase"/> class.
		/// </summary>
		protected BootstrapperBase() {
			if(DesignerProperties.GetIsInDesignMode(new DependencyObject())) return;

			Dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(OnStartup));
			Application.Current.Exit += (s, args) => OnExit(args);
		}


		/// <summary>
		/// Called on application startup.
		/// </summary>
		/// <seealso cref="Application.Startup"/>
		protected abstract void OnStartup();

		/// <summary>
		/// Called on application exit.
		/// </summary>
		/// <param name="args">The <see cref="ExitEventArgs"/> instance containing the event data.</param>
		/// <seealso cref="Application.Exit"/>
		protected virtual void OnExit(ExitEventArgs args) {
			
		}

		/// <summary>
		/// Shows the view for the specified view model.
		/// </summary>
		/// <param name="viewModel">The view model.</param>
		/// <remarks><para>The view must derive from <see cref="Window"/></para></remarks>
		[SuppressMessage("ReSharper", "FlagArgument", Justification = "shut up")]
		protected void Show(object viewModel) {
			var view = ViewLocator.Default.CreateView(viewModel);
			if (view == null) throw new InvalidOperationException("View not loaded!");
			switch (view) {
				case Window w: w.Show(); return;
				default: throw new InvalidOperationException("View can not be shown.");
			}
		}

	}

}
