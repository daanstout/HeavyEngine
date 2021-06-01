using System.Collections.Generic;

using HeavyEngine.Logging;
using HeavyEngine.Rendering;

namespace HeavyEngine {
    public class GameObject : IUpdatable, IRenderable {
        public string Name { get; set; }
        public Transform Transform => transform;

        private readonly Transform transform;
        private readonly List<Component> components;
        private readonly List<IUpdatable> updatables;
        private readonly List<IRenderable> renderables;

        [Dependency] protected ILogger logger;

        protected GameObject() {
            DependencyObtainer.PrimaryInjector.Inject(this);

            updatables = new List<IUpdatable>();
            renderables = new List<IRenderable>();
            transform = new Transform();
        }

        public void AddComponent(Component component) {
            if (!components.Contains(component)) {
                logger.LogWarning($"Trying to add a component that already exists! {component.GetType().Name} - {component}", this);
                return;
            }

            components.Add(component);
        }



        public T GetComponent<T>() where T : Component {
            for (int i = 0; i < components.Count; i++) {
                if (components[i] is T t)
                    return t;
            }

            return default;
        }

        public T[] GetComponents<T>() where T : Component {
            List<T> components = new List<T>();

            for (int i = 0; i < this.components.Count; i++)
                if (this.components[i] is T t)
                    components.Add(t);

            return components.ToArray();
        }

        public void RemoveComponent<T>() where T : Component {
            for (int i = 0; i < components.Count; i++) {
                if (components[i] is T comp) {
                    components.Remove(comp);
                    return;
                }
            }
        }

        public void RemoveComponent(Component component) {
            if (!components.Contains(component)) {
                logger.LogWarning($"Trying to remove component {component.GetType().Name}-{component}, but it is not our component!", this);
                return;
            }

            components.Remove(component);
        }

        public void Update() {
            for (int i = 0; i < updatables.Count; i++)
                updatables[i].Update();
        }

        public void Render() {
            for (int i = 0; i < renderables.Count; i++)
                renderables[i].Render();
        }

        public override string ToString() => Name;

        public static implicit operator bool(GameObject go) => go != null;
    }
}
