namespace HeavyEngine.Injection {
    public interface IServiceContainer<out T> where T : class, new() {
        T Get();
        T Get(IDependencyInjector injector);
        void Reset();
    }
}
