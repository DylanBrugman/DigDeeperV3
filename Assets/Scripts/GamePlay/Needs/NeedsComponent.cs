using Core.ECS;
using Core.ECS.Core.ECS;

namespace GamePlay.Needs {
    public struct NeedsComponent : IComponent {
        public Buffer<Need> Needs;
    }
}