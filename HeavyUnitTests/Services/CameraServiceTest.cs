using HeavyEngine;
using HeavyEngine.Logging;

using Moq;

using OpenTK.Mathematics;

using Xunit;

namespace HeavyUnitTests.Services {
    public class CameraServiceTest : TestBase {
        #region Function Register Camera
        [Fact]
        public void CameraService_RegisterCamera_RegisteringCameraAddsItToActiveCameras() {
            // Arrange
            var service = new CameraService();
            var camera = new Camera(Vector2.One);

            // Act
            service.RegisterCamera(camera);

            // Assert
            Assert.Contains(camera, service.ActiveCameras);
        }

        [Fact]
        public void CameraService_RegisterCamera_FirstCameraBecomesMainCamera() {
            // Arrange
            var service = new CameraService();
            var camera = new Camera(Vector2.One);

            // Act
            service.RegisterCamera(camera);

            // Assert
            Assert.Equal(camera, service.MainCamera);
        }

        [Fact]
        public void CameraService_RegisterCamera_RegisteringCameraTwiceLogsWarning() {
            // Arrange
            var service = new CameraService();
            var loggerMock = new Mock<ILogger>();
            var camera = new Camera(Vector2.One);

            loggerMock.Setup(logger => logger.LogWarning(It.IsAny<string>(), service)).Verifiable();
            Use<ILogger>(loggerMock.Object);
            Inject(service);

            // Act
            service.RegisterCamera(camera);
            service.RegisterCamera(camera);

            // Assert
            loggerMock.Verify(logger => logger.LogWarning(It.IsAny<string>(), service), Times.Once());
        }
        #endregion

        #region Function Set Camera Main
        [Fact]
        public void CameraService_SetCameraMain_SettingCameraAsMainWithoutRegisteringLogsWarning() {
            // Arrange
            var service = new CameraService();
            var loggerMock = new Mock<ILogger>();
            var camera = new Camera(Vector2.One);

            loggerMock.Setup(logger => logger.LogWarning(It.IsAny<string>(), service)).Verifiable();
            Use<ILogger>(loggerMock.Object);
            Inject(service);

            // Act
            service.SetCameraMain(camera);

            // Assert
            loggerMock.Verify(logger => logger.LogWarning(It.IsAny<string>(), service), Times.Once());
        }

        [Fact]
        public void CameraService_SetCameraMain_SettingRegisteredCameraAsMainCameraIsSuccessful() {
            // Arrange
            var service = new CameraService();
            var camera1 = new Camera(Vector2.One);
            var camera2 = new Camera(Vector2.One);

            // Act
            service.RegisterCamera(camera1);
            service.RegisterCamera(camera2);
            service.SetCameraMain(camera2);

            // Assert
            Assert.Equal(camera2, service.MainCamera);
        }
        #endregion

        #region Function Unregister Camera
        [Fact]
        public void CameraService_UnregisterCamera_UnregisteringMainCameraSetsMainCameraToNull() {
            // Arrange
            var service = new CameraService();
            var camera = new Camera(Vector2.One);

            // Act
            service.RegisterCamera(camera);
            service.SetCameraMain(camera);
            service.UnregisterCamera(camera);

            // Assert
            Assert.Null(service.MainCamera);
        }

        [Fact]
        public void CameraService_UnregisterCamera_UnregisteringUnregisteredCameraLogsWarning() {
            // Arrange
            var service = new CameraService();
            var camera = new Camera(Vector2.One);
            var loggerMock = new Mock<ILogger>();

            loggerMock.Setup(logger => logger.LogWarning(It.IsAny<string>(), service)).Verifiable();
            Use<ILogger>(loggerMock.Object);
            Inject(service);

            // Act
            service.UnregisterCamera(camera);

            // Assert
            loggerMock.Verify(logger => logger.LogWarning(It.IsAny<string>(), service), Times.Once());
        }
        #endregion
    }
}
