using System;

namespace GamePlay.Tool {
    [Flags]
    public enum ToolAction : byte {
        None = 0,
        Chop = 1 << 0,
        Mine = 1 << 1,
        Dig  = 1 << 2,
        // …expand as needed
    }
}