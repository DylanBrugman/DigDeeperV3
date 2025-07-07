using Unity.Mathematics;

namespace GamePlay.Pathfinding {
    public struct PathNode
    {
        public int2 Pos;          // centre of the standable tile
        public NodeAction Action; // what the pawn must do while *on* this node
        public ushort ConnStart;  // index into global Connections array
        public ushort ConnCount;  // how many connections start here
    }
}