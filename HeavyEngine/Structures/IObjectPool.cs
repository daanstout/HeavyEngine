using System;
using System.Collections.Generic;
using System.Text;

using HeavyEngine.Rendering;

namespace HeavyEngine.Structures {
    public interface IObjectPool<T> : IUpdatable where T : new() {
        int MaxObjects { get; set; }

        void Initiate();
        T Obtain();
    }
}
