using Features.World;
using UnityEngine;

namespace Core {
    internal class GameConfig : ScriptableObject {
        [SerializeField] private WorldConfig worldConfig;
        
        public WorldConfig WorldConfig => worldConfig;
    }
}