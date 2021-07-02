namespace HeavyEngine.Injection {
    public sealed class TransientContainer<T> : IServiceContainer<T> where T : class, new() {
        public T Get() => new T();
        public T Get(IDependencyInjector injector) {
            var instance = new T();

            injector.Inject(instance);

            (instance as IService)?.Initialize();

            return instance;
        }
        public void Reset() { }
    }
}
