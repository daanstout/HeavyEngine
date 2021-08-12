namespace HeavyEngine.Injection {
    public sealed class SingletonContainer<T> : IServiceContainer<T> where T : class, new() {
        private readonly T instance;
        private bool injected = false;

        public SingletonContainer() {
            instance = new T();
        }

        public T Get(IDependencyInjector injector) {
            if (!injected) {
                injector.Inject(instance);
                (instance as IService)?.Initialize();
                injected = true;
            }

            return instance;
        }
        public T Get() => instance;

        public void Reset() { }
    }
}
