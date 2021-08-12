using OpenTK.Windowing.Common;

namespace HeavyEngine {
    public static class Time {
        public static double TimeScale { get; set; } = 1;
        public static float DeltaTime { get; private set; }
        public static double DeltaDouble { get; private set; }
        public static float UnscaledDeltaTime { get; private set; }
        public static double UnscaledDeltaDouble { get; private set; }
        public static double ScaledTimeSinceStart { get; private set; }
        public static double TimeSinceStart { get; private set; }

        internal static void Update(FrameEventArgs args) {
            DeltaTime = (float)(args.Time * TimeScale);
            DeltaDouble = args.Time * TimeScale;
            ScaledTimeSinceStart += args.Time * TimeScale;
            UnscaledDeltaTime = (float)args.Time;
            UnscaledDeltaDouble = args.Time;
            TimeSinceStart += args.Time;
        }
    }
}
