using DigDeeper.WorldSystem;
using Systems.WorldSystem;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;

namespace Editor.DigDeeper.WorldSystem {
    public static class WorldConfigMenuItems
    {
        [MenuItem("DigDeeper/Create ECSSystem Configs/Earth-Like ECSSystem")]
        public static void CreateEarthLikeWorld()
        {
            var config = WorldConfigPresets.CreateEarthLikeWorld();
            CreateConfigAsset(config, "EarthLikeWorld");
        }
        
        [MenuItem("DigDeeper/Create ECSSystem Configs/Alien ECSSystem")]
        public static void CreateAlienWorld()
        {
            var config = WorldConfigPresets.CreateAlienWorld();
            CreateConfigAsset(config, "AlienWorld");
        }
        
        [MenuItem("DigDeeper/Create ECSSystem Configs/Test ECSSystem")]
        public static void CreateTestWorld()
        {
            var config = WorldConfigPresets.CreateTestWorld();
            CreateConfigAsset(config, "TestWorld");
        }
        
        private static void CreateConfigAsset(WorldGenerationConfig config, string fileName)
        {
            var folderPath = "Assets/Resources/Config/Worlds";
            
            // Create folder if it doesn't exist
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder("Assets/Resources/Config", "Worlds");
            }
            
            var assetPath = $"{folderPath}/{fileName}.asset";
            AssetDatabase.CreateAsset(config, assetPath);
            AssetDatabase.SaveAssets();
            
            Debug.Log($"Created ecsWorld config: {assetPath}");
            Selection.activeObject = config;
        }
        
        [MenuItem("DigDeeper/Setup/Create Runtime ECSSystem Config")]
        public static void CreateRuntimeWorldConfig()
        {
            var go = new GameObject("RuntimeWorldConfig");
            go.AddComponent<RuntimeWorldConfig>();
            
            // Try to find available configs
            var configs = Resources.LoadAll<WorldGenerationConfig>("Config/Worlds");
            var runtimeConfig = go.GetComponent<RuntimeWorldConfig>();
            
            if (configs.Length > 0)
            {
                runtimeConfig._defaultWorldConfig = configs[0];
                runtimeConfig.availableConfigs.AddRange(configs);
            }
            
            Selection.activeGameObject = go;
            Debug.Log("Created RuntimeWorldConfig GameObject");
        }
    }
}