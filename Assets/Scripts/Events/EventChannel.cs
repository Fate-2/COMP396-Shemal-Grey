using System.Collections.Generic;
using UnityEngine;
public abstract class EventChannel<T> : ScriptableObject
{
    private readonly HashSet<EventListener<T>> _observers = new();
    public void Invoke(T value)
    {
        foreach (var observer in _observers)
        {
            observer.Raise(value);
        }
    }
    public void Register(EventListener<T> listener) => _observers.Add(listener);
    public void Deregister(EventListener<T> listener) => _observers.Remove(listener);
}