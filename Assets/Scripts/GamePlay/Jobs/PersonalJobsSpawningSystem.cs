using Core.ECS;
using ECSImpl.Systems;
using UnityEngine;

namespace GamePlay.Jobs {
    
    public sealed class PersonalJobsSpawningSystem : IEcsSystem {
        
        public int ProcessedEntitiesCount { get; private set; }
        
        public void Tick(ECSWorld w, float dt)
        {
            ProcessedEntitiesCount = 0;
            
            var q = w.All<ActionNeedTag, PersonalJobsComponent>(); // entities needing action
            while (q.MoveNext())
            {
                // ref var personalJobs = ref q.B;
                //
                // if (personalJobs.Jobs == null) {
                //     personalJobs.Jobs = new List<Job>();
                // }

                // Add job if needed

                ProcessedEntitiesCount++;
            }
        }
        
    }
}