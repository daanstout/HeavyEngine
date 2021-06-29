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
            var gameObject = new GameObject();
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
            meshRenderer.CreateShader("Shaders/VertexShader.vert", "Shaders/FragmentShader.frag");
            meshRenderer.CreateTexture("Resources/container.png");
            meshRenderer.CreateTexture("Resources/awesomeface.png");

            gameObject.AddComponent(meshRenderer);
            scene.AddGameObject(gameObject);

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
}
