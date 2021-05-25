﻿using System;
using System.Collections.Generic;
using System.Text;

using HeavyEngine;
using HeavyEngine.Injection;
using HeavyEngine.Rendering;

using OpenTK.Graphics.ES30;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace HeavyWindow {
    public class Game2D : Window {
        private readonly ObjectRenderer renderer;

        public Game2D(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) {
            renderer = new ObjectRenderer();
        }

        protected override void SetupServices(IServiceLibrary services) {
            services.BindTag(null, DependencyConstants.LOGGER_CONSOLE_LOGGER);
            base.SetupServices(services);
        }

        protected override void OnLoad() {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            base.OnLoad();
        }

        protected override void OnRenderFrame(FrameEventArgs args) {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            renderer.Render();

            SwapBuffers();
            base.OnRenderFrame(args);
        }

        protected override void OnUnload() => renderer.Dispose();

        public void SetMesh(Mesh mesh) => renderer.SetMesh(mesh);

        public void CreateShaders(string vertexShader, string fragmentShader) => renderer.CreateShader(vertexShader, fragmentShader);
    }
}