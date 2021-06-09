using System;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace HeavyEngine.Rendering {
    public class MeshRenderer : IDisposable, IRenderer {
        public Transform Transform { get; }

        private Mesh Mesh;
        private Shader shader;
        private readonly VertexArrayObject VAO;
        private readonly VertexBufferObject VBO;
        private readonly ElementBufferObject EBO;
        private Texture texture;
        private Texture texture2;

        private bool disposed = false;

        public MeshRenderer() {
            Transform = new Transform();

            VBO = new VertexBufferObject();
            VAO = new VertexArrayObject();
            EBO = new ElementBufferObject();

            CreateShader();
        }

        ~MeshRenderer() {
            Dispose(false);
        }

        public void CreateTexture1(string path) {
            texture = Texture.LoadFromFile(path);
        }

        public void CreateTexture2(string path) {
            texture2 = Texture.LoadFromFile(path);
        }

        public void SetMesh(Mesh mesh) {
            Mesh = mesh;
            VBO.SetData(mesh);

            VAO.Push(3);
            VAO.Push(3);
            VAO.Push(2);
            VAO.SetData();

            EBO.SetData(mesh);
        }

        public void CreateShader(string vertexPath = IRenderer.DEFAULT_VERTEX_SHADER_PATH, string fragmentPath = IRenderer.DEFAULT_FRAGMENT_SHADER_PATH) {
            shader?.Unbind();
            shader?.Dispose();
            shader = new Shader(vertexPath, fragmentPath);
        }

        public void Render() {
            if (Mesh == null)
                return;

            if (Mesh.Vertices.Length == 0)
                return;

            VAO.Bind();
            texture?.Bind(TextureUnit.Texture0);
            texture2?.Bind(TextureUnit.Texture1);
            shader.Bind();
            shader.SetInt("texture0", 0);
            shader.SetInt("texture1", 1);
            shader.SetMat4("transform", Transform.TransMatrix);
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
            texture?.Dispose();

            disposed = true;
        }
    }
}
