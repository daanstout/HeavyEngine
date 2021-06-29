using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HeavyEngine.Injection {
    public class ServiceLibrary : IServiceLibrary {
        protected class Binding : IEquatable<Binding> {
            public Type BaseType { get; set; }
            public string Tag { get; set; }
            public string Target { get; set; }

            public override bool Equals(object obj) => Equals(obj as Binding);
            public bool Equals(Binding other) => other != null && EqualityComparer<Type>.Default.Equals(BaseType, other.BaseType) && Tag == other.Tag && Target == other.Target;
            public override int GetHashCode() => HashCode.Combine(BaseType, Tag, Target);

            public static bool operator ==(Binding left, Binding right) => EqualityComparer<Binding>.Default.Equals(left, right);
            public static bool operator !=(Binding left, Binding right) => !(left == right);
        }

        protected readonly Dictionary<ServiceIdentifier, IServiceContainer<object>> services;
        protected readonly List<IServiceContainer<object>> scopedServices;
        protected readonly HashSet<Binding> bindings;

        protected IEventService eventService;

        public ServiceLibrary() {
            services = new Dictionary<ServiceIdentifier, IServiceContainer<object>>();
            bindings = new HashSet<Binding>();
        }

        ~ServiceLibrary() {
            eventService.Unsubscribe<SceneChangedEvent>(OnSceneChanged);
        }

        public TAbstract Get<TAbstract>(string tag = null) {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            return (TAbstract)Get(identifier);
        }

        public TAbstract Get<TAbstract>(IDependencyInjector injector, string tag = null) {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            return (TAbstract)Get(injector, identifier);
        }

        public object Get(Type type, string tag = null) {
            var identifier = new ServiceIdentifier {
                Type = type,
                Tag = tag
            };

            return Get(identifier);
        }

        public object Get(IDependencyInjector injector, Type type, string tag = null) {
            var identifier = new ServiceIdentifier {
                Type = type,
                Tag = tag
            };

            return Get(injector, identifier);
        }

        public void AddTransient<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            Add(identifier, new TransientContainer<TImplementation>());
        }

        public void AddScoped<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            var container = new ScopedContainer<TImplementation>();
            Add(identifier, container);
            scopedServices.Add(container);
        }

        public void AddSingleton<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            Add(identifier, new SingletonContainer<TImplementation>());
        }

        public bool BindTag<TAbstract>(string tag, string target) {
            var binding = new Binding {
                BaseType = typeof(TAbstract),
                Tag = tag,
                Target = target
            };

            return bindings.Add(binding);
        }

        public bool UnbindTag<TAbstract>(string tag, string target) => bindings.RemoveWhere(binding => binding.BaseType == typeof(TAbstract) && binding.Tag == tag && binding.Target == target) > 0;

        public void FindServices(Assembly assembly) {
            foreach (var type in assembly.GetTypes()) {
                if (type.IsInterface || type.IsAbstract)
                    continue;

                if (typeof(IService).IsAssignableFrom(type)) {
                    RegisterServicesForType(type);
                }
            }
        }

        public void SetupSelf() {
            eventService = Get<IEventService>();

            eventService.Subscribe<SceneChangedEvent>(OnSceneChanged);
        }

        public void OverrideSingleton<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            OverrideService(identifier, new SingletonContainer<TImplementation>());
        }
        
        public void OverrideScoped<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            OverrideService(identifier, new ScopedContainer<TImplementation>());
        }

        public void OverrideTransient<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            OverrideService(identifier, new TransientContainer<TImplementation>());
        }

        protected void RegisterServicesForType(Type type) {
            var attrib = type.GetCustomAttribute<ServiceAttribute>();

            if (attrib == null)
                throw new ArgumentException($"The given argument of type {type} does not have a Service Attribute applied to it");

            var identifier = new ServiceIdentifier {
                Type = attrib.BaseType,
                Tag = attrib.Tag
            };

            IServiceContainer<object> container = null;

            if (attrib.ServiceType == ServiceTypes.Singleton) {
                var containerType = typeof(SingletonContainer<>);
                containerType = containerType.MakeGenericType(type);
                container = (IServiceContainer<object>)Activator.CreateInstance(containerType);
            } else if (attrib.ServiceType == ServiceTypes.Transient) {
                var containerType = typeof(TransientContainer<>);
                containerType.MakeGenericType(type);
                container = (IServiceContainer<object>)Activator.CreateInstance(containerType);
            } else if (attrib.ServiceType == ServiceTypes.Scoped) {
                var containerType = typeof(ScopedContainer<>);
                containerType.MakeGenericType(type);
                container = (IServiceContainer<object>)Activator.CreateInstance(containerType);
                scopedServices.Add(container);
            }

            if (container == null)
                throw new ArgumentException($"The given argument of type {type} does not have a valid setup");

            services.Add(identifier, container);
        }

        protected void Add(ServiceIdentifier identifier, IServiceContainer<object> container) {
            if (services.ContainsKey(identifier))
                throw new ArgumentException();

            services.Add(identifier, container);
        }

        protected object Get(ServiceIdentifier identifier) {
            if (services.ContainsKey(identifier))
                return services[identifier].Get();

            var originalTag = identifier.Tag;

            while (GetBoundIdentifier(identifier)) {
                if (services.ContainsKey(identifier))
                    return services[identifier].Get();
            }

            throw new ArgumentException($"Service for type: {identifier.Type} with binding tag: {identifier.Tag} (original: {originalTag}) has not been registered");
        }

        protected object Get(IDependencyInjector injector, ServiceIdentifier identifier) {
            if (services.ContainsKey(identifier))
                return services[identifier].Get(injector);

            var originalTag = identifier.Tag;

            while (GetBoundIdentifier(identifier)) {
                if (services.ContainsKey(identifier))
                    return services[identifier].Get(injector);
            }

            throw new ArgumentException($"Service for type: {identifier.Type} with binding tag: {identifier.Tag} (original: {originalTag}) has not been registered");
        }

        protected void OverrideService(ServiceIdentifier identifier, IServiceContainer<object> container) {
            services[identifier] = container;
        }

        protected bool GetBoundIdentifier(ServiceIdentifier identifier) {
            var binding = bindings.FirstOrDefault(b => b.Tag == identifier.Tag && b.BaseType == identifier.Type);

            if (binding == null)
                return false;

            identifier.Tag = binding.Target;

            return true;
        }

        protected void OnSceneChanged() {
            foreach (var container in scopedServices)
                container.Reset();
        }
    }
}
