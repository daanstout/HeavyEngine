using System.Reflection;

using HeavyEngine;
using HeavyEngine.Injection;

namespace HeavyUnitTests {
    public class TestInjector : IDependencyInjector {
        public IServiceLibrary Services { get; }

        public TestInjector() {
            Services = new TestServiceLibrary();
        }

        public void Inject(object obj) {
            var type = obj.GetType();

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach(var field in fields) {
                var attrib = field.GetCustomAttribute<DependencyAttribute>();

                if (attrib == null)
                    continue;

                var service = Services.Get(this, field.FieldType, attrib.Tag);

                field.SetValue(obj, service);
            }
        }
    }
}
