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
        }

        public void ResetCharacter()
        {
            Character = new CharacterData();
            AvailablePoints = 10;
            OnPropertyChanged(nameof(Character));
            OnPropertyChanged(nameof(AvailablePoints));
            OnPropertyChanged(nameof(HP));
            OnPropertyChanged(nameof(MP));
            OnPropertyChanged(nameof(STR));
            OnPropertyChanged(nameof(DEF));
            OnPropertyChanged(nameof(INT));
            OnPropertyChanged(nameof(AGI));
            OnPropertyChanged(nameof(LUK));
        }

        // 轉接能力值屬性供 UI 綁定
        public int HP { get => Character.HP; set { Character.HP = value; OnPropertyChanged(); } }
        public int MP { get => Character.MP; set { Character.MP = value; OnPropertyChanged(); } }
        public int STR { get => Character.STR; set { Character.STR = value; OnPropertyChanged(); } }
        public int DEF { get => Character.DEF; set { Character.DEF = value; OnPropertyChanged(); } }
        public int INT { get => Character.INT; set { Character.INT = value; OnPropertyChanged(); } }
        public int AGI { get => Character.AGI; set { Character.AGI = value; OnPropertyChanged(); } }
        public int LUK { get => Character.LUK; set { Character.LUK = value; OnPropertyChanged(); } }

        private void IncreaseAttribute(object param)
        {
            if (AvailablePoints < 1) return;
            if (param is string attr)
            {
                switch (attr)
                {
                    case "HP": HP++; break;
                    case "MP": MP++; break;
                    case "STR": STR++; break;
                    case "DEF": DEF++; break;
                    case "INT": INT++; break;
                    case "AGI": AGI++; break;
                    case "LUK": LUK++; break;
                }
                AvailablePoints--;
            }
        }

        private void DecreaseAttribute(object param)
        {
            if (param is string attr)
            {
                bool canDecrease = false;
                switch (attr)
                {
                    case "HP": canDecrease = HP > 10; if (canDecrease) HP--; break;
                    case "MP": canDecrease = MP > 10; if (canDecrease) MP--; break;
                    case "STR": canDecrease = STR > 10; if (canDecrease) STR--; break;
                    case "DEF": canDecrease = DEF > 10; if (canDecrease) DEF--; break;
                    case "INT": canDecrease = INT > 10; if (canDecrease) INT--; break;
                    case "AGI": canDecrease = AGI > 10; if (canDecrease) AGI--; break;
                    case "LUK": canDecrease = LUK > 10; if (canDecrease) LUK--; break;
                }
                if (canDecrease)
                {
                    AvailablePoints++;
                }
            }
        }

        private void ResetAttributes()
        {
            HP = 10;
            MP = 10;
            STR = 10;
            DEF = 10;
            INT = 10;
            AGI = 10;
            LUK = 10;
            AvailablePoints = 10;
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
