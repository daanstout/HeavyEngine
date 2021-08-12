using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HeavyEngine.Injection {
    public class DependencyCache {
        public class DependencyCacheList {
            public DependencyCacheItem[] Fields { get; }

            public DependencyCacheList(DependencyCacheItem[] fields) => Fields = fields;
        }

        public class DependencyCacheItem {
            public FieldInfo Field { get; }
            public DependencyAttribute Attribute { get; }

            public DependencyCacheItem(FieldInfo field, DependencyAttribute attribute) {
                Field = field;
                Attribute = attribute;
            }
        }

        private readonly Dictionary<Type, DependencyCacheList> cache;

        public DependencyCache() {
            cache = new Dictionary<Type, DependencyCacheList>();
        }

        public DependencyCacheList GetData(Type type) {
            if (cache.ContainsKey(type))
                return cache[type];

            return null;
        }

        public void AddData(Type type, DependencyCacheList list) {
            cache.Add(type, list);
        }

        public bool ContainsData(Type type) => cache.ContainsKey(type);
    }
}
