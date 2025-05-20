using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_pratices_2.Model
{
    public class SaveData
    {
        public CharacterData Character { get; set; }

        public double PlayTimeSeconds { get; set; } // 累積的遊玩秒數
        public int MonsterKills { get; set; } = 0;
        public int DeathCount { get; set; } = 0;

        public DateTime SaveTime { get; set; } = DateTime.Now;
    }

}
