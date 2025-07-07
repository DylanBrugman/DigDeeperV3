using Core.ECS;
using Core.ECS.Core.ECS;

namespace GamePlay.Skills {
    public struct SkillsComponent : IComponent {
        public Buffer<Skill> Skills;
    }
}