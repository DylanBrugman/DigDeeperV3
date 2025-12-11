using UnityEngine;

namespace DigDeeper.WorldSystem {
    [CreateAssetMenu(fileName = "Resource Deposit", menuName = "DigDeeper/Resource Deposit")]
    public class ResourceDeposit
    {
        public ResourceType type;
        [Range(0, 100)] public float abundance; // How much resource is in this tile
        [Range(0, 100)] public float quality; // Quality/grade of the resource
        [Range(0, 100)] public float accessibility; // How easy it is to extract
        
        public ResourceDeposit(ResourceType resourceType, float resourceAbundance, float resourceQuality = 50f, float resourceAccessibility = 100f)
        {
            type = resourceType;
            abundance = resourceAbundance;
            quality = resourceQuality;
            accessibility = resourceAccessibility;
        }
    }
}