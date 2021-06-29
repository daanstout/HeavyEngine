using HeavyEngine;
using HeavyEngine.Rendering;

using OpenTK.Mathematics;

namespace ProceduralGeneration.Scenes {
    public static class SceneMain {
        public static Scene GetScene() {
            var scene = new Scene();

            var mapObject = new GameObject();
            var mapDisplay = new MapDisplay();
            var mapGen = new MapGenerator {
                mapWidth = 200,
                mapHeight = 200,
                noiseScale = 5.0f,
                mapDisplay = mapDisplay
            };

            mapObject.AddComponent(mapGen);
            mapObject.AddComponent(mapDisplay);

            var plane = new GameObject();
            var meshRenderer = new MeshRenderer();
            meshRenderer.CreateTexture();

            mapDisplay.renderer = meshRenderer;

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

            meshRenderer.SetMesh(mesh);
            meshRenderer.CreateShader("Shaders/VertexShader.vert", "Shaders/FragmentShader.frag");

            plane.AddComponent(meshRenderer);

            scene.AddGameObject(mapObject);
            scene.AddGameObject(plane);

            return scene;
        }
    }
}
