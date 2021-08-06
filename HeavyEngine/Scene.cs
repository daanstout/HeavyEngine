using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine {
    public sealed class Scene {
        private readonly List<GameObject> addedGameObjects;
        private readonly List<GameObject> gameObjects;
        private readonly List<GameObject> removedGameObjects;

        public Scene() {
            addedGameObjects = new List<GameObject>();
            gameObjects = new List<GameObject>();
            removedGameObjects = new List<GameObject>();
        }

        public void AddGameObject(GameObject gameObject) => addedGameObjects.Add(gameObject);
        public void RemoveGameObject(GameObject gameObject) => removedGameObjects.Add(gameObject);

        public void Awake() {
            gameObjects.AddRange(addedGameObjects);
            addedGameObjects.Clear();

            for (int i = 0; i < gameObjects.Count; i++)
                gameObjects[i].AwakeGameObject();

            for (int i = 0; i < removedGameObjects.Count; i++)
                gameObjects.Remove(removedGameObjects[i]);

            removedGameObjects.Clear();
        }

        public void Update() {
            var newObjects = addedGameObjects.ToArray();
            addedGameObjects.Clear();

            for (int i = 0; i < newObjects.Length; i++)
                newObjects[i].AwakeGameObject();

            gameObjects.AddRange(newObjects);

            for (int i = 0; i < gameObjects.Count; i++)
                gameObjects[i].UpdateGameObject();

            
        }

        public void Render(Camera camera) {
            foreach (var gameObject in gameObjects)
                gameObject.RenderGameObject(camera);
        }
    }
}
