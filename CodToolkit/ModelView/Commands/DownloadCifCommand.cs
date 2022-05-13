using System;
using System.Windows.Input;

namespace CodToolkit.ModelView.Commands
{
    public class DownloadCifCommand : ICommand
    {
        private readonly Func<bool> _canDownload;
        private readonly Action _downloadCif;

        public DownloadCifCommand(Func<bool> canDownload, Action downloadCif)
        {
            _canDownload = canDownload;
            _downloadCif = downloadCif;
        }

        public bool CanExecute(object parameter)
        {
            return _canDownload();
        }

        public void Execute(object parameter)
        {
            _downloadCif();
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
