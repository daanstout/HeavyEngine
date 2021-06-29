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
        private readonly List<Transform> children;
        private Transform parent;

        public Vector3 Position {
            get => position;
            set {
                position = value;
                AlignLocalWithParent();
            }
        }

        public Vector3 LocalPosition {
            get => localPosition;
            set {
                localPosition = value;
                AlignGlobalWithParent();
            }
        }

        public Quaternion Rotation {
            get => rotation;
            set {
                rotation = value;
                AlignLocalWithParent();
            }
        }

        public Quaternion LocalRotation {
            get => localRotation;
            set {
                localRotation = value;
                AlignGlobalWithParent();
            }
        }

        public Vector3 Scale {
            get => scale;
        }

        public Vector3 LocalScale {
            get => localScale;
            set {
                localScale = value;
                AlignGlobalWithParent();
            }
        }

        public Transform Parent => parent;
        public Transform[] Children => Children.ToArray();

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

            children = new List<Transform>();
        }

        public void SetParent(Transform transform, bool keepGlobalPos = false) {
            if (parent == transform)
                return;

            parent?.children.Remove(this);
            parent = this;
            parent?.children.Add(this);

            if (keepGlobalPos)
                AlignGlobalWithParent();
            else
                AlignLocalWithParent();
        }

        private void AlignLocalWithParent() {
            dirty = true;

            if (parent) {
                localPosition = position - parent.position;
                localRotation = Quaternion.Invert(parent.rotation) * rotation;
            } else {
                localPosition = position;
                localRotation = rotation;
                localScale = scale;
            }
        }

        private void AlignGlobalWithParent() {
            dirty = true;
        }

        private void ApplyChangesDownstream() {
            foreach (var child in children) {
                child.AlignLocalWithParent();
                child.ApplyChangesDownstream();
            }
        }

        public static implicit operator bool(Transform transform) => transform != null;
    }
}
