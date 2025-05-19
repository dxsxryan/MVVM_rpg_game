using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_pratices_2.ViewModel
{
    public class ConfirmDialogViewModel : INotifyPropertyChanged
    {
        public string Message { get; set; }
        public ICommand ConfirmCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action<bool> CloseDialog;

        public ConfirmDialogViewModel(string message)
        {
            Message = message;

            ConfirmCommand = new RelayCommand(_ => CloseDialog?.Invoke(true));
            CancelCommand = new RelayCommand(_ => CloseDialog?.Invoke(false));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
