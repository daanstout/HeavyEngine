using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL4;

namespace HeavyEngine.Rendering {
    public class VertexBufferObject : IDisposable{
        private readonly int id;
        private bool disposed;

        public VertexBufferObject() {
            id = GL.GenBuffer();
            Bind();
        }

        ~VertexBufferObject() {
            Dispose(false);
        }

        public void SetData(Mesh mesh) {
            GL.BufferData(BufferTarget.ArrayBuffer, mesh.Vertices.Length * Vertex.VERTEX_SIZE * sizeof(float), mesh.GetArray(), BufferUsageHint.StaticDraw);
        }

        public void Bind() => GL.BindBuffer(BufferTarget.ArrayBuffer, id);

        public void Unbind() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose = true) {
            if (disposed)
                return;

            Unbind();
            GL.DeleteBuffer(id);

            disposed = true;
        }
    }
}
