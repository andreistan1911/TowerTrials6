using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Range : MonoBehaviour
{
    private Tower _tower;
    private Dictionary<Enemy, float> _enemyTimers; // <enemy object, time spent in range>

    private void Start()
    {
        _tower = transform.parent.GetComponent<Tower>();
        _enemyTimers = new Dictionary<Enemy, float>();

        // Just to be sure
        if (_tower == null)
        {
            Debug.Log("Error: tower not found from range");
        }
    }

    private void Update()
    {
        foreach (var enemy in _enemyTimers.Keys.ToList())
        {
            if (enemy == null)
                continue;

            _enemyTimers[enemy] += Time.deltaTime;

            if (_enemyTimers[enemy] >= _tower.attackRate)
            {
                _enemyTimers[enemy] = 0;
                _tower.Fire(enemy);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // It should be enemy to add in dictionary
        if (!other.CompareTag("Enemy"))
            return;

        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        _enemyTimers.Add(enemy, 0);
        _tower.Fire(enemy);
    }

    private void OnTriggerExit(Collider other)
    {
        // It should be enemy to remove from dictionary
        if (!other.CompareTag("Enemy"))
            return;

        Enemy enemy = other.gameObject.GetComponent<Enemy>();

        _enemyTimers[enemy] = 0;
        _enemyTimers.Remove(enemy);
    }
}
