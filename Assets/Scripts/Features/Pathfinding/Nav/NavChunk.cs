using GamePlay.World.Tilemap;

namespace GamePlay.Pathfinding.Nav {
    public sealed class NavChunk
    {
        public static readonly int Size = Chunk.SIZE;
        public readonly byte[] Flags = new byte[Size * Size];
        public bool Dirty;
        public ref byte At(int lx,int ly) => ref Flags[lx + ly * Size];
    }
}