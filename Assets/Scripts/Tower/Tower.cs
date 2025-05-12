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

    public TowerStats stats;

    private bool enemyInRange;

    public void Start()
    {
        enemyInRange = false;
        range = GetComponentInChildren<Range>();

        Assert.AreNotEqual(0, attackRate);
        Assert.IsNotNull(range);

        bool IamTutorialTower = (transform.parent.name == "Tutorial Tower");

        if (IamTutorialTower)
            range.GetComponent<SphereCollider>().radius = stats.range;

        stats = new(Global.towerValues[element][GetTowerTypeFromName()]);

        damage = stats.damage;
        attackRate = stats.attackRate;
        if (!IamTutorialTower)
            range.GetComponent<SphereCollider>().radius = stats.range;
        cost = stats.cost;
    }

    public Global.TowerType GetTowerTypeFromName()
    {
        string type = name.Replace("None_", "").Replace("Fire_", "").Replace("Lightning_", "").Replace("Water_", "");

        return Global.GetTowerTypeFromString(type);
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
