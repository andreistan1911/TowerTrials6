using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class TowerBomb : Tower
{
    [SerializeField]
    private float launchHeight;

    [SerializeField]
    private GameObject bombPrefab;

    private Transform fireRoot;
    private float lastFire;

    private new void Start()
    {
        base.Start();

        fireRoot = transform.Find("FireRoot");
        lastFire = 0;

        Assert.IsNotNull(fireRoot);
        Assert.IsNotNull(bombPrefab);
        Assert.AreNotEqual(0, launchHeight);
    }

    private void Fire(GameObject target)
    {
        GameObject bombInstance = Instantiate(bombPrefab, fireRoot.position, Quaternion.identity);
        Bomb bomb = bombInstance.GetComponent<Bomb>();

        if (bomb == null)
        {
            Destroy(bombInstance);
            Debug.LogError("Bomb script not found on the projectile prefab.");
            return;
        }

        bomb.Target = target.transform;
        bomb.Damage = damage;
        bomb.Element = element;
        bomb.Height = launchHeight;
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
