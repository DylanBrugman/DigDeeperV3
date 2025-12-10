using Features.World.Tile;
using UnityEngine;

namespace Features.World.Grid {
    public class Chunk {
        private TileRuntime[,] tiles;

        public Chunk(Vector2Int chunkSize) {
            tiles = new TileRuntime[chunkSize.x, chunkSize.y];
        }

        public TileRuntime GetTile(Vector2Int localTileCoord) {
            return tiles[localTileCoord.x, localTileCoord.y];
        }

        public void SetTile(Vector2Int localTileCoord, TileRuntime tile) {
            tiles[localTileCoord.x, localTileCoord.y] = tile;
        }
    }
}