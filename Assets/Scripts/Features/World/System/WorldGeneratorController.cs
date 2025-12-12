using System.Collections.Generic;
using Systems.WorldSystem;
using UnityEngine;

namespace Features.World.System {
    public class WorldGeneratorController : MonoBehaviour {
        
        [SerializeField] private List<WorldGenerationConfig> mapConfigs = new List<WorldGenerationConfig>();
        [SerializeField] private int configIndex = 0;
        [SerializeField] private bool forceGenerateNewWorld = false;
        
    }
}