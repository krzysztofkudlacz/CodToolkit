using System;
using System.Windows.Input;

namespace CodToolkit.ModelView.Commands
{
    public class ShowSearchCodDialogCommand : ICommand
    {
        private readonly Func<bool> _canShowDialog;
        private readonly Action _showDialog;

        public ShowSearchCodDialogCommand(Func<bool> canShowDialog, Action showDialog)
        {
            _canShowDialog = canShowDialog;
            _showDialog = showDialog;
        }

        public bool CanExecute(object parameter)
        {
            return _canShowDialog();
        }

        public void Execute(object parameter)
        {
            _showDialog();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
