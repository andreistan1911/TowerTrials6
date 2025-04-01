using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TowerClassic : Tower
{
    public float bulletSpeed;
    public GameObject bulletPrefab;

    private Transform fireRoot;
    private float lastFire;

    private new void Start()
    {
        base.Start();

        fireRoot = transform.Find("FireRoot");
        lastFire = 0;

        Assert.IsNotNull(fireRoot);
        Assert.IsNotNull(bulletPrefab);
        Assert.AreNotEqual(0, bulletSpeed);
    }

    private void Fire(GameObject target)
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, fireRoot.position, Quaternion.identity);
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
        if (Time.time - lastFire >= attackRate)
        {
            Fire(enemy.transform.Find("ShootRoot").gameObject);
            lastFire = Time.time;
        }
    }
}
