using System;
using System.Collections.Generic;

namespace HeavyEngine.Input {
    public class VelocityKey {
        public class TriggerKeyEntry {
            public string Name { get; set; }
            public TriggerKey TriggerKey { get; set; }
            public float Value { get; set; }

            internal TriggerKeyEntry() { }
        }

        private readonly Dictionary<string, TriggerKeyEntry> triggerKeys = new Dictionary<string, TriggerKeyEntry>();

        public string KeyName { get; set; } = string.Empty;
        public GroupedKeys KeyCombo { get; set; } = GroupedKeys.Any;
        public KeyInput KeyInput { get; set; } = KeyInput.Down;
        public IReadOnlyCollection<TriggerKeyEntry> TriggerKeys => triggerKeys.Values;
        public event Action<float> onKeyTriggered;

        internal VelocityKey() {

        }

        public TriggerKeyEntry CreateTriggerKey(string name, float value) {
            value = Math.Clamp(value, -1.0f, 1.0f);
            var entry = new TriggerKeyEntry {
                Name = name,
                TriggerKey = new TriggerKey() {
                    KeyInput = KeyInput.Down,
                },
                Value = value
            };
            triggerKeys.Add(name, entry);

            return entry;
        }

        public bool DeleteTriggerKey(string name) => triggerKeys.Remove(name);

        public bool DeleteTriggerKey(TriggerKeyEntry entry) => triggerKeys.Remove(entry.Name);

        internal void Trigger(float value) => onKeyTriggered?.Invoke(value);
    }
}
