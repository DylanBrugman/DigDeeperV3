using Features.World;

namespace Core {
    public class GameContextContainer {
        public WorldGridManager WorldGridManager;
        
        public GameContextContainer() {
            WorldGridManager = new WorldGridManager();
        }
    }
}