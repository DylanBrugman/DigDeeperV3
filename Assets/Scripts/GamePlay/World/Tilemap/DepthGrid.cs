using Core.Grid;
using Unity.Mathematics;

namespace GamePlay.World.Tilemap {
    public sealed class DepthGrid : ChunkedGrid<int> {
        public DepthGrid(int2 sizeChunks) : base(sizeChunks) { }
    }
}