using HeavyEngine.Injection;

namespace HeavyEngine {
    public static class DependencyObtainer {
        private static IDependencyInjector injector;

        public static IDependencyInjector PrimaryInjector {
            get {
                if (injector == null)
                    injector = new DependencyInjector();
                return injector;
            }
        }
    }
}
