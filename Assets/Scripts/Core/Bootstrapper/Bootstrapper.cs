// ===================================================================
// UNITY AUTO-LOADING BOOTSTRAP METHODS
// ===================================================================

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    public static class Bootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void InitializeBootstrap()
        {
            String persistentSceneName = "PersistentScene";
            
            if (!SceneManager.GetSceneByName(persistentSceneName).isLoaded)
            {
                SceneManager.LoadScene(persistentSceneName, LoadSceneMode.Additive);
                Debug.Log($"Bootstrapper: Loading persistent scene '{persistentSceneName}' additively.");
            }
            else
            {
                Debug.Log($"Bootstrapper: Persistent scene '{persistentSceneName}' already loaded.");
            }
        }
    }
}