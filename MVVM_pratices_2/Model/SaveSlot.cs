using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_pratices_2.Model
{
    public class SaveSlot
    {
        public int SlotId { get; set; }
        public string CharacterName { get; set; }
        public int Level { get; set; }
        public DateTime Timestamp { get; set; }

        public string DisplayTime => Timestamp == default? "0000/00/00": Timestamp.ToString("yyyy/MM/dd HH:mm");
    }
}
