using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Mathematics;

namespace HeavyEngine {
    /// <summary>
    /// A <see cref="Transform"/> is a position in space, which can be linked to other <see cref="Transform"/>s as either their parent, or their child
    /// </summary>
    public sealed class Transform {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 scale;
        private Matrix4 transMatrix;
        private readonly List<Transform> children;
        private Transform parent;

        private bool dirty = true;

        /// <summary>
        /// The <see cref="Transform"/>'s position relative to their parent
        /// </summary>
        public Vector3 Position {
            get => position;
            set {
                dirty = true;
                position = value;
            }
        }

        /// <summary>
        /// The <see cref="Transform"/>'s global position
        /// </summary>
        public Vector3 GlobalPosition {
            get => parent ? (new Vector4(position, 1.0f) * parent.TransMatrix).Xyz : position;
            set {
                var currentGlobalPos = GlobalPosition;
                var delta = value - currentGlobalPos;
                position += delta;
                dirty = true;
            }
        }

        /// <summary>
        /// The <see cref="Transform"/>'s rotation relative to their parent
        /// </summary>
        public Quaternion Rotation {
            get => rotation;
            set {
                dirty = true;
                rotation = value;
            }
        }

        /// <summary>
        /// The scale of the <see cref="Transform"/>
        /// </summary>
        public Vector3 Scale {
            get => scale;
            set {
                dirty = true;
                scale = value;
            }
        }

        /// <summary>
        /// The <see cref="Parent"/> of this <see cref="Transform"/>
        /// </summary>
        public Transform Parent => parent;
        /// <summary>
        /// The local <see cref="Up"/> direction of the <see cref="Transform"/>
        /// </summary>
        public Vector3 Up => Vector3.Transform(Vector3.UnitY, rotation);
        /// <summary>
        /// The local <see cref="Forward"/> direction of the <see cref="Transform"/>
        /// </summary>
        public Vector3 Forward => Vector3.Transform(-Vector3.UnitZ, rotation);
        /// <summary>
        /// The local <see cref="Right"/> hand side of the <see cref="Transform"/>
        /// </summary>
        public Vector3 Right => Vector3.Transform(Vector3.UnitX, rotation);

        /// <summary>
        /// Gets a view <see cref="Matrix4"/> of this <see cref="Transform"/>
        /// </summary>
        public Matrix4 View => Matrix4.CreateTranslation(position) * Matrix4.CreateFromQuaternion(rotation);

        /// <summary>
        /// Gets the transform <see cref="Matrix4"/> of this <see cref="Transform"/>
        /// </summary>
        public Matrix4 TransMatrix {
            get {
                if (dirty)
                    //transMatrix = Matrix4.CreateTranslation(position) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateScale(scale);
                    transMatrix = Matrix4.CreateScale(scale) * Matrix4.CreateFromQuaternion(rotation) * Matrix4.CreateTranslation(GlobalPosition);

                return transMatrix;
            }
        }

        /// <summary>
        /// Instantiates a new <see cref="Transform"/> with all zero values
        /// </summary>
        public Transform() : this(Vector3.Zero, Quaternion.Identity, Vector3.One) { }

        /// <summary>
        /// Instantiates a new <see cref="Transform"/> with the provided values
        /// </summary>
        /// <param name="position">The <see cref="Position"/> of this <see cref="Transform"/></param>
        /// <param name="eulerRotation">The <see cref="Rotation"/> of this <see cref="Transform"/></param>
        /// <param name="scale">The <see cref="Scale"/> of this <see cref="Transform"/></param>
        public Transform(Vector3 position, Vector3 eulerRotation, Vector3 scale) : this(position, Quaternion.FromEulerAngles(eulerRotation), scale) { }

        /// <summary>
        /// Instantiates a new <see cref="Transform"/> with the provided values
        /// </summary>
        /// <param name="position">The <see cref="Position"/> of this <see cref="Transform"/></param>
        /// <param name="rotation">The <see cref="Rotation"/> of this <see cref="Transform"/></param>
        /// <param name="scale">The <see cref="Scale"/> of this <see cref="Transform"/></param>
        public Transform(Vector3 position, Quaternion rotation, Vector3 scale) {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;

            children = new List<Transform>();
        }

        /// <summary>
        /// Gets the child at the corresponding index
        /// </summary>
        /// <param name="index">The index of the requested child</param>
        /// <returns>The <see cref="Transform"/> at the specified index</returns>
        /// <exception cref="IndexOutOfRangeException">Thrown when the specified index is out of bounds for the children</exception>
        public Transform GetChild(int index) {
            try {
                return children[index];
            } catch (IndexOutOfRangeException ioore) {
                throw ioore;
            }
        }

        /// <summary>
        /// Sets the <see cref="Parent"/> of this <see cref="Transform"/>
        /// </summary>
        /// <param name="parent">The new <see cref="Parent"/> of the <see cref="Transform"/></param>
        /// <param name="keepGlobal">Whether or not the <see cref="Transform"/> should update its local positions to keep the same global position</param>
        public void SetParent(Transform parent, bool keepGlobal = true) {
            if (parent == this || this.parent == parent)
                return;

            this.parent?.children.Remove(this);

            this.parent = parent;

            this.parent?.children.Add(this);
        }

        public static implicit operator bool(Transform transform) => transform != null;
    }
}
