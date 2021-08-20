using System;
using System.Drawing;

using OpenTK.Graphics.OpenGL4;

namespace HeavyEngine.Rendering {
    public class Texture2D : IDisposable {
        public RawImage image;
        private readonly int id;
        private bool disposed;

        public bool GenerateMipmaps { get; set; } = true;
        public TextureUnit TextureUnit { get; set; } = TextureUnit.Texture0;
        public FilterMode FilterMode { get; set; } = FilterMode.Linear;

        public Texture2D() {
            id = GL.GenTexture();
            image = new RawImage(0, 0);
        }

        public Texture2D(string path) : this() {
            using var bitmap = new Bitmap(path);
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            image = new RawImage(bitmap);
        }

        public Texture2D(Bitmap bitmap) : this() {
            bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
            image = new RawImage(bitmap);
        }

        public Texture2D(RawImage image) : this() {
            this.image = image;
        }

        public Texture2D(int width, int height) : this() {
            image = new RawImage(width, height);
        }

        ~Texture2D() {
            Dispose();
        }

        public void ReleaseImage() {
            if (image != null) {
                image.Dispose();
                image = null;
            }
        }

        public void UseImage(RawImage image) {
            this.image?.Dispose();
            this.image = image;
        }

        public void Apply() {
            Bind();

            GL.TexImage2D(TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                image.GetIntPtr());

            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            if (GenerateMipmaps)
                GenerateMipmap();
        }

        public void Bind() {
            GL.ActiveTexture(TextureUnit);

            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)FilterMode);
            TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)FilterMode);

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
            if (disposed)
                return;

            image?.Dispose();
            if (id != 0)
                GL.DeleteTexture(id);

            disposed = true;
        }
    }
}
