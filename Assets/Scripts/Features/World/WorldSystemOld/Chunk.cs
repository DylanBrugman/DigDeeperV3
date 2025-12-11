// using System.Collections.Generic;
// using Systems.WorldSystem;
// using Systems.WorldSystem.Generator;
// using UnityEngine;
//
// namespace DigDeeper.WorldSystem {
//     public class Chunk
//     {
//         public Vector2Int chunkPosition;
//         public Vector2Int worldStartPosition;
//         public Tile[,] tiles;
//         public List<GameObject> tileObjects = new List<GameObject>();
//         public bool isLoaded = false;
//         
//         public Chunk(Vector2Int chunkPos, int chunkSize)
//         {
//             chunkPosition = chunkPos;
//             worldStartPosition = chunkPos * chunkSize;
//             tiles = new Tile[chunkSize, chunkSize];
//         }
//         
//         public void Load()
//         {
//             isLoaded = true;
//             // Create visual tiles for this chunk
//         }
//         
//         public void Unload()
//         {
//             isLoaded = false;
//             // Destroy visual tiles for this chunk
//             foreach (var tileObject in tileObjects)
//             {
//                 if (tileObject != null)
//                 {
//                     Object.Destroy(tileObject);
//                 }
//             }
//             tileObjects.Clear();
//         }
//     }
// }