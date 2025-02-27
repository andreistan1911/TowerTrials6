using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    public float health;
    public float speed;

    public EnemyStats(float health, float speed)
    {
        this.health = health;
        this.speed = speed;
    }

    public EnemyStats(EnemyStats other)
    {
        health = other.health;
        speed = other.speed;
    }
}
