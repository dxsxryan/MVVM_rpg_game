using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using MVVM_pratices_2.Enum;
using MVVM_pratices_2.Model;
using MVVM_pratices_2.ViewModel;

namespace MVVM_pratices_2
{
    public class Connector : INotifyPropertyChanged
    {
        public MainMenuViewModel MainMenuVM { get; } = new MainMenuViewModel();
        public StartViewModel StartVM { get; } = new StartViewModel();
        public SettingsViewModel SettingsVM { get; } = new SettingsViewModel();
        public CreateCharacterViewModel CreateCharacterVM { get; } = new CreateCharacterViewModel();
        public RoleInfoViewModel RoleInfoVM { get; private set; }
        public SaveLoadViewModel SaveVM { get; }
        public SaveLoadViewModel LoadVM { get; }
        public SaveLoadViewModel MainMenuLoadVM { get; }
        public ConfirmDialogViewModel DialogVM { get; set; }
        /// <summary> 彈出式視窗 </summary>
        private bool _isDialogVisible;
        public bool IsDialogVisible
        {
            get => _isDialogVisible;
            set { _isDialogVisible = value; OnPropertyChanged(); }
        }
        /// <summary> 建立時間系統 </summary>
        private DateTime _startTime;
        private DispatcherTimer _timer;
        private TimeSpan _elapsed = TimeSpan.Zero;
        public string CurrentPlayTime => _elapsed.ToString("hh\\:mm\\:ss");

        /// <summary> 子系統切換</summary>
        private object _currentPage;
        public object CurrentPage
        {
            get => _currentPage;
            set
            {
                _currentPage = value;
                OnPropertyChanged();
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
            CreateCharacterVM.OnNavigate += ChangePage;
            
        }
        /// <summary> 子系統切換</summary>
        private SubSystem _SystemValue;
        public SubSystem SystemValue
        {
            get { return _SystemValue; }
            set
            {
                _SystemValue = value;
                OnPropertyChanged();
            }
        }
        private bool _isGameStarted = false;

        public void ChangePage(string target)
        {

            switch (target)
            {
                case "Start":
                    if (!_isGameStarted)
                    {
                        _startTime = DateTime.Now;
                        _timer = new DispatcherTimer
                        {
                            Interval = TimeSpan.FromSeconds(1)
                        };
                        _timer.Tick += (s, e) =>
                        {
                            _elapsed = DateTime.Now - _startTime;
                            OnPropertyChanged(nameof(CurrentPlayTime));

                            if (CurrentPage is RoleInfoViewModel roleInfo)
                                roleInfo.PlayTime = CurrentPlayTime;
                        };
                        _timer.Start();

                        _isGameStarted = true;
                    }
                    CurrentPage = StartVM;
                    break;
                case "Settings":
                    CurrentPage = SettingsVM;
                    break;
                case "SavePage":
                    CurrentPage = SaveVM;
                    break;
                case "MainMenuLoad":
                    CurrentPage = MainMenuLoadVM;
                    break;
                case "LoadPage":
                    CurrentPage = LoadVM;
                    break;
                case "Menu":
                    _timer?.Stop();
                    _timer = null;
                    _isGameStarted = false;
                    _elapsed = TimeSpan.Zero;
                    CurrentPage = MainMenuVM;
                    break;
                case "CreateCharacter":
                    CurrentPage = CreateCharacterVM;
                    break;
                case "RoleInfo":
                    RoleInfoVM = new RoleInfoViewModel(CreateCharacterVM.Character);
                    RoleInfoVM.OnNavigate += ChangePage;
                    RoleInfoVM.PlayTime = CurrentPlayTime;
                    CurrentPage = RoleInfoVM;
                    break;

            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
