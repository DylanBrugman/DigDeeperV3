// #if UNITY_EDITOR
// #if UNITY_EDITOR
// using DigDeeper.WorldSystem;
// using DigDeeper.WorldSystem.Editor;
// using UnityEditor;
// using UnityEngine;
//
// namespace Systems.WorldSystem.DigDeeper.WorldSystem.Editor {
//     // [CustomEditor(typeof(TilemapWorldViewer))]
//     // public class TilemapWorldViewerEditor : UnityEditor.Editor
//     // {
//     //     private TilemapWorldViewer viewer;
//     //     
//     //     private void OnEnable()
//     //     {
//     //         viewer = target as TilemapWorldViewer;
//     //     }
//     //     
//     //     public override void OnInspectorGUI()
//     //     {
//     //         // Draw config section
//     //         EditorGUILayout.LabelField("ECSSystem Configuration", EditorStyles.boldLabel);
//     //         viewer._mapConfig = (WorldGenerationConfig)EditorGUILayout.ObjectField("ECSSystem Config", viewer._mapConfig, typeof(WorldGenerationConfig), false);
//     //         
//     //         EditorGUILayout.Space();
//     //         
//     //         // Draw generation controls
//     //         EditorGUILayout.LabelField("Generation Controls", EditorStyles.boldLabel);
//     //         EditorGUILayout.BeginHorizontal();
//     //         if (GUILayout.Button("GenerateStep ECSSystem", GUILayout.Height(30)))
//     //         {
//     //             viewer.GenerateWorldInEditor();
//     //         }
//     //         if (GUILayout.Button("Clear ECSSystem"))
//     //         {
//     //             viewer.ClearWorld();
//     //         }
//     //         EditorGUILayout.EndHorizontal();
//     //         
//     //         if (viewer.HasGeneratedWorld)
//     //         {
//     //             EditorGUILayout.Space();
//     //             EditorGUILayout.LabelField("ECSSystem Statistics", EditorStyles.boldLabel);
//     //             EditorGUILayout.LabelField($"ChunkSize: {viewer.WorldSizeChunks.x} x {viewer.WorldSizeChunks.y}");
//     //             
//     //             if (viewer.enableChunking)
//     //             {
//     //                 int chunksX = Mathf.CeilToInt((float)viewer.WorldSizeChunks.x / viewer.chunkSize);
//     //                 int chunksY = Mathf.CeilToInt((float)viewer.WorldSizeChunks.y / viewer.chunkSize);
//     //                 EditorGUILayout.LabelField($"Chunks: {chunksX} x {chunksY} = {chunksX * chunksY}");
//     //             }
//     //         }
//     //         
//     //         EditorGUILayout.Space();
//     //         
//     //         // Draw visualization controls
//     //         EditorGUILayout.LabelField("Visualization", EditorStyles.boldLabel);
//     //         
//     //         EditorGUI.BeginChangeCheck();
//     //         viewer.viewMode = (ViewMode)EditorGUILayout.EnumPopup("View Mode", viewer.viewMode);
//     //         viewer.showResourceOverlay = EditorGUILayout.Toggle("Resource Overlay", viewer.showResourceOverlay);
//     //         viewer.showPOIHighlight = EditorGUILayout.Toggle("POI Highlight", viewer.showPOIHighlight);
//     //         
//     //         if (EditorGUI.EndChangeCheck() && viewer.HasGeneratedWorld)
//     //         {
//     //             viewer.RefreshVisualization();
//     //         }
//     //         
//     //         EditorGUILayout.Space();
//     //         
//     //         // Draw performance settings
//     //         EditorGUILayout.LabelField("Performance", EditorStyles.boldLabel);
//     //         viewer.enableChunking = EditorGUILayout.Toggle("Enable Chunking", viewer.enableChunking);
//     //         if (viewer.enableChunking)
//     //         {
//     //             viewer.chunkSize = EditorGUILayout.IntSlider("Chunk ChunkSize", viewer.chunkSize, 32, 128);
//     //         }
//     //         viewer.limitRenderDistance = EditorGUILayout.Toggle("Limit Render Distance", viewer.limitRenderDistance);
//     //         if (viewer.limitRenderDistance)
//     //         {
//     //             viewer.maxRenderDistance = EditorGUILayout.IntSlider("MaxVelocity Render Distance", viewer.maxRenderDistance, 50, 200);
//     //         }
//     //     }
//     // }
//
//     [CustomEditor(typeof(TilemapWorldViewer))]
//     public class TilemapWorldViewerEditor : UnityEditor.Editor {
//         private TilemapWorldViewer viewer;
//
//         private void OnEnable() {
//             viewer = target as TilemapWorldViewer;
//         }
//
//         public override void OnInspectorGUI() {
//             DrawDefaultInspector();
//
//             EditorGUILayout.Space();
//
//             // Generation controls
//             EditorGUILayout.LabelField("ECSSystem Generation", EditorStyles.boldLabel);
//
//             EditorGUILayout.BeginHorizontal();
//             if (GUILayout.Button("GenerateStep ECSSystem", GUILayout.Height(30))) {
//                 viewer.GenerateWorldInEditor();
//             }
//
//             if (GUILayout.Button("Clear ECSSystem")) {
//                 viewer.ClearWorld();
//             }
//
//             EditorGUILayout.EndHorizontal();
//
//             if (viewer.HasGeneratedWorld) {
//                 EditorGUILayout.Space();
//                 EditorGUILayout.LabelField("ECSSystem Statistics", EditorStyles.boldLabel);
//                 EditorGUILayout.LabelField($"ChunkSize: {viewer.WorldSizeChunks.x} x {viewer.WorldSizeChunks.y}");
//
//                 if (viewer.enableChunking) {
//                     int chunksX = Mathf.CeilToInt((float) viewer.WorldSizeChunks.x / viewer.chunkSize);
//                     int chunksY = Mathf.CeilToInt((float) viewer.WorldSizeChunks.y / viewer.chunkSize);
//                     EditorGUILayout.LabelField($"Chunks: {chunksX} x {chunksY} = {chunksX * chunksY}");
//                 }
//             }
//
//             EditorGUILayout.Space();
//
//             // Debug controls
//             EditorGUILayout.LabelField("Debug Tools", EditorStyles.boldLabel);
//
//             if (GUILayout.Button("Refresh Visualization")) {
//                 viewer.RefreshVisualization();
//             }
//
//             // Quick sprite folder setup
//             EditorGUILayout.Space();
//             EditorGUILayout.LabelField("Quick Setup", EditorStyles.boldLabel);
//
//             if (GUILayout.Button("Create Sprite Folders")) {
//                 CreateSpriteFolders();
//             }
//
//             if (GUILayout.Button("Create Sample Sprites")) {
//                 CreateSampleSprites();
//             }
//
//             EditorGUILayout.Space();
//             EditorGUILayout.LabelField("Auto-Update", EditorStyles.boldLabel);
//
//             viewer.enableAutoUpdate = EditorGUILayout.Toggle("Enable Auto Update", viewer.enableAutoUpdate);
//             if (viewer.enableAutoUpdate) {
//                 viewer.updateDelay = EditorGUILayout.Slider("Update Delay", viewer.updateDelay, 0.1f, 3f);
//                 EditorGUILayout.HelpBox("ECSSystem will automatically regenerate when config changes.", MessageType.Info);
//             }
//
//             EditorGUILayout.Space();
//             EditorGUILayout.LabelField("Enhanced Debugging", EditorStyles.boldLabel);
//
//             if (GUILayout.Button("Debug All Sprite Sources")) {
//                 viewer.DebugAllSpriteSources();
//             }
//
//             if (GUILayout.Button("GenerateStep ECSSystem")) {
//                 viewer.GenerateWorldInEditor();
//             }
//
//             // Show sprite loading status
//             if (viewer.tilemapTexture != null) {
//                 EditorGUILayout.HelpBox($"Using tilemap texture: {viewer.tilemapTexture.name}", MessageType.Info);
//             }
//             else {
//                 EditorGUILayout.HelpBox("No tilemap texture assigned. Will use fallback sprite loading.", MessageType.Warning);
//             }
//         }
//
//         private void CreateSpriteFolders() {
//             string[] folders = {
//                 "Assets/Resources", "Assets/Resources/Sprites", "Assets/Resources/Sprites/Tiles", "Assets/Resources/Tiles"
//             };
//
//             foreach (string folder in folders) {
//                 if (!AssetDatabase.IsValidFolder(folder)) {
//                     string parentFolder = System.IO.Path.GetDirectoryName(folder).Replace('\\', '/');
//                     string folderName = System.IO.Path.GetFileName(folder);
//                     AssetDatabase.CreateFolder(parentFolder, folderName);
//                 }
//             }
//
//             AssetDatabase.Refresh();
//             Debug.Log("Created sprite folder structure");
//         }
//
//         private void CreateSampleSprites() {
//             CreateSpriteFolders();
//
//             // Create sample sprites for each tile type
//             foreach (TileType tileType in System.Enum.GetValues(typeof(TileType))) {
//                 if (tileType == TileType.Air) continue; // Skip air
//
//                 CreateSampleSpriteForTileType(tileType);
//             }
//
//             AssetDatabase.Refresh();
//             Debug.Log("Created sample sprites for all tile types");
//         }
//
//         private void CreateSampleSpriteForTileType(TileType tileType) {
//             // Create a 16x16 colored texture
//             var texture = new Texture2D(16, 16);
//             Color tileColor = GetSampleTileColor(tileType);
//
//             Color[] pixels = new Color[16 * 16];
//             for (int i = 0; i < pixels.Length; i++) {
//                 // AddComponent some simple pattern to make it more interesting
//                 int x = i % 16;
//                 int y = i / 16;
//                 float variation = Mathf.Sin(x * 0.5f) * Mathf.Sin(y * 0.5f) * 0.1f;
//                 pixels[i] = tileColor + new Color(variation, variation, variation, 0);
//             }
//
//             texture.SetPixels(pixels);
//             texture.Apply();
//
//             // Save as PNG
//             byte[] pngData = texture.EncodeToPNG();
//             string path = $"Assets/Resources/Sprites/Tiles/{tileType}.png";
//             System.IO.File.WriteAllBytes(path, pngData);
//
//             DestroyImmediate(texture);
//         }
//
//         private Color GetSampleTileColor(TileType tileType) {
//             return tileType switch {
//                 TileType.Grass => Color.green, TileType.Dirt => new Color(0.6f, 0.4f, 0.2f), TileType.Sand => Color.yellow, TileType.Clay => new Color(0.8f, 0.6f, 0.4f), TileType.Stone => Color.gray, TileType.Limestone => new Color(0.9f, 0.9f, 0.8f), TileType.Granite => new Color(0.4f, 0.4f, 0.4f), TileType.Marble => Color.white, TileType.Bedrock => Color.black, TileType.Lava => Color.red, TileType.Water => Color.blue, TileType.Ice => new Color(0.8f, 0.9f, 1f), _ => Color.gray
//             };
//         }
//     }
//
//     public static class TilemapWorldViewerMenuItems {
//         [MenuItem("GameObject/DigDeeper/Tilemap ECSSystem Viewer", false, 10)]
//         public static void CreateTilemapWorldViewer() {
//             var go = new GameObject("Tilemap ECSSystem Viewer");
//             var viewer = go.AddComponent<TilemapWorldViewer>();
//
//             // Try to assign a default config
//             var configs = Resources.LoadAll<WorldGenerationConfig>("Config");
//             if (configs.Length > 0) {
//                 viewer._mapConfig = configs[0];
//             }
//
//             Selection.activeGameObject = go;
//             Debug.Log("Created Tilemap ECSSystem Viewer - much faster than GameObject approach!");
//         }
//     }
// }
// #endif
// #endif