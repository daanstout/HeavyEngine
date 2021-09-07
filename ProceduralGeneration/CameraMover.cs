using System;

using HeavyEngine;
using HeavyEngine.Input;

using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ProceduralGeneration {
    class CameraMover : Component, IUpdatable, IAwakable {
        [Dependency("New")] private readonly HeavyEngine.Input.IInputService inputService;

        public float speed = 10f;
        public float rotationSpeed = 0.5f;

        public void Awake() {
            var forward = inputService.CreateTriggerKey(this, "Forward", KeyInput.Down);
            forward.SubscribeToKey(Keys.W);
            forward.onKeyTriggered += () => Transform.Position += Transform.Forward * speed * Time.DeltaTime;

            var backward = inputService.CreateTriggerKey(this, "Backward", KeyInput.Down);
            backward.SubscribeToKey(Keys.S);
            backward.onKeyTriggered += () => Transform.Position -= Transform.Forward * speed * Time.DeltaTime;

            var velocity = inputService.CreateVelocityKey(this);
            var velFor = velocity.CreateTriggerKey("forward", 1.0f);
            velFor.TriggerKey.SubscribeToKey(Keys.D1);
            var velFor2 = velocity.CreateTriggerKey("forward 2", 1.0f);
            velFor2.TriggerKey.SubscribeToKey(Keys.D2);
            var velBack = velocity.CreateTriggerKey("backward", -1.0f);
            velBack.TriggerKey.SubscribeToKey(Keys.D3);
            var velBack2 = velocity.CreateTriggerKey("backward 2", -1.0f);
            velBack2.TriggerKey.SubscribeToKey(Keys.D4);
        }

        public void Update() {



            //if (inputService.CurrentKeyboardState.IsKeyDown(Keys.W))
            //    Transform.Position += Transform.Forward * speed * Time.DeltaTime;
            //if (inputService.CurrentKeyboardState.IsKeyDown(Keys.S))
            //    Transform.Position -= Transform.Forward * speed * Time.DeltaTime;

            //if (inputService.CurrentKeyboardState.IsKeyDown(Keys.A))
            //    Transform.Position -= Transform.Right * speed * Time.DeltaTime;
            //if (inputService.CurrentKeyboardState.IsKeyDown(Keys.D))
            //    Transform.Position += Transform.Right * speed * Time.DeltaTime;

            //if (inputService.CurrentKeyboardState.IsKeyDown(Keys.Space))
            //    Transform.Position += Transform.Up * speed * Time.DeltaTime;
            //if (inputService.CurrentKeyboardState.IsKeyDown(Keys.LeftShift))
            //    Transform.Position -= Transform.Up * speed * Time.DeltaTime;

            //if (inputService.CurrentKeyboardState.IsKeyDown(Keys.Q))
            //    Transform.Rotation = Quaternion.FromEulerAngles(0.0f, rotationSpeed * Time.DeltaTime, 0.0f) * Transform.Rotation;
            //if (inputService.CurrentKeyboardState.IsKeyDown(Keys.E))
            //    Transform.Rotation = Quaternion.FromEulerAngles(0.0f, -rotationSpeed * Time.DeltaTime, 0.0f) * Transform.Rotation;

            //if (inputService.CurrentKeyboardState.IsKeyDown(Keys.R))
            //    Transform.Rotation = Quaternion.FromEulerAngles(rotationSpeed * Time.DeltaTime, 0.0f, 0.0f) * Transform.Rotation;
            //if (inputService.CurrentKeyboardState.IsKeyDown(Keys.F))
            //    Transform.Rotation = Quaternion.FromEulerAngles(-rotationSpeed * Time.DeltaTime, 0.0f, 0.0f) * Transform.Rotation;
        }
    }
}
