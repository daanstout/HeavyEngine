using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using HeavyEngine.Rendering;

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

        private class UniformData {
            public int location;
            public object data;
            public Action setData;
        }

        public MaterialFlags Flags { get; private set; }
        public Uniform[] Uniforms => uniformCache.Values.ToArray();

        private readonly Dictionary<string, Uniform> uniformCache = new Dictionary<string, Uniform>();
        private UniformData[] uniformData;
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

        public void SetData() {
            for (int i = 0; i < uniformData.Length; i++)
                if (uniformData[i].data != default)
                    uniformData[i].setData();
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
        public void SetColor(string name, Color4 color) => SetData(name, color, ActiveUniformType.FloatVec4);
        public void SetFloat(string name, float value) => SetData(name, value, ActiveUniformType.Float);
        public void SetInt(string name, int value) => SetData(name, value, ActiveUniformType.Int);
        public void SetMat4(string name, Matrix4 value) => SetData(name, value, ActiveUniformType.FloatMat4);
        public void SetVec2(string name, Vector2 vector) => SetData(name, vector, ActiveUniformType.FloatVec2);
        public void SetVec3(string name, Vector3 vector) => SetData(name, vector, ActiveUniformType.FloatVec3);
        #endregion

        private void SetData(string name, object data, ActiveUniformType type) {
            if (uniformCache.TryGetValue(name, out Uniform value))
                if (value.UniformType == type)
                    uniformData[value.Location].data = data;
        }

        private void FillUniformCache() {
            GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);
            uniformData = new UniformData[uniformCount];

            for (int i = 0; i < uniformCount; i++) {
                GL.GetActiveUniform(handle, i, 100, out int length, out int size, out ActiveUniformType type, out string name);
                uniformCache.Add(name, new Uniform(length, size, type, name, i));

                var data = new UniformData() {
                    location = i
                };

                data.setData = type switch {
                    ActiveUniformType.Bool => () => GL.Uniform1(data.location, (int)data.data),
                    ActiveUniformType.Double => () => GL.Uniform1(data.location, (double)data.data),
                    ActiveUniformType.Float => () => GL.Uniform1(data.location, (float)data.data),
                    ActiveUniformType.FloatMat4 => () => { var mat = (Matrix4)data.data; GL.UniformMatrix4(data.location, true, ref mat); }
                    ,
                    ActiveUniformType.FloatVec2 => () => { var vec = (Vector2)data.data; GL.Uniform2(data.location, ref vec); }
                    ,
                    ActiveUniformType.FloatVec3 => () => { var vec = (Vector3)data.data; GL.Uniform3(data.location, ref vec); }
                    ,
                    ActiveUniformType.FloatVec4 => () => { var vec = (Vector4)data.data; GL.Uniform4(data.location, ref vec); }
                    ,
                    ActiveUniformType.Int => () => GL.Uniform1(data.location, (int)data.data),
                    ActiveUniformType.Sampler2D => () => GL.Uniform1(data.location, (int)data.data),
                    ActiveUniformType.UnsignedInt => () => GL.Uniform1(data.location, (uint)data.data),
                    _ => () => Console.WriteLine($"Uniform type of {type} is currently not supported.")
                };

                uniformData[i] = data;
            }
        }
    }

    [Flags]
    public enum MaterialFlags : uint {

    }
}
