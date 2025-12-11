using System.Collections.Generic;
using UnityEngine;

namespace Systems.WorldSystem.Generator {
    public static class VoronoiGenerator
    {
        public static List<Vector2> GenerateVoronoiPoints(int numPoints, Vector2 bounds, int seed) {
            var random = new System.Random(seed);
            var points = new List<Vector2>();
            for(int i=0; i<numPoints; ++i) {
                points.Add(new Vector2(random.Next(0, (int)bounds.x), random.Next(0, (int)bounds.y)));
            }
            return points;
        }
        public static float GetVoronoiNoise(Vector2 position, List<Vector2> points, float scaleFactor) {
            if (points == null || points.Count == 0) return 0.5f;
            float minSqDist = float.MaxValue;
            foreach (var p in points) {
                minSqDist = Mathf.Min(minSqDist, Vector2.SqrMagnitude(position - p));
            }
            // This is a placeholder. Real Voronoi noise is more complex.
            // A simple normalized distance might be: 1.0f - Mathf.Sqrt(minSqDist) / scaleFactor;
            // The original example used Mathf.MaxVelocity(enhancedConfig.worldSize.x, enhancedConfig.worldSize.y) * 0.3f for scale
            // This InitialValue needs to be meaningful in context.
            return Mathf.Clamp01(1.0f - (Mathf.Sqrt(minSqDist) / (scaleFactor * 0.3f))); 
        }
    }
}