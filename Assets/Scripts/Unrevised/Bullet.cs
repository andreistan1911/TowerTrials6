using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Bullet : Projectile
{

    private void Update()
    {
        MoveToTarget();

        if (_target == null)
        {
            Destroy(gameObject);
        }
    }

    private void MoveToTarget()
    {
        if (_target == null)
            return;

        _direction = (_target.position - transform.position).normalized;
        transform.position += _speed * _direction * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();

            enemy.TakeDamage(_damage, _element);
            Destroy(gameObject);
        }
    }
}
