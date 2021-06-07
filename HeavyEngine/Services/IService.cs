namespace HeavyEngine {
    public interface IService {
        /// <summary>
        /// Initializes the service.
        /// <para>This function is always called right after the service has been injected with dependencies and should only be called once.</para>
        /// </summary>
        void Initialize();
    }
}
