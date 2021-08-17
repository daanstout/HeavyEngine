using System;
using System.Collections.Generic;
using System.Reflection;

using HeavyEngine.Injection;

namespace HeavyUnitTests {
    public class TestServiceLibrary : IServiceLibrary {
        protected readonly Dictionary<ServiceIdentifier, IServiceContainer<object>> services = new Dictionary<ServiceIdentifier, IServiceContainer<object>>();
        
        public void AddInstance<TAbstract, TImplementation>(TImplementation implementation, string tag = null) where TImplementation : class, new() {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag,
            };

            services.Add(identifier, new InstanceContainer<TImplementation>(implementation));
        }
        public object Get(IDependencyInjector injector, Type type, string tag = null) {
            var identifier = new ServiceIdentifier {
                Type = type,
                Tag = tag
            };

            if (services.ContainsKey(identifier))
                return services[identifier].Get(injector);

            return null;
        }

        #region Unneeded Functions for Testing
        public void AddScoped<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() => throw new NotImplementedException();
        public void AddSingleton<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() => throw new NotImplementedException();
        public void AddTransient<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() => throw new NotImplementedException();
        public bool BindTag<TAbstract>(string tag, string target) => throw new NotImplementedException();
        public void FindServices(Assembly assembly) => throw new NotImplementedException();
        public void FindServices(IEnumerable<Assembly> assemblies) => throw new NotImplementedException();
        public TAbstract Get<TAbstract>(string tag = null) => throw new NotImplementedException();
        public TAbstract Get<TAbstract>(IDependencyInjector injector, string tag = null) => throw new NotImplementedException();
        public object Get(Type type, string tag = null) => throw new NotImplementedException();
        public void OverrideScoped<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() => throw new NotImplementedException();
        public void OverrideSingleton<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() => throw new NotImplementedException();
        public void OverrideTransient<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() => throw new NotImplementedException();
        public bool UnbindTag<TAbstract>(string tag, string target) => throw new NotImplementedException();
        #endregion
    }
}
