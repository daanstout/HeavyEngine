using System;

namespace HeavyEngine {
    public interface ITimerService {
        void StartTimer(float time, Action action);
    }
}