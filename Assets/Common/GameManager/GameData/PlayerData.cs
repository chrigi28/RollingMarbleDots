using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Assets.Scripts.GameData
{
    [Serializable]
    public class PlayerData
    {
        public static PlayerData Instance;
        public List<LevelData> LevelDatas = new List<LevelData>();
        public int CurrentLevel { get; set; }

        public void AddLevelData(int level, float time, byte stars)
        {
            LevelDatas.Add(new LevelData() {levelNumber = level, stars = stars, time = time});
            SaveData(this);
        }

        private static void SaveData(PlayerData data)
        {
            GameDataManager.SavePlayerData(data);
        }

        public void SetLevelData(int level, float time, byte star)
        {
            var levelData = LevelDatas.FirstOrDefault(f => f.levelNumber == level);
            if (levelData != null && levelData.time > time)
            {
                levelData.time = time;
                levelData.stars = star;
            }
            else
            {
                LevelDatas.Add(new LevelData{levelNumber = level, stars = star, time = time});
            }

            GameDataManager.SavePlayerData(PlayerData.Instance);
        }

        public float GetBestTimeOfCurrentLevel()
        {
            var levelData = LevelDatas.FirstOrDefault(f => f.levelNumber == this.CurrentLevel);
            if (levelData != null)
            {
                return levelData.time;
            }
            else
            {
                return 0;
            }
        }
    }
}