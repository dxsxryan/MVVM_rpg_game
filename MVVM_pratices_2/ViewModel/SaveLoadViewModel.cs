using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVM_pratices_2.Model;
using System.Windows.Input;
using System.Windows;
using System.IO;
using System.Text.Json;
using System;
using MVVM_pratices_2.ViewModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM_pratices_2.ViewModel
{
    public class SaveLoadViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Action<string> OnNavigate { get; set; }

        private readonly bool _isLoadMode;
        private readonly string _origin;
        private readonly Connector _connector;

        public ObservableCollection<SaveSlot> SaveSlots { get; } = new();
        public ICommand SelectSlotCommand { get; }
        public ICommand BackCommand { get; }

        public SaveLoadViewModel(bool isLoadMode, string origin, Connector connector)
        {
            _isLoadMode = isLoadMode;
            _origin = origin;
            _connector = connector;

            SelectSlotCommand = new RelayCommand(param =>
            {
                if (param is SaveSlot slot)
                {
                    HandleSlot(slot.SlotId);
                }
            });

            BackCommand = new RelayCommand(_ => OnNavigate?.Invoke(_origin));

            for (int i = 1; i <= 3; i++)
            {
                SaveSlots.Add(LoadSlotMetadata(i));
            }
        }

        private void HandleSlot(int slot)
        {
            if (_isLoadMode)
                LoadSlotFromFile(slot);
            else
                SaveSlotToFile(slot);
        }

        private void SaveSlotToFile(int slot)
        {
            var data = new SaveData
            {
                Character = _connector.CreateCharacterVM.Character,
                PlayTimeSeconds = _connector.GetTotalPlayTimeSeconds(),
                MonsterKills = 0,
                DeathCount = 0,
                SaveTime = DateTime.Now
            };

            string dir = Path.Combine(AppContext.BaseDirectory, "Save");
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, $"slot{slot}.json");

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);

            // 即時更新畫面顯示
            SaveSlots[slot - 1] = LoadSlotMetadata(slot);
        }
        public void RefreshSlots()
        {
            SaveSlots.Clear();
            for (int i = 1; i <= 3; i++)
                SaveSlots.Add(LoadSlotMetadata(i));
        }

        private void LoadSlotFromFile(int slot)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Save", $"slot{slot}.json");
            if (!File.Exists(path)) return;

            var json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<SaveData>(json);

            _connector.CreateCharacterVM.Character = data.Character;
            _connector.SetLoadedPlayTime(data.PlayTimeSeconds);
            _connector.StartPlayTimeTracking();

            SaveSlots[slot - 1] = LoadSlotMetadata(slot);

            OnNavigate?.Invoke("Start");
        }


        private SaveSlot LoadSlotMetadata(int slot)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Save", $"slot{slot}.json");
            if (!File.Exists(path)) return new SaveSlot { SlotId = slot };

            var json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<SaveData>(json);

            return new SaveSlot
            {
                SlotId = slot,
                CharacterName = data.Character.Name,
                Level = data.Character.Level,
                Timestamp = data.SaveTime
            };
        }
    }
}
