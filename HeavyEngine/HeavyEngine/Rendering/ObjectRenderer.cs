using System;

using OpenTK.Graphics.ES30;

namespace HeavyEngine.Rendering {
    public class ObjectRenderer : IDisposable, IRenderer {
        private Mesh Mesh;
        private Shader shader;
        private readonly VertexArrayObject VAO;
        private readonly VertexBufferObject VBO;
        private readonly ElementBufferObject EBO;

        private bool disposed = false;

        public ObjectRenderer() {
            VBO = new VertexBufferObject();
            VAO = new VertexArrayObject();
            EBO = new ElementBufferObject();

            CreateShader();
        }

        ~ObjectRenderer() {
            Dispose(false);
        }

        public void SetMesh(Mesh mesh) {
            Mesh = mesh;
            VBO.SetData(mesh);

            VAO.Push(mesh.Vertices.Length);
            VAO.SetData();

            EBO.SetData(mesh);
        }

        public void CreateShader(string vertexPath = IRenderer.DEFAULT_VERTEX_SHADER_PATH, string fragmentPath = IRenderer.DEFAULT_FRAGMENT_SHADER_PATH) {
            shader = new Shader(vertexPath, fragmentPath);
        }

        public void Render() {
            if (Mesh == null)
                return;

            if (Mesh.Vertices.Length == 0)
                return;

            shader.Bind();
            VAO.Bind();
            //EBO.Bind();
            //GL.DrawArrays(PrimitiveType.Triangles, 0, Mesh.Vertices.Length);
            GL.DrawElements(PrimitiveType.Triangles, Mesh.Indices.Length, DrawElementsType.UnsignedInt, new IntPtr(0));
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposed)
                return;

            shader.Dispose();
            VAO.Dispose();
            VBO.Dispose();
            EBO.Dispose();

            disposed = true;
        }
    }
}
