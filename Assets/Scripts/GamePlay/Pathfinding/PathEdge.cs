namespace GamePlay.Pathfinding {
    public struct PathEdge
    {
        public int Index;            // index of PathNode in Nodes[]
        public EdgeAction Action;       // what to do to traverse

        public static byte Cost(EdgeAction action) {
            byte cost = 0;
            
            switch (action) {
                case EdgeAction.Walk:          cost = 1; break;
                case EdgeAction.Jump1:         cost = 2; break;
                case EdgeAction.Jump2:         cost = 3; break;
                case EdgeAction.Swim:          cost = 4; break;
                case EdgeAction.ClimbLadder:   cost = 4; break;
                case EdgeAction.ClimbRope:     cost = 5; break;
            }

            return cost;
        }
    }
}