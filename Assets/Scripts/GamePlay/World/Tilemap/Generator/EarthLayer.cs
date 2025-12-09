using System.Collections.Generic;
using DigDeeper.WorldSystem;
using GamePlay.World.Tilemap.Generator;
using UnityEngine;

namespace Systems.WorldSystem.Generator {
    
    [System.Serializable]
    public class EarthLayer {
        public string layerName;
        public int startDepth;
        public int endDepth;
        public TileType primaryTileType;
        public List<TileType> secondaryTileTypes = new List<TileType>();
        public float temperature;
        public float pressure;
        public float density;
        
        [Header("Layer Flow Settings")]
        [Range(0f, 1f)] public float flowIntensity = 0.3f; // How much the layer "flows"
        [Range(0.001f, 0.1f)] public float flowScale = 0.02f; // Scale of flow noise
        [Range(-20f, 20f)] public float flowOffset = 0f; // Vertical offset variation
        
        [Header("Voronoi Settings")]
        public bool useVoronoi = false;
        [Range(5, 50)] public int voronoiPoints = 15; // Number of Voronoi seed points
        [Range(0f, 1f)] public float voronoiInfluence = 0.5f; // How much Voronoi affects layer
        
        [Header("Secondary Material Flow")]
        [Range(0f, 1f)] public float secondaryMaterialFlow = 0.2f; // How flowing secondary materials are
        [Range(0.005f, 0.05f)] public float secondaryFlowScale = 0.015f;

        [Header("Resource Generation")]
        public List<ResourceSpawnRule> resourceRules = new List<ResourceSpawnRule>();
        
    }
}