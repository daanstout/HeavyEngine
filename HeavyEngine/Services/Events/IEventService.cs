using System;

namespace HeavyEngine {
    public interface IEventService {
        void Invoke<TEvent, TData>(TData data) where TEvent : IEvent;
        void Invoke<TEvent>() where TEvent : IEvent;
        void Subscribe<TEvent, TData>(Action<TData> listener) where TEvent : IEvent;
        void Subscribe<TEvent>(Action listener) where TEvent : IEvent;
        void Unsubscribe<TEvent, TData>(Action<TData> listener) where TEvent : IEvent;
        void Unsubscribe<TEvent>(Action listener) where TEvent : IEvent;
    }
}