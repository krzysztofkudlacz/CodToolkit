using System;
using System.Windows.Input;

namespace CodToolkit.ModelView.Commands
{
    public class CloseSearchCodDialogCommand : ICommand
    {
        private readonly Func<bool> _canClose;
        private readonly Action _closeCod;

        public CloseSearchCodDialogCommand(Func<bool> canClose, Action closeCod)
        {
            _canClose = canClose;
            _closeCod = closeCod;
        }

        public bool CanExecute(object parameter)
        {
            return _canClose();
        }

        public void Execute(object parameter)
        {
            _closeCod();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
