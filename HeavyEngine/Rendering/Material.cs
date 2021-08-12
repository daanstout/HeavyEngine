using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace HeavyEngine {
    public sealed class Material : IDisposable {
        public class Uniform {
            public int Length => length;
            public int Size => size;
            public ActiveUniformType UniformType => uniformType;
            public string Name => name;
            public int Location => location;

            private readonly int length;
            private readonly int size;
            private readonly ActiveUniformType uniformType;
            private readonly string name;
            private readonly int location;

            public Uniform(int length, int size, ActiveUniformType uniformType, string name, int location) {
                this.length = length;
                this.size = size;
                this.uniformType = uniformType;
                this.name = name;
                this.location = location;
            }
        }

        public MaterialFlags Flags { get; private set; }
        public Uniform[] GetUniforms => uniformCache.Values.ToArray();

        private readonly Dictionary<string, Uniform> uniformCache = new Dictionary<string, Uniform>();
        private readonly List<Shader> shaders;
        private readonly int handle;
        private bool isFinalized;

        public Material() {
            shaders = new List<Shader>();
            handle = GL.CreateProgram();
        }

        public Material(params Shader[] shaders) {
            foreach (var shader in shaders)
                if (shaders.Any(sha => !sha.Equals(shader) && sha.ShaderType == shader.ShaderType))
                    throw new ArgumentException($"Multiple shaders of the same type were passed (Type: {shader.ShaderType}");

            this.shaders = new List<Shader>(shaders);
            handle = GL.CreateProgram();
        }

        public void AddShader(Shader shader) {
            if (isFinalized)
                throw new InvalidOperationException("Shaders have already been finalized. Please add shaders before finalizing");

            shaders.Add(shader);
        }

        public void FinalizeMaterial() {
            if (shaders == null || shaders.Count == 0)
                return;

            foreach (var shader in shaders)
                GL.AttachShader(handle, shader.Handle);

            GL.LinkProgram(handle);

            foreach (var shader in shaders) {
                GL.DetachShader(handle, shader.Handle);
                shader.Dispose();
            }

            shaders.Clear();

            FillUniformCache();

            isFinalized = true;
        }

        public void Dispose() {
            uniformCache.Clear();
            GC.SuppressFinalize(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFlag(MaterialFlags flag) => Flags |= flag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UnsetFlag(MaterialFlags flag) => Flags &= ~flag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsFlagSet(MaterialFlags flag) => (Flags & flag) == flag;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Bind() => GL.UseProgram(handle);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Unbind() => GL.UseProgram(0);

        #region Set Uniform Functions
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVec2(string name, Vector2 vec) => GL.Uniform2(uniformCache[name].Location, vec);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVec3(string name, Vector3 vec) => GL.Uniform3(uniformCache[name].Location, vec);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetInt(string name, int val) => GL.Uniform1(uniformCache[name].Location, val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetColor(string name, Color4 color) => GL.Uniform4(uniformCache[name].Location, color);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetFloat(string name, float val) => GL.Uniform1(uniformCache[name].Location, val);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TrySetMat4(string name, Matrix4 mat) {
            if (!uniformCache.ContainsKey(name))
                return false;

            GL.UniformMatrix4(uniformCache[name].Location, true, ref mat);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetMat4(string name, Matrix4 mat) => GL.UniformMatrix4(uniformCache[name].Location, true, ref mat);
        #endregion

        private void FillUniformCache() {
            GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);

            for (int i = 0; i < uniformCount; i++) {
                GL.GetActiveUniform(handle, i, 100, out int length, out int size, out ActiveUniformType type, out string name);
                uniformCache.Add(name, new Uniform(length, size, type, name, i));
            }
        }
    }

    [Flags]
    public enum MaterialFlags : uint {

    }
}
