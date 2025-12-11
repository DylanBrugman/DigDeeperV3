// using System;
// using System.Collections.Generic;
// using System.Linq;
// using Core;
// using DigDeeper.WorldSystem;
// using Systems.SaveSystem;
// using Systems.WorldSystem.Generator;
// using Unity.Collections;
// using UnityEngine;
// using UnityEngine.Assertions;
// using UnityEngine.Tilemaps;
// using Tile = Systems.WorldSystem.Generator.Tile;
//
// namespace Systems.WorldSystem {
//     public class TileManager : MonoBehaviour, ISystemManager {
//         [Header("Tile System Configuration")] [SerializeField]
//         private TilemapWorldViewer tilemapViewer;
//
//         // ECSSystem config
//         [ReadOnly] private WorldGenerationConfig _mapConfig;
//         private Tile[,] worldGrid;
//         private Dictionary<Vector2Int, GameObject> tileObjects = new Dictionary<Vector2Int, GameObject>();
//
//         // Chunk system for performance
//         // private Dictionary<Vector2Int, Chunk> loadedChunks = new Dictionary<Vector2Int, Chunk>();
//         private const int CHUNK_SIZE = 32;
//
//         // Properties
//         public bool IsInitialized { get; private set; }
//         public Vector2Int WorldSizeChunks => _mapConfig?.worldSize ?? Vector2Int.zero;
//         public Tile[,] WorldGrid => worldGrid;
//
//         // Events
//         public static event Action<Tile> OnTileChanged;
//         public static event Action<Vector2Int> OnTileDestroyed;
//         public static event Action OnTilemapWorldGenerated;
//
//         public void Initialize() {
//             if (IsInitialized) return;
//
//             IsInitialized = true;
//         }
//
//         public void UpdateSystem() {
//             // Update chunks based on camera position if needed
//         }
//
//         public void CleanupSystem() {
//             DestroyAllTiles();
//         }
//
//         public Tile[,] GenerateTilemap(WorldGenerationConfig config, bool forceGenerateNewTilemapWorld) {
//             Assert.IsNotNull(config, "ECSSystem generation config cannot be null.");
//
//             if (forceGenerateNewTilemapWorld || worldGrid == null) {
//                 worldGrid = GenerateNew(config);
//             }
//
//             tilemapViewer.Show(worldGrid);
//
//             OnTilemapWorldGenerated?.Invoke();
//             return worldGrid;
//         }
//
//         private Tile[,] GenerateNew(WorldGenerationConfig config) {
//             Debug.Log("Generating new ecsWorld...");
//
//             _mapConfig = config;
//             DestroyAllTiles();
//
//             var generator = new TilemapWorldGenerator(_mapConfig);
//             worldGrid = generator.GenerateNewTilemapWorld();
//             return worldGrid;
//         }
//
//         public Tile GetTile(Vector2Int position) {
//             if (IsValidPosition(position)) {
//                 return worldGrid[position.x, position.y];
//             }
//
//             return null;
//         }
//
//         public Tile GetTile(int x, int y) {
//             return GetTile(new Vector2Int(x, y));
//         }
//
//         public List<Tile> GetTilesInRadius(Vector2Int center, float radius) {
//             var tiles = new List<Tile>();
//             int radiusInt = Mathf.CeilToInt(radius);
//
//             for (int x = center.x - radiusInt; x <= center.x + radiusInt; x++) {
//                 for (int y = center.y - radiusInt; y <= center.y + radiusInt; y++) {
//                     var position = new Vector2Int(x, y);
//                     if (Vector2.Distance(center, position) <= radius) {
//                         var tile = GetTile(position);
//                         if (tile != null) {
//                             tiles.Add(tile);
//                         }
//                     }
//                 }
//             }
//
//             return tiles;
//         }
//
//         public bool CanDigTile(Vector2Int position) {
//             var tile = GetTile(position);
//             if (tile == null || tile.tileType == TileType.Air) return false;
//
//             // Check structural integrity
//             if (tile.stability < 20f) return false;
//
//             // Check if tile is accessible
//             if (!tile.isAccessible) return false;
//
//             return true;
//         }
//
//         public bool DigTile(Vector2Int position, float digPower = 100f) {
//             var tile = GetTile(position);
//             if (tile == null || !CanDigTile(position)) return false;
//
//             // Check if dig power is sufficient
//             if (digPower < tile.hardness) return false;
//
//             // Remove tile
//             tile.tileType = TileType.Air;
//             tile.stability = 0f;
//
//             // Destroy visual tile
//             if (tileObjects.ContainsKey(position)) {
//                 Destroy(tileObjects[position]);
//                 tileObjects.Remove(position);
//             }
//
//             // Update surrounding tiles' stability
//             UpdateSurroundingStability(position);
//
//             OnTileDestroyed?.Invoke(position);
//             OnTileChanged?.Invoke(tile);
//
//             return true;
//         }
//
//         private void UpdateSurroundingStability(Vector2Int center) {
//             for (int dx = -2; dx <= 2; dx++) {
//                 for (int dy = -2; dy <= 2; dy++) {
//                     var position = center + new Vector2Int(dx, dy);
//                     var tile = GetTile(position);
//                     if (tile != null && tile.tileType != TileType.Air) {
//                         tile.stability = CalculateStability(position);
//
//                         // Check for potential collapse
//                         if (tile.stability < 10f) {
//                             // Schedule tile for potential collapse
//                             StartCoroutine(CheckTileCollapse(position, 1f));
//                         }
//                     }
//                 }
//             }
//         }
//
//         private System.Collections.IEnumerator CheckTileCollapse(Vector2Int position, float delay) {
//             yield return new WaitForSeconds(delay);
//
//             var tile = GetTile(position);
//             if (tile != null && tile.stability < 5f && tile.tileType != TileType.Air) {
//                 Debug.Log($"Tile collapsed at {position}");
//                 DigTile(position, 1000f); // Force dig
//             }
//         }
//
//         private float CalculateStability(Vector2Int position) {
//             var tile = GetTile(position);
//             if (tile == null || tile.tileType == TileType.Air) return 0f;
//
//             float stability = 100f;
//             int supportCount = 0;
//
//             // Check surrounding tiles for support
//             for (int dx = -1; dx <= 1; dx++) {
//                 for (int dy = -1; dy <= 1; dy++) {
//                     if (dx == 0 && dy == 0) continue;
//
//                     var checkPos = position + new Vector2Int(dx, dy);
//                     var neighborTile = GetTile(checkPos);
//                     if (neighborTile != null && neighborTile.tileType != TileType.Air) {
//                         supportCount++;
//                     }
//                 }
//             }
//
//             // Reduce stability if not enough support
//             stability *= (supportCount / 8f);
//
//             return Mathf.Clamp(stability, 0f, 100f);
//         }
//
//         private float CalculateHardnessForType(TileType tileType) {
//             return tileType switch {
//                 TileType.Air => 0f, TileType.Grass => 5f, TileType.Dirt => 10f, TileType.Sand => 8f, TileType.Clay => 15f, TileType.Stone => 50f, TileType.Limestone => 40f, TileType.Granite => 70f, TileType.Marble => 60f, TileType.Bedrock => 95f, TileType.Lava => 100f, _ => 30f
//             };
//         }
//
//         public List<ResourceDeposit> ExtractResources(Vector2Int position, float extractionPower = 100f) {
//             var tile = GetTile(position);
//             if (tile == null || tile.resources.Count == 0) return new List<ResourceDeposit>();
//
//             var extractedResources = new List<ResourceDeposit>();
//
//             for (int i = tile.resources.Count - 1; i >= 0; i--) {
//                 var resource = tile.resources[i];
//
//                 // Calculate extraction efficiency
//                 float efficiency = Mathf.Clamp01(extractionPower / 100f) * (resource.accessibility / 100f);
//                 float extractedAmount = resource.abundance * efficiency;
//
//                 if (extractedAmount > 0f) {
//                     extractedResources.Add(new ResourceDeposit(
//                         resource.type,
//                         extractedAmount,
//                         resource.quality,
//                         resource.accessibility
//                     ));
//
//                     // Reduce resource in tile
//                     resource.abundance -= extractedAmount;
//
//                     // Remove depleted resources
//                     if (resource.abundance <= 0f) {
//                         tile.resources.RemoveAt(i);
//                     }
//                 }
//             }
//
//             if (extractedResources.Count > 0) {
//                 OnTileChanged?.Invoke(tile);
//             }
//
//             return extractedResources;
//         }
//
//         public bool IsValidPosition(Vector2Int position) {
//             return position.x >= 0 && position.x < WorldSizeChunks.x &&
//                    position.y >= 0 && position.y < WorldSizeChunks.y;
//         }
//
//         private void DestroyAllTiles() {
//             foreach (var tileObject in tileObjects.Values) {
//                 if (tileObject != null) {
//                     Destroy(tileObject);
//                 }
//             }
//
//             tileObjects.Clear();
//             // loadedChunks.Clear();
//         }
//
//         public Dictionary<string, object> GetSaveData() {
//             var tileData = new List<Dictionary<string, object>>();
//
//             if (worldGrid != null) {
//                 for (int x = 0; x < worldGrid.GetLength(0); x++) {
//                     for (int y = 0; y < worldGrid.GetLength(1); y++) {
//                         var tile = worldGrid[x, y];
//                         if (tile.tileType != TileType.Air || tile.resources.Count > 0) {
//                             tileData.Add(SerializeTile(tile));
//                         }
//                     }
//                 }
//             }
//
//             return new Dictionary<string, object> {
//                 ["worldSize"] = WorldSizeChunks, ["tiles"] = tileData, ["configName"] = _mapConfig.name
//             };
//         }
//
//         private Dictionary<string, object> SerializeTile(Tile tile) {
//             var resourceData = tile.resources.Select(r => new Dictionary<string, object> {
//                 ["type"] = r.type.ToString(), ["abundance"] = r.abundance, ["quality"] = r.quality, ["accessibility"] = r.accessibility
//             }).ToList();
//
//             return new Dictionary<string, object> {
//                 ["position"] = tile.position, ["tileType"] = tile.tileType.ToString(), ["biome"] = tile.biome.ToString(), ["hardness"] = tile.hardness, ["stability"] = tile.stability, ["temperature"] = tile.temperature, ["pressure"] = tile.pressure, ["isExplored"] = tile.isExplored, ["isAccessible"] = tile.isAccessible, ["resources"] = resourceData, ["isPointOfInterest"] = tile.isPointOfInterest, ["pointOfInterestId"] = tile.pointOfInterestId
//             };
//         }
//         public bool AreTilesOccupied(Vector2Int position, int configHeight) {
//             List<Tile> tiles = new List<Tile>();
//             tiles.Add(GetTile(position));
//             tiles.Add(GetTile(position.x, position.y + configHeight));
//             
//             return tiles.Count == configHeight && tiles.Any(tile => tile is {IsOccupied: false});
//         }
//
//         // public bool TryGetTileAtWorldPosition(Vector3 worldPos, out Tile tile) {
//         //     tileObjects.
//         //     
//         //     return tilemapViewer.Tilemap.WorldToCell(worldPos);
//         // }
//     }
// }