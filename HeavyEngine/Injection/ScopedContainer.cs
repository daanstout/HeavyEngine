namespace HeavyEngine.Injection {
    public class ScopedContainer<T> : IServiceContainer<T> where T : class, new() {
        private T instance;

        public ScopedContainer() {
            Reset();
        }

        public void Reset() => instance = new T();

        public T Get() => instance;
    }
}
