using System.Collections.Generic;
using System.Linq;

using OpenTK.Mathematics;

namespace HeavyEngine {
    public sealed class Transform {
        private Vector3 position;
        private Vector3 localPosition;
        private Quaternion rotation;
        private Quaternion localRotation;
        private Vector3 scale;
        private Vector3 localScale;
        private Matrix4 transMatrix;

        private bool dirty = true;

        public Vector3 Position {
            get => position;
            set {
                dirty = true;
                position = value;
            }
        }

        public Vector3 LocalPosition {
            get => localPosition;
            set {
                dirty = true;
                localPosition = value;
            }
        }

        public Quaternion Rotation {
            get => rotation;
            set {
                dirty = true;
                rotation = value;
            }
        }

        public Quaternion LocalRotation {
            get => localRotation;
            set {
                dirty = true;
                localRotation = value;
            }
        }

        public Vector3 Scale {
            get => scale;
        }

        public Vector3 LocalScale {
            get => localScale;
            set {
                dirty = true;
                localScale = value;
            }
        }

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
