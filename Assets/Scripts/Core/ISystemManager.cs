namespace Core {
    public interface ISystemManager
    {
        void Initialize();
        void UpdateSystem();
        void CleanupSystem();
        bool IsInitialized { get; }
    }
}