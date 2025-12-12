using UnityEngine;

namespace Core {
    [CreateAssetMenu(fileName = "GameSettings", menuName = "DigDeeper/Game Settings")]
    public class GameSettings : ScriptableObject
    {
        [Header("Game Configuration")]
        public int initialColonistCount = 3;
        public Vector2Int worldSize = new Vector2Int(200, 100);
        public int maxColonists = 50;
        
        [Header("Gameplay Settings")]
        public float gameSpeed = 1f;
        public bool pauseOnLostFocus = true;
        public bool autoSave = true;
        public float autoSaveInterval = 300f; // 5 minutes
        
        [Header("Debug Settings")]
        public bool enableDebugMode = false;
        public bool showPerformanceStats = false;
        public bool logSystemEvents = false;
    }
}