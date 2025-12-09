// #if UNITY_EDITOR
//
// using System.Collections.Generic;
// using DigDeeper.WorldSystem;
// using Editor;
// using Systems.WorldSystem.Generator;
// using UnityEngine;
//
// namespace Systems.WorldSystem {
//     [CreateAssetMenu(fileName = "Baked ECSSystem", menuName = "DigDeeper/Baked ECSSystem Config")]
//     public class BakedWorldData : ScriptableObject
//     {
//         [Header("Source Configuration")]
//         public WorldGenerationConfig _mapConfig;
//         
//         [Header("Baked ECSSystem Config")]
//         public Vector2Int worldSize;
//         public List<SerializedTile> tiles = new List<SerializedTile>();
//         
//         [Header("Metadata")]
//         public string bakingDate;
//         public float bakingTime;
//         public int totalTiles;
//         public int resourceTiles;
//         
//         public void SerializeWorld(Tile[,] ecsWorld)
//         {
//             if (ecsWorld == null) return;
//             
//             worldSize = new Vector2Int(ecsWorld.GetLength(0), ecsWorld.GetLength(1));
//             tiles.Clear();
//             
//             int resourceCount = 0;
//             var startTime = System.DateTime.Now;
//             
//             for (int x = 0; x < worldSize.x; x++)
//             {
//                 for (int y = 0; y < worldSize.y; y++)
//                 {
//                     var tile = ecsWorld[x, y];
//                     if (tile.tileType != TileType.Air || tile.resources.Count > 0)
//                     {
//                         var serializedTile = new SerializedTile
//                         {
//                             position = tile.position,
//                             tileType = tile.tileType,
//                             biome = tile.biome,
//                             hardness = tile.hardness,
//                             stability = tile.stability,
//                             temperature = tile.temperature,
//                             pressure = tile.pressure,
//                             isPointOfInterest = tile.isPointOfInterest,
//                             pointOfInterestId = tile.pointOfInterestId
//                         };
//                         
//                         foreach (var resource in tile.resources)
//                         {
//                             serializedTile.resources.Add(new SerializedResource
//                             {
//                                 type = resource.type,
//                                 abundance = resource.abundance,
//                                 quality = resource.quality,
//                                 accessibility = resource.accessibility
//                             });
//                             
//                             resourceCount++;
//                         }
//                         
//                         tiles.Add(serializedTile);
//                     }
//                 }
//             }
//             
//             // Set metadata
//             bakingDate = System.DateTime.Now.ToString();
//             bakingTime = (float)(System.DateTime.Now - startTime).TotalMilliseconds;
//             totalTiles = tiles.Count;
//             resourceTiles = resourceCount;
//         }
//         
//         public Tile[,] DeserializeWorld()
//         {
//             var ecsWorld = new Tile[worldSize.x, worldSize.y];
//             
//             // Initialize all tiles as air
//             for (int x = 0; x < worldSize.x; x++)
//             {
//                 for (int y = 0; y < worldSize.y; y++)
//                 {
//                     ecsWorld[x, y] = new Tile(new Vector2Int(x, y), TileType.Air);
//                 }
//             }
//             
//             // Apply serialized tile config
//             foreach (var serializedTile in tiles)
//             {
//                 var pos = serializedTile.position;
//                 if (pos.x >= 0 && pos.x < worldSize.x && pos.y >= 0 && pos.y < worldSize.y)
//                 {
//                     var tile = ecsWorld[pos.x, pos.y];
//                     tile.tileType = serializedTile.tileType;
//                     tile.biome = serializedTile.biome;
//                     tile.hardness = serializedTile.hardness;
//                     tile.stability = serializedTile.stability;
//                     tile.temperature = serializedTile.temperature;
//                     tile.pressure = serializedTile.pressure;
//                     tile.isPointOfInterest = serializedTile.isPointOfInterest;
//                     tile.pointOfInterestId = serializedTile.pointOfInterestId;
//                     
//                     foreach (var serializedResource in serializedTile.resources)
//                     {
//                         tile.resources.Add(new ResourceDeposit(
//                             serializedResource.type,
//                             serializedResource.abundance,
//                             serializedResource.quality,
//                             serializedResource.accessibility
//                         ));
//                     }
//                 }
//             }
//             
//             return ecsWorld;
//         }
//     }
// }
// #endif