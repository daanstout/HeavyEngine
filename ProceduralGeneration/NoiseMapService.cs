using System;
using System.Collections.Generic;
using System.Text;

using HeavyEngine;

namespace ProceduralGeneration {
    [Service(typeof(NoiseMapService), ServiceTypes.Singleton)]
    public class NoiseMapService : IService {
        [Dependency] private readonly IPerlinNoiseService perlinNoise;

        public void Initialize() { }

        public float[,] GenerateNoiseMap(int width, int height, float scale) {
            var noiseMap = new float[width, height];

            if (scale <= 0)
                scale = 1e-6f;

            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    float sampleX = x / scale;
                    float sampleY = y / scale;

                    var perlinValue = perlinNoise.Perlin(sampleX, sampleY);

                    noiseMap[x, y] = perlinValue;
                }
            }

            return noiseMap;
        }
    }
}
