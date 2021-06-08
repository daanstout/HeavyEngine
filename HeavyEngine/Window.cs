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
        [Dependency] private protected readonly ITimeService timeService;
        [Dependency] private protected readonly ICoroutineService coroutineService;
        [Dependency] private protected readonly ILogger logger;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            SetupServices(DependencyObtainer.PrimaryInjector.Services);
        }

        protected virtual void SetupServices(IServiceLibrary services) {
            services.FindServices(Assembly.GetExecutingAssembly());
            
            services.SetupSelf();

            DependencyObtainer.PrimaryInjector.Inject(this);

            coroutineService.StartCoroutine(Coroutine());
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            timeService.Update(args);
            eventService.Invoke<UpdateEvent>();

            base.OnUpdateFrame(args);
        }

        public static GameWindowSettings CreateDefaultGameWindowSettings() {
            return new GameWindowSettings {
                UpdateFrequency = 144.0f,
                RenderFrequency = 144.0f,
                IsMultiThreaded = true
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

        private IEnumerator Coroutine() {
            var rand = new Random();
            yield return null;
            yield return null;
            yield return null;
            logger.Log("Now");
        }
    }
}
