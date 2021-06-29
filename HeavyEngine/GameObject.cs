using System.Collections.Generic;

using HeavyEngine.Logging;
using HeavyEngine.Rendering;

namespace HeavyEngine {
    public class GameObject {
        public string Name { get; set; }
        public Transform Transform => transform;

        private readonly Transform transform;
        private readonly List<Component> components;
        private readonly List<IUpdatable> updatables;
        private readonly List<IRenderable> renderables;
        private readonly List<IAwakable> awakables;

        [Dependency] protected ILogger logger;

        public GameObject() {
            DependencyObtainer.PrimaryInjector.Inject(this);

            components = new List<Component>();
            updatables = new List<IUpdatable>();
            renderables = new List<IRenderable>();
            awakables = new List<IAwakable>();
            transform = new Transform();
        }

        public T AddComponent<T>(T component) where T : Component {
            if (components.Contains(component)) {
                logger.LogWarning($"Trying to add a component that already exists! {component.GetType().Name} - {component}", this);
                return default;
            }

            components.Add(component);
            component.GameObject = this;
            DependencyObtainer.PrimaryInjector.Inject(component);

            if (component is IUpdatable updatable)
                updatables.Add(updatable);

            if (component is IRenderable renderable)
                renderables.Add(renderable);

            if (component is IAwakable awakable)
                awakables.Add(awakable);

            return component;
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

        public void AwakeGameObject() {
            Awake();

            for (int i = 0; i < awakables.Count; i++)
                awakables[i].Awake();
        }

        protected virtual void Awake() {

        }

        public void UpdateGameObject() {
            Update();

            for (int i = 0; i < updatables.Count; i++)
                updatables[i].Update();
        }

        protected virtual void Update() {

        }

        public void RenderGameObject() {
            for (int i = 0; i < renderables.Count; i++)
                renderables[i].Render();
        }

        public override string ToString() => Name;

        public static implicit operator bool(GameObject go) => go != null;
    }
}
