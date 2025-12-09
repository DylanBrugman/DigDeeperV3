// #if UNITY_EDITOR
// using System.Linq;
// using DigDeeper.WorldSystem;
// using Systems.WorldSystem;
// using Systems.WorldSystem.Generator;
// using UnityEditor;
// using UnityEngine;
//
// namespace Editor.DigDeeper.WorldSystem.Editor {
//     public class WorldEditorWindow : EditorWindow
//     {
//         private WorldGenerationConfig selectedConfig;
//         private WorldPreviewData currentPreview;
//         private Vector2 scrollPosition;
//         private Vector2 previewScrollPosition;
//         private bool showLayerSettings = true;
//         private bool showResourceSettings = true;
//         private bool showPOISettings = true;
//         private bool showPreview = false;
//         private bool autoRefreshPreview = false;
//         
//         // Preview settings
//         private int previewWidth = 200;
//         private int previewHeight = 100;
//         private float tileSize = 2f;
//         private bool showResources = true;
//         private bool showBiomes = false;
//         private bool showStructure = true;
//         
//         [MenuItem("DigDepper/ECSSystem Editor")]
//         public static void ShowWindow()
//         {
//             var window = GetWindow<WorldEditorWindow>("ECSSystem Editor");
//             window.minSize = new Vector2(800, 600);
//         }
//         
//         private void OnGUI()
//         {
//             DrawHeader();
//             
//             scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
//             
//             DrawConfigurationSection();
//             DrawWorldSettings();
//             DrawLayerSettings();
//             DrawResourceSettings();
//             DrawPOISettings();
//             DrawPreviewSection();
//             DrawActions();
//             
//             EditorGUILayout.EndScrollView();
//         }
//         
//         private void DrawHeader()
//         {
//             EditorGUILayout.Space();
//             var headerStyle = new GUIStyle(EditorStyles.boldLabel)
//             {
//                 fontSize = 18,
//                 alignment = TextAnchor.MiddleCenter
//             };
//             EditorGUILayout.LabelField("ECSSystem Generation Editor", headerStyle);
//             EditorGUILayout.Space();
//         }
//         
//         private void DrawConfigurationSection()
//         {
//             EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//             EditorGUILayout.LabelField("Configuration", EditorStyles.boldLabel);
//             
//             var newConfig = (WorldGenerationConfig)EditorGUILayout.ObjectField(
//                 "ECSSystem Config", selectedConfig, typeof(WorldGenerationConfig), false);
//             
//             if (newConfig != selectedConfig)
//             {
//                 selectedConfig = newConfig;
//                 currentPreview = null;
//             }
//             
//             EditorGUILayout.BeginHorizontal();
//             if (GUILayout.Button("Create New Config"))
//             {
//                 CreateNewWorldConfig();
//             }
//             if (GUILayout.Button("Duplicate Current"))
//             {
//                 DuplicateCurrentConfig();
//             }
//             EditorGUILayout.EndHorizontal();
//             
//             EditorGUILayout.EndVertical();
//         }
//         
//         private void DrawWorldSettings()
//         {
//             if (selectedConfig == null) return;
//             
//             EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//             EditorGUILayout.LabelField("ECSSystem Settings", EditorStyles.boldLabel);
//             
//             var serializedConfig = new SerializedObject(selectedConfig);
//             
//             EditorGUILayout.PropertyField(serializedConfig.FindProperty("worldSize"));
//             EditorGUILayout.PropertyField(serializedConfig.FindProperty("surfaceAirHeightMinimal"));
//             EditorGUILayout.PropertyField(serializedConfig.FindProperty("seed"));
//             
//             EditorGUILayout.Space();
//             EditorGUILayout.LabelField("Noise Settings", EditorStyles.boldLabel);
//             EditorGUILayout.PropertyField(serializedConfig.FindProperty("terrainScale"));
//             EditorGUILayout.PropertyField(serializedConfig.FindProperty("terrainAmplitude"));
//             EditorGUILayout.PropertyField(serializedConfig.FindProperty("caveScale"));
//             EditorGUILayout.PropertyField(serializedConfig.FindProperty("caveThreshold"));
//             
//             serializedConfig.ApplyModifiedProperties();
//             EditorGUILayout.EndVertical();
//         }
//         
//         private void DrawLayerSettings()
//         {
//             if (selectedConfig == null) return;
//             
//             EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//             showLayerSettings = EditorGUILayout.Foldout(showLayerSettings, "Earth Layers", true);
//             
//             if (showLayerSettings)
//             {
//                 var serializedConfig = new SerializedObject(selectedConfig);
//                 var layersProperty = serializedConfig.FindProperty("earthLayers");
//                 
//                 EditorGUILayout.PropertyField(layersProperty, true);
//                 
//                 if (GUILayout.Button("AddComponent Default Layers"))
//                 {
//                     AddDefaultEarthLayers();
//                 }
//                 
//                 serializedConfig.ApplyModifiedProperties();
//             }
//             
//             EditorGUILayout.EndVertical();
//         }
//         
//         private void DrawResourceSettings()
//         {
//             if (selectedConfig == null) return;
//             
//             EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//             showResourceSettings = EditorGUILayout.Foldout(showResourceSettings, "Resource Settings", true);
//             
//             if (showResourceSettings)
//             {
//                 var serializedConfig = new SerializedObject(selectedConfig);
//                 
//                 EditorGUILayout.PropertyField(serializedConfig.FindProperty("generateResources"));
//                 EditorGUILayout.PropertyField(serializedConfig.FindProperty("globalResourceMultiplier"));
//                 EditorGUILayout.PropertyField(serializedConfig.FindProperty("globalResourceRules"), true);
//                 
//                 if (GUILayout.Button("AddComponent Common Resources"))
//                 {
//                     AddCommonResourceRules();
//                 }
//                 
//                 serializedConfig.ApplyModifiedProperties();
//             }
//             
//             EditorGUILayout.EndVertical();
//         }
//         
//         private void DrawPOISettings()
//         {
//             if (selectedConfig == null) return;
//             
//             EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//             showPOISettings = EditorGUILayout.Foldout(showPOISettings, "Points of Interest", true);
//             
//             if (showPOISettings)
//             {
//                 var serializedConfig = new SerializedObject(selectedConfig);
//                 EditorGUILayout.PropertyField(serializedConfig.FindProperty("pointsOfInterest"), true);
//                 
//                 if (GUILayout.Button("AddComponent Sample POIs"))
//                 {
//                     AddSamplePOIs();
//                 }
//                 
//                 serializedConfig.ApplyModifiedProperties();
//             }
//             
//             EditorGUILayout.EndVertical();
//         }
//         
//         private void DrawPreviewSection()
//         {
//             if (selectedConfig == null) return;
//             
//             EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//             showPreview = EditorGUILayout.Foldout(showPreview, "ECSSystem Preview", true);
//             
//             if (showPreview)
//             {
//                 EditorGUILayout.BeginHorizontal();
//                 EditorGUILayout.LabelField("Preview ChunkSize:", GUILayout.Width(100));
//                 previewWidth = EditorGUILayout.IntField(previewWidth, GUILayout.Width(60));
//                 EditorGUILayout.LabelField("x", GUILayout.Width(10));
//                 previewHeight = EditorGUILayout.IntField(previewHeight, GUILayout.Width(60));
//                 EditorGUILayout.EndHorizontal();
//                 
//                 EditorGUILayout.BeginHorizontal();
//                 tileSize = EditorGUILayout.Slider("Tile ChunkSize", tileSize, 1f, 5f);
//                 EditorGUILayout.EndHorizontal();
//                 
//                 EditorGUILayout.BeginHorizontal();
//                 showResources = EditorGUILayout.Toggle("Show Resources", showResources);
//                 showBiomes = EditorGUILayout.Toggle("Show Biomes", showBiomes);
//                 showStructure = EditorGUILayout.Toggle("Show Structure", showStructure);
//                 EditorGUILayout.EndHorizontal();
//                 
//                 EditorGUILayout.BeginHorizontal();
//                 if (GUILayout.Button("GenerateStep Preview"))
//                 {
//                     GeneratePreview();
//                 }
//                 autoRefreshPreview = EditorGUILayout.Toggle("Auto Refresh", autoRefreshPreview);
//                 EditorGUILayout.EndHorizontal();
//                 
//                 if (currentPreview != null)
//                 {
//                     DrawWorldPreview();
//                 }
//             }
//             
//             EditorGUILayout.EndVertical();
//         }
//         
//         private void DrawActions()
//         {
//             if (selectedConfig == null) return;
//             
//             EditorGUILayout.BeginVertical(EditorStyles.helpBox);
//             EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
//             
//             EditorGUILayout.BeginHorizontal();
//             if (GUILayout.Button("Save ECSSystem Template"))
//             {
//                 SaveWorldTemplate();
//             }
//             if (GUILayout.Button("Load ECSSystem Template"))
//             {
//                 LoadWorldTemplate();
//             }
//             EditorGUILayout.EndHorizontal();
//             
//             EditorGUILayout.BeginHorizontal();
//             if (GUILayout.Button("Set as Runtime Default"))
//             {
//                 SetAsRuntimeDefault();
//             }
//             if (GUILayout.Button("Test in Play Mode"))
//             {
//                 TestInPlayMode();
//             }
//             EditorGUILayout.EndHorizontal();
//             
//             EditorGUILayout.EndVertical();
//         }
//         
//         private void DrawWorldPreview()
//         {
//             if (currentPreview?.previewTexture == null) return;
//             
//             var rect = GUILayoutUtility.GetRect(
//                 currentPreview.previewTexture.width * tileSize,
//                 currentPreview.previewTexture.height * tileSize
//             );
//             
//             previewScrollPosition = EditorGUILayout.BeginScrollView(
//                 previewScrollPosition,
//                 GUILayout.MaxHeight(400)
//             );
//             
//             GUI.DrawTexture(rect, currentPreview.previewTexture, ScaleMode.StretchToFill);
//             
//             // Draw additional info
//             EditorGUILayout.LabelField($"Resources Found: {currentPreview.resourceCount}");
//             EditorGUILayout.LabelField($"POIs Placed: {currentPreview.poiCount}");
//             EditorGUILayout.LabelField($"Generation Time: {currentPreview.generationTime:F2}ms");
//             
//             EditorGUILayout.EndScrollView();
//         }
//         
//         private void GeneratePreview()
//         {
//             if (selectedConfig == null) return;
//             
//             var startTime = System.DateTime.Now;
//             
//             // Create temporary config for preview
//             var previewConfig = Instantiate(selectedConfig);
//             previewConfig.worldSize = new Vector2Int(previewWidth, previewHeight);
//             
//             // GenerateStep ecsWorld
//             var generator = new TilemapWorldGenerator(previewConfig);
//             var world = generator.GenerateNewTilemapWorld();
//             
//             // Create preview config
//             currentPreview = new WorldPreviewData
//             {
//                 world = world,
//                 generationTime = (float)(System.DateTime.Now - startTime).TotalMilliseconds
//             };
//             
//             // GenerateStep preview texture
//             CreatePreviewTexture(world);
//             
//             // Count resources and POIs
//             CountPreviewStatistics(world);
//             
//             DestroyImmediate(previewConfig);
//         }
//         
//         private void CreatePreviewTexture(Tile[,] world)
//         {
//             int width = world.GetLength(0);
//             int height = world.GetLength(1);
//             
//             var texture = new Texture2D(width, height);
//             
//             for (int x = 0; x < width; x++)
//             {
//                 for (int y = 0; y < height; y++)
//                 {
//                     var tile = world[x, y];
//                     Color pixelColor = GetTilePreviewColor(tile);
//                     texture.SetPixel(x, height - 1 - y, pixelColor); // Flip Y for proper display
//                 }
//             }
//             
//             texture.Apply();
//             currentPreview.previewTexture = texture;
//         }
//         
//         private Color GetTilePreviewColor(Tile tile)
//         {
//             if (showBiomes)
//             {
//                 return GetBiomeColor(tile.biome);
//             }
//             
//             Color baseColor = GetTileTypeColor(tile.tileType);
//             
//             if (showResources && tile.resources.Count > 0)
//             {
//                 var primaryResource = tile.resources.OrderBy(r => r.abundance).Last();
//                 Color resourceColor = GetResourcePreviewColor(primaryResource.type);
//                 baseColor = Color.Lerp(baseColor, resourceColor, 0.5f);
//             }
//             
//             if (showStructure && tile.stability < 50f)
//             {
//                 baseColor = Color.Lerp(baseColor, Color.red, 0.3f);
//             }
//             
//             if (tile.isPointOfInterest)
//             {
//                 baseColor = Color.Lerp(baseColor, Color.magenta, 0.7f);
//             }
//             
//             return baseColor;
//         }
//         
//         private Color GetTileTypeColor(TileType tileType)
//         {
//             return tileType switch
//             {
//                 TileType.Air => Color.cyan,
//                 TileType.Grass => Color.green,
//                 TileType.Dirt => new Color(0.6f, 0.4f, 0.2f),
//                 TileType.Sand => Color.yellow,
//                 TileType.Clay => new Color(0.8f, 0.6f, 0.4f),
//                 TileType.Stone => Color.gray,
//                 TileType.Limestone => new Color(0.9f, 0.9f, 0.8f),
//                 TileType.Granite => new Color(0.4f, 0.4f, 0.4f),
//                 TileType.Marble => Color.white,
//                 TileType.Bedrock => Color.black,
//                 TileType.Lava => Color.red,
//                 TileType.Water => Color.blue,
//                 TileType.Ice => new Color(0.8f, 0.9f, 1f),
//                 _ => Color.gray
//             };
//         }
//         
//         private Color GetBiomeColor(BiomeType biome)
//         {
//             return biome switch
//             {
//                 BiomeType.Temperate => Color.green,
//                 BiomeType.Desert => Color.yellow,
//                 BiomeType.Arctic => Color.white,
//                 BiomeType.Volcanic => Color.red,
//                 BiomeType.Swamp => new Color(0.3f, 0.6f, 0.3f),
//                 BiomeType.Mountain => Color.gray,
//                 BiomeType.Ocean => Color.blue,
//                 _ => Color.gray
//             };
//         }
//         
//         private Color GetResourcePreviewColor(ResourceType resourceType)
//         {
//             return resourceType switch
//             {
//                 ResourceType.Coal => Color.black,
//                 ResourceType.Iron => new Color(0.8f, 0.4f, 0.2f),
//                 ResourceType.Gold => Color.yellow,
//                 ResourceType.Diamond => Color.cyan,
//                 ResourceType.Ruby => Color.red,
//                 ResourceType.Emerald => Color.green,
//                 ResourceType.Uranium => new Color(0.5f, 1f, 0.2f),
//                 _ => Color.magenta
//             };
//         }
//         
//         private void CountPreviewStatistics(Tile[,] world)
//         {
//             int resourceCount = 0;
//             int poiCount = 0;
//             
//             for (int x = 0; x < world.GetLength(0); x++)
//             {
//                 for (int y = 0; y < world.GetLength(1); y++)
//                 {
//                     var tile = world[x, y];
//                     if (tile.resources.Count > 0) resourceCount++;
//                     if (tile.isPointOfInterest) poiCount++;
//                 }
//             }
//             
//             currentPreview.resourceCount = resourceCount;
//             currentPreview.poiCount = poiCount;
//         }
//         
//         private void CreateNewWorldConfig()
//         {
//             var config = CreateInstance<WorldGenerationConfig>();
//             
//             // Set default values
//             config.worldSize = new Vector2Int(200, 100);
//             config.surfaceAirHeightMinimal = 10f;
//             config.seed = Random.Range(1000, 99999);
//             
//             var path = EditorUtility.SaveFilePanelInProject(
//                 "Save ECSSystem Config",
//                 "NewWorldConfig",
//                 "asset",
//                 "Choose where to save the ecsWorld configuration"
//             );
//             
//             if (!string.IsNullOrEmpty(path))
//             {
//                 AssetDatabase.CreateAsset(config, path);
//                 AssetDatabase.SaveAssets();
//                 selectedConfig = config;
//             }
//         }
//         
//         private void DuplicateCurrentConfig()
//         {
//             if (selectedConfig == null) return;
//             
//             var duplicate = Instantiate(selectedConfig);
//             duplicate.seed = Random.Range(1000, 99999);
//             
//             var path = EditorUtility.SaveFilePanelInProject(
//                 "Save Duplicated Config",
//                 selectedConfig.name + "_Copy",
//                 "asset",
//                 "Choose where to save the duplicated configuration"
//             );
//             
//             if (!string.IsNullOrEmpty(path))
//             {
//                 AssetDatabase.CreateAsset(duplicate, path);
//                 AssetDatabase.SaveAssets();
//                 selectedConfig = duplicate;
//             }
//         }
//         
//         private void AddDefaultEarthLayers()
//         {
//             if (selectedConfig == null) return;
//             
//             selectedConfig.earthLayers.Clear();
//             
//             // Surface Layer
//             selectedConfig.earthLayers.Add(new EarthLayer
//             {
//                 layerName = "Surface",
//                 startDepth = 0,
//                 endDepth = 5,
//                 primaryTileType = TileType.Dirt,
//                 temperature = 20f,
//                 pressure = 1f
//             });
//             
//             // Shallow Rock
//             selectedConfig.earthLayers.Add(new EarthLayer
//             {
//                 layerName = "Shallow Rock",
//                 startDepth = 5,
//                 endDepth = 25,
//                 primaryTileType = TileType.Stone,
//                 temperature = 25f,
//                 pressure = 1.5f
//             });
//             
//             // Deep Rock
//             selectedConfig.earthLayers.Add(new EarthLayer
//             {
//                 layerName = "Deep Rock",
//                 startDepth = 25,
//                 endDepth = 60,
//                 primaryTileType = TileType.Granite,
//                 temperature = 40f,
//                 pressure = 3f
//             });
//             
//             // Bedrock
//             selectedConfig.earthLayers.Add(new EarthLayer
//             {
//                 layerName = "Bedrock",
//                 startDepth = 60,
//                 endDepth = 100,
//                 primaryTileType = TileType.Bedrock,
//                 temperature = 80f,
//                 pressure = 5f
//             });
//             
//             EditorUtility.SetDirty(selectedConfig);
//         }
//         
//         private void AddCommonResourceRules()
//         {
//             if (selectedConfig == null) return;
//             
//             selectedConfig.globalResourceRules.Add(new ResourceSpawnRule
//             {
//                 resourceType = ResourceType.Coal,
//                 spawnChance = 0.1f,
//                 minAbundance = 20f,
//                 maxAbundance = 80f,
//                 baseQuality = 50f,
//                 clusterSize = new Vector2(3, 8)
//             });
//             
//             selectedConfig.globalResourceRules.Add(new ResourceSpawnRule
//             {
//                 resourceType = ResourceType.Iron,
//                 spawnChance = 0.08f,
//                 minAbundance = 15f,
//                 maxAbundance = 60f,
//                 baseQuality = 60f,
//                 clusterSize = new Vector2(2, 6)
//             });
//             
//             selectedConfig.globalResourceRules.Add(new ResourceSpawnRule
//             {
//                 resourceType = ResourceType.Gold,
//                 spawnChance = 0.02f,
//                 minAbundance = 5f,
//                 maxAbundance = 30f,
//                 baseQuality = 80f,
//                 clusterSize = new Vector2(1, 3)
//             });
//             
//             EditorUtility.SetDirty(selectedConfig);
//         }
//         
//         private void AddSamplePOIs()
//         {
//             if (selectedConfig == null) return;
//             
//             var cavePOI = new PointOfInterestConfig
//             {
//                 id = "ancient_cave",
//                 displayName = "Ancient Cave",
//                 size = new Vector2Int(8, 6),
//                 spawnWeight = 1f,
//                 minDistanceFromOthers = 30f,
//                 spawnOnSurface = false,
//                 depthRange = new Vector2(15f, 40f)
//             };
//             
//             cavePOI.guaranteedResources.Add(new ResourceDeposit(ResourceType.Gold, 50f, 90f));
//             cavePOI.guaranteedResources.Add(new ResourceDeposit(ResourceType.Diamond, 20f, 95f));
//             
//             selectedConfig.pointsOfInterest.Add(cavePOI);
//             
//             EditorUtility.SetDirty(selectedConfig);
//         }
//         
//         private void SaveWorldTemplate()
//         {
//             if (selectedConfig == null || currentPreview?.world == null) return;
//             
//             var template = new WorldTemplate
//             {
//                 config = selectedConfig,
//                 worldData = SerializeWorld(currentPreview.world),
//                 previewTexture = currentPreview.previewTexture.EncodeToPNG()
//             };
//             
//             var path = EditorUtility.SaveFilePanelInProject(
//                 "Save ECSSystem Template",
//                 "WorldTemplate",
//                 "asset",
//                 "Choose where to save the ecsWorld template"
//             );
//             
//             if (!string.IsNullOrEmpty(path))
//             {
//                 AssetDatabase.CreateAsset(template, path);
//                 AssetDatabase.SaveAssets();
//             }
//         }
//         
//         private void LoadWorldTemplate()
//         {
//             var path = EditorUtility.OpenFilePanel("Load ECSSystem Template", "Assets", "asset");
//             if (!string.IsNullOrEmpty(path))
//             {
//                 // Implementation for loading ecsWorld template
//                 Debug.Log("ECSSystem template loading not yet implemented");
//             }
//         }
//         
//         private void SetAsRuntimeDefault()
//         {
//             if (selectedConfig == null) return;
//             
//             var runtimeConfig = FindObjectOfType<RuntimeWorldConfig>();
//             if (runtimeConfig == null)
//             {
//                 Debug.LogWarning("No RuntimeWorldConfig found in scene. Create one first.");
//                 return;
//             }
//             
//             runtimeConfig._defaultWorldConfig = selectedConfig;
//             EditorUtility.SetDirty(runtimeConfig);
//             
//             Debug.Log($"Set {selectedConfig.name} as runtime default ecsWorld configuration");
//         }
//         
//         private void TestInPlayMode()
//         {
//             if (selectedConfig == null) return;
//             
//             SetAsRuntimeDefault();
//             EditorApplication.isPlaying = true;
//         }
//         
//         private WorldData SerializeWorld(Tile[,] world)
//         {
//             // Implementation for ecsWorld serialization
//             return new WorldData();
//         }
//         
//         private void Update()
//         {
//             if (autoRefreshPreview && selectedConfig != null)
//             {
//                 // Auto-refresh preview every few seconds
//                 GeneratePreview();
//             }
//         }
//     }
// }
// #endif