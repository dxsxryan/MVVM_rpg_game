// Connector.cs（計時系統整合 + StartPlayTimeTracking 方法）
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
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
        public InventoryViewModel InventoryVM { get; } = new InventoryViewModel();
        public RoleInfoViewModel RoleInfoVM { get; private set; }
        public SaveLoadViewModel SaveVM { get; }
        public SaveLoadViewModel LoadVM { get; }
        public SaveLoadViewModel MainMenuLoadVM { get; }
        public ConfirmDialogViewModel DialogVM { get; set; }

        private bool _isDialogVisible;
        public bool IsDialogVisible
        {
            get => _isDialogVisible;
            set { _isDialogVisible = value; OnPropertyChanged(); }
        }

        private TimeSpan _loadedPlayTime = TimeSpan.Zero;
        private DispatcherTimer _timer;
        private DateTime _startTime;
        private TimeSpan _elapsed = TimeSpan.Zero;

        public string CurrentPlayTime => (_loadedPlayTime + _elapsed).ToString("hh\\:mm\\:ss");
        public double GetTotalPlayTimeSeconds() => (_loadedPlayTime + _elapsed).TotalSeconds;
        public void SetLoadedPlayTime(double seconds) => _loadedPlayTime = TimeSpan.FromSeconds(seconds);

        public void StartPlayTimeTracking()
        {
            _elapsed = TimeSpan.Zero;
            _startTime = DateTime.Now;

            _timer?.Stop();
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) =>
            {
                _elapsed = DateTime.Now - _startTime;
                OnPropertyChanged(nameof(CurrentPlayTime));

                if (CurrentPage is RoleInfoViewModel roleInfo)
                    roleInfo.PlayTime = CurrentPlayTime;
            };
            _timer.Start();
        }

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

        public Connector()
        {
            CurrentPage = MainMenuVM;

            LoadVM = new SaveLoadViewModel(true, "Start", this);
            SaveVM = new SaveLoadViewModel(false, "Start", this);
            MainMenuLoadVM = new SaveLoadViewModel(true, "Menu", this);

            MainMenuVM.OnNavigate += ChangePage;
            StartVM.OnNavigate += ChangePage;
            SettingsVM.OnNavigate += ChangePage;
            SaveVM.OnNavigate += ChangePage;
            LoadVM.OnNavigate += ChangePage;
            InventoryVM.OnNavigate += ChangePage;
            MainMenuLoadVM.OnNavigate += ChangePage;
            CreateCharacterVM.OnNavigate += ChangePage;
        }

        private SubSystem _SystemValue;
        public SubSystem SystemValue
        {
            get => _SystemValue;
            set { _SystemValue = value; OnPropertyChanged(); }
        }

        private bool _isGameStarted = false;

        public void ChangePage(string target)
        {
            switch (target)
            {
                case "Start":
                    if (!_isGameStarted)
                    {
                        StartPlayTimeTracking();
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
                    MainMenuLoadVM.RefreshSlots();
                    CurrentPage = MainMenuLoadVM;
                    break;
                case "LoadPage":
                    LoadVM.RefreshSlots();
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
                    CreateCharacterVM.ResetCharacter();
                    CurrentPage = CreateCharacterVM;
                    break;
                case "RoleInfo":
                    RoleInfoVM = new RoleInfoViewModel(CreateCharacterVM.Character);
                    RoleInfoVM.OnNavigate += ChangePage;
                    RoleInfoVM.PlayTime = CurrentPlayTime;
                    CurrentPage = RoleInfoVM;
                    break;
                case "Inventory":
                    CurrentPage = InventoryVM;
                    break;
            }
        }

        public ICommand ButtonSubSystem { get; set; }

        public class SubSystemConvert : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
                => (int)((SubSystem)value);

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
                => throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
