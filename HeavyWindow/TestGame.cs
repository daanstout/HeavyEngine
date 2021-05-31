using HeavyEngine;
using HeavyEngine.Injection;
using HeavyEngine.Services;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace HeavyWindow {
    public class TestGame : Window {
        public TestGame(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            var _ = new TestObject();
        }

        protected override void SetupServices(IServiceLibrary services) {
            services.BindTag(null, DependencyConstants.LOGGER_CONSOLE_LOGGER);
            base.SetupServices(services);
        }

        protected override void OnRenderFrame(FrameEventArgs args) {

        }
    }

    public class TestObject : GameObject {
        [Dependency] private CameraService cameraService;

        public TestObject() : base() {
            var cam = new Camera(Vector2.One);
            cameraService.SetCameraMain(cam);
        }
    }
}
