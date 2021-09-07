using System;
using System.Collections.Generic;
using System.Linq;

namespace HeavyEngine.Threading {
    [Service(typeof(ThreadingService), ServiceTypes.Singleton)]
    public sealed class ThreadingService : IService, IThreadingService {
        private static readonly int DEFAULT_THREAD_SIZE = Environment.ProcessorCount;

        private class QueueItem {
            public Action task;
            public Action completionCallback;
        }

        [Dependency] private readonly IEventService eventService;

        private IThreadTask[] threads;
        private readonly Queue<QueueItem> tasks;
        private int runningThreads;
        private int targetThreadCount;

        public ThreadingService() {
            runningThreads = 0;
            tasks = new Queue<QueueItem>();

            threads = new IThreadTask[DEFAULT_THREAD_SIZE];

            for (int i = 0; i < threads.Length; i++)
                threads[i] = new ThreadTask();

            runningThreads = 0;
            targetThreadCount = DEFAULT_THREAD_SIZE;
        }

        ~ThreadingService() {
            eventService.Unsubscribe<UpdateEvent>(Update);
        }

        public void Initialize() {
            eventService.Subscribe<UpdateEvent>(Update);
        }

        public void SetThreadCount(int threadCount) {
            targetThreadCount = threadCount;

            if (runningThreads > threadCount) {
                return;
            }

            var threads = new IThreadTask[threadCount];

            int threadIndex = 0;
            for (int i = 0; i < threadCount; i++) {
                while (this.threads[threadIndex].IsAvailable && threadIndex < this.threads.Length - 1)
                    threadIndex++;

                if (threadIndex >= this.threads.Length)
                    break;

                threads[i] = this.threads[threadIndex];
            }

            this.threads = threads;
        }

        public void Update() {
            foreach (var thread in threads) {
                if (!thread.IsAvailable)
                    thread.Update();
            }

            if (threads.Length > targetThreadCount && runningThreads < targetThreadCount)
                SetThreadCount(targetThreadCount);

            while (tasks.Count > 0 && runningThreads < targetThreadCount) {
                var availableThread = FirstAvailableThread();

                if (availableThread == null)
                    break;

                var item = tasks.Dequeue();

                availableThread.Initialize(item.task, () => {
                    runningThreads--;

                    item.completionCallback?.Invoke();
                });

                runningThreads++;
            }
        }

        public void QueueTask(Action task) {
            tasks.Enqueue(new QueueItem() { task = task });
        }

        public void QueueTask(Action task, Action completionCallback) {
            tasks.Enqueue(new QueueItem() { task = task, completionCallback = completionCallback });
        }

        private IThreadTask FirstAvailableThread() => threads.FirstOrDefault(thread => thread.IsAvailable);
    }
}
