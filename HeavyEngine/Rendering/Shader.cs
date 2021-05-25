using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using OpenTK.Graphics.ES30;
using OpenTK.Mathematics;

namespace HeavyEngine {
    public class Shader : IDisposable {
        private readonly Dictionary<string, int> uniformCache;
        private readonly int handle;
        private bool isDisposed = false;
        private bool isBound = false;

        public Shader(string vertexPath, string fragmentPath) {
            uniformCache = new Dictionary<string, int>();

            var vertexShader = CreateShader(vertexPath, ShaderType.VertexShader);
            var fragmentShader = CreateShader(fragmentPath, ShaderType.FragmentShader);

            handle = GL.CreateProgram();
            GL.AttachShader(handle, vertexShader);
            GL.AttachShader(handle, fragmentShader);

            GL.LinkProgram(handle);

            GL.DetachShader(handle, vertexShader);
            GL.DetachShader(handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            FillUniformCache();
        }

        ~Shader() {
            Dispose(false);
        }

        public void Bind() {
            GL.UseProgram(handle);
            isBound = true;
        }

        public void Unbind() {
            GL.UseProgram(0);
            isBound = false;
        }

        protected virtual void Dispose(bool disposing) {
            if (isDisposed)
                return;

            GL.DeleteProgram(handle);

            isDisposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SetVec2(string name, Vector2 vec) => GL.Uniform2(uniformCache[name], vec);

        public void SetVec3(string name, Vector3 vec) => GL.Uniform3(uniformCache[name], vec);

        public void SetInt(string name, int val) => GL.Uniform1(uniformCache[name], val);

        public void SetColor(string name, Color4 color) => GL.Uniform4(uniformCache[name], color);

        public void SetFloat(string name, float val) => GL.Uniform1(uniformCache[name], val);

        public void SetMat4(string name, Matrix4 mat) => GL.UniformMatrix4(uniformCache[name], true, ref mat);

        private string ReadShader(string path) {
            using var reader = new StreamReader(path, Encoding.UTF8);
            return reader.ReadToEnd();
        }

        private int CreateShader(string path, ShaderType shaderType) {
            var source = ReadShader(path);
            var shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, source);

            GL.CompileShader(shader);
            var infoLog = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(infoLog)) {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(infoLog);
                Console.ForegroundColor = color;
            }

            return shader;
        }

        private void FillUniformCache() {
            bool unbind = !isBound;
            if (!isBound)
                Bind();

            GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);
            for (int i = 0; i < uniformCount; i++) {
                GL.GetActiveUniform(handle, i, 100, out int length, out int size, out ActiveUniformType type, out string name);
                uniformCache.Add(name, i);
            }

            if (unbind)
                Unbind();
        }
    }
}
