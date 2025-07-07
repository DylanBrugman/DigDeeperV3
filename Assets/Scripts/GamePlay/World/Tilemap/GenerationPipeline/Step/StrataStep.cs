using Core.Grid;
using GamePlay.World.Tilemap.Generator;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace GamePlay.World.Tilemap.GenerationPipeline.Step {
    public sealed class StrataStep : ITilemapGenerationStep {
        
        readonly Band[] _bands;
        public StrataStep(params Band[] bands) => _bands = bands;

        public JobHandle Schedule(WorldGenerationContext context, JobHandle deps)
        {
            int width = context.SurfaceHeights.Length;
            return new StrataJob {
                Heights   = context.SurfaceHeights,
                TileGridWriter = context.TileGrid.AsParallelWriter(),
                Bands     = _bands,
                SizeX     = width
            }.Schedule(width, 64, deps);
        }

        [BurstCompile]
        struct StrataJob : IJobParallelFor
        {
            [ReadOnly]  public NativeArray<byte> Heights;
            // public TileGrid TileGrid;
            [NativeDisableParallelForRestriction] public NativeArray<Band> BandsNative;
            public ChunkedGrid<Tile>.GridWriter<Tile> TileGridWriter;

            public Band[] Bands;
            public int SizeX;

            public void Execute(int xi)
            {
                int surfaceY = Heights[xi];
                for (int b = 0; b < Bands.Length; b++)
                {
                    var band   = Bands[b];
                    float n    = noise.cnoise(new float2(xi, band.scale*100f) * band.scale);
                    int depth  = (int)math.round(math.lerp(band.min, band.max, (n+1)*.5f));
                    int upY    = surfaceY - depth;
                    for (int y = upY; y > upY - 8; y--) {
                        TileGrid.At(xi, y).Type = band.tile;
                    }
                }
            }
        }
        
        public struct Band {
            public TileType tile;
            public int min;
            public int max;
            public float scale;
        }
    }
}