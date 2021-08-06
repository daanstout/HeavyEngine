using System;
using System.Collections.Generic;
using System.Text;

using HeavyEngine;

namespace ProceduralGeneration {
    [Service(typeof(NoiseMapService), ServiceTypes.Singleton)]
    public class NoiseMapService : IService {
        public void Initialize() { }

        public float[,] GenerateNoiseMap(int width, int height, float scale) {
            var noiseMap = new float[width, height];

            if (scale <= 0)
                scale = 1e-6f;

            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    var sampleX = x / scale;
                    var sampleY = y / scale;

                    var perlinValue = PerlinNoise.Perlin(sampleX, sampleY);

                    noiseMap[x, y] = perlinValue;
                }
            }

            return noiseMap;
        }
    }
}
