using Features.World;
using Features.World.Grid;
using UnityEngine;

namespace Core {
    public class GameManager : MonoBehaviour {
        
        [SerializeField] private bool autoStartGame = true;
        [SerializeField] private GameConfig gameConfig;

        private WorldGridManager _worldGridManager;
        
        private void Start() {
            if (autoStartGame) {
                StartNewGame();
            }
        }

        private void StartNewGame() {
            _worldGridManager.GenerateNew(gameConfig.WorldConfig);
            
        }
    }
}