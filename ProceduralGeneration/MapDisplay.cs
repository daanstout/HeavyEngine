using HeavyEngine;
using HeavyEngine.Rendering;

namespace ProceduralGeneration {
    public class MapDisplay : Component , IAwakable {
        public MeshRenderer imageRenderer;

        public void Awake() => imageRenderer.Texture.FilterMode = FilterMode.Nearest;

        public void DrawBitmap(RawImage bitmap) {
            imageRenderer.SetMesh(Mesh.Plane);
            imageRenderer.Texture.UseImage(bitmap);
            imageRenderer.Texture.Apply();
            //renderer.Transform.Scale = new Vector3(width, 1, height);
        }

        public void DrawMesh(MeshData meshData, RawImage texture) {
            imageRenderer.SetMesh(meshData.CreateMesh());
            imageRenderer.Texture.UseImage(texture);
            imageRenderer.Texture.Apply();
        }
    }
}
