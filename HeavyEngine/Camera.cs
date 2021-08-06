using OpenTK.Mathematics;

namespace HeavyEngine {
    public sealed class Camera : Component {
        [Dependency] private readonly ICameraService cameraService;

        public float FoV { get; set; }
        public Vector2 Size { get; set; }
        public Matrix4 Projection => Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FoV), Size.X / Size.Y, 0.1f, 1000.0f);
        public Matrix4 View => Matrix4.LookAt(Transform.Position, Transform.Position + Transform.Forward, Transform.Up);

        public Camera(Vector2 size) {
            Size = size;
            FoV = 70;
        }

        public override void OnAdded() {
            cameraService.RegisterCamera(this);
        }

        ~Camera() {
            cameraService.UnregisterCamera(this);
        }
    }
}
