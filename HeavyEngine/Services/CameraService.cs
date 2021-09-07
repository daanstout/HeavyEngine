using System.Collections.Generic;
//using System.Linq;

using HeavyEngine.Logging;
using HeavyEngine.Linq;

namespace HeavyEngine {
    /// <summary>
    /// The <see cref="CameraService"/> holds references to all the <see cref="Camera"/> in the <see cref="Scene"/>
    /// </summary>
    [Service(typeof(ICameraService), ServiceTypes.Singleton)]
    public sealed class CameraService : IService, ICameraService {
        private readonly List<Camera> cameras;
        private Camera mainCamera;
        [Dependency] private readonly ILogger logger;

        /// <inheritdoc/>
        public Camera MainCamera => mainCamera;

        /// <inheritdoc/>
        public Camera[] ActiveCameras => cameras.ToArray();

        /// <summary>
        /// Instantiates a new <see cref="CameraService"/>
        /// </summary>
        public CameraService() {
            cameras = new List<Camera>();
        }

        /// <inheritdoc/>
        public void Initialize() { }

        /// <inheritdoc/>
        public void RegisterCamera(Camera camera) {
            if (cameras.Contains(camera)) {
                logger.LogWarning($"{camera} is trying to be registered while it is already registered", this);
                return;
            }

            cameras.Add(camera);

            if (cameras.Count == 1)
                mainCamera = camera;
        }

        /// <inheritdoc/>
        public void UnregisterCamera(Camera camera) {
            if (!cameras.Contains(camera)) {
                logger.LogWarning($"{camera} is trying to be deregistered while it is not registered", this);
                return;
            }

            cameras.Remove(camera);

            if (mainCamera == camera)
                mainCamera = cameras.FirstOrDefault(); // Will probably make the first camera in the list the main camera

            cameras.Remove(camera);
        }

        /// <inheritdoc/>
        public void SetCameraMain(Camera camera) {
            if (!cameras.Contains(camera)) {
                logger.LogWarning($"{camera} is trying to be set as the main camera while it is not registered", this);
                return;
            }

            mainCamera = camera;
        }
    }
}
