namespace HeavyEngine.Injection {
    public class TransientContainer<T> : IServiceContainer<T> where T : class, new() {
        public T Get() => new T();
        public void Reset() { }
    }
}
