using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVM_pratices_2.ViewModel
{
    public class StartViewModel
    {
        public ICommand RoleInfoCommand { get; }
        public ICommand InventoryCommand { get; }
        public ICommand MonsterItemsCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand LoadCommand { get; }


        public event Action<string> OnNavigate;

        public StartViewModel()
        {
            RoleInfoCommand = new RelayCommand(o => OnNavigate?.Invoke("RoleInfo"));
            InventoryCommand = new RelayCommand(o => Show("背包道具"));
            MonsterItemsCommand = new RelayCommand(o => Show("怪物道具"));
            MenuCommand = new RelayCommand(o => OnNavigate?.Invoke("Menu"));
            SaveCommand = new RelayCommand(ｏ => OnNavigate?.Invoke("SavePage"));
            LoadCommand = new RelayCommand(ｏ => OnNavigate?.Invoke("LoadPage"));
        }

        private void Show(string msg)
        {
            System.Windows.MessageBox.Show($"{msg} 尚未實作", "提示");
        }
    }
}
