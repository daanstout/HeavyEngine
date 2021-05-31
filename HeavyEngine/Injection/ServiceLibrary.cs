using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HeavyEngine.Injection {
    public class ServiceLibrary : IServiceLibrary {
        private struct Binding : IEquatable<Binding> {
            public bool HasBinding { get; set; }
            public string Tag { get; set; }
            public string Target { get; set; }

            public override bool Equals(object obj) => obj is Binding bindings && Equals(bindings);
            public bool Equals(Binding other) => Tag == other.Tag;
            public override int GetHashCode() => HashCode.Combine(Tag);

            public static bool operator ==(Binding left, Binding right) => left.Equals(right);
            public static bool operator !=(Binding left, Binding right) => !(left == right);
        }

        private readonly Dictionary<ServiceIdentifier, IServiceContainer<object>> services;
        private readonly HashSet<Binding> bindings;

        internal ServiceLibrary() {
            services = new Dictionary<ServiceIdentifier, IServiceContainer<object>>();
            bindings = new HashSet<Binding>();
        }

        public TAbstract Get<TAbstract>(string tag = null) {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            return (TAbstract)Get(identifier);
        }

        public object Get(Type type, string tag = null) {
            var identifier = new ServiceIdentifier {
                Type = type,
                Tag = tag
            };

            return Get(identifier);
        }

        public void AddTransient<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            Add(identifier, new TransientContainer<TImplementation>());
        }

        public void AddSingleton<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new() {
            var identifier = new ServiceIdentifier {
                Type = typeof(TAbstract),
                Tag = tag
            };

            Add(identifier, new SingletonContainer<TImplementation>());
        }

        public bool BindTag(string tag, string target) {
            var binding = new Binding {
                Tag = tag,
                Target = target,
                HasBinding = true
            };

            return bindings.Add(binding);
        }

        public void FindServices(Assembly assembly) {
            foreach(var type in assembly.GetTypes()) {
                if (type.IsInterface || type.IsAbstract)
                    continue;

                if (typeof(IService).IsAssignableFrom(type)) {
                    RegisterServicesForType(type);
                }
            }
        }

        private void RegisterServicesForType(Type type) {
            var attrib = type.GetCustomAttribute<ServiceAttribute>();

            if (attrib == null)
                throw new ArgumentException($"The given argument of type {type} does not have a Service Attribute applied to it");

            var identifier = new ServiceIdentifier {
                Type = attrib.BaseType,
                Tag = attrib.Tag
            };

            IServiceContainer<object> container = null;

            if(attrib.ServiceType == ServiceTypes.Singleton) {
                var containerType = typeof(SingletonContainer<>);
                containerType = containerType.MakeGenericType(type);
                container = (IServiceContainer<object>)Activator.CreateInstance(containerType);
            }else if(attrib.ServiceType == ServiceTypes.Transient) {
                var containerType = typeof(TransientContainer<>);
                containerType.MakeGenericType(type);
                container = (IServiceContainer<object>)Activator.CreateInstance(containerType);
            }else if(attrib.ServiceType == ServiceTypes.Scoped) {

            }

            if(container == null) 
                throw new ArgumentException($"The given argument of type {type} does not have a valid setup");

            services.Add(identifier, container);
        }

        private void Add(ServiceIdentifier identifier, IServiceContainer<object> container) {
            if (services.ContainsKey(identifier))
                throw new ArgumentException();

            services.Add(identifier, container);
        }

        private object Get(ServiceIdentifier identifier) {
            if (services.ContainsKey(identifier))
                return services[identifier].Get();

            var binding = bindings.FirstOrDefault(b => b.Tag == identifier.Tag);

            if (!binding.HasBinding)
                throw new ArgumentException($"Service for type: {identifier.Type} with tag: {identifier.Tag} has not been registered yet and no binding has been registered");

            identifier.Tag = binding.Target;

            if (services.ContainsKey(identifier))
                return services[identifier].Get();

            throw new ArgumentException($"Service for type: {identifier.Type} with binding tag: {identifier.Tag} (original: {binding.Tag}) has not been registered");
        }
    }
}
