using System;

namespace Core.ECS {
    /// Immutable, hash-friendly handle.
    public readonly struct EntityId : IEquatable<EntityId>
    {
        public readonly int Value;
        static int _next = 1;

        public EntityId(int v) => Value = v;
        public static EntityId New() => new EntityId(_next++);

        public bool  Equals(EntityId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is EntityId e && e.Equals(this);
        public override int GetHashCode() => Value;
        public override string ToString() => $"E{Value}";
        public static implicit operator bool(EntityId e) => e.Value != 0;
    }
}