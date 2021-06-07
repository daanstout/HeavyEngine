using OpenTK.Windowing.Common;

namespace HeavyEngine {
    public interface ITimeService {
        float DeltaTime { get; }
        double DeltaDouble { get; }
        double ScaledTimeSinceStart { get; }
        double TimeScale { get; set; }
        double TimeSinceStart { get; }
        float UnscaledDeltaTime { get; }
        double UnscaledDeltaDouble { get; }

        void Update(FrameEventArgs args);
    }
}