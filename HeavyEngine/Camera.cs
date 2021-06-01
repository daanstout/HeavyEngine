using OpenTK.Mathematics;

namespace HeavyEngine {
    public class Camera {
        public Transform Transform { get; }
        public float FoV { get; set; }
        public Vector2 Size { get; set; }
        public Matrix4 Projection => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FoV), Size.X / Size.Y, 0.1f, 1000.0f);

        public Camera(Vector2 size) {
            Transform = new Transform();
            Size = size;
            FoV = 70;
        }
    }
}
