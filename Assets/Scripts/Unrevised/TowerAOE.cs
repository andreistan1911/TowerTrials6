using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAOE : Tower
{
    override public void Fire(Enemy enemy)
    {
        // todo here the sticky note -> update buff state somehow -> copy from towerclassic
        UpdateBuffState();
        enemy.TakeDamage(damage, element, _buffCode);
    }
}
