using DigDeeper.WorldSystem;
using GamePlay.World.Tilemap.Generator;
using Systems.WorldSystem.Generator;
using UnityEngine;

namespace Systems.WorldSystem {
    [System.Serializable]
    public class VoronoiPoint {
        public Vector2 position;
        public TileType tileType;
        public float influence;
        public EarthLayer sourceLayer;

        public VoronoiPoint(Vector2 pos, TileType type, float inf, EarthLayer layer) {
            position = pos;
            tileType = type;
            influence = inf;
            sourceLayer = layer;
        }
    }
}