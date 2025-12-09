using System.ComponentModel;

namespace Systems.JobSystem.Requirements {
    public interface IRequirement<T>
    {
        bool IsMet(T obj);
    
        string GetFailDescription();
    }
}