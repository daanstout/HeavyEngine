using System;
using System.Collections.Generic;

using HeavyEngine.Injection;

namespace HeavyEngine {
    public static class DependencyObtainer {
        private static readonly List<IDependencyInjector> injectors = new List<IDependencyInjector>();
        private static int primaryInjector = -1;

        public static IDependencyInjector PrimaryInjector => injectors[primaryInjector];

        static DependencyObtainer() {
            RegisterInjector(new DependencyInjector());
            SetInjectorAsPrimary(injectors[0]);
        }

        public static void RegisterInjector(IDependencyInjector injector) {
            if (injector == null)
                throw new ArgumentNullException($"Injector is null!");

            if (!injectors.Contains(injector))
                injectors.Add(injector);
        }

        public static void SetInjectorAsPrimary(IDependencyInjector injector) {
            var index = injectors.IndexOf(injector);

            if (index != -1)
                primaryInjector = index;
        }
    }
}
