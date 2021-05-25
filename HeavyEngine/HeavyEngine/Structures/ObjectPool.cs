using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine.Structures {
    public class ObjectPool<T> : IObjectPool<T> where T : new() {
        public int MaxObjects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private int numInUse;
        private T[] objects;

        public void Initiate() {
            objects = new T[MaxObjects + 1];

            for(int i = 0; i < MaxObjects; i++)
                objects[i] = new T();

            numInUse = 0;
        }

        public T Obtain() {
            if (numInUse == MaxObjects)
                return default;

            var obj = objects[numInUse];
            numInUse++;
            return obj;
        }

        public void Render() => throw new NotImplementedException();
        public void Update() => throw new NotImplementedException();
    }
}
