using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using MVVM_pratices_2.Enum;
using MVVM_pratices_2.ViewModel;

namespace MVVM_pratices_2
{
    public class Connector : INotifyPropertyChanged
    {
        public MainMenuViewModel MainMenuVM { get; } = new MainMenuViewModel();
        public StartViewModel StartVM { get; } = new StartViewModel();
        public SettingsViewModel SettingsVM { get; } = new SettingsViewModel();
        public SaveLoadViewModel SaveVM { get; }
        public SaveLoadViewModel LoadVM { get; }
        public SaveLoadViewModel MainMenuLoadVM { get; }
        /// <summary> 子系統切換</summary>
        private object _currentPage;
        public object CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                NotifyPropertyChanged();
            }
        }
        private ICommand _ButtonSubSystem;
        public ICommand ButtonSubSystem
        {
            get { return _ButtonSubSystem; }
            set { _ButtonSubSystem = value; }
        }
        public class SubSystemConvert : IValueConverter
        {
            object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return (int)((SubSystem)value);
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }

        public Connector()
        {
            // 預設為主選單畫面
            CurrentPage = MainMenuVM;

            //存檔紀錄
            LoadVM = new SaveLoadViewModel(true, "Start");
            SaveVM = new SaveLoadViewModel(false, "Start");
            MainMenuLoadVM = new SaveLoadViewModel(true, "Menu");

            // 讓主選單畫面能觸發切換功能
            MainMenuVM.OnNavigate += ChangePage;
            StartVM.OnNavigate += ChangePage;
            SettingsVM.OnNavigate += ChangePage;
            SaveVM.OnNavigate += ChangePage;
            LoadVM.OnNavigate += ChangePage;
            MainMenuLoadVM.OnNavigate += ChangePage;
        }
        /// <summary> 子系統切換</summary>
        private SubSystem _SystemValue;
        public SubSystem SystemValue
        {
            get { return _SystemValue; }
            set
            {
                _SystemValue = value;
                NotifyPropertyChanged();
            }
        }
        public void ChangePage(string target)
        {
            switch (target)
            {
                case "Start":
                    CurrentPage = StartVM;
                    break;
                case "Settings":
                    CurrentPage = SettingsVM;
                    break;
                case "Menu":
                    CurrentPage = MainMenuVM;
                    break;
                case "SavePage":
                    CurrentPage = SaveVM;
                    break;
                case "LoadPage":
                    CurrentPage = LoadVM;
                    break;
                case "MainMenuLoad":
                    CurrentPage = MainMenuLoadVM;
                    break;

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
