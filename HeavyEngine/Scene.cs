using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine {
    public sealed class Scene {
        private readonly HashSet<GameObject> gameObjects;

        public Scene() {
            gameObjects = new HashSet<GameObject>();
        }

        public bool AddGameObject(GameObject gameObject) => gameObjects.Add(gameObject);

        public void Awake() {
            foreach (var gameObject in gameObjects)
                gameObject.AwakeGameObject();
        }

        public void Update() {
            foreach (var gameObject in gameObjects)
                gameObject.UpdateGameObject();
        }

        public void Render(Camera camera) {
            foreach (var gameObject in gameObjects)
                gameObject.RenderGameObject(camera);
        }
    }
}
