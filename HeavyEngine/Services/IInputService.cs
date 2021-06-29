using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HeavyEngine {
    public interface IInputService {
        KeyboardState CurrentKeyboardState { get; }
        KeyboardState PreviousKeyboardState { get; }

        void Update(KeyboardState keyboardState);
    }
}