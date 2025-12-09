using System.Collections.Generic;
using System.Linq;
using DigDeeper.WorldSystem;
using UnityEngine;

namespace Systems.WorldSystem {
    public class RuntimeWorldConfig : MonoBehaviour
    {
        [Header("Default Configuration")]
        public WorldGenerationConfig _defaultWorldConfig;
        
        [Header("Available Configurations")]
        public List<WorldGenerationConfig> availableConfigs = new List<WorldGenerationConfig>();
        
        [Header("Runtime Settings")]
        public bool loadDefaultOnStart = true;
        public bool allowConfigSwitching = true;
        public string preferredConfigName;
        
        // Current configuration
        private WorldGenerationConfig currentConfig;
        
        // Properties
        public WorldGenerationConfig CurrentConfig => currentConfig;
        public List<WorldGenerationConfig> AvailableConfigs => availableConfigs;
        
        // Events
        public static event System.Action<WorldGenerationConfig> OnConfigChanged;
        
        private void Start()
        {
            if (loadDefaultOnStart)
            {
                LoadDefaultConfiguration();
            }
        }
        
        public void LoadDefaultConfiguration()
        {
            WorldGenerationConfig configToLoad = null;
            
            // Try to load preferred config by name
            if (!string.IsNullOrEmpty(preferredConfigName))
            {
                configToLoad = availableConfigs.FirstOrDefault(c => c.name == preferredConfigName);
            }
            
            // Fall back to default config
            if (configToLoad == null)
            {
                configToLoad = _defaultWorldConfig;
            }
            
            // Fall back to first available config
            if (configToLoad == null && availableConfigs.Count > 0)
            {
                configToLoad = availableConfigs[0];
            }
            
            if (configToLoad != null)
            {
                SetCurrentConfiguration(configToLoad);
            }
            else
            {
                Debug.LogError("No ecsWorld configuration available! Please assign a default config.");
            }
        }
        
        public void SetCurrentConfiguration(WorldGenerationConfig config)
        {
            if (config == null)
            {
                Debug.LogError("Cannot set null ecsWorld configuration");
                return;
            }
            
            currentConfig = config;
            OnConfigChanged?.Invoke(currentConfig);
            
            Debug.Log($"ECSSystem configuration changed to: {config.name}");
            
        }
        
        public void SetConfigurationByName(string configName)
        {
            var config = availableConfigs.FirstOrDefault(c => c.name == configName);
            if (config != null)
            {
                SetCurrentConfiguration(config);
            }
            else
            {
                Debug.LogWarning($"ECSSystem configuration '{configName}' not found");
            }
        }
        
        public void SetConfigurationByIndex(int index)
        {
            if (index >= 0 && index < availableConfigs.Count)
            {
                SetCurrentConfiguration(availableConfigs[index]);
            }
            else
            {
                Debug.LogWarning($"ECSSystem configuration index {index} out of range");
            }
        }
        
        public string[] GetAvailableConfigNames()
        {
            return availableConfigs.Where(c => c != null).Select(c => c.name).ToArray();
        }
        
        public void RefreshAvailableConfigs()
        {
            // Load all ecsWorld configs from Resources
            var configs = Resources.LoadAll<WorldGenerationConfig>("Config/Worlds");
            availableConfigs.Clear();
            availableConfigs.AddRange(configs);
            
            Debug.Log($"Found {availableConfigs.Count} ecsWorld configurations");
        }
    }
}