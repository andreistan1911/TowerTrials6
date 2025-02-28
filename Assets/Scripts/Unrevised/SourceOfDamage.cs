using UnityEngine;

public class SourceOfDamage : MonoBehaviour
{
    protected float _damage;
    protected Global.Element _element;
    protected int _buffCode;

    public float Damage
    {
        set { _damage = value; }
    }

    public Global.Element Element
    {
        set { _element = value; }
    }

    public int BuffCode
    {
        set { _buffCode = value; }
        get { return _buffCode; }
    }
}