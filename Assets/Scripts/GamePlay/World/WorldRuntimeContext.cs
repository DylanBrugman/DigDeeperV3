using System;
using GamePlay.Map.Generator.New;
using GamePlay.Map.Generator.New.Core.WorldGen;
using GamePlay.Pathfinding.Nav;
using GamePlay.World.Tilemap;
using Unity.Collections;
using Unity.Mathematics;

namespace GamePlay.World {

    public sealed class WorldRuntimeContext : IDisposable {
        public readonly int2 SizeChunks;
        public readonly TileGrid TileGrid;
        public readonly NavGrid  NavGrid;

        public WorldRuntimeContext(WorldData data)
        {
            SizeChunks = data.SizeChunks;

            TileGrid = new TileGrid(SizeChunks);
            NavGrid  = new NavGrid(SizeChunks); 
        }

        public void Dispose()
        {
            TileGrid.Dispose();
            NavGrid.Dispose();
        }
    }

}