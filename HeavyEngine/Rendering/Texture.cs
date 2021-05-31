using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

using OpenTK.Graphics.OpenGL4;

namespace HeavyEngine.Rendering {
    public class Texture : IDisposable {
        private readonly int id;
        private bool disposed;

        private Texture() {
            id = GL.GenTexture();
        }

        ~Texture() {
            Dispose(false);
        }

        public static Texture LoadFromFile(string path, bool generateMipmaps = true) {
            var texture = new Texture();

            GL.ActiveTexture(TextureUnit.Texture0);
            texture.Bind();

            using var image = new Bitmap(path);
            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);

            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);

            texture.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            texture.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            texture.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            texture.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if (generateMipmaps)
                texture.GenerateMipmap();

            return texture;
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0) {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, id);
        }

        public void Unbind(TextureUnit unit = TextureUnit.Texture0) {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        public void TexParameter(TextureTarget target, TextureParameterName paramName, int param) {
            GL.TexParameter(target, paramName, param);
        }

        public void TexParameter(TextureTarget target, TextureParameterName paramName, float[] param) {
            GL.TexParameter(target, paramName, param);
        }

        public void GenerateMipmap() {
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposed)
                return;

            GL.DeleteTexture(id);

            disposed = true;
        }
    }
}
