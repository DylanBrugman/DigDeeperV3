// using UnityEngine;
//
// namespace Systems.WorldSystem.DigDeeper.WorldSystem {
//     public class NoiseGenerator {
//         private int seed;
//         private float scale;
//
//         public NoiseGenerator(int noiseSeed, float noiseScale) {
//             seed = noiseSeed;
//             scale = noiseScale;
//         }
//
//         public float GetNoise(float x, float y) {
//             return Mathf.PerlinNoise(x * scale + seed * 0.01f, y * scale + seed * 0.01f) * 2f - 1f;
//         }
//
//         public float GetNoise(int x, int y) {
//             return GetNoise((float) x, (float) y);
//         }
//     }
// }