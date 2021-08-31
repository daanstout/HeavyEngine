using HeavyEngine;

using OpenTK.Mathematics;

namespace ProceduralGeneration {
    public static class MeshGenerator {
        public static MeshData GenerateTerrainMesh(float[,] heightMap) {
            var width = heightMap.GetLength(0);
            var height = heightMap.GetLength(1);
            var topLeftX = (width - 1) / -2.0f;
            var topLeftZ = (height - 1) / 2.0f;

            var meshData = new MeshData(width, height);
            var vertexIndex = 0;

            for(var y = 0; y < height; y++) {
                for(var x = 0; x < width; x++) {
                    meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightMap[x, y] * 10.0f, topLeftZ - y);
                    meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                    if(x < width - 1 && y < height - 1) {
                        meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                        meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                    }

                    vertexIndex++;
                }
            }

            return meshData;
        }
    }

    public class MeshData {
        public Vector3[] vertices;
        public Vector2[] uvs;
        public uint[] triangles;

        private int triangleIndex;

        public MeshData(int meshWidth, int meshHeight) {
            vertices = new Vector3[meshWidth * meshHeight];
            uvs = new Vector2[meshWidth * meshHeight];
            triangles = new uint[(meshWidth - 1) * (meshHeight - 1) * 6];
        }

        public void AddTriangle(int a, int b, int c) {
            triangles[triangleIndex + 0] = (uint)a;
            triangles[triangleIndex + 1] = (uint)b;
            triangles[triangleIndex + 2] = (uint)c;

            triangleIndex += 3;
        }

        public Mesh CreateMesh() {
            var mesh = new Mesh {
                Vertices = new Vertex[vertices.Length],
                Indices = triangles
            };

            for(int i = 0; i < vertices.Length; i++) {
                mesh.Vertices[i] = new Vertex { position = vertices[i], textureCoordinates = uvs[i] };
            }

            return mesh;
        }
    }
}
