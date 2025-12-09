using System.Collections.Generic;
using Core.ECS;
using Core.ECS.Core.ECS;

namespace GamePlay.Jobs {
    public struct PersonalJobsComponent : IComponent {
        public Buffer<JobData> Jobs;
    }
}