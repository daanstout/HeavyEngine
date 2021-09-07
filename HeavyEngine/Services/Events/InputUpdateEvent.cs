using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HeavyEngine {
    public class InputUpdateEvent : IEvent { }
    public struct InputUpdateEventArgs {
        public KeyboardState NewKeyboardState { get; }

        public InputUpdateEventArgs(KeyboardState newKeyboardState) => NewKeyboardState = newKeyboardState;
    }
}
