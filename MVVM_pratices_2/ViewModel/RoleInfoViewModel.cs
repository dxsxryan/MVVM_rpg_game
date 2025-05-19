using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using MVVM_pratices_2.Model;

namespace MVVM_pratices_2.ViewModel
{
    public class RoleInfoViewModel : INotifyPropertyChanged
    {
        public CharacterData Character { get; set; }

        private string _playTime;
        public string PlayTime
        {
            get => _playTime;
            set { _playTime = value; OnPropertyChanged(); }
        }

        public int MonsterKillCount { get; set; }
        public int DeathCount { get; set; }

        public string HPDisplay => $"{Character.HP * 50}/{Character.HP * 50}";
        public string MPDisplay => $"{Character.MP * 50}/{Character.MP * 50}";

        public ICommand BackCommand { get; }
        public event Action<string> OnNavigate;

        public RoleInfoViewModel(CharacterData character)
        {
            Character = character;
            MonsterKillCount = 0;
            DeathCount = 0;

            BackCommand = new RelayCommand(_ => OnNavigate?.Invoke("Start"));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

}
