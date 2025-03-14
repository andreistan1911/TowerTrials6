using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAOE : Tower
{
    override public void Fire(Enemy enemy)
    {
        enemy.TakeDamage(damage, element);
    }
}
