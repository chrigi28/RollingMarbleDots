using System;
using System.Collections.Generic;

namespace Assets.Scripts.GameData
{
    [Serializable]
    public class LevelData
    {
        public int levelNumber;
        public float time;
        public byte stars = 0;
    }
}