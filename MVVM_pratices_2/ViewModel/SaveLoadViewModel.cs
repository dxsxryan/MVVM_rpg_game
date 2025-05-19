using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM_pratices_2.Model;
using System.Windows.Input;
using System.Windows;

namespace MVVM_pratices_2.ViewModel
{
    public class SaveLoadViewModel
    {
        public ObservableCollection<SaveSlot> SaveSlots { get; set; }
        public bool IsLoadMode { get; set; } // true: 讀檔；false: 存檔
        public string ReturnTo { get; set; }
        public ICommand SelectSlotCommand { get; }
        public ICommand BackCommand { get; }
        public event Action<string> OnNavigate;

        public SaveLoadViewModel(bool isLoadMode, string returnTo)
        {
            IsLoadMode = isLoadMode;
            ReturnTo = returnTo;
            BackCommand = new RelayCommand(_ => OnNavigate?.Invoke(ReturnTo));

            SaveSlots = new ObservableCollection<SaveSlot>
            {
                new SaveSlot { SlotId = 1, CharacterName = "", Level = 10, Timestamp = DateTime.Now.AddMinutes(0) },
                new SaveSlot { SlotId = 2, CharacterName = "", Level = 0, Timestamp = default },
                new SaveSlot { SlotId = 3, CharacterName = "", Level = 0, Timestamp = default },
            };

            SelectSlotCommand = new RelayCommand(ExecuteSlot);
            BackCommand = new RelayCommand(o =>{OnNavigate?.Invoke(ReturnTo);});
        }

        private void ExecuteSlot(object param)
        {
            if (param is SaveSlot slot)
            {
                if (IsLoadMode)
                {
                    System.Windows.MessageBox.Show($"讀取 {slot.CharacterName} Lv.{slot.Level} 的存檔。", "讀取成功");
                }
                else
                {
                    slot.CharacterName = "亞倫";
                    slot.Level = 99;
                    slot.Timestamp = DateTime.Now;
                    System.Windows.MessageBox.Show($"已儲存至欄位 {slot.SlotId}。", "儲存成功");
                }
            }
        }
    }
}
