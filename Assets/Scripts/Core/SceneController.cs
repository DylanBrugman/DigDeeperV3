using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core {
    public class SceneController : MonoBehaviour
    {
        public static SceneController Instance { get; private set; }
        
        [Header("Scene Configuration")]
        [SerializeField] private float minLoadingTime = 1f;
        [SerializeField] private bool showLoadingProgress = true;
        
        // Loading state
        private bool isLoading = false;
        private AsyncOperation currentLoadOperation;
        
        // Events
        public static event Action<string> OnSceneLoadStarted;
        public static event Action<string> OnSceneLoadCompleted;
        public static event Action<float> OnSceneLoadProgress;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void LoadScene(string sceneName, bool useLoadingScreen = true)
        {
            if (isLoading)
            {
                Debug.LogWarning("Scene load already in progress");
                return;
            }
            
            if (useLoadingScreen)
            {
                StartCoroutine(LoadSceneWithLoadingScreen(sceneName));
            }
            else
            {
                StartCoroutine(LoadSceneDirectly(sceneName));
            }
        }
        
        private IEnumerator LoadSceneWithLoadingScreen(string sceneName)
        {
            isLoading = true;
            OnSceneLoadStarted?.Invoke(sceneName);
            
            // Load loading scene first
            yield return SceneManager.LoadSceneAsync("LoadingScene");
            
            // Wait a frame for loading scene to initialize
            yield return null;
            
            // Start loading target scene
            currentLoadOperation = SceneManager.LoadSceneAsync(sceneName);
            currentLoadOperation.allowSceneActivation = false;
            
            float elapsedTime = 0f;
            
            while (!currentLoadOperation.isDone)
            {
                elapsedTime += Time.unscaledDeltaTime;
                
                float progress = Mathf.Clamp01(currentLoadOperation.progress / 0.9f);
                OnSceneLoadProgress?.Invoke(progress);
                
                // Wait minimum loading time and ensure loading is complete
                if (currentLoadOperation.progress >= 0.9f && elapsedTime >= minLoadingTime)
                {
                    currentLoadOperation.allowSceneActivation = true;
                }
                
                yield return null;
            }
            
            isLoading = false;
            OnSceneLoadCompleted?.Invoke(sceneName);
        }
        
        private IEnumerator LoadSceneDirectly(string sceneName)
        {
            isLoading = true;
            OnSceneLoadStarted?.Invoke(sceneName);
            
            currentLoadOperation = SceneManager.LoadSceneAsync(sceneName);
            
            while (!currentLoadOperation.isDone)
            {
                OnSceneLoadProgress?.Invoke(currentLoadOperation.progress);
                yield return null;
            }
            
            isLoading = false;
            OnSceneLoadCompleted?.Invoke(sceneName);
        }
        
        public bool IsLoadingScene => isLoading;
        public float LoadingProgress => currentLoadOperation?.progress ?? 0f;
    }
}

public class SceneController : MonoBehaviour {
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() { }

    // Update is called once per frame
    void Update() { }
}