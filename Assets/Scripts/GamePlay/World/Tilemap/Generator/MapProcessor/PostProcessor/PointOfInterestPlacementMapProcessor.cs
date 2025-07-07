// using System.Collections.Generic;
// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator;
// using GamePlay.World;
// using Systems.WorldSystem.Generator;
// using Systems.WorldSystem.Generator.MapProcessor;
// using UnityEngine;
//
// namespace Systems.WorldSystem {
//     public class PointOfInterestPlacementMapProcessor : IMapProcessor
//     {
//         public string StepName => "Placing Points of Interest (Map Pass)";
//
//         public void Process(Tile[,] world, TilemapGeneratorContext context)
//         {
//             var config = context.Config;
//             if (config.pointsOfInterest == null || config.pointsOfInterest.Count == 0) return;
//
//             var placedPOIs = new List<Vector2Int>();
//
//             foreach (var poiConfig in config.pointsOfInterest)
//             {
//                 int attempts = 0;
//                 int maxAttempts = 100; 
//
//                 while (attempts < maxAttempts)
//                 {
//                     Vector2Int position = GenerateRandomPOIPosition(poiConfig, context, world);
//                     if (CanPlacePOI(world, position, poiConfig, context, placedPOIs))
//                     {
//                         PlacePOI(world, position, poiConfig, context);
//                         placedPOIs.Add(position);
//                         break;
//                     }
//                     attempts++;
//                 }
//             }
//         }
//
//         private Vector2Int GenerateRandomPOIPosition(PointOfInterestConfig poiConfig, TilemapGeneratorContext context, Tile[,] world)
//         {
//             // Ensure POI fits within ecsWorld boundaries if size is a factor from edge
//             int borderX = poiConfig.size.x / 2; 
//             int borderY = poiConfig.size.y / 2;
//
//             int x = context.Random.Next(borderX, context.Config.worldSize.x - borderX);
//             int y;
//
//             // Use the context's FindSurfaceLevel which inspects the current state of 'ecsWorld'
//             int currentSurfaceY = context.FindSurfaceLevel(x, world);
//
//
//             if (poiConfig.spawnOnSurface)
//             {
//                 // Adjust based on POI size, e.g., bottom of POI at surface
//                 y = currentSurfaceY - poiConfig.size.y +1; 
//             }
//             else
//             {
//                 int minDepth = Mathf.RoundToInt(poiConfig.depthRange.x);
//                 int maxDepth = Mathf.RoundToInt(poiConfig.depthRange.y);
//                 
//                 // Convert depth to y-coordinates (0 is bottom, worldSize.y-1 is top)
//                 // currentSurfaceY is the y-coord of the surface. GetDepth increases as y decreases.
//                 int minYPosFromSurface = currentSurfaceY - maxDepth; // MaxVelocity depth means lower Y
//                 int maxYPosFromSurface = currentSurfaceY - minDepth; // Min depth means higher Y
//
//                 // Clamp to ensure y is within valid depth range relative to current surface
//                 int targetYMin = Mathf.Max(borderY, minYPosFromSurface);
//                 int targetYMax = Mathf.Min(context.Config.worldSize.y - 1 - borderY, maxYPosFromSurface);
//                 
//                 if (targetYMax < targetYMin) targetYMax = targetYMin; // Ensure valid range
//                 if (targetYMin >= targetYMax) y = targetYMin; // if range is zero or negative
//                 else y = context.Random.Next(targetYMin, targetYMax + 1);
//             }
//             return new Vector2Int(x - poiConfig.size.x / 2, Mathf.Clamp(y, 0, context.Config.worldSize.y - poiConfig.size.y));
//         }
//
//         private bool CanPlacePOI(Tile[,] world, Vector2Int position, PointOfInterestConfig poiConfig, TilemapGeneratorContext context, List<Vector2Int> existingPOIs)
//         {
//             // Check min distance from other POIs
//             foreach (var existingPOIcenter in existingPOIs) // Assuming existingPOIs stores centers
//             {
//                 // Calculate center of current POI proposal for distance check
//                 Vector2 currentPOICenter = new Vector2(position.x + poiConfig.size.x / 2f, position.y + poiConfig.size.y / 2f);
//                 if (Vector2.Distance(currentPOICenter, existingPOIcenter) < poiConfig.minDistanceFromOthers)
//                 {
//                     return false;
//                 }
//             }
//
//             // Check if area is valid and not overlapping existing POI markers
//             for (int dx = 0; dx < poiConfig.size.x; dx++)
//             {
//                 for (int dy = 0; dy < poiConfig.size.y; dy++)
//                 {
//                     var checkPos = position + new Vector2Int(dx, dy);
//                     if (!context.IsValidPosition(checkPos) || world[checkPos.x, checkPos.y].isPointOfInterest)
//                     {
//                         return false;
//                     }
//                     // Optional: AddComponent check for requiresSolidGround or other conditions
//                     // if (poiConfig.requiresSolidGround && ecsWorld[checkPos.x, checkPos.y].tileType == TileType.Air) return false;
//                 }
//             }
//             return true;
//         }
//
//         private void PlacePOI(Tile[,] world, Vector2Int position, PointOfInterestConfig poiConfig, TilemapGeneratorContext context)
//         {
//             for (int dx = 0; dx < poiConfig.size.x; dx++)
//             {
//                 for (int dy = 0; dy < poiConfig.size.y; dy++)
//                 {
//                     var tilePos = position + new Vector2Int(dx, dy);
//                     if (context.IsValidPosition(tilePos))
//                     {
//                         var tile = world[tilePos.x, tilePos.y];
//                         tile.isPointOfInterest = true;
//                         tile.pointOfInterestId = poiConfig.id;
//                         
//                         // Example: POI structure blocks could override existing tiles
//                         // tile.tileType = TileType.Stone; 
//                         // tile.TileColor = Color.yellow; // Highlight POI areas
//
//                         if (poiConfig.guaranteedResources != null)
//                         {
//                             foreach (var resource in poiConfig.guaranteedResources)
//                             {
//                                 tile.AddResource(new ResourceDeposit(resource.type, resource.abundance, resource.quality, resource.accessibility));
//                             }
//                         }
//                     }
//                 }
//             }
//         }
//     }
// }