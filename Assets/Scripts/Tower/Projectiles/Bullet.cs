using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Bullet : Projectile
{
    private void Update()
    {
        FollowTarget();

        if (target == null)
        {
            Destroy(gameObject);
        }
    }

    private void FollowTarget()
    {
        if (target == null)
            return;

        direction = (target.position - transform.position).normalized;
        transform.position += speed * direction * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();

        // ignore if detected object is not an enemy
        if (enemy == null)
            return;

        enemy.TakeDamage(damage, element);
        Destroy(gameObject);
    }
}
