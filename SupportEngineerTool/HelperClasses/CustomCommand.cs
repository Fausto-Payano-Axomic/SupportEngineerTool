using System;
using System.Windows.Input;

namespace SupportEngineerTool.HelperClasses {
    public class CustomCommand : ICommand {
        private Action<object> execute;
        private Predicate<object> canExecute;

        public CustomCommand(Action<object> execute, Predicate<object> canExecute) {

            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            bool b = this.canExecute == null ? true : canExecute(parameter);
            return b;
        }

        public void Execute(object parameter) {
            this.execute(parameter);
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }

        }
    }
}
