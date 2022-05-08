using System;
using System.Windows.Input;

namespace CodToolkit.ModelView.Commands
{
    public class SearchCodCommand : ICommand
    {
        private readonly Func<bool> _canSearch;
        private readonly Action _searchCod;

        public SearchCodCommand(Func<bool> canSearch, Action searchCod)
        {
            _canSearch = canSearch;
            _searchCod = searchCod;
        }

        public bool CanExecute(object parameter)
        {
            return _canSearch();
        }

        public void Execute(object parameter)
        {
            _searchCod();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
