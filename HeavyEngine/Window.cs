using System.Reflection;

using HeavyEngine.Injection;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace HeavyEngine {
    public class Window : GameWindow {
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            SetupServices(DependencyObtainer.PrimaryInjector.Services);
        }

        protected virtual void SetupServices(IServiceLibrary services) {
            services.FindServices(Assembly.GetExecutingAssembly());

            services.SetupSelf();
        }

        public static GameWindowSettings CreateDefaultGameWindowSettings() {
            return new GameWindowSettings {
                UpdateFrequency = 60.0f,
                RenderFrequency = 60.0f,
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
    }
}
