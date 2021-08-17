using System.Drawing;

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
                mapDisplay = mapDisplay,
                regions = new[] {
                    new MapGenerator.TerrainType {
                        name = "Water Deep",
                        height = 0.3f,
                        color = Color.FromArgb(51, 99, 188)
                    },
                    new MapGenerator.TerrainType {
                        name = "Water Shallow",
                        height = 0.4f,
                        color = Color.FromArgb(51, 99, 192)
                    },
                    new MapGenerator.TerrainType {
                        name = "Sand",
                        height = 0.45f,
                        color = Color.FromArgb(210, 209, 125)
                    },
                    new MapGenerator.TerrainType {
                        name = "Grass",
                        height = 0.55f,
                        color = Color.FromArgb(85, 152, 23)
                    },
                    new MapGenerator.TerrainType {
                        name = "Grass 2",
                        height = 0.6f,
                        color = Color.FromArgb(60, 109, 18)
                    },
                    new MapGenerator.TerrainType {
                        name = "Rock",
                        height = 0.7f,
                        color = Color.FromArgb(91, 69, 61)
                    },
                    new MapGenerator.TerrainType {
                        name = "Rock 2",
                        height = 0.9f,
                        color = Color.FromArgb(75, 60, 54)
                    },
                    new MapGenerator.TerrainType {
                        name = "Snow",
                        height = 1.0f,
                        color = Color.White
                    }
                }
            };

            meshRenderer.SetMesh(Mesh.Plane);
            meshRenderer.CreateTexture(100, 100);
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
