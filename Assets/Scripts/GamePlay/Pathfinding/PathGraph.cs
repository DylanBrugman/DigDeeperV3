using Unity.Collections;

namespace GamePlay.Pathfinding {
    public struct PathGraph
    {
        public NativeArray<PathNode> Nodes;
        public NativeArray<PathEdge> Connections;
    }
}