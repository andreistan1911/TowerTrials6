using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : SourceOfDamage
{
    protected Transform target;
    protected float speed;

    protected Vector3 direction;

    public Transform Target
    {
        set { target = value; }
    }

    public float Speed
    {
        set { speed = value; }
    }
}
