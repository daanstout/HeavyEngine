using System;

using HeavyEngine;
using HeavyEngine.Rendering;

using OpenTK.Mathematics;

namespace ProceduralGeneration.Scenes {
    public static class SceneMain {
        public static Scene GetScene() {
            var scene = new Scene();

            var cameraObject = new GameObject();
            cameraObject.AddComponent(new Camera(new Vector2(640, 480)));
            cameraObject.AddComponent(new CameraMover());
            cameraObject.Transform.Position = new Vector3(0.0f, 1.0f, 0.0f);
            cameraObject.Transform.Rotation = Quaternion.FromEulerAngles(-Mathf.PI * 0.5f, 0.0f, 0.0f);

            var mapObject = new GameObject();
            var mapDisplay = new MapDisplay();
            var meshRenderer = new MeshRenderer();

            var mapGen = new MapGenerator {
                mapWidth = 100,
                mapHeight = 100,
                noiseScale = 27.6f,
                octaves = 4,
                persistance = 0.5f,
                lacunarity = 2.0f,
                seed = 0,
                offset = Vector2.Zero,
                mapDisplay = mapDisplay
            };

            var planeMesh = new Mesh {
                Vertices = new[] {
                    new Vertex { position = new Vector3(0.5f, 0.0f, 0.5f), textureCoordinates = new Vector2(0.0f, 0.0f) },
                    new Vertex { position = new Vector3(0.5f, 0.0f, -0.5f), textureCoordinates = new Vector2(0.0f, 1.0f) },
                    new Vertex { position = new Vector3(-0.5f, 0.0f, 0.5f), textureCoordinates = new Vector2(1.0f, 0.0f) },
                    new Vertex { position = new Vector3(-0.5f, 0.0f, -0.5f), textureCoordinates = new Vector2(1.0f, 1.0f) }
                },
                Indices = new uint[] {
                    0, 1, 2,
                    1, 3, 2
                },
            };

            meshRenderer.SetMesh(planeMesh);
            meshRenderer.CreateTexture(200, 200);
            meshRenderer.AddShaderFile("Shaders/VertexShader.vert", OpenTK.Graphics.OpenGL4.ShaderType.VertexShader);
            meshRenderer.AddShaderFile("Shaders/FragmentShader.frag", OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader);
            meshRenderer.FinalizeMaterial();

            mapDisplay.renderer = meshRenderer;

            mapObject.AddComponent(mapGen);
            mapObject.AddComponent(mapDisplay);
            mapObject.AddComponent(meshRenderer);

            scene.AddGameObject(cameraObject);
            scene.AddGameObject(mapObject);

            return scene;
        }
    }
}
