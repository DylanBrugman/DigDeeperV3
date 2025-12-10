using System.Collections.Generic;
using Features.World.Tile;
using UnityEngine;

namespace Features.World.Grid
{
    public sealed class TileGrid
    {
        private readonly Vector2Int chunkSize = new (16, 16);
        private readonly Dictionary<Vector2Int, Chunk> chunks = new Dictionary<Vector2Int, Chunk>();
        
        public Chunk GetOrCreateChunk(Vector2Int chunkCoord)
        {
            if (chunks.TryGetValue(chunkCoord, out Chunk existing))
            {
                return existing;
            }

            Chunk created = new Chunk(chunkSize);
            chunks.Add(chunkCoord, created);
            return created;
        }

        public TileRuntime GetTile(Vector2Int worldPosition)
        {
            Vector2Int chunkCoord = new Vector2Int(
                worldPosition.x / chunkSize.x,
                worldPosition.y / chunkSize.y
            );

            Vector2Int localTileCoord = new Vector2Int(
                worldPosition.x % chunkSize.x,
                worldPosition.y % chunkSize.y
            );

            Chunk chunk = GetOrCreateChunk(chunkCoord);
            return chunk.GetTile(localTileCoord);
        }

        public void SetTile(Vector2Int worldPosition, TileRuntime tile)
        {
            Vector2Int chunkCoord = new Vector2Int(
                worldPosition.x / chunkSize.x,
                worldPosition.y / chunkSize.y
            );

            Vector2Int localTileCoord = new Vector2Int(
                worldPosition.x % chunkSize.x,
                worldPosition.y % chunkSize.y
            );

            Chunk chunk = GetOrCreateChunk(chunkCoord);
            chunk.SetTile(localTileCoord, tile);
        }
    }
}