using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine.Injection {
    /// <summary>
    /// <see cref="IDependencyInjector"/> contains services that can be injected into fields marked with <see cref="DependencyAttribute"/>
    /// </summary>
    public interface IDependencyInjector {
        /// <summary>
        /// The library that contains all the services the <see cref="IDependencyInjector"/> will try to inject into objects
        /// </summary>
        IServiceLibrary Services { get; }

        /// <summary>
        /// Injects an object's fields that are marked with <see cref="DependencyAttribute"/> with services
        /// </summary>
        /// <param name="obj">The object to inject</param>
        void Inject(object obj);
    }
}
