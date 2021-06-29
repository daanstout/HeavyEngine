using System;
using System.Drawing;
using System.Drawing.Imaging;

using OpenTK.Graphics.OpenGL4;

namespace HeavyEngine.Rendering {
    public class Texture2D : IDisposable {
        private readonly int id;
        public Bitmap bitmap;
        private bool disposed;

        public bool GenerateMipmaps { get; set; } = true;
        public TextureUnit TextureUnit { get; set; } = TextureUnit.Texture0;

        public Texture2D() {
            id = GL.GenTexture();
            bitmap = new Bitmap(1, 1);
        }

        public Texture2D(string path) : this() {
            bitmap = new Bitmap(path);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        public Texture2D(Bitmap bitmap) : this() {
            this.bitmap = bitmap;
            this.bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
        }

        public Texture2D(int width, int height) : this() {
            bitmap = new Bitmap(width, height);
        }

        ~Texture2D() {
            Dispose(false);
        }

        public void SetPixel(int x, int y, Color color) {
            bitmap.SetPixel(x, y, color);
        }

        public void SetSize(int width, int height) {
            bitmap?.Dispose();
            bitmap = new Bitmap(width, height);
        }

        public void ReleaseBitmap() {
            if (bitmap != null) {
                bitmap.Dispose();
                bitmap = null;
            }
        }

        public void UseBitmap(Bitmap bitmap) => this.bitmap = bitmap;

        public bool IsSize(int width, int height) => bitmap.Width == width && bitmap.Height == height;

        public void LoadFromFile(string path) {
            bitmap = new Bitmap(path);
        }

        public void Apply() {
            Bind();

            var data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                bitmap.Width,
                bitmap.Height,
                0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                data.Scan0);

            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if (GenerateMipmaps)
                GenerateMipmap();

            bitmap.UnlockBits(data);
        }

        public void Bind() {
            GL.ActiveTexture(TextureUnit);
            GL.BindTexture(TextureTarget.Texture2D, id);
        }

        public void Unbind() {
            GL.ActiveTexture(TextureUnit);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void TexParameter(TextureTarget target, TextureParameterName paramName, int param) {
            GL.TexParameter(target, paramName, param);
        }

        private void GenerateMipmap() {
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing) {
            if (disposed)
                return;

            bitmap?.Dispose();
            if (id != 0)
                GL.DeleteTexture(id);

            disposed = true;
        }
    }
}
