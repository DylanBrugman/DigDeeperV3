using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ECSImpl.Systems;
using GamePlay.Jobs;
using GamePlay.Movement;
using GamePlay.Needs;
using GamePlay.Pathfinding.Nav;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Core.ECS
{
    /// <summary>
    /// Central update loop for ECS systems.
    /// Provides lightweight per‑system profiling and exposes the
    /// latest timings in the Inspector at runtime.
    /// </summary>
    public sealed class ECSSystemRunner : MonoBehaviour {
        [Tooltip("Print per‑system timings to the Console each frame.")]
        [SerializeField] private bool logSystemDurations = false;

        [Header("Runtime metrics (read‑only)")]
        [SerializeField] private List<ECSSystemInfo> systemsInfo = new();

        [SerializeField] private double totalMs;
        [SerializeField] private int totalEntitiesProcessed;
        
        public ECSWorld ECSWorld { get; private set; }
        public List<ECSSystemInfo> SystemsInfo => systemsInfo;

        private readonly List<IEcsSystem> _systems = new();
        private readonly Dictionary<Type, ECSSystemInfo> _infoMap = new();

        private void Awake()
        {
            ECSWorld = new ECSWorld();
            ServiceLocator.Register(ECSWorld);

            RegisterSystem(new NeedDecaySystem());
            RegisterSystem(new PersonalJobsSpawningSystem());
            RegisterSystem(new MovementSystem());
            RegisterSystem(new ViewSyncSystem());
            RegisterSystem(new NavRebuildSystem());
            
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            totalMs = 0;
            totalEntitiesProcessed = 0;
            
            TickECSSystems(dt);

            if (logSystemDurations) {
                DumpDurations();                
            }
        }

        private void TickECSSystems(float dt) {
            foreach (IEcsSystem system in _systems)
            {
                // Inline profiling
                Stopwatch sw = Stopwatch.StartNew();
                system.Tick(ECSWorld, dt);
                sw.Stop();

                ECSSystemInfo info = _infoMap[system.GetType()];
                info.LastFrameMs = sw.Elapsed.TotalMilliseconds;
                info.TotalMs += info.LastFrameMs;
                info.ProcessedEntitiesCount = system.ProcessedEntitiesCount;
                
                totalMs += info.LastFrameMs;
                totalEntitiesProcessed += info.ProcessedEntitiesCount;
            }
        }

        #region Helpers

        private void RegisterSystem(IEcsSystem system)
        {
            _systems.Add(system);

            var info = new ECSSystemInfo(system.GetType().Name, system.GetType());
            systemsInfo.Add(info);
            _infoMap[system.GetType()] = info;
        }

        private void DumpDurations()
        {
            foreach (var info in systemsInfo.OrderByDescending(i => i.LastFrameMs))
            {
                Debug.Log($"{info.Name,-25} | {info.LastFrameMs:0.000} ms  (Σ {info.TotalMs:0.0} ms)");
            }
        }

        #endregion
    }

    /// <summary>Serializable container for per‑system runtime metrics.</summary>
    [Serializable]
    public sealed class ECSSystemInfo
    {
        [HideInInspector] public string Name;
        [HideInInspector] public Type SystemType;
        [Tooltip("Execution time in the last frame (ms)")]
        public double LastFrameMs;
        [Tooltip("Accumulated execution time over the lifetime of the scene (ms)")]
        public double TotalMs;
        [Tooltip("Number of entities processed in the last tick")]
        public int ProcessedEntitiesCount;

        public ECSSystemInfo(string name, Type type)
        {
            Name = name;
            SystemType = type;
        }
    }

}
