using System;
using System.Collections.Generic;
using System.Text;

using HeavyEngine;
using HeavyEngine.Injection;
using HeavyEngine.Logging;
using HeavyEngine.Rendering;

using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace HeavyWindow {
    public class Game2D : Window {
        private readonly MeshRenderer renderer;

        public Game2D(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            renderer = new MeshRenderer();
        }

        protected override void SetupServices(IServiceLibrary services) {
            services.BindTag<ILogger>(null, DependencyConstants.LOGGER_CONSOLE_LOGGER);
            base.SetupServices(services);
        }

        protected override void OnLoad() {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            base.OnRenderFrame(args);

            //renderer.Render();

            SwapBuffers();
        }

        protected override void OnUnload() => renderer.Dispose();

        public void SetMesh(Mesh mesh) => renderer.SetMesh(mesh);

        public void AddTexture(string path) => renderer.CreateTexture(path);

        public void AddTexture2(string path) => renderer.CreateTexture(path);

        public Transform GetRendererTransform() => renderer.Transform;

        public void CreateShaders(string vertexShader, string fragmentShader) => renderer.CreateShader(vertexShader, fragmentShader);
    }
}
