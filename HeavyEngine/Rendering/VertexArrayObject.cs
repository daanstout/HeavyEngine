using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Graphics.OpenGL4;

namespace HeavyEngine.Rendering {
    public class VertexArrayObject : IDisposable {
        private readonly int id;
        private readonly List<int> partitions;
        private bool disposed = false;

        public VertexArrayObject() {
            id = GL.GenVertexArray();
            partitions = new List<int>();
            Bind();
        }

        ~VertexArrayObject() {
            Dispose(false);
        }

        public void Push(int size) {
            partitions.Add(size);
        }

        public void SetData() {
            int offset = 0;
            for(int i = 0; i < partitions.Count; i++) {
                GL.EnableVertexAttribArray(i);
                GL.VertexAttribPointer(i, partitions[i], VertexAttribPointerType.Float, false, Vertex.VERTEX_SIZE * sizeof(float), offset * sizeof(float));
                offset += partitions[i];
            }
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
