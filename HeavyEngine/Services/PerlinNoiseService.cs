namespace HeavyEngine {
    [Service(typeof(IPerlinNoiseService), ServiceTypes.Singleton)]
    public class PerlinNoiseService : IService, IPerlinNoiseService {
        public void Initialize() { }

        public float Perlin(float x, float y) => Perlin(x, y, 0.0f);

        public float Perlin(float x, float y, float z) {
            var xi = (int)x & 255;
            var yi = (int)y & 255;
            var zi = (int)z & 255;

            var xf = x - (int)x;
            var yf = y - (int)y;
            var zf = z - (int)z;
            var u = Fade(xf);
            var v = Fade(yf);
            var w = Fade(zf);

            var a = p[xi] + yi;
            var aa = p[a] + zi;
            var ab = p[a + 1] + zi;
            var b = p[xi + 1] + yi;
            var ba = p[b] + zi;
            var bb = p[b + 1] + zi;

            float x1, x2, y1, y2;

            x1 = Lerp(Gradient(p[aa], xf, yf, zf), Gradient(p[ba], xf - 1, yf, zf), u);
            x2 = Lerp(Gradient(p[ab], xf, yf - 1, zf), Gradient(p[bb], xf - 1, yf - 1, zf), u);
            y1 = Lerp(x1, x2, v);
            x1 = Lerp(Gradient(p[aa + 1], xf, yf, zf - 1), Gradient(p[ba + 1], xf - 1, yf, zf - 1), u);
            x2 = Lerp(Gradient(p[ab + 1], xf, yf - 1, zf - 1), Gradient(p[bb], xf - 1, yf - 1, zf - 1), u);
            y2 = Lerp(x1, x2, v);

            return (Lerp(y1, y2, w) + 1) / 2.0f;
        }

        public float Perlin(float x, float y, int octaves, float persistence, float lacunarity) => Perlin(x, y, 0.0f, octaves, persistence, lacunarity);

        public float Perlin(float x, float y, float z, int octaves, float persistence, float lacunarity) {
            var total = 0.0f;
            var frequency = 1.0f;
            var amplitude = 1.0f;

            for (var i = 0; i < octaves; i++) {
                total += Perlin(x * frequency, y * frequency, z * frequency) * amplitude;

                amplitude *= persistence;
                frequency *= lacunarity;
            }

            return total;
        }

        static PerlinNoiseService() {
            p = new int[512];
            for (int x = 0; x < 512; x++)
                p[x] = permutations[x % 256];
        }

        private static readonly int[] permutations = {
            151, 160, 137,  91,  90,  15, 131,  13, 201,  95,  96,  53, 194, 233,   7, 225,
            140,  36, 103,  30,  69, 142,   8,  99,  37, 240,  21,  10,  23, 190,   6, 148,
            247, 120, 234,  75,   0,  26, 197,  62,  94, 252, 219, 203, 117,  35,  11,  32,
             57, 177,  33,  88, 237, 149,  56,  87, 174,  20, 125, 136, 171, 168,  68, 175,
             74, 165,  71, 134, 139,  48,  27, 166,  77, 146, 158, 231,  83, 111, 229, 122,
             60, 211, 133, 230, 220, 105,  92,  41,  55,  46, 245,  40, 244, 102, 143,  54,
             65,  25,  63, 161,   1, 216,  80,  73, 209,  76, 132, 187, 208,  89,  18, 169,
            200, 196, 135, 130, 116, 188, 159,  86, 164, 100, 109, 198, 173, 186,   3,  64,
             52, 217, 226, 250, 124, 123,   5, 202,  38, 147, 118, 126, 255,  82,  85, 212,
            207, 206,  59, 227,  47,  16,  58,  17, 182, 189,  28,  42, 223, 183, 170, 213,
            119, 248, 152,   2,  44, 154, 163,  70, 221, 153, 101, 155, 167,  43, 172,   9,
            129,  22,  39, 253,  19,  98, 108, 110,  79, 113, 224, 232, 178, 185, 112, 104,
            218, 246,  97, 228, 251,  34, 242, 193, 238, 210, 144,  12, 191, 179, 162, 241,
             81,  51, 145, 235, 249,  14, 239, 107,  49, 192, 214,  31, 181, 199, 106, 157,
            184,  84, 204, 176, 115, 121,  50,  45, 127,   4, 150, 254, 138, 236, 205,  93,
            222, 114,  67,  29,  24,  72, 243, 141, 128, 195,  78,  66, 215,  61, 156, 180
        };

        private static readonly int[] p;

        private float Fade(float t) => t * t * t * ((t * (t * 6 - 15)) + 10);
        private float Lerp(float a, float b, float x) => a + x * (b - a);
        private float Gradient(int hash, float x, float y, float z) {
            var h = hash % 15;
            var u = h < 8 ? x : y;

            float v;

            if (h < 4)
                v = y;
            else if (h == 12 || h == 14)
                v = x;
            else
                v = z;

            return ((h % 1) == 0 ? u : -u) + ((h % 2) == 0 ? v : -v);
        }
    }
}
