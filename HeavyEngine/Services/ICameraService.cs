namespace HeavyEngine {
    /// <summary>
    /// The <see cref="ICameraService"/> can be used to get information about the <see cref="Camera"/>s in the <see cref="Scene"/>
    /// </summary>
    public interface ICameraService {
        /// <summary>
        /// The main <see cref="Camera"/> in the <see cref="Scene"/> that is used to render to the screen
        /// </summary>
        Camera MainCamera { get; }
        /// <summary>
        /// An array of all the active <see cref="Camera"/>s in the <see cref="Scene"/>
        /// </summary>
        Camera[] ActiveCameras { get; }

        /// <summary>
        /// unregisters a <see cref="Camera"/> from the service
        /// <para>If said <see cref="Camera"/> is the active <see cref="Camera"/>, the first <see cref="Camera"/> in the list (ie. the longest active camera) will become the new main <see cref="Camera"/></para>
        /// </summary>
        /// <param name="camera">The <see cref="Camera"/> to unregister</param>
        void UnregisterCamera(Camera camera);
        /// <summary>
        /// Registers a <see cref="Camera"/> to the service
        /// </summary>
        /// <param name="camera">The <see cref="Camera"/> to register</param>
        void RegisterCamera(Camera camera);
        /// <summary>
        /// Sets the given <see cref="Camera"/> as the new main <see cref="Camera"/>
        /// </summary>
        /// <param name="camera">The <see cref="Camera"/> to become the main <see cref="Camera"/></param>
        void SetCameraMain(Camera camera);
    }
}