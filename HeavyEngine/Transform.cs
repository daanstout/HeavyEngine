using OpenTK.Mathematics;

namespace HeavyEngine {
    public class Transform {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;
        private Matrix4 transMatrix;

        private bool dirty = true;

        public Vector3 Position {
            get => position;
            set {
                position = value;
                dirty = true;
            }
        }

        public Quaternion Rotation {
            get => rotation;
            set {
                rotation = value;
                dirty = true;
            }
        }

        public Vector3 Scale {
            get => scale;
            set {
                scale = value;
                dirty = true;
            }
        }

        public Matrix4 View => Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);

        public Matrix4 TransMatrix {
            get {
                if (dirty)
                    transMatrix = Matrix4.CreateTranslation(position) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateScale(scale);
                    //transMatrix = Matrix4.CreateScale(scale) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);

                return transMatrix;
            }
        }

        public Transform() : this(Vector3.Zero, Quaternion.Identity, Vector3.One) { }

        public Transform(Vector3 position, Vector3 eulerRotation, Vector3 scale) : this(position, Quaternion.FromEulerAngles(eulerRotation), scale) { }

        public Transform(Vector3 position, Quaternion rotation, Vector3 scale) {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }
    }
}
