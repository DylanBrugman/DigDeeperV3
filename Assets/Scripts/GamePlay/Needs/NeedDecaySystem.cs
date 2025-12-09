using Core.ECS;
using ECSImpl.Systems;
using Unity.Mathematics;

namespace GamePlay.Needs {
    public sealed class NeedDecaySystem : IEcsSystem
    {
        
        public int ProcessedEntitiesCount { get; private set; }
        public void Tick(ECSWorld ecsWorld, float dt)
        {
            ProcessedEntitiesCount = 0;
            
            var it = ecsWorld.All<Need>();
            while (it.MoveNext())
            {
                ref var need = ref it.Component;

                need.Value = math.max(0f, need.Value - need.DecayRatePerMinute * dt);

                NeedLevel newNeedLevel =
                    need.Value == 0f                         ? NeedLevel.Empty    :
                    need.Value <= need.CriticalThreshold     ? NeedLevel.Critical :
                    need.Value <= need.ActionThreshold       ? NeedLevel.Action   :
                    NeedLevel.Normal;

                if (newNeedLevel != need.Level)
                {
                    need.Level = newNeedLevel;                     // write back
                    // optional: add/remove tag components for one-frame reactions
                    UpdateTags(ecsWorld, it.EntityId, newNeedLevel);
                }
                
                ProcessedEntitiesCount++;
            }
        }

        static void UpdateTags(ECSWorld ecsWorld, EntityId entityId, NeedLevel needLevel)
        {
            ecsWorld.Remove<ActionNeedTag>(entityId);
            ecsWorld.Remove<CriticalNeedTag>(entityId);
            ecsWorld.Remove<EmptyNeedTag>(entityId);

            switch (needLevel)
            {
                case NeedLevel.Action:    ecsWorld.Add(entityId, new ActionNeedTag());    break;
                case NeedLevel.Critical:  ecsWorld.Add(entityId, new CriticalNeedTag());  break;
                case NeedLevel.Empty:     ecsWorld.Add(entityId, new EmptyNeedTag());     break;
            }
        }
    }
}