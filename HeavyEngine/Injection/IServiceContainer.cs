namespace HeavyEngine.Injection {
    public interface IServiceContainer<out T> where T : class, new() {
        T Get();
        void Reset();
    }
}
