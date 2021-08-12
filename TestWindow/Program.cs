using System;

using HeavyEngine;
using HeavyEngine.Rendering;

using OpenTK.Mathematics;

namespace HeavyWindow {
    public class Program {
        private static void Main(string[] args) {
            Console.WriteLine("Starting up");

            //TestGame();
            Game2D();
        }

        private static void TestGame() {
            using var game = new TestGame(Window.CreateDefaultGameWindowSettings(), Window.CreateDefaultNativeWindowSettings(640, 480, "Game"));

            game.Run();
        }

        private static void Game2D() {
            using var game = new Game2D(Window.CreateDefaultGameWindowSettings(), Window.CreateDefaultNativeWindowSettings());

            var scene = new Scene();
            var gameObjectMesh = new GameObject();
            var meshRenderer = new MeshRenderer();

            var meshOld = new Mesh {
                Vertices = new Vertex[] {
                    new Vertex { position = new Vector3(0.5f, 0.5f, 0.0f), textureCoordinates = new Vector2(1.0f, 1.0f) },
                    new Vertex { position = new Vector3(0.5f, -0.5f, 0.0f), textureCoordinates = new Vector2(1.0f, 0.0f) },
                    new Vertex { position = new Vector3(-0.5f, -0.5f, 0.0f), textureCoordinates = new Vector2(0.0f, 0.0f) },
                    new Vertex { position = new Vector3(-0.5f, 0.5f, 0.0f), textureCoordinates = new Vector2(0.0f, 1.0f) }
                },
                Indices = new uint[] {
                    0, 1, 3,
                    1, 2, 3
                }
            };

            var vertices = new Vertex[] {
                new Vertex { position = new Vector3(0.5f, 0.5f, 0.0f), textureCoordinates = new Vector2(1.0f, 1.0f) },
                new Vertex { position = new Vector3(0.5f, -0.5f, 0.0f), textureCoordinates = new Vector2(1.0f, 0.0f) },
                new Vertex { position = new Vector3(-0.5f, 0.5f, 0.0f), textureCoordinates = new Vector2(0.0f, 1.0f) },
                new Vertex { position = new Vector3(0.5f, -0.5f, 0.0f), textureCoordinates = new Vector2(1.0f, 0.0f) },
                new Vertex { position = new Vector3(-0.5f, -0.5f, 0.0f), textureCoordinates = new Vector2(0.0f, 0.0f) },
                new Vertex { position = new Vector3(-0.5f, 0.5f, 0.0f), textureCoordinates = new Vector2(0.0f, 1.0f) }
            };

            var mesh = Mesh.Flatten(vertices);

            var unflattened = Mesh.Unflatten(meshOld);

            meshRenderer.SetMesh(mesh);
            meshRenderer.AddShaderFile("Shaders/VertexShader.vert", OpenTK.Graphics.OpenGL4.ShaderType.VertexShader);
            meshRenderer.AddShaderFile("Shaders/FragmentShader.frag", OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader);
            meshRenderer.CreateTexture("Resources/container.png");
            meshRenderer.FinalizeMaterial();

            var gameObjectCamera = new GameObject();
            var camera = gameObjectCamera.AddComponent(new Camera(new Vector2(640, 480)));
            gameObjectCamera.AddComponent(new CameraMover());
            camera.Transform.Position = new Vector3(0.0f, 0.0f, 3.0f);

            gameObjectMesh.AddComponent(meshRenderer);
            gameObjectMesh.Transform.Rotation = Quaternion.FromEulerAngles(15.0f, 20.0f, 25.0f);

            var gameObjectMesh2 = new GameObject();
            var meshRenderer2 = gameObjectMesh2.AddComponent(new MeshRenderer());
            meshRenderer2.SetMesh(mesh);
            meshRenderer2.AddShaderFile("Shaders/VertexShader.vert", OpenTK.Graphics.OpenGL4.ShaderType.VertexShader);
            meshRenderer2.AddShaderFile("Shaders/FragmentShader.frag", OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader);
            meshRenderer2.CreateTexture("Resources/container.png");
            meshRenderer2.FinalizeMaterial();

            gameObjectMesh2.Transform.Position = new Vector3(2.0f, 2.0f, 2.0f);


            scene.AddGameObject(gameObjectMesh);
            scene.AddGameObject(gameObjectMesh2);
            scene.AddGameObject(gameObjectCamera);


            game.LoadScene(scene);

            //game.SetMesh(mesh);
            //game.CreateShaders("Shaders/VertexShader.vert", "Shaders/FragmentShader.frag");
            //game.AddTexture("Resources/container.png");
            //game.AddTexture2("Resources/awesomeface.png");
            //game.GetRendererTransform().Rotation = Quaternion.FromEulerAngles(0.0f, 0.0f, MathHelper.DegreesToRadians(20.0f));
            //game.GetRendererTransform().LocalScale = new Vector3(1.1f, 1.1f, 1.1f);
            //game.GetRendererTransform().Position = new Vector3(0.1f, 0.1f, 0.0f);

            game.Run();
        }
    }

    class CameraMover : Component, IUpdatable {
        [Dependency] private readonly IInputService inputService;

        public float speed = 1.5f;
        public float rotationSpeed = 0.5f;

        public void Update() {
            if (inputService.CurrentKeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.W))
                Transform.Position += Transform.Forward * speed * Time.DeltaTime;
            if (inputService.CurrentKeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.S))
                Transform.Position -= Transform.Forward * speed * Time.DeltaTime;

            if (inputService.CurrentKeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.A))
                Transform.Position -= Transform.Right * speed * Time.DeltaTime;
            if (inputService.CurrentKeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.D))
                Transform.Position += Transform.Right * speed * Time.DeltaTime;

            if (inputService.CurrentKeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Space))
                Transform.Position += Transform.Up * speed * Time.DeltaTime;
            if (inputService.CurrentKeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.LeftShift))
                Transform.Position -= Transform.Up * speed * Time.DeltaTime;

            if (inputService.CurrentKeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Q))
                Transform.Rotation = Quaternion.FromEulerAngles(0.0f, rotationSpeed * Time.DeltaTime, 0.0f) * Transform.Rotation;
            if (inputService.CurrentKeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.E))
                Transform.Rotation = Quaternion.FromEulerAngles(0.0f, -rotationSpeed * Time.DeltaTime, 0.0f) * Transform.Rotation;
        }
    }
}
