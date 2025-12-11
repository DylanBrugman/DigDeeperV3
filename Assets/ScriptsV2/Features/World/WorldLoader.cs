using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using Core.ECS;
using GamePlay.Map;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay.World {
    public sealed class WorldLoader : MonoBehaviour {
        
        [SerializeField] private string gameplayScene = "GameScene";
        [SerializeField] private WorldGeneratedPrefab worldSaveScriptableObject;
        
        public event Action<float> OnProgress; // 0‑1 progress bar
        public event Action OnWorldReady; // fired after PostScene spawners

        private List<IWorldSpawner> _spawners = new List<IWorldSpawner>();
        private CancellationTokenSource _cts;
        private ECSWorld _ecsWorld = null;
        private WorldGenerator _worldGenerator;

        private void Awake() {
            ServiceLocator.Register(this);
            DontDestroyOnLoad(this);
        }

        private void Start() {
            _ecsWorld = ServiceLocator.GetOrThrow<ECSWorld>();
            _worldGenerator = ServiceLocator.GetOrThrow<WorldGenerator>();
            
            _spawners = ServiceLocator.GetAll<IWorldSpawner>().ToList();
        }
        
        public void Load(WorldSourceType worldSourceType) {
            IWorldSource source = worldSourceType switch {
                WorldSourceType.Generated => _worldGenerator,
                WorldSourceType.SaveFile => throw new NotImplementedException(),
                WorldSourceType.ScriptableObject => worldSaveScriptableObject,
                _ => throw new ArgumentOutOfRangeException(nameof(worldSourceType), worldSourceType, null)
            };
            Load(source);
        }

        public void Load(IWorldSource source) {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            StartCoroutine(LoadRoutine(source, _cts.Token));
        }

        public void Cancel() => _cts?.Cancel();

        private IEnumerator LoadRoutine(IWorldSource worldSource, CancellationToken ct) {
            Debug.Log("Loading scene: LoadingScene");
            Scene loadingScene = SceneManager.GetSceneByName("LoadingScene");
            if (loadingScene.isLoaded && SceneManager.GetActiveScene() != loadingScene) {
                SceneManager.SetActiveScene(loadingScene);
            }
            
            var sceneOp = SceneManager.LoadSceneAsync(gameplayScene, LoadSceneMode.Additive);
            sceneOp.allowSceneActivation = false;

            WorldData bundle = null;
            bool done = false;
            var loadEnum = worldSource.LoadCoroutine(worldData => {
                    bundle = worldData;
                    done = true;
                },
                p => OnProgress?.Invoke(p * .5f), ct);
            
            while (!done) {
                if (!loadEnum.MoveNext()) break;
                yield return loadEnum.Current;
                if (ct.IsCancellationRequested) yield break;
            }

            var worldRuntimeContext = new WorldRuntimeContext(bundle);
            ServiceLocator.Register(worldRuntimeContext);
            OnProgress?.Invoke(.65f);
            yield return null;

            foreach (var spawner in _spawners) {
                spawner.SpawnPreScene(bundle, worldRuntimeContext, _ecsWorld);
            }

            OnProgress?.Invoke(.8f);
            yield return null;

            while (sceneOp.progress < 0.9f) {
                OnProgress?.Invoke(.8f + sceneOp.progress * .1f); // maps 0‑0.9 → 0.8‑0.89
                yield return null;
                if (ct.IsCancellationRequested) yield break;
            }

            OnProgress?.Invoke(.9f);

            /* Activate scene */
            sceneOp.allowSceneActivation = true;
            while (!sceneOp.isDone) {
                OnProgress?.Invoke(.9f + sceneOp.progress * .1f);
                yield return null;
            }

            /* Post‑scene spawners (GameObjects land in gameplay scene) */
            foreach (var spawner in _spawners) {
                spawner.SpawnPostScene(bundle, worldRuntimeContext, _ecsWorld);
            }

            OnProgress?.Invoke(1f);
            OnWorldReady?.Invoke();
        }
    }
}