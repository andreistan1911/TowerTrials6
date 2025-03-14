using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Tower : MonoBehaviour
{
    [Tooltip("Damage")]
    public float damage;
    [Tooltip("Seconds between shots")]
    public float attackRate;
    [Tooltip("Tower element")]
    public Global.Element element;

    public void Start()
    {
        Assert.AreNotEqual(0, attackRate);
    }

    abstract public void Fire(Enemy enemy);
}
