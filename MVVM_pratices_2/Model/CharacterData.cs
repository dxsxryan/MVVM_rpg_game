using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_pratices_2.Model
{
    public class CharacterData
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(); }
        }

        private int _hp = 10;
        public int HP
        {
            get => _hp;
            set { _hp = value; OnPropertyChanged(); }
        }

        private int _mp = 10;
        public int MP
        {
            get => _mp;
            set { _mp = value; OnPropertyChanged(); }
        }

        private int _str = 10;
        public int STR
        {
            get => _str;
            set { _str = value; OnPropertyChanged(); }
        }

        private int _def = 10;
        public int DEF
        {
            get => _def;
            set { _def = value; OnPropertyChanged(); }
        }

        private int _int = 10;
        public int INT
        {
            get => _int;
            set { _int = value; OnPropertyChanged(); }
        }

        private int _agi = 10;
        public int AGI
        {
            get => _agi;
            set { _agi = value; OnPropertyChanged(); }
        }

        private int _luk = 10;
        public int LUK
        {
            get => _luk;
            set { _luk = value; OnPropertyChanged(); }
        }

        private int _money = 0;
        public int Money
        {
            get => _money;
            set { _money = value; OnPropertyChanged(); }
        }

        private int _exp = 0;
        public int Exp
        {
            get => _exp;
            set { _exp = value; OnPropertyChanged(); }
        }

        private int _level = 1;
        public int Level
        {
            get => _level;
            set { _level = value; OnPropertyChanged(); }
        }

        private List<string> _inventory = new List<string>();
        public List<string> Inventory
        {
            get => _inventory;
            set { _inventory = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
