// using System;
// using DigDeeper.WorldSystem;
// using Editor.DigDeeper.WorldSystem.Editor;
// using Systems.WorldSystem;
// using Systems.WorldSystem.DigDeeper.WorldSystem;
// using Systems.WorldSystem.Generator;
// using UnityEditor;
// using UnityEngine;
//
// namespace Editor {
//     [CustomEditor(typeof(WorldGenerationConfig))]
//     public class WorldGenerationConfigEditor : UnityEditor.Editor
//     {
//         private bool showPreview = false;
//         private Texture2D previewTexture;
//         
//         public override void OnInspectorGUI()
//         {
//             if (GUILayout.Button("Update ecsWorld")) {
//                 var config = target as WorldGenerationConfig;
//                 if (config == null) return;
//                 config.UpdateWorld();
//             }
//             
//             DrawDefaultInspector();
//             
//             EditorGUILayout.Space();
//             
//             if (GUILayout.Button("Open in ECSSystem Editor"))
//             {
//                 var window = EditorWindow.GetWindow<WorldEditorWindow>("ECSSystem Editor");
//                 var field = typeof(WorldEditorWindow).GetField("selectedConfig", 
//                     System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
//                 field?.SetValue(window, target);
//             }
//             
//             EditorGUILayout.Space();
//             
//             showPreview = EditorGUILayout.Foldout(showPreview, "Quick Preview");
//             if (showPreview)
//             {
//                 if (GUILayout.Button("GenerateStep Small Preview"))
//                 {
//                     GenerateQuickPreview();
//                 }
//                 
//                 if (previewTexture != null)
//                 {
//                     var rect = GUILayoutUtility.GetRect(200, 100);
//                     GUI.DrawTexture(rect, previewTexture, ScaleMode.StretchToFill);
//                 }
//             }
//         }
//         
//         private void GenerateQuickPreview()
//         {
//             var config = target as WorldGenerationConfig;
//             if (config == null) return;
//             
//             // Create small preview
//             var previewConfig = Instantiate(config);
//             previewConfig.worldSize = new Vector2Int(50, 25);
//             //
//             // var generator = new InfiniteWorldGenerator(previewConfig);
//             // var ecsWorld = generator.GenerateChunk(Camera.current.WorldToScreenPoint());
//             //
//             // // Create texture
//             // CreatePreviewTexture(ecsWorld);
//             
//             DestroyImmediate(previewConfig);
//         }
//         
//         private void CreatePreviewTexture(Tile[,] world)
//         {
//             int width = world.GetLength(0);
//             int height = world.GetLength(1);
//             
//             if (previewTexture != null)
//             {
//                 DestroyImmediate(previewTexture);
//             }
//             
//             previewTexture = new Texture2D(width, height);
//             
//             for (int x = 0; x < width; x++)
//             {
//                 for (int y = 0; y < height; y++)
//                 {
//                     var tile = world[x, y];
//                     Color pixelColor = GetTileColor(tile);
//                     previewTexture.SetPixel(x, height - 1 - y, pixelColor);
//                 }
//             }
//             
//             previewTexture.Apply();
//         }
//         
//         private Color GetTileColor(Tile tile)
//         {
//             Color baseColor = tile.tileType switch
//             {
//                 TileType.Air => Color.cyan,
//                 TileType.Grass => Color.green,
//                 TileType.Dirt => new Color(0.6f, 0.4f, 0.2f),
//                 TileType.Stone => Color.gray,
//                 TileType.Granite => new Color(0.4f, 0.4f, 0.4f),
//                 TileType.Bedrock => Color.black,
//                 _ => Color.gray
//             };
//             
//             if (tile.resources.Count > 0)
//             {
//                 baseColor = Color.Lerp(baseColor, Color.yellow, 0.3f);
//             }
//             
//             return baseColor;
//         }
//     }
// }