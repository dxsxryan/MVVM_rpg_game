using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MVVM_pratices_2.Model;
using MVVM_pratices_2.Enum;

namespace MVVM_pratices_2.ViewModel
{
    public class InventoryViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ItemSlot> InventoryItems { get; set; } = new();

        private ItemSlot _selectedItem;
        public ItemSlot SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != null) _selectedItem.IsSelected = false;
                _selectedItem = value;
                if (_selectedItem != null) _selectedItem.IsSelected = true;

                OnPropertyChanged();
                OnPropertyChanged(nameof(SelectedItemDescription));
                OnPropertyChanged(nameof(CanEquip));
                OnPropertyChanged(nameof(CanUse));
            }
        }

        public string SelectedItemDescription =>
            SelectedItem == null ? ""
            : $"{SelectedItem.Description}\n\n能力：{SelectedItem.Stats}\n價格：{SelectedItem.SellPrice} G";

        public bool CanEquip => SelectedItem?.Type == ItemType.Equipment;
        public bool CanUse => SelectedItem?.Type == ItemType.Usable;

        public ICommand EquipCommand { get; }
        public ICommand UseCommand { get; }
        public ICommand DropCommand { get; }
        public ICommand BackCommand { get; }
        public ICommand SelectItemCommand { get; }

        public InventoryViewModel()
        {
            EquipCommand = new RelayCommand(_ => Equip(), _ => CanEquip);
            UseCommand = new RelayCommand(_ => Use(), _ => CanUse);
            DropCommand = new RelayCommand(_ => Drop(), _ => SelectedItem != null);
            BackCommand = new RelayCommand(_ => OnNavigate?.Invoke("Start"));
            SelectItemCommand = new RelayCommand(obj =>
            {
                if (obj is ItemSlot slot)
                {
                    SelectedItem = slot;
                }
            });

            // 範例資料
            InventoryItems.Add(new ItemSlot
            {
                Name = "鋼劍",
                Stats = "+10 攻擊力",
                Description = "一把鋒利的劍。",
                SellPrice = 120,
                Type = ItemType.Equipment
            });
            InventoryItems.Add(new ItemSlot
            {
                Name = "回復藥水",
                Stats = "回復 50 HP",
                Description = "常見的治療藥劑。",
                SellPrice = 50,
                Type = ItemType.Usable
            });
            InventoryItems.Add(new ItemSlot
            {
                Name = "獸牙",
                Stats = "無",
                Description = "從野獸身上掉落的尖牙。",
                SellPrice = 30,
                Type = ItemType.Material
            });
        }

        private void Equip() { }
        private void Use() { }
        private void Drop()
        {
            if (SelectedItem != null)
            {
                InventoryItems.Remove(SelectedItem);
                SelectedItem = null;
            }
        }

        public Action<string> OnNavigate { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
