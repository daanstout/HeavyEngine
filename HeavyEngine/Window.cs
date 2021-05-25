using System;
using System.Collections.Generic;
using System.Text;

using HeavyEngine.Injection;
using HeavyEngine.Logging;
using HeavyEngine.Services;

using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace HeavyEngine {
    public class Window : GameWindow {
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            SetupServices(DependencyObtainer.PrimaryInjector.Services);
        }

        protected virtual void SetupServices(IServiceLibrary services) {
            services.AddSingleton<ILogger, ConsoleLogger>(DependencyConstants.LOGGER_CONSOLE_LOGGER);
            services.AddSingleton<ILogger, FileLogger>(DependencyConstants.LOGGER_FILE_LOGGER);
            services.AddSingleton<CameraService, CameraService>();
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
