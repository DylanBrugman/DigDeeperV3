using System;
using System.Collections;

namespace Features.World.System {
    public interface IWorldSource {
        IEnumerator Generate(Action<GamePlay.World.World> onDone);
    }
}