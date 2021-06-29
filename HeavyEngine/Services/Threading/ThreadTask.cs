using System;
using System.Threading;

namespace HeavyEngine.Threading {
    public sealed class ThreadTask : IThreadTask {
        private Thread thread;
        private Action onCompleteCallback;

        public bool IsAvailable { get; private set; }

        public void Initialize(Action workload, Action completionCallback) {
            IsAvailable = false;

            thread = new Thread(new ThreadStart(workload)) {
                IsBackground = true
            };

            thread.Start();

            onCompleteCallback = completionCallback;
        }

        public void Update() {
            if (thread.Join(TimeSpan.Zero)) {
                onCompleteCallback.Invoke();

                IsAvailable = true;
            }
        }
    }
}
