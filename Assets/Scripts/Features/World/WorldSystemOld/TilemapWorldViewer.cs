// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DigDeeper.WorldSystem;
// using GamePlay.World.Tilemap.Generator;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.Tilemaps;
// using Tile = Systems.WorldSystem.Generator.Tile;
//
// namespace Systems.WorldSystem {
//     public class TilemapWorldViewer : MonoBehaviour {
//         [Header("Tilemap References")] [Tooltip("Tilemap containing tiles (drag from Project)")]
//         [SerializeField] private Tilemap tilemap;
//         [SerializeField] private TilemapRenderer tilemapRenderer;
//
//         [Header("Sprite Sources")] [Tooltip("Tilemap texture containing sprites (drag from Project)")] 
//         [SerializeField] private Texture2D tilemapTexture;
//         [SerializeField] private bool debugSpriteLoading;
//
//         private Dictionary<TileType, Sprite> tileSprites = new ();
//         private Tile[,] _tiles;
//         
//         public Tilemap Tilemap => tilemap;
//
//         private void Start() {
//             LoadSpritesFromTilemapTexture();
//         }
//
//         [ContextMenu("Clear NativeTileGrid")]
//         public void ClearTiles() {
//             tilemap.ClearAllTiles();
//         }
//
//         public void Show(Tile[,] tiles) {
//             _tiles = tiles;
//
//             if (tilemap == null || tilemapRenderer == null) {
//                 Debug.LogError("tilemap or tilemapRenderer is not assigned.");
//                 return;
//             }
//
//             if (tileSprites == null || tileSprites.Count == 0) {
//                 Debug.Log("[TilemapViewer] No tile sprites loaded, loading from tilemap texture...");
//                 LoadSpritesFromTilemapTexture();
//             }
//             
//             tilemap.ClearAllTiles();
//             (Vector3Int[], TileBase[]) unityTiles = ConvertToUnityTiles(tiles);
//             tilemap.SetTiles(unityTiles.Item1, unityTiles.Item2);
//             Debug.Log($"[TilemapViewer] Rendered {tiles.GetLength(0) * tiles.GetLength(1)} tiles.");
//         }
//
//         private (Vector3Int[], TileBase[]) ConvertToUnityTiles(Tile[,] tiles) {
//             List<Vector3Int> positions = new List<Vector3Int>(tiles.Length);
//             List<TileBase> tileBases = new List<TileBase>(tiles.Length);
//             
//             Sprite fallbackSprite = CreateColoredSprite(Color.red, "FallbackTile");
//
//             foreach (Tile tile in tiles) {
//                 if (tile == null) {
//                     Debug.LogWarning("[TilemapViewer] Found null tile in the tile array, skipping.");
//                     continue;
//                 }
//
//                 positions.Add(new Vector3Int(tile.position.x, tile.position.y, 0));
//                 
//                 // Create a Unity TileBase for each tile
//                 UnityEngine.Tilemaps.Tile unityTile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
//                 if (!tileSprites.ContainsKey(tile.tileType)) {
//                     unityTile.sprite = fallbackSprite;
//                     Debug.LogWarning($"[TilemapViewer] No sprite found for tile type {tile.tileType}, using fallback sprite.");
//                 }
//
//                 unityTile.sprite = tileSprites[tile.tileType];
//                 unityTile.name = $"Tile_{tile.tileType}" + $"_{tile.position.x}_{tile.position.y}";
//                 tileBases.Add(unityTile);
//             }
//             return (positions.ToArray(), tileBases.ToArray());
//         }
//
//         private Sprite CreateColoredSprite(Color getTileTypeColor, string toString) {
//             Texture2D texture = new Texture2D(16, 16);
//             Color[] pixels = new Color[16 * 16];
//
//             // Fill the texture with the specified color
//             for (int i = 0; i < pixels.Length; i++) {
//                 pixels[i] = getTileTypeColor;
//             }
//
//             texture.SetPixels(pixels);
//             texture.Apply();
//
//             Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), Vector2.one * 0.5f, 16);
//             sprite.name = toString;
//
//             return sprite;
//         }
//
//         private void LoadSpritesFromTilemapTexture() {
//             if (tilemapTexture == null) {
//                 if (debugSpriteLoading) {
//                     Debug.Log("[TilemapViewer] No tilemap texture assigned");
//                 }
//                 return;
//             }
//
//             if (debugSpriteLoading) {
//                 Debug.Log($"[TilemapViewer] Loading sprites from tilemap texture: {tilemapTexture.name}");
//             }
//
//             // Get all sprites from the tilemap texture
//             string texturePath = AssetDatabase.GetAssetPath(tilemapTexture);
//             var sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>().ToArray();
//
//             if (debugSpriteLoading) {
//                 Debug.Log($"[TilemapViewer] Found {sprites.Length} sprites in tilemap texture");
//                 foreach (var sprite in sprites) {
//                     Debug.Log($"[TilemapViewer] Tilemap sprite: '{sprite.name}'");
//                 }
//             }
//
//             foreach (var sprite in sprites) {
//                 // Try to match sprite name to TileType
//                 if (TryMatchSpriteToTileType(sprite.name, out var tileType)) {
//                     tileSprites[tileType] = sprite;
//                     if (debugSpriteLoading) {
//                         Debug.Log($"[TilemapViewer] Matched tilemap sprite '{sprite.name}' to {tileType}");
//                     }
//                 }
//             }
//         }
//
//         private bool TryMatchSpriteToTileType(string spriteName, out TileType tileType) {
//             if (Enum.TryParse(spriteName, true, out tileType)) {
//                 return true;
//             }
//
//             return false;
//         }
//
//         public void GenerateWorldInEditor() {
//             throw new System.NotImplementedException("TilemapWorldViewer.GenerateWorldInEditor is not implemented yet.");
//         }
//     }
// }
//
// // // ===================================================================
// // // TILEMAP SPRITE LOADER - EDITOR & RUNTIME COMPATIBLE
// // // Handles sprites within tilemap assets and different loading contexts
// // // ===================================================================
// //
// // #if UNITY_EDITOR
// // using System;
// // using System.Collections.Generic;
// // using System.Linq;
// // using DigDeeper.WorldSystem;
// // using Systems.WorldSystem;
// // using Systems.WorldSystem.DigDeeper.WorldSystem.Editor;
// // using Systems.WorldSystem.Generator;
// // using UnityEditor;
// // using UnityEngine;
// // using UnityEngine.SceneManagement;
// // using UnityEngine.Tilemaps;
// // using Tile = Systems.WorldSystem.Generator.Tile;
// // using UnityTile = UnityEngine.Tilemaps.Tile;
// // // ReSharper disable InconsistentNaming
// //
// // namespace DigDeeper.WorldSystem.Editor
// // {
// //     [System.Serializable]
// //     public class TilemapWorldViewer : MonoBehaviour {
// //         private static TilemapWorldViewer _instance;
// //         public static TilemapWorldViewer Instance {
// //             get {
// //                 if (_instance == null) {
// //                     _instance = FindObjectOfType<TilemapWorldViewer>();
// //                     if (_instance == null) {
// //                         _instance = new GameObject("TilemapWorldViewer").AddComponent<TilemapWorldViewer>();
// //                     }
// //                 }
// //                 return _instance;
// //             }
// //         }
// //         
// //         [Header("ECSSystem Configuration")] public WorldGenerationConfig _mapConfig;
// //
// //         [Header("Visualization")] public ViewMode viewMode = ViewMode.NativeTileGrid;
// //         public bool showResourceOverlay = true;
// //         public bool showPOIHighlight = true;
// //
// //         [Header("Auto-Update Settings")] public bool enableAutoUpdate = true;
// //         public float updateDelay = 0.5f;
// //
// //         private Coroutine updateCoroutine;
// //         private WorldGenerationConfig lastConfig;
// //         private int lastConfigHash;
// //
// //         [Header("Performance")] public bool enableChunking = true;
// //         [Range(32, 128)] public int chunkSize = 64;
// //         public bool limitRenderDistance = false;
// //         [Range(50, 200)] public int maxRenderDistance = 100;
// //
// //         [Header("Sprite Loading")] public bool debugSpriteLoading = true;
// //
// //         [Header("Sprite Sources")] [Tooltip("Tilemap texture containing sprites (drag from Project)")]
// //         public Texture2D tilemapTexture;
// //
// //         [Tooltip("Individual sprite files from Resources")]
// //         public string spriteResourcePath = "Sprites/NativeTileGrid";
// //
// //         [Tooltip("Pre-made tile assets from Resources")]
// //         public string tileResourcePath = "NativeTileGrid";
// //
// //         [Tooltip("Fallback to TileManager manager at runtime")]
// //         public bool useTileSystemFallback = true;
// //         
// //         // Tilemap components
// //         private Grid grid;
// //         private GameObject tilemapContainer;
// //         private Tilemap baseTilemap;
// //         private TilemapRenderer baseTilemapRenderer;
// //         private Tilemap overlayTilemap;
// //         private TilemapRenderer overlayTilemapRenderer;
// //
// //         // Tile assets
// //         private Dictionary<TileType, TileBase> tileAssets = new Dictionary<TileType, TileBase>();
// //         private Dictionary<ResourceType, TileBase> resourceTileAssets = new Dictionary<ResourceType, TileBase>();
// //         private Dictionary<TileType, Sprite> tileSprites = new Dictionary<TileType, Sprite>();
// //
// //         // ECSSystem config
// //         [System.NonSerialized] protected Tile[,] generatedWorld;
// //         [System.NonSerialized] protected bool worldGenerated = false;
// //
// //         // Chunk system
// //         private Dictionary<Vector2Int, TilemapChunk> loadedChunks = new Dictionary<Vector2Int, TilemapChunk>();
// //
// //         // Properties
// //         public bool HasGeneratedWorld => worldGenerated && generatedWorld != null;
// //         public Vector2Int WorldSizeChunks => _mapConfig?.worldSize ?? Vector2Int.zero;
// //
// //         protected virtual void OnValidate() {
// //             if (_mapConfig != null && HasGeneratedWorld) {
// //                 EditorApplication.delayCall += () => {
// //                     if (this != null) RefreshVisualization();
// //                 };
// //             }
// //         }
// //
// //         [ContextMenu("GenerateStep ECSSystem")]
// //         public virtual void GenerateWorldInEditor() {
// //             if (_mapConfig == null) {
// //                 Debug.LogWarning("No ecsWorld config assigned to TilemapWorldViewer");
// //                 return;
// //             }
// //
// //             var startTime = System.DateTime.Now;
// //
// //             // GenerateStep ecsWorld config
// //             var generator = new TilemapWorldGenerator(_mapConfig);
// //             generatedWorld = generator.GenerateNewTilemapWorld();
// //             worldGenerated = true;
// //
// //             // Setup tilemap system
// //             SetupTilemapSystem();
// //
// //             // Load sprites from multiple sources
// //             LoadAllSpriteSources();
// //
// //             // Create tilemap visualization
// //             if (enableChunking) {
// //                 CreateChunkedVisualization();
// //             }
// //             else {
// //                 CreateDirectVisualization();
// //             }
// //
// //             var generationTime = (System.DateTime.Now - startTime).TotalMilliseconds;
// //             Debug.Log($"Tilemap ecsWorld generated: {generationTime:F1}ms");
// //
// //             EditorUtility.SetDirty(this);
// //             UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
// //         }
// //
// //         // ===================================================================
// //         // ENHANCED SPRITE LOADING - MULTIPLE SOURCES
// //         // ===================================================================
// //
// //         private void LoadAllSpriteSources() {
// //             tileSprites.Clear();
// //             tileAssets.Clear();
// //             resourceTileAssets.Clear();
// //
// //             if (debugSpriteLoading) {
// //                 Debug.Log("[TilemapViewer] Loading sprites from all sources...");
// //             }
// //
// //             // Priority order for sprite loading:
// //             // 1. Tilemap texture (your case)
// //             // 2. TileManager at runtime (your working case)
// //             // 3. Individual Resources sprites
// //             // 4. Pre-made tile assets
// //             // 5. Generated fallbacks
// //
// //             LoadSpritesFromTilemapTexture();
// //             // LoadSpritesFromTileSystem();
// //             // LoadSpritesFromResources();
// //             // LoadTileAssetsFromResources();
// //             // CreateFallbackTileAssets();
// //
// //             if (debugSpriteLoading) {
// //                 Debug.Log($"[TilemapViewer] Final sprite count: {tileSprites.Count} sprites, {tileAssets.Count} tile assets");
// //             }
// //         }
// //
// //         private void LoadSpritesFromTilemapTexture() {
// //             if (tilemapTexture == null) {
// //                 if (debugSpriteLoading) {
// //                     Debug.Log("[TilemapViewer] No tilemap texture assigned");
// //                 }
// //
// //                 return;
// //             }
// //
// //             if (debugSpriteLoading) {
// //                 Debug.Log($"[TilemapViewer] Loading sprites from tilemap texture: {tilemapTexture.name}");
// //             }
// //
// //             // Get all sprites from the tilemap texture
// //             string texturePath = AssetDatabase.GetAssetPath(tilemapTexture);
// //             var sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>().ToArray();
// //
// //             if (debugSpriteLoading) {
// //                 Debug.Log($"[TilemapViewer] Found {sprites.Length} sprites in tilemap texture");
// //                 foreach (var sprite in sprites) {
// //                     Debug.Log($"[TilemapViewer] Tilemap sprite: '{sprite.name}'");
// //                 }
// //             }
// //
// //             foreach (var sprite in sprites) {
// //                 // Try to match sprite name to TileType
// //                 if (TryMatchSpriteToTileType(sprite.name, out var tileType)) {
// //                     tileSprites[tileType] = sprite;
// //                     if (debugSpriteLoading) {
// //                         Debug.Log($"[TilemapViewer] Matched tilemap sprite '{sprite.name}' to {tileType}");
// //                     }
// //                 }
// //             }
// //         }
// //
// //         private void LoadSpritesFromTileSystem() {
// //             if (!useTileSystemFallback) {
// //                 return;
// //             }
// //
// //             // Try to get sprites from TileManager (your runtime working case)
// //             var tileSystem = FindObjectOfType<TileManager>();
// //             if (tileSystem == null) {
// //                 // Try to find it through singleton
// //                 try {
// //                     // Use reflection to access TileManager.Instance if available
// //                     var tileSystemType = System.Type.GetType("DigDeeper.WorldSystem.TileManager");
// //                     if (tileSystemType != null) {
// //                         var instanceProperty = tileSystemType.GetProperty("Instance",
// //                             System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
// //                         if (instanceProperty != null) {
// //                             tileSystem = instanceProperty.GetValue(null) as TileManager;
// //                         }
// //                     }
// //                 }
// //                 catch (System.Exception e) {
// //                     if (debugSpriteLoading) {
// //                         Debug.Log($"[TilemapViewer] Could not access TileManager: {e.Message}");
// //                     }
// //                 }
// //             }
// //
// //             if (tileSystem != null) {
// //                 if (debugSpriteLoading) {
// //                     Debug.Log("[TilemapViewer] Found TileManager, attempting to get sprites");
// //                 }
// //
// //                 // Try to access TileManager's sprite dictionary through reflection
// //                 try {
// //                     var tileSystemType = tileSystem.GetType();
// //                     var spritesField = tileSystemType.GetField("tileSprites",
// //                         System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
// //
// //                     if (spritesField != null) {
// //                         var systemSprites = spritesField.GetValue(tileSystem) as Dictionary<TileType, Sprite>;
// //                         if (systemSprites != null) {
// //                             foreach (var kvp in systemSprites) {
// //                                 if (!tileSprites.ContainsKey(kvp.Key)) {
// //                                     tileSprites[kvp.Key] = kvp.InitialValue;
// //                                     if (debugSpriteLoading) {
// //                                         Debug.Log($"[TilemapViewer] Got sprite from TileManager: {kvp.Key} -> {kvp.InitialValue.name}");
// //                                     }
// //                                 }
// //                             }
// //                         }
// //                     }
// //                 }
// //                 catch (System.Exception e) {
// //                     if (debugSpriteLoading) {
// //                         Debug.Log($"[TilemapViewer] Could not access TileManager sprites: {e.Message}");
// //                     }
// //                 }
// //             }
// //             else if (debugSpriteLoading) {
// //                 Debug.Log("[TilemapViewer] No TileManager found for sprite fallback");
// //             }
// //         }
// //
// //         private void LoadSpritesFromResources() {
// //             if (debugSpriteLoading) {
// //                 Debug.Log($"[TilemapViewer] Loading individual sprites from Resources/{spriteResourcePath}");
// //             }
// //
// //             var sprites = Resources.LoadAll<Sprite>(spriteResourcePath);
// //
// //             if (debugSpriteLoading) {
// //                 Debug.Log($"[TilemapViewer] Found {sprites.Length} individual sprites");
// //             }
// //
// //             foreach (var sprite in sprites) {
// //                 if (TryMatchSpriteToTileType(sprite.name, out var tileType)) {
// //                     if (!tileSprites.ContainsKey(tileType)) // Don't override tilemap sprites
// //                     {
// //                         tileSprites[tileType] = sprite;
// //                         if (debugSpriteLoading) {
// //                             Debug.Log($"[TilemapViewer] Matched individual sprite '{sprite.name}' to {tileType}");
// //                         }
// //                     }
// //                 }
// //             }
// //         }
// //
// //         private void LoadTileAssetsFromResources() {
// //             if (debugSpriteLoading) {
// //                 Debug.Log($"[TilemapViewer] Loading tile assets from Resources/{tileResourcePath}");
// //             }
// //
// //             var tiles = Resources.LoadAll<TileBase>(tileResourcePath);
// //
// //             foreach (var tile in tiles) {
// //                 if (TryMatchSpriteToTileType(tile.name, out var tileType)) {
// //                     tileAssets[tileType] = tile;
// //                     if (debugSpriteLoading) {
// //                         Debug.Log($"[TilemapViewer] Loaded tile asset '{tile.name}' for {tileType}");
// //                     }
// //                 }
// //
// //                 // Check for resource tiles
// //                 if (tile.name.StartsWith("Resource_")) {
// //                     string resourceName = tile.name.Replace("Resource_", "");
// //                     if (System.Enum.TryParse<ResourceType>(resourceName, true, out var resourceType)) {
// //                         resourceTileAssets[resourceType] = tile;
// //                         if (debugSpriteLoading) {
// //                             Debug.Log($"[TilemapViewer] Loaded resource tile '{tile.name}' for {resourceType}");
// //                         }
// //                     }
// //                 }
// //             }
// //         }
// //
// //         private bool TryMatchSpriteToTileType(string spriteName, out TileType tileType) {
// //             // Direct enum match (case insensitive)
// //             if (System.Enum.TryParse<TileType>(spriteName, true, out tileType)) {
// //                 return true;
// //             }
// //
// //             // Remove common suffixes/prefixes and try again
// //             string cleanName = spriteName.ToLower()
// //                 .Replace("_tile", "")
// //                 .Replace("tile_", "")
// //                 .Replace("_sprite", "")
// //                 .Replace("sprite_", "");
// //
// //             if (System.Enum.TryParse<TileType>(cleanName, true, out tileType)) {
// //                 return true;
// //             }
// //
// //             // Partial matching for common variations
// //             string lowerName = spriteName.ToLower();
// //
// //             if (lowerName.Contains("grass")) {
// //                 tileType = TileType.Grass;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("dirt")) {
// //                 tileType = TileType.Dirt;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("stone")) {
// //                 tileType = TileType.Stone;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("sand")) {
// //                 tileType = TileType.Sand;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("clay")) {
// //                 tileType = TileType.Clay;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("limestone")) {
// //                 tileType = TileType.Limestone;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("granite")) {
// //                 tileType = TileType.Granite;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("marble")) {
// //                 tileType = TileType.Marble;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("bedrock")) {
// //                 tileType = TileType.Bedrock;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("lava")) {
// //                 tileType = TileType.Lava;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("water")) {
// //                 tileType = TileType.Water;
// //                 return true;
// //             }
// //
// //             if (lowerName.Contains("ice")) {
// //                 tileType = TileType.Ice;
// //                 return true;
// //             }
// //             
// //             if (lowerName.Contains("air")) {
// //                 tileType = TileType.Air;
// //                 return true;
// //             }
// //             return false;
// //         }
// //
// //         private void CreateFallbackTileAssets() {
// //             int createdCount = 0;
// //
// //             // Create tile assets for all types that don't have them
// //             foreach (TileType tileType in System.Enum.GetValues(typeof(TileType))) {
// //                 if (!tileAssets.ContainsKey(tileType)) {
// //                     tileAssets[tileType] = CreateTileFromSprite(tileType);
// //                     createdCount++;
// //                 }
// //             }
// //
// //             // Create resource tile assets
// //             foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType))) {
// //                 if (resourceType != ResourceType.None && !resourceTileAssets.ContainsKey(resourceType)) {
// //                     resourceTileAssets[resourceType] = CreateResourceTile(resourceType);
// //                     createdCount++;
// //                 }
// //             }
// //
// //             if (debugSpriteLoading) {
// //                 Debug.Log($"[TilemapViewer] Created {createdCount} fallback tile assets");
// //             }
// //         }
// //
// //         private TileBase CreateTileFromSprite(TileType tileType) {
// //             Sprite sprite = null;
// //
// //             // Use loaded sprite if available
// //             if (tileSprites.ContainsKey(tileType)) {
// //                 sprite = tileSprites[tileType];
// //                 if (debugSpriteLoading) {
// //                     Debug.Log($"[TilemapViewer] Creating tile asset for {tileType} using sprite: {sprite.name}");
// //                 }
// //             }
// //             else {
// //                 // Create fallback colored sprite
// //                 sprite = CreateColoredSprite(GetTileTypeColor(tileType), tileType.ToString());
// //                 if (debugSpriteLoading) {
// //                     Debug.Log($"[TilemapViewer] Creating fallback tile asset for {tileType}");
// //                 }
// //             }
// //
// //             var tile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
// //             tile.sprite = sprite;
// //             tile.name = $"Tile_{tileType}";
// //
// //             return tile;
// //         }
// //
// //         private TileBase CreateResourceTile(ResourceType resourceType) {
// //             var sprite = CreateColoredSprite(GetResourceColor(resourceType), $"Resource_{resourceType}");
// //             var tile = ScriptableObject.CreateInstance<UnityEngine.Tilemaps.Tile>();
// //             tile.sprite = sprite;
// //             tile.name = $"Resource_{resourceType}";
// //
// //             return tile;
// //         }
// //
// //         private Sprite CreateColoredSprite(Color color, string name) {
// //             var texture = new Texture2D(16, 16);
// //             Color[] pixels = new Color[16 * 16];
// //
// //             // Create simple pattern to make tiles distinguishable
// //             for (int i = 0; i < pixels.Length; i++) {
// //                 int x = i % 16;
// //                 int y = i / 16;
// //
// //                 // AddComponent border and pattern
// //                 if (x == 0 || x == 15 || y == 0 || y == 15) {
// //                     pixels[i] = Color.Lerp(color, Color.black, 0.3f); // Darker border
// //                 }
// //                 else if ((x + y) % 4 == 0) {
// //                     pixels[i] = Color.Lerp(color, Color.white, 0.1f); // Slight highlight pattern
// //                 }
// //                 else {
// //                     pixels[i] = color;
// //                 }
// //             }
// //
// //             texture.SetPixels(pixels);
// //             texture.Apply();
// //
// //             var sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), Vector2.one * 0.5f, 16);
// //             sprite.name = name;
// //
// //             return sprite;
// //         }
// //
// //         // ===================================================================
// //         // DEBUG AND INSPECTOR METHODS
// //         // ===================================================================
// //
// //         [ContextMenu("Debug All Sprite Sources")]
// //         public void DebugAllSpriteSources() {
// //             Debug.Log("=== COMPREHENSIVE SPRITE DEBUG ===");
// //
// //             // Check tilemap texture
// //             if (tilemapTexture != null) {
// //                 string texturePath = AssetDatabase.GetAssetPath(tilemapTexture);
// //                 var sprites = AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>().ToArray();
// //                 Debug.Log($"Tilemap Texture '{tilemapTexture.name}': {sprites.Length} sprites");
// //                 foreach (var sprite in sprites.Take(10)) // Show first 10
// //                 {
// //                     Debug.Log($"  - {sprite.name}");
// //                 }
// //
// //                 if (sprites.Length > 10) {
// //                     Debug.Log($"  ... and {sprites.Length - 10} more");
// //                 }
// //             }
// //             else {
// //                 Debug.Log("No tilemap texture assigned");
// //             }
// //
// //             // Check TileManager
// //             var tileSystem = FindObjectOfType<TileManager>();
// //             Debug.Log($"TileManager found: {tileSystem != null}");
// //
// //             // Check Resources
// //             var resourceSprites = Resources.LoadAll<Sprite>(spriteResourcePath);
// //             Debug.Log($"Resource sprites in '{spriteResourcePath}': {resourceSprites.Length}");
// //
// //             var resourceTiles = Resources.LoadAll<TileBase>(tileResourcePath);
// //             Debug.Log($"Resource tiles in '{tileResourcePath}': {resourceTiles.Length}");
// //
// //             // Check loaded results
// //             Debug.Log($"Final loaded sprites: {tileSprites.Count}");
// //             Debug.Log($"Final tile assets: {tileAssets.Count}");
// //
// //             // Show what we have for each TileType
// //             foreach (TileType tileType in System.Enum.GetValues(typeof(TileType))) {
// //                 bool hasSprite = tileSprites.ContainsKey(tileType);
// //                 bool hasAsset = tileAssets.ContainsKey(tileType);
// //                 string spriteName = hasSprite ? tileSprites[tileType].name : "NONE";
// //                 Debug.Log($"  {tileType}: Sprite={spriteName}, Asset={hasAsset}");
// //             }
// //         }
// //
// //         // Rest of the class remains the same...
// //         // (SetupTilemapSystem, CreateDirectVisualization, etc.)
// //
// //         protected void SetupTilemapSystem() {
// //             if (tilemapContainer != null) {
// //                 DestroyImmediate(tilemapContainer);
// //             }
// //
// //             tilemapContainer = new GameObject("Tilemap ECSSystem");
// //             tilemapContainer.transform.SetParent(transform);
// //             tilemapContainer.transform.localPosition = Vector3.zero;
// //
// //             grid = tilemapContainer.AddComponent<Grid>();
// //             grid.cellSize = Vector3.one;
// //
// //             var baseTilemapGO = new GameObject("Base Tilemap");
// //             baseTilemapGO.transform.SetParent(tilemapContainer.transform);
// //             baseTilemap = baseTilemapGO.AddComponent<Tilemap>();
// //             baseTilemapRenderer = baseTilemapGO.AddComponent<TilemapRenderer>();
// //             baseTilemapRenderer.sortingOrder = 0;
// //
// //             var overlayTilemapGO = new GameObject("Overlay Tilemap");
// //             overlayTilemapGO.transform.SetParent(tilemapContainer.transform);
// //             overlayTilemap = overlayTilemapGO.AddComponent<Tilemap>();
// //             overlayTilemapRenderer = overlayTilemapGO.AddComponent<TilemapRenderer>();
// //             overlayTilemapRenderer.sortingOrder = 1;
// //             overlayTilemapRenderer.material = CreateOverlayMaterial();
// //         }
// //
// //         private Material CreateOverlayMaterial() {
// //             var material = new Material(Shader.Find("Sprites/Default"));
// //             material.color = new Color(1, 1, 1, 0.7f);
// //             return material;
// //         }
// //
// //         protected void CreateDirectVisualization() {
// //             if (generatedWorld == null) return;
// //             
// //             Debug.Log($"[TilemapViewer] Generated ecsWorld with size {WorldSizeChunks.x}x{WorldSizeChunks.y}");
// //             Debug.Log(generatedWorld[0, 0].ToString());;
// //             Debug.Log(generatedWorld[0, 0].customSprite);;
// //
// //             int width = generatedWorld.GetLength(0);
// //             int height = generatedWorld.GetLength(1);
// //
// //             var positions = new List<Vector3Int>();
// //             var baseTiles = new List<TileBase>();
// //             var overlayPositions = new List<Vector3Int>();
// //             var overlayTiles = new List<TileBase>();
// //
// //             for (int x = 0; x < width; x++) {
// //                 for (int y = 0; y < height; y++) {
// //                     var tile = generatedWorld[x, y];
// //                     var position = new Vector3Int(x, y, 0);
// //
// //                     if (ShouldRenderTile(tile, x, y)) {
// //                         var tileAsset = CreateTileFromSprite(tile.tileType);
// //                         positions.AddComponent(position);
// //                         baseTiles.AddComponent(tileAsset);                        
// //
// //                         // var tileAsset = GetTileAsset(tile);
// //                         // if (tileAsset != null) {
// //                         //     positions.AddComponent(position);
// //                         //     baseTiles.AddComponent(tileAsset);
// //                         //
// //                         //     var overlayTile = GetOverlayTileAsset(tile);
// //                         //     if (overlayTile != null) {
// //                         //         overlayPositions.AddComponent(position);
// //                         //         overlayTiles.AddComponent(overlayTile);
// //                         //     }
// //                         // }
// //                     }
// //                 }
// //             }
// //
// //             if (positions.Count > 0) {
// //                 baseTilemap.SetTiles(positions.ToArray(), baseTiles.ToArray());
// //             }
// //
// //             if (overlayPositions.Count > 0) {
// //                 overlayTilemap.SetTiles(overlayPositions.ToArray(), overlayTiles.ToArray());
// //             }
// //
// //             Debug.Log($"[TilemapViewer] Rendered {positions.Count} tiles");
// //         }
// //
// //         protected void CreateChunkedVisualization() {
// //             if (generatedWorld == null) return;
// //
// //             int width = generatedWorld.GetLength(0);
// //             int height = generatedWorld.GetLength(1);
// //
// //             int chunksX = Mathf.CeilToInt((float) width / chunkSize);
// //             int chunksY = Mathf.CeilToInt((float) height / chunkSize);
// //
// //             for (int chunkX = 0; chunkX < chunksX; chunkX++) {
// //                 for (int chunkY = 0; chunkY < chunksY; chunkY++) {
// //                     CreateChunk(new Vector2Int(chunkX, chunkY));
// //                 }
// //             }
// //
// //             Debug.Log($"[TilemapViewer] Created {loadedChunks.Count} chunks");
// //         }
// //
// //         // Include other necessary methods...
// //         private void CreateChunk(Vector2Int chunkCoord) { /* Implementation */
// //         }
// //
// //         private bool ShouldRenderTile(Tile tile, int x, int y) {
// //             return tile.tileType != TileType.Air;
// //         }
// //
// //         private TileBase GetTileAsset(Tile tile) {
// //             return tileAssets.ContainsKey(tile.tileType) ? tileAssets[tile.tileType] : null;
// //         }
// //
// //         private TileBase GetOverlayTileAsset(Tile tile) {
// //             return null;
// //         }
// //
// //         private Color GetTileTypeColor(TileType tileType) {
// //             return Color.gray;
// //         }
// //
// //         private Color GetResourceColor(ResourceType resourceType) {
// //             return Color.magenta;
// //         }
// //
// //         public void RefreshVisualization() {
// //             if (HasGeneratedWorld) GenerateWorldInEditor();
// //         }
// //
// //         public void ClearWorld() { /* Implementation */
// //         }
// //     }
// // }
// // #endif