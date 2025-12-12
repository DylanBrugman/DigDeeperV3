
using VContainer;
using VContainer.Unity;

namespace Core.DI {
    public class GameLifetimeScope : LifetimeScope {
        protected override void Configure(IContainerBuilder builder) {
            builder.Register<HelloWorldService>(VContainer.Lifetime.Singleton);
            builder.Register<GamePresenter>(VContainer.Lifetime.Singleton);
        }
    }
}