using System;

using HeavyEngine;

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

            var mesh = new Mesh {
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

            game.SetMesh(mesh);
            game.CreateShaders("Shaders/VertexShader.vert", "Shaders/FragmentShader.frag");
            game.AddTexture("Resources/container.png");

            game.Run();
        }
    }
}
