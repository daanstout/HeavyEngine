using System;
using System.Drawing;

using HeavyEngine;

using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ProceduralGeneration {
    public class MapGenerator : Component, IAwakable, IUpdatable {
        public enum DrawMode {
            NoiseMap,
            ColorMap,
            Mesh
        }

        public struct TerrainType {
            public string name;
            public float height;
            public Color color;
        }

        [Dependency] private readonly NoiseMapService mapService;
        [Dependency] private readonly IInputService inputService;

        public DrawMode drawMode;
        public int mapWidth;
        public int mapHeight;
        public float noiseScale;
        public int octaves;
        public float persistance;
        public float lacunarity;
        public int seed;
        public Vector2 offset;

        public TerrainType[] regions = Array.Empty<TerrainType>();

        public MapDisplay mapDisplay;

        public void Awake() => GenerateMap();

        public void Update() {
            var change = false;
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.D1)) {
                noiseScale += 10 * Time.DeltaTime;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.D2)) {
                noiseScale -= 10 * Time.DeltaTime;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyPressed(Keys.D3)) {
                octaves++;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyPressed(Keys.D4)) {
                octaves--;
                if (octaves <= 0)
                    octaves = 1;
                else
                    change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.D5)) {
                persistance += 2 * Time.DeltaTime;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.D6)) {
                persistance -= 2 * Time.DeltaTime;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.D7)) {
                lacunarity += 2 * Time.DeltaTime;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.D8)) {
                lacunarity -= 2 * Time.DeltaTime;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyPressed(Keys.D9)) {
                seed++;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyPressed(Keys.D0)) {
                seed--;
                change = true;
            }

            if (inputService.CurrentKeyboardState.IsKeyPressed(Keys.LeftControl) || inputService.CurrentKeyboardState.IsKeyPressed(Keys.RightControl)) {
                drawMode = drawMode switch {
                    DrawMode.ColorMap => DrawMode.NoiseMap,
                    DrawMode.NoiseMap => DrawMode.Mesh,
                    DrawMode.Mesh => DrawMode.ColorMap,
                    _ => DrawMode.NoiseMap
                };
                change = true;
            }

            if (noiseScale < 0)
                noiseScale = 1e-6f;

            if (change)
                GenerateMap();
        }

        private void GenerateMap() {
            var noiseMap = mapService.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, seed, octaves, persistance, lacunarity, offset);

            Color[] colorMap = new Color[mapWidth * mapHeight];

            for (int y = 0; y < mapHeight; y++) {
                for (int x = 0; x < mapWidth; x++) {
                    var currentHeight = noiseMap[x, y];

                    for (int i = 0; i < regions.Length; i++) {
                        if (currentHeight <= regions[i].height) {
                            colorMap[y * mapWidth + x] = regions[i].color;
                            break;
                        }
                    }
                }
            }

            if (drawMode == DrawMode.NoiseMap)
                mapDisplay.DrawBitmap(TextureGenerator.BitmapFromHeightMap(noiseMap));
            else if (drawMode == DrawMode.ColorMap)
                mapDisplay.DrawBitmap(TextureGenerator.BitmapFromColorMap(colorMap, mapWidth, mapHeight));
            else if (drawMode == DrawMode.Mesh)
                mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap), TextureGenerator.BitmapFromColorMap(colorMap, mapWidth, mapHeight));
        }
    }
}
