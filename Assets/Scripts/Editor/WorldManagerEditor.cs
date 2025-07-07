// using Systems.WorldSystem;
// using UnityEditor;
// using UnityEngine;
//
// namespace Editor {
//     
//     [CustomEditor(typeof(WorldManager))]
//     public class WorldManagerEditor : UnityEditor.Editor {
//         
//         public override void OnInspectorGUI() {
//             base.OnInspectorGUI();
//
//             WorldManager worldManager = (WorldManager)target;
//
//             if (GUILayout.Button("GenerateStep ECSSystem")) {
//                 worldManager.GenerateWorld();
//                 EditorUtility.SetDirty(worldManager);
//             }
//         }
//     }
// }