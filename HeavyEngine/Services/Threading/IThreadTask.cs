using System;

namespace HeavyEngine.Threading {
    public interface IThreadTask {
        bool IsAvailable { get; }

        void Initialize(Action workload, Action completionCallback);
        void Update();
    }
}