using UnityEngine;

[CreateAssetMenu(menuName = "Game Event - Void")]
public class GameEvent : GameEventBase
{
    public void TriggerEvent()
    {
        for (int i = listeners.Count - 1; i >= 0; --i)
            listeners[i].OnEventTriggered();
    }
}