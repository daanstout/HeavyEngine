using System.Reflection;

using HeavyEngine;
using HeavyEngine.Injection;
using HeavyEngine.Logging;

using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ProceduralGeneration {
    public class GameWindow : Window {
        public GameWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            //GL.Enable(EnableCap.Texture2D);
        }

        protected override void OnLoad() {
            base.OnLoad();
            
            LoadScene(SceneCreator.GetMainScene());
            
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
        }

        protected override void SetupServices(IServiceLibrary services) {
            base.SetupServices(services);
            
            services.FindServices(Assembly.GetExecutingAssembly());
            services.BindTag<ILogger>(null, DependencyConstants.LOGGER_CONSOLE_LOGGER);
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            base.OnRenderFrame(args);
            SwapBuffers();
        }
    }
}
