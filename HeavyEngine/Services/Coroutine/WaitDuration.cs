using System;
using System.Collections;

namespace HeavyEngine {
    public sealed class WaitDuration : IEnumerator {
        public object Current => null;
        public float Duration { get; set; }

        private readonly float initialDuration;

        public WaitDuration(float duration) {
            initialDuration = Duration = duration;
        }

        public WaitDuration(TimeSpan duration) {
            initialDuration = Duration = duration.Seconds;
        }

        public bool MoveNext() => Duration < 0;
        public void Reset() => Duration = initialDuration;
    }
}
