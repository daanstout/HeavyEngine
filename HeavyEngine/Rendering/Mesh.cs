using System;

namespace HeavyEngine {
    public class Mesh {
        public string Name { get; set; }
        public Vertex[] Vertices { get; set; }
        public uint[] Indices { get; set; }

        public Mesh() {
            Vertices = Array.Empty<Vertex>();
            Indices = Array.Empty<uint>();
        }

        public float[] GetArray() {
            var arr = new float[Vertices.Length * Vertex.VERTEX_SIZE];

            for (int i = 0; i < Vertices.Length; i++) {
                var floatArr = Vertices[i].FloatArray();
                for (int j = 0; j < floatArr.Length; j++) {
                    arr[(i * floatArr.Length) + j] = floatArr[j];
                }
            }

            return arr;
        }
    }
}
