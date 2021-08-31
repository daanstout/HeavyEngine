using HeavyEngine.Injection;

namespace HeavyUnitTests {
    /// <summary>
    /// A base class for Test Classes that makes injection easier
    /// </summary>
    public abstract class TestBase {
        private readonly IDependencyInjector injector = new TestInjector();

        /// <summary>
        /// Sets the object to return when the provided abstract class is requested with the given tag
        /// </summary>
        /// <typeparam name="TAbstract">The abstract type the object implements</typeparam>
        /// <param name="implementation">The implementation to provide</param>
        /// <param name="tag">The tag of the implementation</param>
        protected void Use<TAbstract>(object implementation, string tag = null) {
            injector.Services.AddInstance<TAbstract, object>(implementation, tag);
        }

        /// <summary>
        /// Injects an object with dependencies
        /// </summary>
        /// <param name="obj">The object to inject</param>
        protected void Inject(object obj) => injector.Inject(obj);
    }
}
