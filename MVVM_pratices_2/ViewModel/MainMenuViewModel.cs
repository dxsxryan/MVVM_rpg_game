using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MVVM_pratices_2.ViewModel
{
    public class MainMenuViewModel
    {
        public ICommand StartCommand { get; }
        public ICommand LoadCommand { get; }
        public ICommand SettingsCommand { get; }
        public ICommand ExitCommand { get; }
        
        // 這個事件會在按下按鈕時觸發
        public event Action<string> OnNavigate;
        public MainMenuViewModel()
        {
            StartCommand = new RelayCommand(o => OnNavigate?.Invoke("CreateCharacter"));
            LoadCommand = new RelayCommand(o => OnNavigate?.Invoke("MainMenuLoad"));
            SettingsCommand = new RelayCommand(o => OnNavigate?.Invoke("Settings"));
            ExitCommand = new RelayCommand(o => Application.Current.Shutdown());
        }
    }
}
