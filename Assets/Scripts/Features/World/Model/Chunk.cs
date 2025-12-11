using System;

namespace GamePlay.World.Tilemap {
    public sealed class Chunk<T> where T : struct
    {
        public static readonly T EmptyCell = default;   // ref return for “not found”

        public readonly T[] Cells;                      // length == 1024
        public bool Dirty;
        private int _nonZero;                           // optional reclaim counter

        public Chunk() => Cells = new T[Chunk.SIZE * Chunk.SIZE];

        public ref T At(int lx, int ly) => ref Cells[lx + ly * Chunk.SIZE];

        /* --- optional memory reclaim -------------------------------------- */
        public void Set(int lx, int ly, T value, Func<T, bool> isZero = null)
        {
            ref T cell = ref At(lx, ly);
            if (isZero != null)
            {
                if (isZero(cell) && !isZero(value)) _nonZero++;
                if (!isZero(cell) && isZero(value)) _nonZero--;
            }
            cell = value;
            Dirty = true;
        }
        public bool IsEmpty(Func<T, bool> isZero) => _nonZero == 0 && isZero != null;
    }

    public static class Chunk {
        public static readonly int SIZE = 32;                     // 32×32 cells
    }
}