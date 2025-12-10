using Features.World.Grid;

namespace Features.World {
    public class WorldGridManager {

        private WorldConfig _worldConfig;
        private TileGrid _tileGrid;
        private WorldGridGenerator _worldGridGenerator;

        public void GenerateNew(WorldConfig worldConfig) {
            //Generate grid
            TileGrid tileGrid = _worldGridGenerator.GenerateNewWorld(worldConfig);
            
        }
        
        public TileGrid GetWorldGrid() {
            return _tileGrid;
        }
    }
}