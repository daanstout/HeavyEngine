using HeavyEngine;
using HeavyEngine.Injection;
using HeavyEngine.Logging;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace HeavyWindow {
    public class TestGame : Window {
        public TestGame(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
        }

        protected override void SetupServices(IServiceLibrary services) {
            services.BindTag<ILogger>(null, DependencyConstants.LOGGER_CONSOLE_LOGGER);
            base.SetupServices(services);
        }

        protected override void OnRenderFrame(FrameEventArgs args) {

        }
    }
}
