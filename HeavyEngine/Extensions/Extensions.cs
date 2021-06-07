namespace HeavyEngine.Extensions {
    /// <summary>
    /// A class that provides extensions for various classes.
    /// </summary>
    public static class Extensions {
        /// <summary>
        /// Injects the object with dependencies with the primary <see cref="Injection.IDependencyInjector"/>.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to inject</param>
        public static void Inject(this object obj) {
            DependencyObtainer.PrimaryInjector.Inject(obj);
        }
    }
}
