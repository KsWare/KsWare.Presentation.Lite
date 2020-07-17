using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KsWare.Presentation.Lite {

	[SuppressMessage("ReSharper", "MemberCanBeProtected.Global", Justification = "public API")]
	[SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global", Justification = "public API")]
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "public API")]
	public abstract class UserCommandBase : NotifyPropertyChangedBase, IUserCommand, ICommand {

		private EventHandler _canExecuteChanged;
		private bool _canExecute;
		private string _name;

		/// <summary>
		/// Initializes a new instance of the <see cref="UserCommandBase"/> class.
		/// </summary>
		protected UserCommandBase() {
			var name = GetType().Name;
			if (name.EndsWith("Command")) name = name.Substring(0, name.Length - 7);
			Name = name;
		}


		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null" />.</param>
		public abstract void Execute(object parameter);

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		public virtual void Execute() => Execute(null);

		public virtual Task ExecuteAsync() => Task.Run(Execute);

		public virtual Task ExecuteAsync(object parameter) => Task.Run(Execute);

		/// <summary>
		/// Gets or sets a value indicating whether this instance can execute.
		/// </summary>
		/// <value><c>true</c> if this instance can execute; otherwise, <c>false</c>.</value>
		public virtual bool CanExecute {
			get => _canExecute;
			set {
				if(!Set(ref _canExecute, value)) return;
				_canExecuteChanged?.Invoke(this,EventArgs.Empty);
			}
		}

		bool ICommand.CanExecute(object parameter) => _canExecute;

		[SuppressMessage("ReSharper", "DelegateSubtraction", Justification = "no alternative")]
		event EventHandler ICommand.CanExecuteChanged {
			add => _canExecuteChanged+=value; 
			remove => _canExecuteChanged-=value;
		}

		/// <summary>
		/// Gets or sets the name for this command.
		/// </summary>
		/// <value>The name for this command.</value>
		public string Name { get => _name; set => Set(ref _name, value);}
	}

}
