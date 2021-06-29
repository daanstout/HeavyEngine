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

        /// <summary>
        /// Instantiates a new <see cref="DependencyInjector"/> with a provided <see cref="IServiceLibrary"/>
        /// </summary>
        /// <param name="serviceLibrary">The <see cref="IServiceLibrary"/> that the injector should use</param>
        public DependencyInjector(IServiceLibrary serviceLibrary) {
            Services = serviceLibrary;
        }

        /// <inheritdoc/>
        public void Inject(object obj) {
            if (obj == null)
                throw new ArgumentNullException(obj.GetType().Name);

            var type = obj.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var field in fields)
                InjectField(obj, field, field.GetCustomAttribute<DependencyAttribute>());
        }

        protected void InjectField(object obj, FieldInfo field, DependencyAttribute attribute) {
            if (attribute == null)
                return;

            if (field.GetValue(obj) != null)
                return;

            var service = Services.Get(this, field.FieldType, attribute.Tag);

            field.SetValue(obj, service);
        }
    }
}
