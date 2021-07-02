using System;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace HeavyEngine.Rendering {
    public class MeshRenderer : Component, IDisposable, IRenderer, IRenderable {
        private Mesh Mesh;
        private Shader shader;
        private readonly VertexArrayObject VAO;
        private readonly VertexBufferObject VBO;
        private readonly ElementBufferObject EBO;
        public Texture2D Texture { get; private set; }

        private bool disposed = false;

        public MeshRenderer() {
            VBO = new VertexBufferObject();
            VAO = new VertexArrayObject();
            EBO = new ElementBufferObject();

            CreateShader();
        }

        ~MeshRenderer() {
            Dispose(false);
        }

        public void CreateTexture(int width, int height) {
            Texture = new Texture2D(width, height);
        }

        public void CreateTexture() {
            Texture = new Texture2D();
        }

        public void CreateTexture(string path) {
            Texture = new Texture2D(path) {
                GenerateMipmaps = true,
                TextureUnit = TextureUnit.Texture0
            };
            Texture.Apply();
        }

        public void SetTexture(Texture2D texture) {
            this.Texture?.Dispose();
            this.Texture = texture;
            this.Texture.Apply();
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

        public void Render(Camera camera) {
            if (Mesh == null)
                return;

            if (Mesh.Vertices.Length == 0)
                return;

            VAO.Bind();
            Texture?.Bind();
            shader.Bind();
            //shader.SetInt("texture0", 0);
            //shader.SetInt("texture1", 1);
            shader.TrySetMat4("transform", Transform.TransMatrix);
            //shader.TrySetMat4("view", camera.Transform.TransMatrix);
            //shader.TrySetMat4("projection", camera.Projection);

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
            Texture?.Dispose();

            disposed = true;
        }
    }
}
