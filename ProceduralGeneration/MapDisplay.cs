using System.Drawing;

using HeavyEngine;
using HeavyEngine.Rendering;

namespace ProceduralGeneration {
    public class MapDisplay : Component , IAwakable {
        public MeshRenderer renderer;

        public void Awake() => renderer.Texture.FilterMode = FilterMode.Nearest;

        public void DrawBitmap(RawImage bitmap) {
            //renderer.Texture.UseBitmap(bitmap);
            renderer.Texture.UseImage(bitmap);
            renderer.Texture.Apply();
            //renderer.Transform.Scale = new Vector3(width, 1, height);
        }
    }
}
