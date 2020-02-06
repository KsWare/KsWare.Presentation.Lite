// ***********************************************************************
// Assembly         : KsWare.Presentation.Lite
// Author           : SchreinerK
// Created          : 2020-02-02
//
// ***********************************************************************
// <copyright file="RelayCommand.cs" company="KsWare">
//     Copyright © by KsWare. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows.Input;

namespace KsWare.Presentation.Lite {


	/// <summary>
	/// Class RelayCommand.
	/// Implements the <see cref="System.Windows.Input.ICommand" />
	/// </summary>
	/// <seealso cref="System.Windows.Input.ICommand" />
	public class RelayCommand : ICommand {

		private static bool DefaultCanExecute(object parameter) => true;


		private readonly Predicate<object> _canExecute;

		private readonly Action<object> _execute;

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand"/> class.
		/// </summary>
		/// <param name="execute">The execute action.</param>
		/// <exception cref="ArgumentNullException">if <paramref name="execute"/></exception>
		public RelayCommand(Action<object> execute) : this(execute, DefaultCanExecute) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RelayCommand"/> class.
		/// </summary>
		/// <param name="execute">The execute action.</param>
		/// <param name="canExecute">The can execute predicate.</param>
		/// <exception cref="ArgumentNullException">if <paramref name="execute"/> or <paramref name="canExecute"/> is null. </exception>
		public RelayCommand(Action<object> execute, Predicate<object> canExecute) {
			_execute = execute ?? throw new ArgumentNullException(nameof(execute));
			_canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
		}

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged {
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
		/// <returns><see langword="true" /> if this command can be executed; otherwise, <see langword="false" />.</returns>
		public bool CanExecute(object parameter) => _canExecute != null && _canExecute(parameter);

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
		public void Execute(object parameter) => _execute(parameter);


	}
}
