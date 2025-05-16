using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_pratices_2.ViewModel
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        private int _masterVolume = 100;
        public int MasterVolume
        {
            get { return _masterVolume; }
            set
            {
                if (_masterVolume != value)
                {
                    _masterVolume = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _bgmVolume = 100;
        public int BGMVolume
        {
            get { return _bgmVolume; }
            set
            {
                if (_bgmVolume != value)
                {
                    _bgmVolume = value;
                    OnPropertyChanged();
                }
            }
        }

        private int _sfxVolume = 100;
        public int SFXVolume
        {
            get { return _sfxVolume; }
            set
            {
                if (_sfxVolume != value)
                {
                    _sfxVolume = value;
                    OnPropertyChanged();
                }
            }
        }
        public event Action<string> OnNavigate;
        public ICommand MenuCommand { get; }
        public SettingsViewModel()
        {
            MenuCommand = new RelayCommand(o => OnNavigate?.Invoke("Menu"));

        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
