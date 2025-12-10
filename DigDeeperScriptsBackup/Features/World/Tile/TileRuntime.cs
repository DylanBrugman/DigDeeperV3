using UnityEngine;

namespace Features.World.Tile {
    public class TileRuntime {
        private Vector2Int position;
        private TileType tileType;
        // private string tileConfigId;
        // private TileType tileType;
        // private float temperature;
        // private float pressure;
        // private float structuralIntegrity;
        // private List<Resource> resources;


        public Vector2Int Position {
            get => position;
            set => position = value;
        }

        public TileType TileType {
            get => tileType;
            set => tileType = value;
        }
    }
}