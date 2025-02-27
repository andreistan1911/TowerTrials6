using UnityEngine;

public class OnTakeDamageListener: GameEventListener<float>
{
    public override void OnEventTriggered<TParam>(TParam parameter)
    {
        if (parameter is float castedParam)
        {
            onEventTriggered?.Invoke(castedParam);
        }
    }
}
