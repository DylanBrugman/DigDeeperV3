namespace GamePlay.Pathfinding.Nav {
    [System.Flags]
    public enum NavBits : byte
    {
        None       = 0,
        Standable  = 1 << 0,   // solid floor & 1‑tile air above
        Climbable  = 1 << 1,   // ladder / rope present in THIS tile
        Water      = 1 << 2,   // liquid ≥ threshold
        Hazard     = 1 << 3    // fire, spikes, etc.
    }
}