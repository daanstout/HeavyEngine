using System;

using OpenTK.Graphics.OpenGL4;

namespace HeavyEngine.Rendering {
    public sealed class ElementBufferObject : IDisposable {
        private readonly int id;
        private bool disposed;

        public ElementBufferObject() {
            id = GL.GenBuffer();
        }

        ~ElementBufferObject() {
            Dispose(false);
        }

        public void SetData(Mesh mesh) {
            Bind();
            GL.BufferData(BufferTarget.ElementArrayBuffer, mesh.Indices.Length * sizeof(uint), mesh.Indices, BufferUsageHint.StaticDraw);
        }

        public void Bind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, id);

        public void Unbind() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposed)
                return;

            Unbind();
            GL.DeleteBuffer(id);

            disposed = true;
        }
    }
}
