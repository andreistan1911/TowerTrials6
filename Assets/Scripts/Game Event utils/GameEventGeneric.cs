using System.Collections.Generic;
using UnityEngine;

public abstract class GameEventBase : ScriptableObject
{
    protected List<GameEventListenerBase> listeners = new();

    public void AddListener(GameEventListenerBase listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(GameEventListenerBase listener)
    {
        listeners.Remove(listener);
    }
}

public class GameEvent<T> : GameEventBase
{
    public void TriggerEvent(T parameter)
    {
        for (int i = listeners.Count - 1; i >= 0; --i)
            listeners[i].OnEventTriggered(parameter);
    }
}