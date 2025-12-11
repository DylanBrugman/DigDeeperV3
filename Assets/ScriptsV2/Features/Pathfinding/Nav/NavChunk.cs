using GamePlay.Map.Generator.New.Core.WorldGen;

namespace GamePlay.Pathfinding.Nav {
    public sealed class NavChunk
    {
        public const int Size = TileChunk.Size;
        public readonly byte[] Flags = new byte[Size * Size];
        public bool Dirty;
        public ref byte At(int lx,int ly) => ref Flags[lx + ly * Size];
    }
}