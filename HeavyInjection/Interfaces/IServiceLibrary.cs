using System;
using System.Collections.Generic;
using System.Reflection;

namespace HeavyEngine.Injection {
    /// <summary>
    /// A <see cref="ServiceLibrary"/> is a library of every service that can be retrieved through it.
    /// By implementing this, you can keep track of every service needed and obtain them when needed.
    /// </summary>
    public interface IServiceLibrary {
        /// <summary>
        /// Adds a service as a singleton service, meaning throughout the runtime, there is only 1 instance.
        /// </summary>
        /// <typeparam name="TAbstract">The abstract type, which is the type requested by dependees of the service.</typeparam>
        /// <typeparam name="TImplementation">The implementation type of the abstract type.
        /// <para>This can be the same type as the abstract type.</para></typeparam>
        /// <param name="tag">A tag that can be used to differentiate between different versions of the same abstract type. Defaults to <see langword="null"/></param>
        void AddSingleton<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new();
        /// <summary>
        /// Adds a service as a scoped service, meaning within scenes, the instance stays the same, but it is reset when the scene changes.
        /// </summary>
        /// <typeparam name="TAbstract">The abstract type, which is the type requested by dependees of the service.</typeparam>
        /// <typeparam name="TImplementation">The implementation type of the abstract type.
        /// <para>This can be the same type as the abstract type.</para></typeparam>
        /// <param name="tag">A tag that can be used to differentiate between different versions of the same abstract type. Defaults to <see langword="null"/></param>
        void AddScoped<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new();
        /// <summary>
        /// Adds a service as a transient service, meaning everytime it is requested, a new instance will be generated.
        /// </summary>
        /// <typeparam name="TAbstract">The abstract type, which is the type requested by dependees of the service.</typeparam>
        /// <typeparam name="TImplementation">The implementation type of the abstract type.
        /// <para>This can be the same type as the abstract type.</para></typeparam>
        /// <param name="tag">A tag that can be used to differentiate between different versions of the same abstract type. Defaults to <see langword="null"/></param>
        void AddTransient<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new();
        /// <summary>
        /// Binds two tags together, meaning that if a service with the first tag is requested, but not present,
        /// the <see cref="IServiceLibrary"/> will look for the target tag instead
        /// </summary>
        /// <typeparam name="TAbstract">The base type that is requested as dependency</typeparam>
        /// <param name="tag">The first tag that the dependee will request</param>
        /// <param name="target">The tag that should be looked at if the first tag isn't present</param>
        /// <returns>True if the binding was succesfully added</returns>
        bool BindTag<TAbstract>(string tag, string target);
        /// <summary>
        /// Unbinds two bound tags
        /// </summary>
        /// <typeparam name="TAbstract">The base type that is requested as dependency</typeparam>
        /// <param name="tag">The first tag that the dependpee will request</param>
        /// <param name="target">The target tag that no longer needs to be bound</param>
        /// <returns>True if the binding was succesfully removed</returns>
        bool UnbindTag<TAbstract>(string tag, string target);
        /// <summary>
        /// Gets a service of the given type, potentially with the given tag.
        /// </summary>
        /// <typeparam name="TAbstract">The type of service requested.</typeparam>
        /// <param name="tag">The tag used to potentially specify which service exactly, if multiple of the same type exist.</param>
        /// <returns>The service requested.</returns>
        TAbstract Get<TAbstract>(string tag = null);
        /// <summary>
        /// Gets a service of the given type, potentially with the given tag, and injects it with an <see cref="IDependencyInjector"/>.
        /// </summary>
        /// <typeparam name="TAbstract">The type of <see cref="IService"/> to get</typeparam>
        /// <param name="injector">The <see cref="IDependencyInjector"/> to inject with</param>
        /// <param name="tag">The tag to use to differentiate between different services with the same base type</param>
        /// <returns>The service requested</returns>
        TAbstract Get<TAbstract>(IDependencyInjector injector, string tag = null);
        /// <summary>
        /// Gets a service of the given type, potentially with the given tag.
        /// </summary>
        /// <param name="type">The type of service requested.</param>
        /// <param name="tag">The tag used to potentially specify which service exactly, if multiple of the same type exist.</param>
        /// <returns>The service requested.</returns>
        object Get(Type type, string tag = null);
        /// <summary>
        /// Gets a service of the given type, potentially with the given tag, and injects it with an <see cref="IDependencyInjector"/>.
        /// </summary>
        /// <param name="injector">The <see cref="IDependencyInjector"/> to inject with</param>
        /// <param name="type">The type of <see cref="IService"/> to get</param>
        /// <param name="tag">The tag to use to differentiate between different services with the same base type</param>
        object Get(IDependencyInjector injector, Type type, string tag = null);
        /// <summary>
        /// Finds all services that implement <see cref="IService"/> in the given assembly.
        /// <para>The services need to have the <see cref="ServiceAttribute"/>.</para>
        /// <para>To get the assembly, <see cref="Assembly.GetExecutingAssembly"/> can be used.</para>
        /// </summary>
        /// <param name="assembly">The assembly to look in</param>
        void FindServices(Assembly assembly);
        /// <summary>
        /// Finds all services that implement <see cref="IService"/> in the given assemblies.
        /// <para>The services need to have the <see cref="ServiceAttribute"/>.</para>
        /// </summary>
        /// <param name="assemblies">The assemblies to look in</param>
        void FindServices(IEnumerable<Assembly> assemblies);
        /// <summary>
        /// Overrides a set singleton service with another service.
        /// <para>If the abstract-tag combination doesn't exist yet, simply adds it</para>
        /// </summary>
        /// <typeparam name="TAbstract">The abstract type the dependee requests</typeparam>
        /// <typeparam name="TImplementation">The implementation that should be provided</typeparam>
        /// <param name="tag">The tag to differentiate with other abstract versions</param>
        void OverrideSingleton<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new();
        /// <summary>
        /// Overrides a set scoped service with another service.
        /// <para>If the abstract-tag combination doesn't exist yet, simply adds it</para>
        /// </summary>
        /// <typeparam name="TAbstract">The abstract type the dependee requests</typeparam>
        /// <typeparam name="TImplementation">The implementation that should be provided</typeparam>
        /// <param name="tag">The tag to differentiate with other abstract versions</param>
        void OverrideScoped<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new();
        /// <summary>
        /// Overrides a set transient service with another service.
        /// <para>If the abstract-tag combination doesn't exist yet, simply adds it</para>
        /// </summary>
        /// <typeparam name="TAbstract">The abstract type the dependee requests</typeparam>
        /// <typeparam name="TImplementation">The implementation that should be provided</typeparam>
        /// <param name="tag">The tag to differentiate with other abstract versions</param>
        void OverrideTransient<TAbstract, TImplementation>(string tag = null) where TImplementation : class, new();
        /// <summary>
        /// Allows an <see cref="IServiceLibrary"/> to setup itself with services.
        /// <para>Call this after all the required services have been added.</para>
        /// </summary>
        void SetupSelf();
    }
}