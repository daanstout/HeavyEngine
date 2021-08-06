using System;
using System.Collections;
using System.Reflection;

using HeavyEngine.Injection;
using HeavyEngine.Logging;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace HeavyEngine {
    public class Window : GameWindow {
        [Dependency] private protected readonly IEventService eventService;
        [Dependency] private protected readonly IInputService inputService;
        [Dependency] private protected ICameraService cameraService;
        [Dependency] private protected readonly ICoroutineService coroutineService;
        [Dependency] private protected readonly ILogger logger;

        protected Scene currentScene;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            SetupServices(DependencyObtainer.PrimaryInjector.Services);
        }

        public virtual void LoadScene(Scene scene) {
            currentScene = scene;

            currentScene.Awake();
        }

        protected virtual void SetupServices(IServiceLibrary services) {
            services.FindServices(Assembly.GetExecutingAssembly());
            services.BindTag<ILogger>(null, DependencyConstants.LOGGER_CONSOLE_LOGGER);
            
            services.SetupSelf();

            DependencyObtainer.PrimaryInjector.Inject(this);
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            if (!IsFocused)
                return;

            Time.Update(args);
            eventService.Invoke<UpdateEvent>();
            inputService.Update(KeyboardState);

            currentScene?.Update();

            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            if (!IsFocused)
                return;

            currentScene?.Render(cameraService.MainCamera);

            base.OnRenderFrame(args);
        }

        public static GameWindowSettings CreateDefaultGameWindowSettings() {
            return new GameWindowSettings {
                UpdateFrequency = 144.0f,
                RenderFrequency = 144.0f,
                IsMultiThreaded = false
            };
        }

        public static NativeWindowSettings CreateDefaultNativeWindowSettings(int width = 640, int height = 480, string title = "Metal Engine") {
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
