using System;

namespace HeavyEngine.Threading {
    public interface IThreadingService {
        void QueueTask(Action task);
        void QueueTask(Action task, Action completionCallback);
        void SetThreadCount(int threadCount);
        void Update();
    }
}