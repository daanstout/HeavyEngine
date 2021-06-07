using System;
using System.Collections.Generic;
using System.Text;

namespace HeavyEngine {
    [Service(typeof(ITimerService), ServiceTypes.Singleton)]
    public class TimerService : IService, ITimerService {
        private class TimedObject {
            public float timeRemaining;
            public Action action;
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

        public void StartTimer(float time, Action action) => timedObjects.Add(new TimedObject() { timeRemaining = time, action = action });

        private void Update() {
            var objectsToRemove = new List<TimedObject>();
            foreach (var timedObject in timedObjects) {
                timedObject.timeRemaining -= timeService.DeltaTime;

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
