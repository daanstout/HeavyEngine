using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine {
    public abstract class Component {
        public GameObject GameObject { get; internal set; }
        public string Name => GameObject.Name;
        public Transform Transform => GameObject.Transform;

        public virtual void OnAdded() { }
        public T AddComponent<T>(T component) where T : Component => GameObject.AddComponent(component);
        public T GetComponent<T>() where T : Component => GameObject.GetComponent<T>();
        public T[] GetComponents<T>() where T : Component => GameObject.GetComponents<T>();
        public void RemoveComponent<T>() where T : Component => GameObject.RemoveComponent<T>();
        public void RemoveComponent(Component component) => GameObject.RemoveComponent(component);

        public static implicit operator bool(Component comp) => comp != null;
    }
}
