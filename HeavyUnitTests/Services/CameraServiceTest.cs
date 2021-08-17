using HeavyEngine;
using HeavyEngine.Logging;

using Moq;

using OpenTK.Mathematics;

using Xunit;

namespace HeavyUnitTests.Services {
    public class CameraServiceTest : TestBase {
        [Fact]
        public void CameraService_SettingCameraAsMainWithoutRegisteringLogsWarning() {
            // Arrange
            var service = new CameraService();
            var mock = new Mock<ILogger>();
            var cam = new Camera(Vector2.One);

            mock.Setup(logger => logger.LogWarning(It.IsAny<string>(), service)).Verifiable();
            Use<ILogger>(mock.Object);
            Inject(service);

            // Act
            service.SetCameraMain(cam);

            // Assert
            mock.Verify(logger => logger.LogWarning(It.IsAny<string>(), service), Times.Once());
        }

        [Fact]
        public void CameraService_SettingRegisteredCameraAsMainCameraIsSuccessful() {
            // Arrange
            var service = new CameraService();
            var cam = new Camera(Vector2.One);

            // Act
            service.RegisterCamera(cam);
            service.SetCameraMain(cam);

            // Assert
            Assert.Equal(service.MainCamera, cam);
        }

        [Fact]
        public void CameraService_RegisteringCameraTwiceLogsWarning() {
            // Arrange
            var service = new CameraService();
            var mock = new Mock<ILogger>();
            var cam = new Camera(Vector2.One);

            mock.Setup(logger => logger.LogWarning(It.IsAny<string>(), service)).Verifiable();
            Use<ILogger>(mock.Object);
            Inject(service);

            // Act
            service.RegisterCamera(cam);
            service.RegisterCamera(cam);

            // Assert
            mock.Verify(logger => logger.LogWarning(It.IsAny<string>(), service), Times.Once());
        }

        [Fact]
        public void CameraService_DeregisteringMainCameraSetsMainCameraToNull() {
            // Arrange
            var service = new CameraService();
            var cam = new Camera(Vector2.One);

            // Act
            service.RegisterCamera(cam);
            service.SetCameraMain(cam);
            service.UnregisterCamera(cam);

            // Assert
            Assert.Null(service.MainCamera);
        }
    }
}
