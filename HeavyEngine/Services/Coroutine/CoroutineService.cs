using System.Collections;
using System.Collections.Generic;

namespace HeavyEngine {
    /// <summary>
    /// The <see cref="CoroutineService"/> allows you to start coroutines that run over the span of multiple frames.
    /// </summary>
    [Service(typeof(ICoroutineService), ServiceTypes.Singleton)]
    public sealed class CoroutineService : IService, ICoroutineService {
        [Dependency] private readonly IEventService eventService;

        private readonly List<IEnumerator> coroutines;

        public CoroutineService() {
            coroutines = new List<IEnumerator>();
        }

        ~CoroutineService() {
            eventService.Unsubscribe<UpdateEvent>(Update);
        }

        public void Initialize() {
            eventService.Subscribe<UpdateEvent>(Update);
        }

        /// <inheritdoc/>
        public void StartCoroutine(IEnumerator coroutine) {
            coroutine.MoveNext();
            coroutines.Add(coroutine);
        }

        private void Update() {
            var coroutinesToRemove = new List<IEnumerator>();

            foreach (var coroutine in coroutines) {
                if (coroutine.Current is WaitDuration wd)
                    wd.Duration -= Time.DeltaTime;

                if (coroutine.Current is IEnumerator ie)
                    if (!ie.MoveNext())
                        continue;

                if (!coroutine.MoveNext()) {
                    coroutinesToRemove.Add(coroutine);
                }
            }

            foreach (var coroutine in coroutinesToRemove) {
                coroutines.Remove(coroutine);
            }
        }
    }
}
