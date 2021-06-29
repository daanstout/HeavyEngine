using OpenTK.Windowing.Common;

namespace HeavyEngine {
    [Service(typeof(ITimeService), ServiceTypes.Singleton)]
    public sealed class TimeService : IService, ITimeService {
        public double TimeScale { get; set; } = 1;
        public float DeltaTime { get; private set; }
        public double DeltaDouble { get; private set; }
        public float UnscaledDeltaTime { get; private set; }
        public double UnscaledDeltaDouble { get; private set; }
        public double ScaledTimeSinceStart { get; private set; }
        public double TimeSinceStart { get; private set; }

        public void Initialize() { }

        public void Update(FrameEventArgs args) {
            DeltaTime = (float)(args.Time * TimeScale);
            DeltaDouble = args.Time * TimeScale;
            ScaledTimeSinceStart += args.Time * TimeScale;
            UnscaledDeltaTime = (float)args.Time;
            UnscaledDeltaDouble = args.Time;
            TimeSinceStart += args.Time;
        }
    }
}
