using System;
using System.Reflection;

namespace HeavyEngine.Injection {
    public class DependencyInjector : IDependencyInjector {
        public IServiceLibrary Services { get; }

        internal DependencyInjector() {
            Services = new ServiceLibrary();
        }

        public void Inject(object obj) {
            if (obj == null)
                throw new ArgumentNullException(obj.GetType().Name);

            var type = obj.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
                Inject(obj, field, field.GetCustomAttribute<DependencyAttribute>());
        }

        private void Inject(object obj, FieldInfo field, DependencyAttribute attribute) {
            if (attribute == null)
                return;

            if (field.GetValue(obj) != null)
                return;

            var service = Services.Get(field.FieldType, attribute.Tag);

            Inject(service);

            field.SetValue(obj, service);
        }
    }
}
