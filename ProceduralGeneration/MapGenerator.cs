using HeavyEngine;

using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ProceduralGeneration {
    public class MapGenerator : Component, IAwakable, IUpdatable {
        [Dependency] private readonly NoiseMapService mapService;
        [Dependency] private readonly IInputService inputService;

        public int mapWidth;
        public int mapHeight;
        public float noiseScale;
        public int octaves;
        public float persistance;
        public float lacunarity;
        public int seed;
        public Vector2 offset;

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

            if (noiseScale < 0)
                noiseScale = 1e-6f;

            if (change)
                GenerateMap();
        }

        private void GenerateMap() {
            var noiseMap = mapService.GenerateNoiseMap(mapWidth, mapHeight, noiseScale, seed, octaves, persistance, lacunarity, offset);

            mapDisplay.DrawNoiseMap(noiseMap);
        }
    }
}
