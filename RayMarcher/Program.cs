using System;

using HeavyEngine;

namespace RayMarcher {
    class Program {
        static void Main(string[] args) {
            var marcher = new RayMarcher(Window.CreateDefaultGameWindowSettings(), Window.CreateDefaultNativeWindowSettings());

            marcher.Run();
        }
    }
}
