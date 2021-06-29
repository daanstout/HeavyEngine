using System.Drawing;

using HeavyEngine;
using HeavyEngine.Rendering;

using OpenTK.Mathematics;

namespace ProceduralGeneration {
    public class MapDisplay : Component {
        public MeshRenderer renderer;

        public void DrawNoiseMap(float[,] noiseMap) {
            var width = noiseMap.GetLength(0);
            var height = noiseMap.GetLength(1);

            if (!renderer.Texture.IsSize(width, height))
                renderer.Texture.SetSize(width, height);

            for (var y = 0; y < height; y++) {
                for (var x = 0; x < width; x++) {
                    renderer.Texture.SetPixel(x, y, LerpColor(Color.Black, Color.White, noiseMap[x, y]));
                }
            }

            renderer.Texture.Apply();
            renderer.Transform.LocalScale = new Vector3(width, 1, height);
        }

        private Color LerpColor(Color a, Color b, float x) => Color.FromArgb(255, Lerp(a.R, b.R, x), Lerp(a.G, b.G, x), Lerp(a.B, b.B, x));

        private int Lerp(int a, int b, float x) => (int)(a + (x * (b - a)));
    }
}
