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

    [HideInInspector]
    public Range range;

    [HideInInspector]
    public int cost;

    private bool enemyInRange;

    public void Start()
    {
        enemyInRange = false;

        Assert.AreNotEqual(0, attackRate);
    }

    abstract public void Fire(Enemy enemy);

    public void EnemyIsInRange()
    {
        enemyInRange = true;
    }

    public void NoEnemyInRange()
    {
        enemyInRange = false;
    }

    protected bool IsEnemyInRange()
    {
        return enemyInRange;
    }
}
