using UnityEngine;

public class SourceOfDamage : MonoBehaviour
{
    protected float damage;
    protected Global.Element element;

    public float Damage
    {
        set { damage = value; }
    }

    public Global.Element Element
    {
        set { element = value; }
    }
}