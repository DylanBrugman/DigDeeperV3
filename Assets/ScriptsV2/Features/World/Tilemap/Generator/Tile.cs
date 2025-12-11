// using System;
// using System.Collections.Generic;
// using System.Linq;
// using DigDeeper.WorldSystem;
// using UnityEngine;
//
// namespace Systems.WorldSystem.Generator {
//     [Serializable]
//     public class Tile
//     {
//         // PositionComponent
//         public Vector2Int position;
//         
//         // Basic Properties
//         public TileType tileType;
//         public Matter matter = Matter.Solid;
//         public BiomeType biome;
//         public bool isExplored = false;
//         public bool isAccessible = true;
//         
//         // Physical Properties
//         public float hardness = 50f; // How difficult to dig
//         public float stability = 100f; // Structural integrity
//         public float temperature = 20f;
//         public float pressure = 1f;
//         
//         // Resources (multiple per tile)
//         public List<ResourceDeposit> resources = new List<ResourceDeposit>();
//         
//         public int currentDepth ; //Depth is based on the surface height
//         
//         public bool IsOccupied => matter == Matter.Gas;
//         
//         // Visual Properties
//         private Color _tileColor = default;
//         public Color TileColor {
//             get {
//                 if (_tileColor == default)
//                 {
//                     _tileColor = GetTileColor();
//                 }
//                 return _tileColor;
//             }
//             set
//             {
//                 _tileColor = value;
//             }
//         }
//
//         // Metadata
//         public bool isPointOfInterest = false;
//         public string pointOfInterestId;
//         
//         public Tile(Vector2Int pos, TileType type)
//         {
//             position = pos;
//             tileType = type;
//         }
//         
//         public bool HasResource(ResourceType resourceType)
//         {
//             return resources.Any(r => r.type == resourceType);
//         }
//         
//         public ResourceDeposit GetResource(ResourceType resourceType)
//         {
//             return resources.FirstOrDefault(r => r.type == resourceType);
//         }
//         
//         public void AddResource(ResourceDeposit deposit)
//         {
//             var existing = GetResource(deposit.type);
//             if (existing != null)
//             {
//                 // Combine resources
//                 existing.abundance = Mathf.Min(100f, existing.abundance + deposit.abundance);
//                 existing.quality = (existing.quality + deposit.quality) / 2f;
//             }
//             else
//             {
//                 resources.Add(deposit);
//             }
//         }
//         
//         public float GetTotalResourceValue()
//         {
//             return resources.Sum(r => r.abundance * r.quality * 0.01f);
//         }
//         
//         public bool CanSupport(float weight)
//         {
//             return stability >= weight && tileType != TileType.Air;
//         }
//         
//         private Color GetTileColor() {
//             switch (tileType) {
//                 case TileType.Air:
//                     return Color.clear; // Air is transparent
//                     break;
//                 case TileType.Grass:
//                     return new Color(0.2f, 0.8f, 0.2f); // Grass green
//                     break;
//                 case TileType.Dirt:
//                     return new Color(0.6f, 0.4f, 0.2f); // Dirt brown
//                     break;
//                 case TileType.Clay:
//                     return new Color(0.8f, 0.4f, 0.2f); // Clay reddish-brown
//                     break;
//                 case TileType.Sand:
//                     return new Color(0.9f, 0.8f, 0.6f); // Sand yellowish
//                     break;
//                 case TileType.Stone:
//                     return new Color(0.5f, 0.5f, 0.5f); // Stone gray
//                     break;
//                 case TileType.Limestone:
//                     return new Color(0.8f, 0.8f, 0.6f); // Limestone light gray
//                     break;
//                 case TileType.Granite:
//                     return new Color(0.4f, 0.4f, 0.4f); // Granite dark gray
//                     break;
//                 case TileType.Marble:
//                     return new Color(0.9f, 0.9f, 0.9f); // Marble white
//                     break;
//                 case TileType.Bedrock:
//                     return new Color(0.2f, 0.2f, 0.2f); // Bedrock dark gray
//                     break;
//                 case TileType.Lava:
//                     return new Color(1.0f, 0.2f, 0.0f); // Lava bright red
//                     break;
//                 case TileType.Water:
//                     return new Color(0.0f, 0.5f, 1.0f); // Water blue
//                     break;
//                 case TileType.Ice:
//                     return new Color(0.8f, 0.9f, 1.0f); // Ice light blue
//                     break;
//                 default:
//                     throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
//             }
//         }
//     }
//
//     public enum Matter {
//         Solid,
//         Liquid,
//         Gas,
//         Plasma
//     }
// }