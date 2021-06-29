namespace HeavyEngine {
    public interface IPerlinNoiseService {
        float Perlin(float x, float y);
        float Perlin(float x, float y, float z);
        float Perlin(float x, float y, float z, int octaves, float persistence, float lacunarity);
        float Perlin(float x, float y, int octaves, float persistence, float lacunarity);
    }
}