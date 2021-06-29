using HeavyEngine;

using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ProceduralGeneration {
    public class MapGenerator : Component, IAwakable, IUpdatable {
        [Dependency] private readonly NoiseMapService mapService;
        [Dependency] private readonly IInputService inputService;
        [Dependency] private readonly ITimeService timeService;

        public int mapWidth;
        public int mapHeight;
        public float noiseScale;

        public MapDisplay mapDisplay;

        public void Awake() {
            GenerateMap();
        }

        public void Update() {
            bool change = false;
            if (inputService.CurrentKeyboardState.IsKeyPressed(Keys.Q)) {
                mapWidth--;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyPressed(Keys.W)) {
                mapWidth++;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyPressed(Keys.A)) {
                mapHeight--;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyPressed(Keys.S)) {
                mapHeight++;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.Z)) {
                noiseScale -= 2 * timeService.DeltaTime;
                change = true;
            }
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.X)) {
                noiseScale += 2 * timeService.DeltaTime;
                change = true;
            }

            if (noiseScale < 0)
                noiseScale = 1e-6f;

            if (change)
                GenerateMap();
        }

        public void GenerateMap() {
            var noiseMap = mapService.GenerateNoiseMap(mapWidth, mapHeight, noiseScale);

            mapDisplay.DrawNoiseMap(noiseMap);
        }
    }
}
