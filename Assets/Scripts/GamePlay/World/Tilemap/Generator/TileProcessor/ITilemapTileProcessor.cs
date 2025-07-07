namespace GamePlay.World.Tilemap.Generator.TileProcessor {
    public interface ITilemapTileProcessor
    {
        string StepName { get; }
        // void Initialize(TilemapGeneratorContext context);
        void Process(ref Tile tile, WorldGenerationContext worldGenerationContext);
    }
}