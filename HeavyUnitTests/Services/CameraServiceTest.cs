using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using HeavyEngine;
using HeavyEngine.Logging;
using HeavyEngine.Services;

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

        private void Inject(object toInject, object injectWith, string fieldToInject) {
            var field = toInject.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .First(fld => fld.Name == fieldToInject);
            field.SetValue(toInject, injectWith);
        }
    }
}
