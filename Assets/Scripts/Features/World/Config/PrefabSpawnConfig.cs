using System;

namespace GamePlay.World {
    [Serializable]
    public struct PrefabSpawnConfig {
        public string PrefabId;
        public int X, Y;
    }
}