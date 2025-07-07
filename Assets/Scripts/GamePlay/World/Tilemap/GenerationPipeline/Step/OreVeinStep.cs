using System;
using GamePlay.World.Tilemap.Generator;
using Unity.Jobs;

namespace GamePlay.World.Tilemap.GenerationPipeline.Step {
    
    public sealed class OreVeinStep : ITilemapGenerationStep {
        public JobHandle Schedule(WorldGenerationContext context, JobHandle dependsOn) {
            dependsOn.Complete();                         // read NativeTileGrid safely
            Random random = context.Random;

            int width  = context.SizeChunks.x * TileChunk.Size;
            int height = context.SizeChunks.y * TileChunk.Size;

            for (int i = 0; i < 200; i++)        // 200 veins
            {
                int x = random.Next(width);
                int y = random.Next(height/2);   // below surface
                CarveVein(context.Tiles, x, y, random);
            }
            return default;
        }

        void CarveVein(TileGrid grid, int sx, int sy, Random random)
        {
            for (int i = 0; i < 30; i++)        // vein length
            {
                int x = sx + random.Next(-2, 3);
                int y = sy + random.Next(-2, 3);
                ref var t = ref grid.At(x, y);
                if (t.Type == TileType.Stone) t.Type = TileType.OreCopper;
            }
        }
    }
}