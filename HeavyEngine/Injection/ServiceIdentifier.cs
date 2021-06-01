using System;
using System.Collections.Generic;

namespace HeavyEngine.Injection {
    internal struct ServiceIdentifier : IEquatable<ServiceIdentifier> {
        public Type Type { get; set; }
        public string Tag { get; set; }

        public override bool Equals(object obj) => obj is ServiceIdentifier identifier && Equals(identifier);
        public bool Equals(ServiceIdentifier other) => EqualityComparer<Type>.Default.Equals(Type, other.Type) && Tag == other.Tag;
        public override int GetHashCode() => HashCode.Combine(Type, Tag);

        public static bool operator ==(ServiceIdentifier left, ServiceIdentifier right) => left.Equals(right);
        public static bool operator !=(ServiceIdentifier left, ServiceIdentifier right) => !(left == right);
    }
}
