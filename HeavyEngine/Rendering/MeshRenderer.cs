using System;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace HeavyEngine.Rendering {
    public class MeshRenderer : Component, IDisposable, IRenderable {
        private Mesh mesh;
        private readonly Material material;
        private readonly VertexArrayObject VAO;
        private readonly VertexBufferObject VBO;
        private readonly ElementBufferObject EBO;
        public Texture2D Texture { get; private set; }

        private bool disposed = false;

        public MeshRenderer() {
            VBO = new VertexBufferObject();
            VAO = new VertexArrayObject();
            EBO = new ElementBufferObject();
            material = new Material();
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
            Texture?.Dispose();
            Texture = texture;
            Texture.Apply();
        }

        public void SetMesh(Mesh mesh) {
            this.mesh = mesh;
            VBO.SetData(mesh);

            VAO.Push(3);
            VAO.Push(3);
            VAO.Push(2);
            VAO.SetData();

            EBO.SetData(mesh);
        }

        public void AddShaderFile(string path, ShaderType shaderType) => material.AddShader(Shader.CreateFromFile(path, shaderType));

        public void AddShaderSource(string source, ShaderType shaderType) => material.AddShader(Shader.CreateFromSource(source, shaderType));

        public void FinalizeMaterial() => material.FinalizeMaterial();

        public void Render(Camera camera) {
            if (mesh == null)
                return;

            if (mesh.Vertices.Length == 0)
                return;

            VAO.Bind();
            Texture?.Bind();
            material.Bind();
            material.SetMat4("transform", Transform.TransMatrix);
            material.SetMat4("view", camera.View);
            material.SetMat4("projection", camera.Projection);
            material.SetData();

            GL.DrawElements(PrimitiveType.Triangles, mesh.Indices.Length, DrawElementsType.UnsignedInt, new IntPtr(0));
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposed)
                return;

            material.Dispose();
            VAO.Dispose();
            VBO.Dispose();
            EBO.Dispose();
            Texture?.Dispose();

            disposed = true;
        }
    }
}
