using System.Linq;
using System.Reflection;

using HeavyEngine;
using HeavyEngine.Logging;

using Moq;

using OpenTK.Mathematics;

using Xunit;

namespace HeavyUnitTests.Services {
    public class CameraServiceTest {
        [Fact]
        public void CameraService_SettingCameraAsMainWithoutRegisteringLogsWarning() {
            var service = new CameraService();

            var mock = new Mock<ILogger>();
            var cam = new Camera(Vector2.One);

            mock.Setup(logger => logger.LogWarning(It.IsAny<string>(), service)).Verifiable();
            Inject(service, mock.Object, "logger");

            service.SetCameraMain(cam);

            mock.Verify(logger => logger.LogWarning(It.IsAny<string>(), service), Times.Once());
        }

        [Fact]
        public void CameraService_SettingRegisteredCameraAsMainCameraIsSuccessful() {
            var service = new CameraService();
            var cam = new Camera(Vector2.One);

            service.RegisterCamera(cam);
            service.SetCameraMain(cam);

            Assert.Equal(service.MainCamera, cam);
        }

        [Fact]
        public void CameraService_RegisteringCameraTwiceLogsWarning() {
            var service = new CameraService();

            var mock = new Mock<ILogger>();
            mock.Setup(logger => logger.LogWarning(It.IsAny<string>(), service)).Verifiable();
            Inject(service, mock.Object, "logger");

            var cam = new Camera(Vector2.One);

            service.RegisterCamera(cam);
            service.RegisterCamera(cam);

            mock.Verify(logger => logger.LogWarning(It.IsAny<string>(), service), Times.Once());
        }

        [Fact]
        public void CameraService_DeregisteringMainCameraSetsMainCameraToNull() {
            var service = new CameraService();

            var cam = new Camera(Vector2.One);

            service.RegisterCamera(cam);
            service.SetCameraMain(cam);
            service.UnregisterCamera(cam);

            Assert.Null(service.MainCamera);
        }

        private void Inject(object toInject, object injectWith, string fieldToInject) {
            var field = toInject.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .First(fld => fld.Name == fieldToInject);
            field.SetValue(toInject, injectWith);
        }
    }
}
