using System;
using System.Collections.Generic;

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

        /// <summary>
        /// Flattens a list of vertices into a mesh that uses indices, keeping the order the same
        /// </summary>
        /// <param name="vertices">The list of vertices to flatten</param>
        /// <returns>A mesh with the vertices flattened</returns>
        public static Mesh Flatten(IEnumerable<Vertex> vertices) {
            var uniqueVertices = new List<Vertex>();
            var indices = new List<uint>();
            var vertexDictionary = new Dictionary<Vertex, uint>();

            foreach (var vertex in vertices) {
                if (!vertexDictionary.ContainsKey(vertex)) {
                    vertexDictionary.Add(vertex, (uint)uniqueVertices.Count);
                    uniqueVertices.Add(vertex);
                }

                indices.Add(vertexDictionary[vertex]);
            }

            return new Mesh {
                Indices = indices.ToArray(),
                Vertices = uniqueVertices.ToArray()
            };
        }

        public static Vertex[] Unflatten(Mesh mesh) {
            var vertices = new Vertex[mesh.Indices.Length];

            for(int i = 0; i < mesh.Indices.Length; i++)
                vertices[i] = mesh.Vertices[mesh.Indices[i]];

            return vertices;
        }
    }
}
