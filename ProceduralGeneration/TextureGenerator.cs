using System.Drawing;

using HeavyEngine.Rendering;

namespace ProceduralGeneration {
    public static class TextureGenerator {
        public static RawImage BitmapFromColorMap(Color[] colorMap, int width, int height) {
            var image = new RawImage(width, height);
            //Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    image.SetPixel(x, y, colorMap[y * width + x]);
                }
            }

            return image;
        }

        public static RawImage BitmapFromHeightMap(float[,] heightMap) {
            var width = heightMap.GetLength(0);
            var height = heightMap.GetLength(1);

            var colorMap = new Color[width * height];
            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    colorMap[y * width + x] = LerpColor(Color.Black, Color.White, heightMap[x, y]);
                }
            }

            return BitmapFromColorMap(colorMap, width, height);
        }

        private static Color LerpColor(Color a, Color b, float x) => Color.FromArgb(255, Lerp(a.R, b.R, x), Lerp(a.G, b.G, x), Lerp(a.B, b.B, x));

        private static int Lerp(int a, int b, float x) => (int)(a + (x * (b - a)));
    }
}
