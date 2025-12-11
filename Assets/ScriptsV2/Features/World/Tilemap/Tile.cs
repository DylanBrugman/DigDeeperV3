using GamePlay.World.Tilemap.Generator;

namespace GamePlay.World.Tilemap {
    public struct Tile
    {
        public TileType Type;     // 1 byte
        public byte HitPoints;    // digging progress / durability
        public byte Light;        // simple lighting
        public byte Flags;        // bit‑field (HasPlant, Ladder, Rope, etc.)
        public byte CurrentDepth; // current depth in the world, used for world generation

        public void Clear() {
            Type = TileType.Air;    // reset to air
            HitPoints = 0;          // no damage
            Light = 0;              // no light
            Flags = 0;              // no flags set
        }
    }
}