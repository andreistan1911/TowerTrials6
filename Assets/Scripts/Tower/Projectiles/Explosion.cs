using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : SourceOfDamage
{
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        if (enemy == null)
            return;

        enemy.TakeDamage(damage, element);
    }
}
