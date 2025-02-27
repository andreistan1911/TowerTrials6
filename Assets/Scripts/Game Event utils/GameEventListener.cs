using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class GameEventListenerBase : MonoBehaviour
{
    public abstract void OnEventTriggered();
    public abstract void OnEventTriggered<T>(T parameter);
}

public class GameEventListener : GameEventListenerBase
{
    public GameEvent gameEvent;
    public UnityEvent onEventTriggered;

    private void OnEnable()
    {
        gameEvent?.AddListener(this);
    }

    private void OnDisable()
    {
        gameEvent?.RemoveListener(this);
    }

    public override void OnEventTriggered()
    {
        onEventTriggered?.Invoke();
    }

    public override void OnEventTriggered<T>(T parameter)
    {
        onEventTriggered?.Invoke();
    }
}

public class GameEventListener<T> : GameEventListenerBase
{
    public GameEvent gameEvent;
    public UnityEvent<T> onEventTriggered;

    private void OnEnable()
    {
        gameEvent?.AddListener(this);
    }

    private void OnDisable()
    {
        gameEvent?.RemoveListener(this);
    }

    public override void OnEventTriggered()
    {
        // Do nothing for the non-parameterized case
    }

    public override void OnEventTriggered<TParam>(TParam parameter)
    {
        if (parameter is T castedParam)
        {
            onEventTriggered?.Invoke(castedParam);
        }
    }
}
