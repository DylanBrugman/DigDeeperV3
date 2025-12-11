using System;
using System.Collections;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;

namespace GamePlay.World {
    public interface IWorldSource {
        IEnumerator Generate(Action<WorldDTO> onDone, Action<float> onProgress, CancellationToken ct);
    }
}