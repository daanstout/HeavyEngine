namespace HeavyEngine {
    /// <summary>
    /// A <see cref="IService"/> can be injected into objects to provide with access to global state in a safe manner
    /// </summary>
    public interface IService {
        /// <summary>
        /// Initializes the service.
        /// <para>This function is always called right after the service has been injected with dependencies and should only be called once.</para>
        /// </summary>
        void Initialize();
    }
}
