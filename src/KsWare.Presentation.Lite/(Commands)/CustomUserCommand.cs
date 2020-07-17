using System;

namespace KsWare.Presentation.Lite {

	public class CustomUserCommand : UserCommandBase {

		public CustomUserCommand() { }

		public CustomUserCommand(Action executeAction) {
			ExecuteAction = executeAction;
			CanExecute = true;
		}

		public CustomUserCommand(Action executeAction, bool canExecute) {
			ExecuteAction = executeAction;
			CanExecute = canExecute;
		}

		public Action ExecuteAction { get; set; }

		public override void Execute(object parameter) => ExecuteAction?.Invoke();
	}

	public class CustomUserCommand<TParameter> : UserCommandBase {

		public CustomUserCommand() { }

		public CustomUserCommand(Action<TParameter> executeAction) {
			ExecuteAction = executeAction;
			CanExecute = true;
		}

		public CustomUserCommand(Action<TParameter> executeAction, bool canExecute) {
			ExecuteAction = executeAction;
			CanExecute = canExecute;
		}

		public Action<TParameter> ExecuteAction { get; set; }

		public override void Execute(object parameter) => Execute((TParameter) parameter);

		public void Execute(TParameter parameter) => ExecuteAction?.Invoke(parameter);

	}

}
