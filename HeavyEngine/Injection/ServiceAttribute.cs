using System;

namespace HeavyEngine {
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute {
        public Type BaseType { get; }
        public ServiceTypes ServiceType { get; }
        public string Tag { get; }

        public ServiceAttribute(Type baseType, ServiceTypes serviceType, string tag = null) {
            BaseType = baseType;
            ServiceType = serviceType;
            Tag = tag;
        }
    }
}
