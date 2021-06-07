using System;
using System.Reflection;

namespace HeavyEngine.Injection {
    public class DependencyInjector : IDependencyInjector {
        /// <inheritdoc/>
        public IServiceLibrary Services { get; }

        /// <summary>
        /// Instantiates a new <see cref="DependencyInjector"/>
        /// </summary>
        public DependencyInjector() {
            Services = new ServiceLibrary();
        }

        /// <inheritdoc/>
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

            var service = Services.Get(this, field.FieldType, attribute.Tag);

            field.SetValue(obj, service);
        }
    }
}
