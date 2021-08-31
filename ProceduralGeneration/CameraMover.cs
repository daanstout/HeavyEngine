using HeavyEngine;

using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ProceduralGeneration {
    class CameraMover : Component, IUpdatable {
        [Dependency] private readonly IInputService inputService;

        public float speed = 10f;
        public float rotationSpeed = 0.5f;

        public void Update() {
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.W))
                Transform.Position += Transform.Forward * speed * Time.DeltaTime;
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.S))
                Transform.Position -= Transform.Forward * speed * Time.DeltaTime;

            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.A))
                Transform.Position -= Transform.Right * speed * Time.DeltaTime;
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.D))
                Transform.Position += Transform.Right * speed * Time.DeltaTime;

            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.Space))
                Transform.Position += Transform.Up * speed * Time.DeltaTime;
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.LeftShift))
                Transform.Position -= Transform.Up * speed * Time.DeltaTime;

            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.Q))
                Transform.Rotation = Quaternion.FromEulerAngles(0.0f, rotationSpeed * Time.DeltaTime, 0.0f) * Transform.Rotation;
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.E))
                Transform.Rotation = Quaternion.FromEulerAngles(0.0f, -rotationSpeed * Time.DeltaTime, 0.0f) * Transform.Rotation;

            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.R))
                Transform.Rotation = Quaternion.FromEulerAngles(rotationSpeed * Time.DeltaTime, 0.0f, 0.0f) * Transform.Rotation;
            if (inputService.CurrentKeyboardState.IsKeyDown(Keys.F))
                Transform.Rotation = Quaternion.FromEulerAngles(-rotationSpeed * Time.DeltaTime, 0.0f, 0.0f) * Transform.Rotation;
        }
    }
}
