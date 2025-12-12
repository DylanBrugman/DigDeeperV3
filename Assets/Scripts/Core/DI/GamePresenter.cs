namespace Core.DI {
    public class GamePresenter
    {
        readonly HelloWorldService helloWorldService;

        public GamePresenter(HelloWorldService helloWorldService)
        {
            this.helloWorldService = helloWorldService;
        }
    }
}