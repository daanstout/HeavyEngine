using System;
using System.Collections.Generic;
using System.Text;

using HeavyEngine;

using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RayMarcher {
    public class RayMarcher : HeavyEngine.Window {
        private readonly float[] vertices = {
                -1.0f, -1.0f, 0.0f,
                 1.0f, -1.0f, 0.0f,
                -1.0f,  1.0f, 0.0f,

                -1.0f,  1.0f, 0.0f,
                 1.0f, -1.0f, 0.0f,
                 1.0f,  1.0f, 0.0f
        };

        private int VBO;
        private int VAO;
        private Shader shader;

        private WorldObject WO;
        private Camera camera;
        private float camSpeed = 50.0f;
        private float camRotSpeed = 5.0f;
        private Random rand;

        public RayMarcher(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings) { }

        protected override void OnLoad() {
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            VBO = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

            VAO = GL.GenVertexArray();
            GL.BindVertexArray(VAO);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            shader = new Shader("Shaders/VertexShader.vert", "Shaders/DopeShader.frag");
            shader.Bind();

            WO = new WorldObject {
                Position = new Vector3(0.0f, 0.0f, 100.0f),
                Color = Color4.Red,
                Radius = 50f
            };

            camera = new Camera(Size);
            //camera = new Transform(Vector3.Zero, Quaternion.Identity, Vector3.One);

            rand = new Random();

            base.OnLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args) {
            if (KeyboardState.IsKeyDown(Keys.W))
                camera.Transform.Position += Vector3.UnitZ * camSpeed * (float)args.Time;
            if (KeyboardState.IsKeyDown(Keys.S))
                camera.Transform.Position -= Vector3.UnitZ * camSpeed * (float)args.Time;
            if (KeyboardState.IsKeyDown(Keys.D))
                camera.Transform.Position += Vector3.UnitX * camSpeed * (float)args.Time;
            if (KeyboardState.IsKeyDown(Keys.A))
                camera.Transform.Position -= Vector3.UnitX * camSpeed * (float)args.Time;
            if (KeyboardState.IsKeyDown(Keys.Space))
                camera.Transform.Position += Vector3.UnitY * camSpeed * (float)args.Time;
            if (KeyboardState.IsKeyDown(Keys.LeftShift))
                camera.Transform.Position -= Vector3.UnitY * camSpeed * (float)args.Time;

            if (KeyboardState.IsKeyDown(Keys.Q))
                camera.Transform.Rotation *= Quaternion.FromEulerAngles(camRotSpeed * (float)args.Time, 0.0f, 0.0f);
            if (KeyboardState.IsKeyDown(Keys.E))
                camera.Transform.Rotation *= Quaternion.FromEulerAngles(-camRotSpeed * (float)args.Time, 0.0f, 0.0f);
        }

        protected override void OnRenderFrame(FrameEventArgs e) {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            var randVec2 = new Vector2(rand.Next(0, Size.X), rand.Next(0, Size.Y));
            var randVec = randVec2 - Size / 2;

            var xAngle = randVec.X / Size.X;
            var yAngle = randVec.Y / Size.Y;
            var xAngleFov = xAngle * 70;
            var yAngleFov = yAngle * 70;

            var proj = camera.Projection;
            var view = camera.Transform.View;
            var projView = view * proj;
            var inv = Matrix4.Invert(projView);

            shader.Bind();
            shader.SetVec3("WO.Pos", WO.Position);
            shader.SetColor("WO.Color", WO.Color);
            shader.SetFloat("WO.Radius", WO.Radius);
            shader.SetInt("MaxStepCount", 500);
            shader.SetVec3("Cam.Pos", Vector3.Zero);
            shader.SetVec3("Cam.Dir", Vector3.UnitZ);
            shader.SetMat4("Cam.Transform", camera.Transform.TransMatrix);
            shader.SetInt("Cam.FoV", 70);
            shader.SetVec2("Cam.ScreenSize", Size);
            //shader.SetMat4("InvViewProj", Matrix4.Invert(camera.Projection * camera.Transform.View));
            GL.BindVertexArray(VAO);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 6);

            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnResize(ResizeEventArgs e) {
            GL.Viewport(0, 0, Size.X, Size.Y);

            base.OnResize(e);
        }

        protected override void OnUnload() {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.DeleteBuffer(VBO);
            GL.UseProgram(0);

            GL.DeleteBuffer(VBO);
            GL.DeleteVertexArray(VAO);

            shader.Dispose();

            base.OnUnload();
        }
    }

    public struct WorldObject {
        public Vector3 Position;
        public Color4 Color;
        public float Radius;
    }
}
