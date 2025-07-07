using Core.ECS;

namespace GamePlay.Tool {
    public struct ToolTag : IComponent {
        public string   ToolConfigId;
        public ToolAction ToolAction;
    }
}