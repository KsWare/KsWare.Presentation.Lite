using System.Threading.Tasks;
using System.Windows.Input;

namespace KsWare.Presentation.Lite {

	public interface IUserCommand : ICommand {

		void Execute();
		void Execute(object parameter);

		Task ExecuteAsync();

		Task ExecuteAsync(object parameter);

		new bool CanExecute { get; }

		string Name { get; }

	}

}
