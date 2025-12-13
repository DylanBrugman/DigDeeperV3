using System.Threading.Tasks;
using Features.World.Model;

namespace Features.World.System {
    public interface IWorldGenerationStep
    {
        string StepName { get; }
        Task GenerateStep(WorldGenerationContext worldGenerationContext);
    }
}