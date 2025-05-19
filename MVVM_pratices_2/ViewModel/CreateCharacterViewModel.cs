using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Text;
using System.Threading.Tasks;
using MVVM_pratices_2.Model;

namespace MVVM_pratices_2.ViewModel
{
    public class CreateCharacterViewModel : INotifyPropertyChanged
    {

        public CharacterData Character { get; set; } = new CharacterData();

        private int _availablePoints = 10;
        public int AvailablePoints
        {
            get => _availablePoints;
            set { _availablePoints = value; OnPropertyChanged(); }
        }

        public string Notification { get; set; }
        private bool _isNotificationVisible;
        public bool IsNotificationVisible
        {
            get => _isNotificationVisible;
            set { _isNotificationVisible = value; OnPropertyChanged(); }
        }

        public ICommand IncreaseCommand { get; }
        public ICommand DecreaseCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand FinishCommand { get; }

        public event Action<string> OnNavigate;

        public CreateCharacterViewModel()
        {
            IncreaseCommand = new RelayCommand(IncreaseAttribute);
            DecreaseCommand = new RelayCommand(DecreaseAttribute);
            ResetCommand = new RelayCommand(o => ResetAttributes());
            FinishCommand = new RelayCommand(o => Finish());
            Character.PropertyChanged += (s, e) => OnPropertyChanged(nameof(Character));

        }

        private void IncreaseAttribute(object param)
        {
            if (AvailablePoints < 1) return;
            if (param is string attr)
            {
                switch (attr)
                {
                    case "HP": Character.HP++; break;
                    case "MP": Character.MP++; break;
                    case "STR": Character.STR++; break;
                    case "DEF": Character.DEF++; break;
                    case "INT": Character.INT++; break;
                    case "AGI": Character.AGI++; break;
                    case "LUK": Character.LUK++; break;
                }
                AvailablePoints--;
                OnPropertyChanged(attr);
            }
        }

        private void DecreaseAttribute(object param)
        {
            if (param is string attr)
            {
                bool canDecrease = false;
                switch (attr)
                {
                    case "HP": canDecrease = Character.HP > 10; if (canDecrease) Character.HP--; break;
                    case "MP": canDecrease = Character.MP > 10; if (canDecrease) Character.MP--; break;
                    case "STR": canDecrease = Character.STR > 10; if (canDecrease) Character.STR--; break;
                    case "DEF": canDecrease = Character.DEF > 10; if (canDecrease) Character.DEF--; break;
                    case "INT": canDecrease = Character.INT > 10; if (canDecrease) Character.INT--; break;
                    case "AGI": canDecrease = Character.AGI > 10; if (canDecrease) Character.AGI--; break;
                    case "LUK": canDecrease = Character.LUK > 10; if (canDecrease) Character.LUK--; break;
                }
                if (canDecrease)
                {
                    AvailablePoints++;
                    OnPropertyChanged(attr);
                }
            }
        }

        private void ResetAttributes()
        {
            Character.HP = 10;
            Character.MP = 10;
            Character.STR = 10;
            Character.DEF = 10;
            Character.INT = 10;
            Character.AGI = 10;
            Character.LUK = 10;
            AvailablePoints = 10;
            OnPropertyChanged(string.Empty);
        }

        private async void Finish()
        {
            if (string.IsNullOrWhiteSpace(Character.Name))
            {
                Notification = "請輸入角色名稱。";
                IsNotificationVisible = true;
                OnPropertyChanged(nameof(Notification));
                await Task.Delay(2000);
                IsNotificationVisible = false;
                return;
            }
            OnNavigate?.Invoke("Start");
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
