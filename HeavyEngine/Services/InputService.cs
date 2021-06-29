using System;
using System.Collections.Generic;
using System.Text;

using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HeavyEngine {
    [Service(typeof(IInputService), ServiceTypes.Singleton)]
    public class InputService : IService, IInputService {
        public KeyboardState CurrentKeyboardState { get; private set; }
        public KeyboardState PreviousKeyboardState { get; private set; }

        public void Initialize() { }

        public void Update(KeyboardState keyboardState) {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = keyboardState;
        }
    }
}
