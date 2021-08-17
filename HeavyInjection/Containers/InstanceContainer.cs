using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine.Injection {
    public class InstanceContainer<T> : IServiceContainer<T> where T : class, new() {
        private readonly T instance;
        private bool injected = false;

        public InstanceContainer(T instance) => this.instance = instance;

        public void Reset() { }

        public T Get() => instance;
        public T Get(IDependencyInjector injector) {
            if (!injected) {
                injector.Inject(instance);
                (instance as IService)?.Initialize();
                injected = true;
            }

            return instance;
        }
    }
}
