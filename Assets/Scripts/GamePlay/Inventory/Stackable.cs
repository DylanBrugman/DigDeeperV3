using Core.ECS;

namespace GamePlay.Inventory {
    public class Stackable : IComponent {
        public byte Count { get; set; }
    }
}