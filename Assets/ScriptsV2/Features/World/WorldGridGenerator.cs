using Features.World.Grid;
using Features.World.Tile;
using UnityEngine;

namespace Features.World {
    internal class WorldGridGenerator {
        public TileGrid GenerateNewWorld(WorldConfig worldConfig) {
            TileGrid tileGrid = new TileGrid();
            for (int x = 0; x < worldConfig.worldSize.x; x++) {
                for (int y = 0; y < worldConfig.worldSize.y; y++) {
                    TileRuntime tile = new TileRuntime();
                    tile.Position = new Vector2Int(x, y);
                    tile.TileType = TileType.Dirt;
                    
                    tileGrid.SetTile(tile.Position, tile);
                }
            }
            return tileGrid;
        }
    }
}