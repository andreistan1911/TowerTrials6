using UnityEngine;

[System.Serializable]
public class TowerStats
{
    public float damage;
    public float attackRate;
    public float range;
    public int cost;

    public TowerStats(float damage, float attackRate, float range, int cost)
    {
        this.damage = damage;
        this.attackRate = attackRate;
        this.range = range;
        this.cost = cost;
    }

    public TowerStats(TowerStats other)
    {
        damage = other.damage;
        attackRate = other.attackRate;
        range = other.range;
        cost = other.cost;
    }
}
