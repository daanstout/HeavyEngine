using System;
using System.Collections.Generic;
using System.Linq;

using OpenTK.Windowing.GraphicsLibraryFramework;

namespace HeavyEngine.Input {
    [Service(typeof(IInputService), ServiceTypes.Singleton, "New")]
    public class InputService : IService, IInputService {
        private class KeyEntry<T> {
            public object Context { get; set; }
            public T Key { get; set; }
        }

        [Dependency] private readonly IEventService eventService;

        private readonly List<KeyEntry<TriggerKey>> triggerKeys = new List<KeyEntry<TriggerKey>>();
        private readonly List<KeyEntry<VelocityKey>> velocityKeys = new List<KeyEntry<VelocityKey>>();

        ~InputService() => eventService.Unsubscribe<InputUpdateEvent, InputUpdateEventArgs>(OnInputUpdated);

        public void Initialize() => eventService.Subscribe<InputUpdateEvent, InputUpdateEventArgs>(OnInputUpdated);

        public TriggerKey CreateTriggerKey(object context, string actionName = "", KeyInput keyInput = KeyInput.Pressed) {
            var keyEntry = new KeyEntry<TriggerKey> {
                Context = context,
                Key = new TriggerKey {
                    KeyName = actionName,
                    KeyInput = keyInput,
                }
            };
            triggerKeys.Add(keyEntry);
            return keyEntry.Key;
        }

        public VelocityKey CreateVelocityKey(object context) {
            var keyEntry = new KeyEntry<VelocityKey> {
                Context = context,
                Key = new VelocityKey()
            };
            velocityKeys.Add(keyEntry);
            return keyEntry.Key;
        }

        public bool DestroyKey(TriggerKey key) => triggerKeys.RemoveAll(entry => entry.Key == key) > 0;

        public bool DestroyKey(VelocityKey key) => velocityKeys.RemoveAll(entry => entry.Key == key) > 0;

        private void OnInputUpdated(InputUpdateEventArgs inputUpdate) {
            var triggerKeysToRemove = new List<KeyEntry<TriggerKey>>();
            foreach (var keyEntry in triggerKeys) {
                if (keyEntry.Context == null)
                    triggerKeysToRemove.Add(keyEntry);
                else if (IsKeyActive(keyEntry.Key, inputUpdate.NewKeyboardState))
                    keyEntry.Key.Trigger();
            }
            
            foreach (var triggerKeyToRemove in triggerKeysToRemove)
                triggerKeys.Remove(triggerKeyToRemove);

            var velocityKeysToRemove = new List<KeyEntry<VelocityKey>>();
            foreach (var keyEntry in velocityKeys) {
                if (keyEntry.Context == null) {
                    velocityKeysToRemove.Add(keyEntry);
                    continue;
                }

                var result = 0.0f;
                var activeKeys = 0;
                foreach (var entry in keyEntry.Key.TriggerKeys) {
                    if (IsKeyActive(entry.TriggerKey, inputUpdate.NewKeyboardState)) {
                        result += entry.Value;
                        activeKeys++;
                    }
                }

                if (activeKeys == 0)
                    activeKeys = 1;

                keyEntry.Key.Trigger(result / activeKeys);
            }

            foreach (var velocityKeyToRemove in velocityKeysToRemove)
                velocityKeys.Remove(velocityKeyToRemove);
        }

        private bool IsKeyActive(TriggerKey key, KeyboardState keyboardState) {
            return key.KeyInput switch {
                KeyInput.Pressed => IsKeyActive(key, keyboardState.IsKeyPressed),
                KeyInput.Down => IsKeyActive(key, keyboardState.IsKeyDown),
                KeyInput.Released => IsKeyActive(key, keyboardState.IsKeyReleased),
                _ => false
            };
        }

        private bool IsKeyActive(TriggerKey key, Func<Keys, bool> checkFunction) {
            return key.KeyCombo switch {
                GroupedKeys.All => key.SubscribedKeys.All(checkFunction),
                GroupedKeys.Any => key.SubscribedKeys.Any(checkFunction),
                _ => false
            };
        }
    }
}
