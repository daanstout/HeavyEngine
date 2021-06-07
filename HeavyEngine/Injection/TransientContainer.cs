﻿namespace HeavyEngine.Injection {
    public class TransientContainer<T> : IServiceContainer<T> where T : class, new() {
        public T Get() => new T();
        public T Get(IDependencyInjector injector) {
            var instance = new T();

            injector.Inject(instance);

            ((IService)instance).Initialize();

            return instance;
        }
        public void Reset() { }
    }
}
