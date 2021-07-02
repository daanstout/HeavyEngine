using System;
using System.Collections.Generic;
using System.Reflection;

namespace HeavyEngine.Injection {
    public class DependencyInjector : IDependencyInjector {
        /// <inheritdoc/>
        public IServiceLibrary Services { get; }
        private readonly DependencyCache dependencyCache;

        /// <summary>
        /// Instantiates a new <see cref="DependencyInjector"/>
        /// </summary>
        public DependencyInjector() {
            Services = new ServiceLibrary();
            dependencyCache = new DependencyCache();
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
                throw new ArgumentNullException(nameof(obj));

            var type = obj.GetType();

            var fields = dependencyCache.GetData(type);

            if(fields == null) {
                fields = GetDependentFields(type);
            }

            foreach(var field in fields.Fields) {
                InjectField(obj, field.Field, field.Attribute);
            }
        }

        protected DependencyCache.DependencyCacheList GetDependentFields(Type type) {
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            var dependentFields = new List<DependencyCache.DependencyCacheItem>();

            foreach(var field in fields) {
                var attrib = field.GetCustomAttribute<DependencyAttribute>();

                if (attrib != null)
                    dependentFields.Add(new DependencyCache.DependencyCacheItem(field, attrib));
            }

            var dependencyCacheList = new DependencyCache.DependencyCacheList(dependentFields.ToArray());

            dependencyCache.AddData(type, dependencyCacheList);

            return dependencyCacheList;
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
