namespace HeavyEngine.Injection {
    public sealed class ScopedContainer<T> : IServiceContainer<T> where T : class, new() {
        private T instance;
        private bool injected = false;

        public ScopedContainer() {
            Reset();
        }

        public void Reset() {
            instance = new T();
            injected = false;
        }

        public T Get() => instance;
        public T Get(IDependencyInjector injector) {
            if (!injected) {
                injector.Inject(instance);
                ((IService)instance).Initialize();
                injected = true;
            }

            return instance;
        }
    }
}
