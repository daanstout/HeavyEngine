using HeavyEngine;

namespace ProceduralGeneration {
    class Program {
        static void Main(string[] args) {
            using var window = new GameWindow(Window.CreateDefaultGameWindowSettings(), Window.CreateDefaultNativeWindowSettings());

            window.Run();
        }
    }
}
