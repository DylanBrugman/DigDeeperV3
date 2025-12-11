using System;
using System.Collections;
using Core;
using GamePlay.World;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    [Header("Game Configuration")]
    [SerializeField] private GameSettings gameSettings;
    [SerializeField] private bool autoStartGame = false;
    [SerializeField] private GameState initialGameState = GameState.Playing;
    
    [Header("World load configuration")]
    [SerializeField] private WorldSourceType worldSourceType;
    private WorldLoader _worldLoader;
    
    private GameState _currentGameState;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
        
        SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
    }

    private IEnumerator Start() {
        _worldLoader = ServiceLocator.GetOrThrow<WorldLoader>();
        _currentGameState = initialGameState;
        Debug.Log("Initial game state: " + initialGameState);

        //Waits one frame in which all start methods are called before changing the game state
        yield return null;
        ChangeGameState(_currentGameState);
    }
    
    private void ChangeGameState(GameState currentGameState) {
        switch (currentGameState) {
            case GameState.MainMenu:
                if (SceneManager.GetActiveScene().name != "MainMenu") {
                    SceneManager.LoadScene("MainMenu");
                }
                break;
            case GameState.Loading:
                if (SceneManager.GetActiveScene().name != "LoadingScene") {
                    SceneManager.LoadScene("LoadingScene");
                }
                break;
            case GameState.Playing:
                // _worldLoader.Load(worldSourceType);
                break;
            case GameState.Paused:
                break;
            case GameState.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(currentGameState), currentGameState, null);
        }
    }
}

// using System;
// using DigDeeper.ColonistSystem;
// using DigDeeper.WorldSystem;
// using Systems.ColonistSystem;
// using Systems.SaveSystem;
// using Systems.WorldSystem;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// namespace Core {
//     public class GameManager : MonoBehaviour, ISystemManager {
//         public static GameManager Instance { get; private set; }
//
//         [Header("Game Configuration")] [SerializeField]
//         private GameSettings gameSettings;
//
//         [SerializeField] private bool autoStartGame = false;
//         [SerializeField] private GameState initialGameState = GameState.Playing;
//
//         // Game State
//         private GameState currentGameState = GameState.MainMenu;
//         private bool isPaused = false;
//         private bool isInitialized = false;
//
//         // Properties
//         public bool IsInitialized => isInitialized;
//         public GameState CurrentGameState => currentGameState;
//         public bool IsPaused => isPaused;
//         public GameSettings Settings => gameSettings;
//
//         // Events
//         public static event Action<GameState> OnGameStateChanged;
//         public static event Action OnGamePaused;
//         public static event Action OnGameResumed;
//
//         private void OnEnable() {
//             BootstrapManager.OnBootstrapComplete += OnBootstrapComplete;
//         }
//
//         private void OnDisable() {
//             BootstrapManager.OnBootstrapComplete -= OnBootstrapComplete;
//         }
//
//         private void OnBootstrapComplete() {
//             // Set initial game state based on current scene
//             currentGameState = initialGameState;
//             Debug.Log("Initial game state: " + initialGameState);
//             switch (CurrentGameState) {
//                 case GameState.MainMenu:
//                     if (SceneManager.GetActiveScene().name != "MainMenu") {
//                         SceneManager.LoadScene("MainMenu");
//                     }
//                     break;
//                 case GameState.Loading:
//                     if (SceneManager.GetActiveScene().name != "LoadingScene") {
//                         SceneManager.LoadScene("LoadingScene");
//                     }
//                     break;
//                 case GameState.Playing:
//                     if (SceneManager.GetActiveScene().name != "GameScene") {
//                         SceneManager.LoadScene("GameScene");
//                     }
//
//                     if (autoStartGame) {
//                         StartNewGame();
//                     }
//
//                     break;
//                 case GameState.Paused:
//                     if (SceneManager.GetActiveScene().name != "GameScene") {
//                         SceneManager.LoadScene("MainMenu");
//                     }
//                     break;
//                 case GameState.GameOver:
//                     break;
//                 default:
//                     throw new ArgumentOutOfRangeException();
//             }
//             GameStateChanged(CurrentGameState);
//         }
//
//         private void Awake() {
//             if (Instance == null) {
//                 Instance = this;
//                 DontDestroyOnLoad(gameObject);
//             }
//             else {
//                 Destroy(gameObject);
//             }
//         }
//
//         public void Initialize() {
//             if (isInitialized) return;
//
//             // Load game settings
//             if (gameSettings == null) {
//                 gameSettings = Resources.Load<GameSettings>("Config/GameSettings");
//             }
//             isInitialized = true;
//         }
//
//         public void UpdateSystem() {
//             // Handle input and game state updates
//             HandleInput();
//         }
//
//         public void CleanupSystem() {
//             // Cleanup when shutting down
//         }
//
//         private void HandleInput() {
//             if (Input.GetKeyDown(KeyCode.Escape)) {
//                 if (currentGameState == GameState.Playing) {
//                     TogglePause();
//                 }
//             }
//         }
//
//         public void GameStateChanged(GameState newState) {
//             if (currentGameState == newState) return;
//
//             var previousState = currentGameState;
//             currentGameState = newState;
//
//             Debug.Log($"Game state changed: {previousState} -> {newState}");
//             OnGameStateChanged?.Invoke(newState);
//         }
//
//         public void StartNewGame() {
//             // Initialize game ecsWorld
//             // if (WorldManager.Instance != null)
//             // {
//             Debug.Log("Starting new game...");
//             WorldManager.Instance.GenerateWorld();
//             // }
//
//             // Spawn initial colonists
//             if (ColonistManager.Instance != null) {
//                 SpawnInitialColonists();
//             }
//         }
//
//         public void LoadGame(string saveFileName) {
//             GameStateChanged(GameState.Loading);
//
//             if (SaveManager.Instance != null) {
//                 SaveManager.Instance.LoadGame(saveFileName);
//             }
//
//             GameStateChanged(GameState.Playing);
//         }
//
//         public void SaveGame(string saveFileName) {
//             if (SaveManager.Instance != null) {
//                 SaveManager.Instance.SaveGame(saveFileName);
//             }
//         }
//
//         public void TogglePause() {
//             if (isPaused) {
//                 ResumeGame();
//             }
//             else {
//                 PauseGame();
//             }
//         }
//
//         public void PauseGame() {
//             if (!isPaused && currentGameState == GameState.Playing) {
//                 isPaused = true;
//                 Time.timeScale = 0f;
//                 OnGamePaused?.Invoke();
//             }
//         }
//
//         public void ResumeGame() {
//             if (isPaused) {
//                 isPaused = false;
//                 Time.timeScale = 1f;
//                 OnGameResumed?.Invoke();
//             }
//         }
//
//         public void ReturnToMainMenu() {
//             ResumeGame(); // Ensure time scale is reset
//             GameStateChanged(GameState.MainMenu);
//             SceneManager.LoadScene("MainMenu");
//         }
//
//         public void QuitGame() {
//             SaveGame("autosave");
//
// #if UNITY_EDITOR
//             UnityEditor.EditorApplication.isPlaying = false;
// #else
//             Application.Quit();
// #endif
//         }
//
//         private void SpawnInitialColonists() {
//             if (gameSettings != null && ColonistManager.Instance != null) {
//                 for (int i = 0; i < gameSettings.initialColonistCount; i++) {
//                     var spawnPosition = new Vector2Int(i * 2, 0); // Simple spawn pattern
//                     ColonistManager.Instance.SpawnColonist(spawnPosition);
//                 }
//             }
//         }
//     }
// }