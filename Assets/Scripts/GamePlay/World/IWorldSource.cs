using System;
using System.Collections;
using System.Threading;
using Core.ECS;
using GamePlay.Map.Generator.New.Core.WorldGen;
using Unity.Mathematics;
using UnityEngine;

namespace GamePlay.World {

    public interface IWorldSource
    {
        IEnumerator LoadCoroutine(Action<WorldData> onDone, Action<float> onProgress, CancellationToken ct);
    }
}