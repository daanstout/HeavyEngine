using System.Reflection;

using HeavyEngine.Injection;
using HeavyEngine.Logging;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace HeavyEngine {
    public class Window : GameWindow {
        [Dependency] private protected readonly IEventService eventService;
        [Dependency] private protected readonly IInputService inputService;
        [Dependency] private protected readonly ICameraService cameraService;
        [Dependency] private protected readonly ILogger logger;

        protected Scene currentScene;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            SetupServices(DependencyObtainer.PrimaryInjector.Services);

            GL.Enable(EnableCap.DepthTest);
        }

        public virtual void LoadScene(Scene scene) {
            currentScene = scene;

            currentScene.Awake();
        }

        protected virtual void SetupServices(IServiceLibrary services) {
            services.FindServices(Assembly.GetExecutingAssembly());
            services.BindTag<ILogger>(null, DependencyConstants.LOGGER_CONSOLE_LOGGER);
            
            DependencyObtainer.PrimaryInjector.Inject(this);
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            if (!IsFocused)
                return;

            /* Update order:
             * - Time
             * - Input
             * - Update Event
             * - Scene
             */

            Time.Update(args);
            eventService.Invoke<InputUpdateEvent, InputUpdateEventArgs>(new InputUpdateEventArgs(KeyboardState));
            eventService.Invoke<UpdateEvent>();
            inputService.Update(KeyboardState);
            currentScene?.Update();

            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            if (!IsFocused)
                return;

            GL.ClearDepth(1);
            

            currentScene?.Render(cameraService.MainCamera);

            base.OnRenderFrame(args);
        }

        public static GameWindowSettings CreateDefaultGameWindowSettings() {
            return new GameWindowSettings {
                UpdateFrequency = 0.0f,
                RenderFrequency = 0.0f,
                IsMultiThreaded = false
            };
        }

        public static NativeWindowSettings CreateDefaultNativeWindowSettings(int width = 640, int height = 480, string title = "Heavy Engine") {
            var settings = new NativeWindowSettings {
                Title = title,
                WindowState = WindowState.Normal,
                WindowBorder = WindowBorder.Fixed,
                Size = new Vector2i(width, height),
                IsFullscreen = false
            };

            return settings;
        }
    }
}
