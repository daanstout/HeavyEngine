namespace HeavyEngine {
    /// <summary>
    /// The <see cref="Component"/> of the Entity-Component System. All <see cref="Component"/>s should inherit from this class.
    /// </summary>
    public abstract class Component {
        /// <summary>
        /// The <see cref="HeavyEngine.GameObject"/> this <see cref="Component"/> sits on.
        /// </summary>
        public GameObject GameObject { get; internal set; }
        /// <summary>
        /// The name of the <see cref="HeavyEngine.GameObject"/> this <see cref="Component"/> sits on.
        /// </summary>
        public string Name => GameObject.Name;
        /// <summary>
        /// The <see cref="HeavyEngine.Transform"/> of the <see cref="HeavyEngine.GameObject"/> this <see cref="Component"/> sits on.
        /// </summary>
        public Transform Transform => GameObject.Transform;

        /// <summary>
        /// Is called after a <see cref="Component"/> has been added to a <see cref="HeavyEngine.GameObject"/> after being injected with potential dependencies.
        /// <para>Will only be called once.</para>
        /// </summary>
        public virtual void OnAdded() { }
        /// <summary>
        /// Adds a <see cref="GameObject"/> of the provided type to the <see cref="HeavyEngine.GameObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component"/> to add.</typeparam>
        /// <param name="component">The <see cref="Component"/> to add to the <see cref="HeavyEngine.GameObject"/>.</param>
        /// <returns>The <see cref="Component"/> that was added.</returns>
        public T AddComponent<T>(T component) where T : Component => GameObject.AddComponent(component);
        /// <summary>
        /// Gets the first <see cref="Component"/> of the given type on this <see cref="HeavyEngine.GameObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component"/> to get</typeparam>
        /// <returns>The first found <see cref="Component"/>, or <see langword="null"/> if none of the provided type were found.</returns>
        public T GetComponent<T>() where T : Component => GameObject.GetComponent<T>();
        /// <summary>
        /// Gets an array of all <see cref="Component"/>s of the given type on the <see cref="HeavyEngine.GameObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="Component"/> to get.</typeparam>
        /// <returns>An array containing all <see cref="Component"/>s of the provided type on the <see cref="HeavyEngine.GameObject"/>.</returns>
        public T[] GetComponents<T>() where T : Component => GameObject.GetComponents<T>();
        /// <summary>
        /// Removes the first found <see cref="Component"/> of the given type from the <see cref="HeavyEngine.GameObject"/>.
        /// </summary>
        /// <typeparam name="T">The type of <see cref="GameObject"/> to remove</typeparam>
        public void RemoveComponent<T>() where T : Component => GameObject.RemoveComponent<T>();
        /// <summary>
        /// Removes the provided <see cref="Component"/> from the <see cref="HeavyEngine.GameObject"/>.
        /// </summary>
        /// <param name="component"></param>
        public void RemoveComponent(Component component) => GameObject.RemoveComponent(component);

        /// <summary>
        /// Converts this <see cref="Component"/>s existance to <see cref="bool"/> form.
        /// </summary>
        /// <param name="comp">The <see cref="Component"/> to convert.</param>
        public static implicit operator bool(Component comp) => comp != null;
    }
}
