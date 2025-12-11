// using Systems.WorldSystem.Generator;
// using UnityEngine;
//
// namespace Systems.WorldSystem {
//     public class TileClicker : MonoBehaviour {
//
//         private TileManager _tileManager;
//         
//         public void OnMouseDown() {
//             // Get the position of the mouse click in ecsWorld coordinates
//             Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
//             worldPos.z = 0; // Set z to 0 for 2D
//
//             // Log or handle the tile click
//             Debug.Log($"Tile clicked at position: {worldPos}");
//             // _tileManager.TryGetTileAtWorldPosition(worldPos, out Tile tile);
//         }
//     }
// }