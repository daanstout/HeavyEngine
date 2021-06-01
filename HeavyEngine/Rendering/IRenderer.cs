using System;

namespace HeavyEngine.Rendering {
    public interface IRenderer : IDisposable, IRenderable {
        const string DEFAULT_VERTEX_SHADER_PATH = "Shaders/DefaultVertexShader.vert";
        const string DEFAULT_FRAGMENT_SHADER_PATH = "Shaders/DefaultFragmentShader.frag";

        void CreateShader(string vertexPath = "Shaders/DefaultVertexShader.vert", string fragmentPath = "Shaders/DefaultFragmentShader.frag");
        void SetMesh(Mesh mesh);
    }
}
