using DigDeeper.WorldSystem;

namespace GamePlay.World {
    using System.Collections.Generic;
    using GamePlay.World.Tilemap.Generator;
    using UnityEngine;

    namespace DigDeeper.WorldSystem {
        public class BiomeConfig
        {
            public BiomeType biomeType;
            public Color biomeColor = Color.white;
            public float temperatureModifier = 0f;
            public float humidityRange = 0.5f;
            public List<TileType> preferredSurfaceTiles = new List<TileType>();
            public List<ResourceSpawnRule> biomeSpecificResources = new List<ResourceSpawnRule>();
        
            [Header("GetDepth Limits")]
            public bool limitByDepth = false;
            public float minDepth = 0f; // Only exists at this depth or deeper
            public float maxDepth = 100f; // Only exists at this depth or shallower
            [Range(0f, 1f)] public float depthFalloff = 0.2f; // How quickly influence fades at depth limits
        }
    }

    public enum BiomeType {
        Forrest,
        Grassland,
        Swamp,
        Desert,
        Tundra,
        Snow,
    }

}