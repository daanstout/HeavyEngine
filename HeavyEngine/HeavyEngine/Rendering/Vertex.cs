using System;

using OpenTK.Mathematics;

namespace HeavyEngine {
    public class Vertex {
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
    }
}
