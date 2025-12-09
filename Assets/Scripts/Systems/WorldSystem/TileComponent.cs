// using System.Collections.Generic;
// using System.Linq;
// using DigDeeper.WorldSystem;
// using Systems.WorldSystem.Generator;
// using UnityEngine;
//
// namespace Systems.WorldSystem {
//     
//     [RequireComponent(typeof(SpriteRenderer))]
//     public class TileComponent : MonoBehaviour
//     {
//         [Header("Tile Reference")]
//         [SerializeField] private Tile tileData;
//         
//         // Visual components
//         private SpriteRenderer spriteRenderer;
//         private BoxCollider2D boxCollider;
//         
//         // Resource indicator
//         [SerializeField] private GameObject resourceIndicatorPrefab;
//         private List<GameObject> resourceIndicators = new List<GameObject>();
//         
//         public Tile TileData => tileData;
//         
//         private void Awake()
//         {
//             spriteRenderer = GetComponent<SpriteRenderer>();
//             boxCollider = GetComponent<BoxCollider2D>();
//             
//             if (boxCollider == null)
//             {
//                 boxCollider = gameObject.AddComponent<BoxCollider2D>();
//             }
//         }
//         
//         public void Initialize(Tile tile)
//         {
//             tileData = tile;
//             UpdateVisuals();
//             CreateResourceIndicators();
//         }
//         
//         private void UpdateVisuals()
//         {
//             if (spriteRenderer != null && tileData != null)
//             {
//                 spriteRenderer.color = tileData.TileColor;
//                 
//                 // AddComponent visual effects based on tile properties
//                 if (tileData.stability < 30f)
//                 {
//                     spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.red, 0.3f);
//                 }
//                 
//                 if (tileData.temperature > 100f)
//                 {
//                     spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.yellow, 0.2f);
//                 }
//             }
//         }
//         
//         private void CreateResourceIndicators()
//         {
//             if (tileData?.resources == null || resourceIndicatorPrefab == null) return;
//             
//             // Clear existing indicators
//             foreach (var indicator in resourceIndicators)
//             {
//                 if (indicator != null) Destroy(indicator);
//             }
//             resourceIndicators.Clear();
//             
//             // Create new indicators for significant resources
//             foreach (var resource in tileData.resources)
//             {
//                 if (resource.abundance > 20f) // Only show significant deposits
//                 {
//                     var indicator = Instantiate(resourceIndicatorPrefab, transform);
//                     indicator.transform.localPosition = Vector3.zero;
//                     
//                     // Set indicator color based on resource type
//                     var indicatorRenderer = indicator.GetComponent<SpriteRenderer>();
//                     if (indicatorRenderer != null)
//                     {
//                         indicatorRenderer.color = GetResourceColor(resource.type);
//                         indicatorRenderer.color = new Color(indicatorRenderer.color.r, indicatorRenderer.color.g, indicatorRenderer.color.b, 0.7f);
//                     }
//                     
//                     resourceIndicators.Add(indicator);
//                     break; // Only show one indicator per tile for clarity
//                 }
//             }
//         }
//         
//         private Color GetResourceColor(ResourceType resourceType)
//         {
//             return resourceType switch
//             {
//                 ResourceType.Coal => Color.black,
//                 ResourceType.Iron => new Color(0.8f, 0.4f, 0.2f),
//                 ResourceType.Copper => new Color(0.8f, 0.5f, 0.2f),
//                 ResourceType.Gold => Color.yellow,
//                 ResourceType.Silver => Color.white,
//                 ResourceType.Diamond => Color.cyan,
//                 ResourceType.Ruby => Color.red,
//                 ResourceType.Emerald => Color.green,
//                 ResourceType.Sapphire => Color.blue,
//                 ResourceType.Uranium => Color.green,
//                 ResourceType.Oil => Color.black,
//                 _ => Color.gray
//             };
//         }
//         
//         private void OnMouseDown()
//         {
//             // Handle tile click for debugging or UI
//             if (tileData != null)
//             {
//                 Debug.Log($"Clicked tile at {tileData.position}: {tileData.tileType}");
//                 Debug.Log($"Resources: {string.Join(", ", tileData.resources.Select(r => $"{r.type}({r.abundance:F1})"))}");
//                 Debug.Log($"Hardness: {tileData.hardness:F1}, Stability: {tileData.stability:F1}");
//             }
//         }
//         
//         public void RefreshVisuals()
//         {
//             UpdateVisuals();
//             CreateResourceIndicators();
//         }
//     }
// }