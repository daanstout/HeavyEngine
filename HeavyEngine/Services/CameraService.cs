using System.Collections.Generic;

using HeavyEngine.Injection;
using HeavyEngine.Logging;

namespace HeavyEngine.Services {
    [Service(typeof(CameraService), ServiceTypes.Singleton)]
    public class CameraService : IService {
        private readonly List<Camera> cameras;
        private Camera mainCamera;
        [Dependency] private readonly ILogger logger;

        public Camera MainCamera => mainCamera;
        
        public CameraService() {
            cameras = new List<Camera>();
        }

        public void RegisterCamera(Camera camera) {
            if (cameras.Contains(camera)) {
                logger.LogWarning($"{camera} is trying to be registered while it is already registered", this);
                return;
            }

            cameras.Add(camera);
        }

        public void DeregisterCamera(Camera camera) {
            if (!cameras.Contains(camera)) {
                logger.LogWarning($"{camera} is trying to be deregistered while it is not registered", this);
                return;
            }

            if (mainCamera == camera)
                mainCamera = null; // Will probably make the first camera in the list the main camera

            cameras.Remove(camera);
        }

        public void SetCameraMain(Camera camera) {
            if (!cameras.Contains(camera)) {
                logger.LogWarning($"{camera} is trying to be set as the main camera while it is not registered", this);
                return;
            }

            mainCamera = camera;
        }
    }
}
