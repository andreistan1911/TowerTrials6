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

    private Transform _fireRoot;
    private float _lastFire;

    private new void Start()
    {
        base.Start();

        _fireRoot = transform.Find("FireRoot");
        _lastFire = 0;

        Assert.IsNotNull(_fireRoot);
        Assert.IsNotNull(bombPrefab);
        Assert.AreNotEqual(0, launchHeight);
    }

    private void Fire(GameObject target)
    {
        GameObject bombInstance = Instantiate(bombPrefab, _fireRoot.position, Quaternion.identity);
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
        if (Time.time - _lastFire >= attackRate)
        {
            Fire(enemy.transform.Find("ShootRoot").gameObject);
            _lastFire = Time.time;
        }
    }
}
