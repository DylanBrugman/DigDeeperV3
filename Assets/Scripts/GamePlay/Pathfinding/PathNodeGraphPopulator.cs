// using GamePlay.Map.NativeTileGrid;
// using GamePlay.Pathfinding.Core.Nav;
// using Unity.Collections;
// using Unity.Mathematics;
// using NavBits = GamePlay.Map.NativeTileGrid.NavBits;
//
// namespace GamePlay.Pathfinding {
//     public static class PathNodeGraphPopulator {
//         
//         void BuildChunkGraph(int2 chunkCoord,
//             ref NativeList<PathNode> nodes,
//             ref NativeList<PathEdge> edges,
//             ref NativeHashMap<int2, int> nodeLookup,
//             NavGrid navGrid
//         ) {
//             int baseX = chunkCoord.x * TileChunk.SIZE;
//             int baseY = chunkCoord.y * TileChunk.SIZE;
//
//             for (int lx = 0; lx < TileChunk.SIZE; lx++)
//             for (int ly = 0; ly < TileChunk.SIZE; ly++) {
//                 int2 pos = new int2(baseX + lx, baseY + ly);
//                 byte nav = At(pos);
//
//                 // 1) create node if standable *or* climbable start
//                 if ((nav & NavBits.Standable) != 0)
//                     CreateNode(pos, NodeAction.Walk);
//                 else if ((nav & NavBits.Water) != 0)
//                     CreateNode(pos, NodeAction.Swim);
//             }
//
//             // 2) create connections (4-way + extras)
//             foreach (var kv in nodeLookup) {
//                 int2 p = kv.Key;
//                 int hereIdx = kv.Value;
//                 var hereBits = NavFlags.At(p);
//
//                 // Walk / swim to R,L,U,D if target node exists and traversable
//                 foreach (int2 dir in FourDirs) {
//                     int2 q = p + dir;
//                     if (nodeLookup.TryGetValue(q, out int tgt))
//                         AddEdge(hereIdx, tgt, EdgeAction.Walk, cost: 1);
//                 }
//
//                 // Jumps and ladders
//                 if ((hereBits & NavBits.Climbable) != 0)
//                     TryAddVerticalEdges(p, hereIdx, nodeLookup);
//                 if (CanJumpFrom(hereBits))
//                     TryAddJumpEdges(p, hereIdx, nodeLookup);
//             }
//
//             // ── local helpers ───────────────────
//             void CreateNode(int2 pos, NodeAction a) {
//                 if (nodeLookup.ContainsKey(pos)) return;
//                 nodeLookup[pos] = nodes.Length;
//                 nodes.Add(new PathNode {
//                     Pos = pos, Action = a, ConnStart = 0, ConnCount = 0 // will patch later
//                 });
//             }
//
//             void AddEdge(int from, int to, EdgeAction act, byte cost) {
//                 var idx = edges.Length;
//                 edges.Add(new PathEdge {Target = to, Action = act, Cost = cost});
//                 var n = nodes[from];
//                 if (n.ConnCount == 0) n.ConnStart = (ushort) idx;
//                 n.ConnCount++;
//                 nodes[from] = n;
//             }
//         }
//     }
// }