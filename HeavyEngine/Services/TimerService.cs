using System;
using System.Collections.Generic;

namespace HeavyEngine {
    [Service(typeof(ITimerService), ServiceTypes.Singleton)]
    public sealed class TimerService : IService, ITimerService {
        private class TimedObject {
            public float timeRemaining;
            public Action action;
            public bool useScaledTime;
        }

        [Dependency] private readonly ITimeService timeService;
        [Dependency] private readonly IEventService eventService;

        private readonly List<TimedObject> timedObjects = new List<TimedObject>();

        ~TimerService() {
            eventService.Unsubscribe<UpdateEvent>(Update);
        }

        public void Initialize() {
            eventService.Subscribe<UpdateEvent>(Update);
        }

        public void StartTimer(float time, Action action, bool useScaledTime) => timedObjects.Add(new TimedObject() { timeRemaining = time, action = action, useScaledTime = useScaledTime });

        private void Update() {
            var objectsToRemove = new List<TimedObject>();
            foreach (var timedObject in timedObjects) {
                timedObject.timeRemaining -= (timedObject.useScaledTime ? timeService.DeltaTime : timeService.UnscaledDeltaTime);

                if (timedObject.timeRemaining < 0.0f)
                    objectsToRemove.Add(timedObject);
            }

            foreach (var timedObject in objectsToRemove) {
                timedObject.action();

                timedObjects.Remove(timedObject);
            }
        }
    }
}
