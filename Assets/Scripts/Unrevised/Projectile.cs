using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : SourceOfDamage
{
    protected Transform _target;
    protected float _speed;

    protected Vector3 _direction;

    public Transform Target
    {
        set { _target = value; }
    }

    public float Speed
    {
        set { _speed = value; }
    }
}
