using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine.Injection {
    public class SingletonContainer<T> : IServiceContainer<T> where T : class, new() {
        private readonly T instance;
        
        public SingletonContainer() {
            instance = new T();
        }

        public T Get() => instance;
    }
}
