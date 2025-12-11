// using Systems.WorldSystem.Generator;
// using UnityEditor;
// using UnityEngine;
//
// namespace Systems.WorldSystem {
//     public class EditorTileComponent : MonoBehaviour
//     {
//         [SerializeField] private Tile tileData;
//         
//         public Tile TileData => tileData;
//         
//         public void Initialize(Tile tile)
//         {
//             tileData = tile;
//             name = $"Tile_{tile.position.x}_{tile.position.y} ({tile.tileType})";
//         }
//         
//         // Show tile info when selected
//         private void OnDrawGizmosSelected()
//         {
//             if (tileData == null) return;
//             
//             // Draw tile info
//             var style = new GUIStyle();
//             style.normal.textColor = Color.white;
//             
// #if UNITY_EDITOR
//             Handles.Label(transform.position + Vector3.up * 0.5f,
//                 $"{tileData.tileType}\nResources: {tileData.resources.Count}\nStability: {tileData.stability:F1}",
//                 style);
// #endif
//         }
//     }
// }