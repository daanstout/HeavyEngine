using System;
using System.Collections.Generic;
using System.Text;

using HeavyEngine.Injection;
using HeavyEngine.Logging;
using HeavyEngine.Extensions;

namespace HeavyEngine.Services {
    public class CameraService {
        private readonly List<Camera> cameras;
        private Camera mainCamera;
        [Dependency()] private readonly ILogger logger;

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
