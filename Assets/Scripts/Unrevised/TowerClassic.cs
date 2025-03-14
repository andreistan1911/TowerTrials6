using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TowerClassic : Tower
{
    public float bulletSpeed;
    public GameObject bulletPrefab;

    private Transform _fireRoot;
    private float _lastFire;

    private new void Start()
    {
        base.Start();

        _fireRoot = transform.Find("FireRoot");
        _lastFire = 0;

        Assert.IsNotNull(_fireRoot);
        Assert.IsNotNull(bulletPrefab);
        Assert.AreNotEqual(0, bulletSpeed);
    }

    private void Fire(GameObject target)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, _fireRoot.position, Quaternion.identity);
        Bullet bullet = bulletInstance.GetComponent<Bullet>();

        if (bullet == null)
        {
            Destroy(bulletInstance);
            Debug.LogError("Bullet script not found on the projectile prefab.");
            return;
        }

        bullet.Target = target.transform;
        bullet.Damage = damage;
        bullet.Speed = bulletSpeed;
        bullet.Element = element;
    }

    override public void Fire(Enemy enemy)
    {
        if (Time.time - _lastFire >= attackRate)
        {
            Fire(enemy.transform.Find("ShootRoot").gameObject);
            _lastFire = Time.time;
        }
    }
}
