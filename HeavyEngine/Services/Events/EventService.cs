using System;
using System.Collections.Generic;

using HeavyEngine.Injection;

namespace HeavyEngine.Services {
    [Service(typeof(IEventService), ServiceTypes.Singleton)]
    public class EventService : IService, IEventService {
        private class EventNode<TData> {
            public event Action<TData> evt;

            public void Invoke(TData data) => evt?.Invoke(data);
        }

        private class EventNode {
            public event Action evt;

            public void Invoke() => evt?.Invoke();
        }

        private readonly Dictionary<Type, object> events;

        public EventService() {
            events = new Dictionary<Type, object>();
        }

        public void Subscribe<TEvent>(Action listener) where TEvent : IEvent {
            if (!events.ContainsKey(typeof(TEvent)))
                events.Add(typeof(TEvent), new EventNode());

            ((EventNode)events[typeof(TEvent)]).evt += listener;
        }

        public void Subscribe<TEvent, TData>(Action<TData> listener) where TEvent : IEvent {
            if (!events.ContainsKey(typeof(TEvent)))
                events.Add(typeof(TEvent), new EventNode<TData>());

            ((EventNode<TData>)events[typeof(TEvent)]).evt += listener;
        }

        public void Unsubscribe<TEvent>(Action listener) where TEvent : IEvent {
            ((EventNode)events[typeof(TEvent)]).evt -= listener;
        }

        public void Unsubscribe<TEvent, TData>(Action<TData> listener) where TEvent : IEvent {
            ((EventNode<TData>)events[typeof(TEvent)]).evt -= listener;
        }

        public void Invoke<TEvent>() where TEvent : IEvent {
            ((EventNode)events[typeof(TEvent)]).Invoke();
        }

        public void Invoke<TEvent, TData>(TData data) where TEvent : IEvent {
            ((EventNode<TData>)events[typeof(TEvent)]).Invoke(data);
        }
    }
}
