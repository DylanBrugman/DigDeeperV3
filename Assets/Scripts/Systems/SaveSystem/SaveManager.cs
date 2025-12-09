using Core;

namespace Systems.SaveSystem {
    public class SaveManager : ISystemManager {
        
        public static SaveManager Instance { get; private set; }
        
        public void Initialize() {
            throw new System.NotImplementedException();
        }

        public void UpdateSystem() {
            throw new System.NotImplementedException();
        }

        public void CleanupSystem() {
            throw new System.NotImplementedException();
        }

        public bool IsInitialized { get; }

        public void LoadGame(string saveFileName) {
            throw new System.NotImplementedException();
        }

        public void SaveGame(string saveFileName) {
            throw new System.NotImplementedException();
        }
    }
}