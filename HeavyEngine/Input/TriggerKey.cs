using System;
using System.Collections.Generic;

using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HeavyEngine.Input {
    /// <summary>
    /// A <see cref="TriggerKey"/> is a kind of key that can trigger when a button is active, firing the <see cref="onKeyTriggered"/> event.
    /// </summary>
    public class TriggerKey {
        private readonly List<Keys> subscribedKeys = new List<Keys>();

        /// <summary>
        /// The name of the <see cref="TriggerKey"/>
        /// </summary>
        public string KeyName { get; set; } = string.Empty;
        /// <summary>
        /// Indicates whether all keys or any keys need to be pressed to trigger the key
        /// </summary>
        public GroupedKeys KeyCombo { get; set; } = GroupedKeys.Any;
        /// <summary>
        /// Indicates whether the <see cref="TriggerKey"/> should listen to being pressed, released, or held down
        /// </summary>
        public KeyInput KeyInput { get; set; } = KeyInput.Pressed;
        /// <summary>
        /// The <see cref="Keys"/> this <see cref="TriggerKey"/> listens to
        /// </summary>
        public IReadOnlyCollection<Keys> SubscribedKeys => subscribedKeys;
        /// <summary>
        /// The event that gets fired when the key gets triggered
        /// </summary>
        public event Action onKeyTriggered;

        internal TriggerKey() { }

        /// <summary>
        /// Adds a certain <see cref="Keys"/> as a key that can trigger this <see cref="TriggerKey"/>
        /// </summary>
        /// <param name="key">The key to start to listen to</param>
        public void SubscribeToKey(Keys key) {
            if (!subscribedKeys.Contains(key))
                subscribedKeys.Add(key);
        }

        /// <summary>
        /// Adds a set of <see cref="Keys"/> as keys that can trigger this <see cref="TriggerKey"/>
        /// </summary>
        /// <param name="keys">The keys to start to listen to</param>
        public void SubscribeToKeys(params Keys[] keys) {
            foreach (var key in keys)
                if (!subscribedKeys.Contains(key))
                    subscribedKeys.Add(key);
        }

        /// <summary>
        /// Stops listening to a <see cref="Keys"/>
        /// </summary>
        /// <param name="key">The key to stop listening to</param>
        /// <returns>The result of the removal operation</returns>
        public bool UnsubscribeFromKey(Keys key) => subscribedKeys.Remove(key);

        internal void Trigger() => onKeyTriggered?.Invoke();
    }
}
