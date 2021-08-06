using System.Collections.Generic;
using System.Linq;

using OpenTK.Mathematics;

namespace HeavyEngine {
    public sealed class Transform {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;
        private Matrix4 transMatrix;

        private bool dirty = true;

        public Vector3 Position {
            get => position;
            set {
                dirty = true;
                position = value;
            }
        }


        public Quaternion Rotation {
            get => rotation;
            set {
                dirty = true;
                rotation = value;
            }
        }


        public Vector3 Scale {
            get => scale;
            set {
                dirty = true;
                scale = value;
            }
        }

        public Vector3 Up => Vector3.Transform(Vector3.UnitY, rotation);
        public Vector3 Forward => Vector3.Transform(-Vector3.UnitZ, rotation);
        public Vector3 Right => Vector3.Transform(Vector3.UnitX, rotation);

        public Matrix4 View => Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(position);

        public Matrix4 TransMatrix {
            get {
                if (dirty)
                    transMatrix = Matrix4.CreateTranslation(position) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateScale(scale);

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

        public static implicit operator bool(Transform transform) => transform != null;
    }
}
