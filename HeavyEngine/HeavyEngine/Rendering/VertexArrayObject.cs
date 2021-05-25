using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.ES30;

namespace HeavyEngine.Rendering {
    public class VertexArrayObject : IDisposable {
        private readonly int id;
        private int size;
        private bool disposed = false;

        public VertexArrayObject() {
            id = GL.GenVertexArray();
            Bind();
        }

        ~VertexArrayObject() {
            Dispose(false);
        }

        public void Push(int size) {
            this.size += size;
        }

        public void SetData(int index = 0) {
            GL.VertexAttribPointer(index, size, VertexAttribPointerType.Float, false, Vertex.VERTEX_SIZE * sizeof(float), 0);

            GL.EnableVertexAttribArray(0);
        }

        public void Bind() => GL.BindVertexArray(id);

        public void Unbind() => GL.BindVertexArray(0);

        public void Dispose() {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposed)
                return;

            Unbind();
            GL.DeleteVertexArray(id);

            disposed = true;
        }
    }
}
