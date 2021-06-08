using System;
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
        /// <param name="tag">The first tag that the dependee will request</param>
        /// <param name="target">The tag that should be looked at if the first tag isn't present</param>
        /// <returns>True if the binding was succesfully added</returns>
        bool BindTag<TAbstract>(string tag, string target);
        bool UnbindTag<TAbstract>(string tag, string target);
        /// <summary>
        /// Gets a service of the given type, potentially with the given tag.
        /// </summary>
        /// <typeparam name="TAbstract">The type of service requested.</typeparam>
        /// <param name="tag">The tag used to potentially specify which service exactly, if multiple of the same type exist.</param>
        /// <returns>The service requested.</returns>
        TAbstract Get<TAbstract>(string tag = null);
        TAbstract Get<TAbstract>(IDependencyInjector injector, string tag = null);
        /// <summary>
        /// Gets a service of the given type, potentially with the given tag.
        /// </summary>
        /// <param name="type">The type of service requested.</param>
        /// <param name="tag">The tag used to potentially specify which service exactly, if multiple of the same type exist.</param>
        /// <returns>The service requested.</returns>
        object Get(Type type, string tag = null);
        object Get(IDependencyInjector injector, Type type, string tag = null);
        /// <summary>
        /// Finds all services that implement <see cref="IService"/> in the given assembly.
        /// <para>The services need to have the <see cref="ServiceAttribute"/>.</para>
        /// <para>To get the assembly, <see cref="Assembly.GetExecutingAssembly"/> can be used.</para>
        /// </summary>
        /// <param name="assembly">The assembly to look in</param>
        void FindServices(Assembly assembly);
        /// <summary>
        /// Allows an <see cref="IServiceLibrary"/> to setup itself with services.
        /// <para>Call this after all the required services have been added.</para>
        /// </summary>
        void SetupSelf();
    }
}