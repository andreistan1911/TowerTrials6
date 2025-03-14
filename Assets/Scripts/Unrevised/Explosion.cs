using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : SourceOfDamage
{
    private void OnTriggerEnter(Collider other)
    {
        // It should be enemy to deal damage
        if (!other.CompareTag("Enemy"))
            return;

        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        enemy.TakeDamage(_damage, _element);
    }
}
