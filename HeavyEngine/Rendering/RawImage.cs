using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using OpenTK.Mathematics;

namespace HeavyEngine.Rendering {
    public class RawImage : IDisposable {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public bool PointerIsValid => pointer.HasValue;

        private byte[] data;
        private GCHandle? pointer;

        public RawImage(int width, int height) {
            data = new byte[width * height * 4];
            Width = width;
            Height = height;
        }

        public RawImage(Bitmap bitmap) {
            data = (byte[])new ImageConverter().ConvertTo(bitmap, typeof(byte[]));
            Width = bitmap.Width;
            Height = bitmap.Height;
        }

        ~RawImage() {
            Dispose();
        }

        public IntPtr GetIntPtr() {
            if (!pointer.HasValue)
                pointer = GCHandle.Alloc(data, GCHandleType.Pinned);

            return pointer.Value.AddrOfPinnedObject();
        }

        public void SetPixel(int x, int y, int color) {
            var index = GetIndex(x, y);
            data[index + 0] = (byte)((color >> 0) & 255);
            data[index + 1] = (byte)((color >> 8) & 255);
            data[index + 2] = (byte)((color >> 16) & 255);
            data[index + 3] = (byte)((color >> 24) & 255);
        }

        public void SetPixel(int x, int y, Color4 color) {
            var index = GetIndex(x, y);
            data[index + 0] = (byte)(color.R * 255);
            data[index + 1] = (byte)(color.G * 255);
            data[index + 2] = (byte)(color.B * 255);
            data[index + 3] = (byte)(color.A * 255);
        }

        public void SetPixel(int x, int y, byte[] bytes) => Array.Copy(bytes, (y * Width) + x, data, (y * Width) + x, 4);

        public void SetPixels(int[] colors) {
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    var index = GetIndex(x, y);
                    data[index + 0] = (byte)((colors[y * Width + x] >> 0) & 255);
                    data[index + 1] = (byte)((colors[y * Width + x] >> 8) & 255);
                    data[index + 2] = (byte)((colors[y * Width + x] >> 16) & 255);
                    data[index + 3] = (byte)((colors[y * Width + x] >> 24) & 255);
                }
            }
        }

        public void SetPixels(Color4[] colors) {
            for (int y = 0; y < Height; y++) {
                for (int x = 0; x < Width; x++) {
                    var index = GetIndex(x, y);
                    data[index + 0] = (byte)(colors[y * Width + x].R * 255);
                    data[index + 1] = (byte)(colors[y * Width + x].G * 255);
                    data[index + 2] = (byte)(colors[y * Width + x].B * 255);
                    data[index + 3] = (byte)(colors[y * Width + x].A * 255);
                }
            }
        }

        public void SetPixels(byte[] bytes) {
            if (bytes.Length == data.Length) {
                if (pointer.HasValue) {
                    pointer.Value.Free();
                    pointer = null;
                }

                data = bytes;
            }
        }

        public Color4 GetPixel(int x, int y) {
            var index = GetIndex(x, y);
            return new Color4 {
                R = data[index + 0] / 255.0f,
                G = data[index + 1] / 255.0f,
                B = data[index + 2] / 255.0f,
                A = data[index + 3] / 255.0f
            };
        }

        public void Dispose() {
            if (pointer.HasValue) {
                pointer.Value.Free();
                pointer = null;
            }

            data = null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetIndex(int x, int y) => (y * Width + x) * 4;
    }
}
