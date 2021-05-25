using System;

namespace HeavyEngine.Injection {
    /// <summary>
    /// An attribute that can be used to indicate a field should be injected by the <see cref="IDependencyInjector"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DependencyAttribute : Attribute {
        /// <summary>
        /// A tag that can be given to the dependency to get a certain instance of the dependency, instead of the default instance
        /// <para>If this value is <see langword="null"/>, the default instance will be used</para>
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// Instantiates a <see cref="DependencyAttribute"/> on a field to indicate this value should be injected by the <see cref="IDependencyInjector"/>
        /// </summary>
        /// <param name="tag">Optional - Setting this value indicates that a specific instance should be used instead of the default instance</param>
        public DependencyAttribute(string tag = null) => Tag = tag;
    }
}
