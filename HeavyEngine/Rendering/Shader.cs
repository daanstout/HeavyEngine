using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using OpenTK.Graphics.OpenGL4;

namespace HeavyEngine {
    public class Shader : IDisposable, IEquatable<Shader> {
        private readonly int handle;
        private bool isDisposed = false;

        public int Handle => handle;
        public ShaderType ShaderType { get; }

        private Shader(ShaderType shaderType) {
            handle = GL.CreateShader(shaderType);
            ShaderType = shaderType;
        }

        ~Shader() {
            Dispose();
        }

        public static Shader CreateFromFile(string path, ShaderType shaderType) {
            using var reader = new StreamReader(path, Encoding.UTF8);
            return CreateFromSource(reader.ReadToEnd(), shaderType);
        }

        public static Shader CreateFromSource(string source, ShaderType shaderType) {
            var shader = new Shader(shaderType);
            GL.ShaderSource(shader.handle, source);
            GL.CompileShader(shader.handle);

            var infoLog = GL.GetShaderInfoLog(shader.handle);

            if (!string.IsNullOrEmpty(infoLog)) {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(infoLog);
                Console.ForegroundColor = color;
            }

            return shader;
        }

        public void Dispose() {
            if (isDisposed)
                return;

            GL.DeleteShader(handle);

            isDisposed = true;
            GC.SuppressFinalize(this);
        }

        public override bool Equals(object obj) => Equals(obj as Shader);
        public bool Equals(Shader other) => other != null && handle == other.handle;
        public override int GetHashCode() => HashCode.Combine(handle);

        public static bool operator ==(Shader left, Shader right) => EqualityComparer<Shader>.Default.Equals(left, right);
        public static bool operator !=(Shader left, Shader right) => !(left == right);
    }
}
