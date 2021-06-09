using System;
using System.Collections.Generic;

using OpenTK.Mathematics;

namespace HeavyEngine {
    public class Vertex : IEquatable<Vertex> {
        public const int VERTEX_SIZE = 8;

        public Vector3 position = Vector3.Zero;
        public Vector3 normal = Vector3.Zero;
        public Vector2 textureCoordinates = Vector2.Zero;
        
        public float[] FloatArray() {
            return new float[] {
                position.X, position.Y, position.Z,
                normal.X, normal.Y, normal.Z,
                textureCoordinates.X, textureCoordinates.Y
            };
        }

        public override bool Equals(object obj) => Equals(obj as Vertex);
        public bool Equals(Vertex other) => other != null && position.Equals(other.position) && normal.Equals(other.normal) && textureCoordinates.Equals(other.textureCoordinates);


        public override int GetHashCode() => HashCode.Combine(position, normal, textureCoordinates);

        public static bool operator ==(Vertex left, Vertex right) => EqualityComparer<Vertex>.Default.Equals(left, right);
        public static bool operator !=(Vertex left, Vertex right) => !(left == right);
    }
}
