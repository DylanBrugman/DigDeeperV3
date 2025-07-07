// using DigDeeper.WorldSystem;
// using GamePlay.Map.Generator;
// using GamePlay.World;
// using GamePlay.World.Tilemap.Generator;
// using GamePlay.World.Tilemap.Generator.TileProcessor;
// using Systems.WorldSystem.Generator;
// using Systems.WorldSystem.Generator.TileProcessor;
// using UnityEngine;
//
// namespace Systems.WorldSystem {
//     public class BaseTerrainTilemapTileProcessor : ITilemapTileProcessor
//     {
//         public string StepName => "Generating Base Terrain (Tile)";
//
//         public void Initialize(TilemapGeneratorContext context) {
//             
//         }
//
//         public void Process(Tile tile, Tile[,] currentWorldState, TilemapGeneratorContext context)
//         {
//             int surfaceHeight = context.SurfaceHeights[tile.position.x]; // Use pre-calculated surface height
//
//             TileType tileType;
//             if (tile.position.y > surfaceHeight) tileType = TileType.Air;
//             else if (tile.position.y == surfaceHeight) tileType = TileType.Grass;
//             else if (tile.position.y > surfaceHeight - 5) tileType = TileType.Dirt;
//             else tileType = TileType.Stone;
//
//             tile.tileType = tileType;
//             // tile.GetDepth = Mathf.MaxVelocity(0, surfaceHeight - y);
//             
//             // Set initial tile color based on type
//             switch (tileType)
//             {
//                 case TileType.Air: tile.TileColor = Color.cyan; break;
//                 case TileType.Grass: tile.TileColor = Color.green; break;
//                 case TileType.Dirt: tile.TileColor = new Color(0.55f, 0.27f, 0.07f); break; // Brown
//                 case TileType.Stone: tile.TileColor = Color.gray; break;
//                 default: tile.TileColor = Color.magenta; break; // Default for unhandled
//             }
//         }
//     }
// }