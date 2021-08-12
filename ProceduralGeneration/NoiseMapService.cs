using System;

using HeavyEngine;

using OpenTK.Mathematics;

namespace ProceduralGeneration {
    [Service(typeof(NoiseMapService), ServiceTypes.Singleton)]
    public class NoiseMapService : IService {
        public void Initialize() { }

        public float[,] GenerateNoiseMap(int width, int height, float scale, int seed, int octaves, float persistance, float lacunarity, Vector2 offset) {
            var noiseMap = new float[width, height];

            var random = new Random(seed);
            Vector2[] octaveOffsets = new Vector2[octaves];

            for(int i = 0; i < octaves; i++) {
                float offsetX = random.Next(-100000, 100000) + offset.X;
                float offsetY = random.Next(-100000, 100000) + offset.Y;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if (scale <= 0)
                scale = 1e-6f;

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            float halfWidth = width / 2.0f;
            float halfHeight = height / 2.0f;

            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    var amplitude = 1.0f;
                    var frequency = 1.0f;
                    var noiseHeight = 0.0f;

                    for (int i = 0; i < octaves; i++) {
                        var sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].X;
                        var sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].Y;

                        var perlinValue = PerlinNoise.Perlin(sampleX, sampleY) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight > maxNoiseHeight)
                        maxNoiseHeight = noiseHeight;
                    if (noiseHeight < minNoiseHeight)
                        minNoiseHeight = noiseHeight;

                    noiseMap[x, y] = noiseHeight;
                }
            }

            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }

            return noiseMap;
        }
    }
}
