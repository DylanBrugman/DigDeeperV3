using GamePlay.World.Tilemap;
using GamePlay.World.Tilemap.Generator;

namespace GamePlay.Map.Generator.New.Core.WorldGen {
    public sealed class TileChunk
    {
        public const int Size = 32;                        // 32×32 tiles per chunk
        public readonly Tile[] Tiles = new Tile[Size * Size];
        public bool Dirty;
        public ref Tile At(int lx, int ly) => ref Tiles[lx + ly * Size];

        public TileChunk()
        {
            // Initialise new chunk to all‑air: safer than relying on default(Tile)
            for (int i = 0; i < Tiles.Length; i++)
                Tiles[i].Type = TileType.Air;
        }

        public void Clear() {
            for (int i = 0; i < Tiles.Length; i++) {
                Tiles[i].Clear();
            }
            Dirty = false;                   // Clear dirty flag
        }
    }
}